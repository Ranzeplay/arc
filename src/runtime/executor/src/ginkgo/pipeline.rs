use arc_shared::ginkgo::instructions::InstructionType;
use arc_shared::ginkgo::{BuiltinType, Function, Package, PackageType, Symbol};
use arc_shared::models::encodings::data_type_enc::{DataTypeEncoding, Mutability};
use arc_shared::models::execution::data::{DataValue, DataValueType};
use log::error;
use std::cell::RefCell;
use std::collections::BTreeMap;
use std::rc::Rc;

pub fn entry(package: &Package) -> Result<u8, String> {
    let desc = &package.descriptor;
    let entry_func = match desc.package_type {
        PackageType::Library => {
            return Err("Library packages cannot be executed".to_string());
        }
        PackageType::Executable => {
            if let Some(entry) = package.symbols.get(&desc.entrypoint_function_id) {
                match entry {
                    Symbol::Function(func) => func,
                    _ => {
                        return Err("Entry point is not a function".to_string());
                    }
                }
            } else {
                return Err("Entry point function not found".to_string());
            }
        }
    };

    let result = exec_func(
        package,
        entry_func,
        desc.entrypoint_function_id,
        &mut BTreeMap::new(),
    );
    result.map(|_| 0)
}

struct FuncCallPrepProps {
    ireg_ids: Vec<u16>,
    func_id: u32,
}

impl FuncCallPrepProps {
    fn new(id: u32) -> Self {
        FuncCallPrepProps {
            ireg_ids: vec![],
            func_id: id,
        }
    }
}

pub fn exec_func(
    package: &Package,
    func: &Function,
    func_id: u32,
    iregs: &mut BTreeMap<u16, Rc<RefCell<DataValue>>>,
) -> Result<Option<Rc<RefCell<DataValue>>>, String> {
    macro_rules! get_ireg_value {
        ($reg_id:expr) => {
            iregs
                .get(&$reg_id)
                .expect(&format!("No register found for id {}", $reg_id))
                .clone()
                .borrow()
                .clone()
        };
    }

    if func.code_length == 0 {
        error!("Function {} is external and cannot be executed", func_id);
        return Err("Function is external and cannot be executed".to_string());
    }

    let entry_instruction_index = package
        .instructions
        .binary_search_by_key(&func.code_offset, |instr| instr.offset)
        .expect("Function code offset not found in instructions");

    let mut current_instruction_index = entry_instruction_index;

    let mut fcp = FuncCallPrepProps::new(0);

    loop {
        let current_instruction = &package.instructions[current_instruction_index];
        let instr = match current_instruction.instruction_type() {
            Ok(instr) => instr,
            Err(err) => {
                error!(
                    "Failed to convert instruction at offset {}: {}",
                    current_instruction.offset, err
                );
                return Err("Failed to convert instruction".to_string());
            }
        };

        match instr {
            InstructionType::NoOperation(_) => {}
            InstructionType::Add(x) => {
                let a = iregs
                    .get(&x.src_a)
                    .expect("No register found")
                    .clone()
                    .borrow()
                    .clone();
                let b = iregs
                    .get(&x.src_b)
                    .expect("No register found")
                    .clone()
                    .borrow()
                    .clone();

                let enc = a.data_type.clone();

                match (&a.value, &b.value) {
                    (DataValueType::Integer(a), DataValueType::Integer(b)) => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Integer(a + b),
                            })),
                        );
                    }
                    (DataValueType::Decimal(a), DataValueType::Decimal(b)) => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Decimal(a + b),
                            })),
                        );
                    }
                    _ => panic!("Unexpected instruction type in add"),
                }
            }
            InstructionType::Subtract(x) => {
                let a = get_ireg_value!(x.src_a);
                let b = get_ireg_value!(x.src_b);

                let enc = a.data_type.clone();

                match (&a.value, &b.value) {
                    (DataValueType::Integer(a), DataValueType::Integer(b)) => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Integer(a - b),
                            })),
                        );
                    }
                    (DataValueType::Decimal(a), DataValueType::Decimal(b)) => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Decimal(a - b),
                            })),
                        );
                    }
                    _ => panic!("Unexpected instruction type in subtract"),
                }
            }
            InstructionType::Multiply(x) => {
                let a = get_ireg_value!(x.src_a);
                let b = get_ireg_value!(x.src_b);

                let enc = a.data_type.clone();

                match (&a.value, &b.value) {
                    (DataValueType::Integer(a), DataValueType::Integer(b)) => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Integer(a * b),
                            })),
                        );
                    }
                    (DataValueType::Decimal(a), DataValueType::Decimal(b)) => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Decimal(a * b),
                            })),
                        );
                    }
                    _ => panic!("Unexpected instruction type in multiply"),
                }
            }
            InstructionType::Divide(x) => {
                let a = get_ireg_value!(x.src_a);
                let b = get_ireg_value!(x.src_b);

                let enc = a.data_type.clone();

                match (&a.value, &b.value) {
                    (DataValueType::Integer(a), DataValueType::Integer(b)) => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Integer(a / b),
                            })),
                        );
                    }
                    (DataValueType::Decimal(a), DataValueType::Decimal(b)) => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Decimal(a / b),
                            })),
                        );
                    }
                    _ => panic!("Unexpected instruction type in divide"),
                }
            }
            InstructionType::Modulo(x) => {
                let a = get_ireg_value!(x.src_a);
                let b = get_ireg_value!(x.src_b);

                let enc = a.data_type.clone();

                match (&a.value, &b.value) {
                    (DataValueType::Integer(a), DataValueType::Integer(b)) => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Integer(a % b),
                            })),
                        );
                    }
                    (DataValueType::Decimal(a), DataValueType::Decimal(b)) => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Decimal(a % b),
                            })),
                        );
                    }
                    _ => panic!("Unexpected instruction type in modulo"),
                }
            }
            InstructionType::Inverse(x) => {
                let a = get_ireg_value!(x.src);

                let enc = a.data_type.clone();

                match &a.value {
                    DataValueType::Integer(a) => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Integer(-a),
                            })),
                        );
                    }
                    DataValueType::Decimal(a) => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Decimal(-a),
                            })),
                        );
                    }
                    _ => panic!("Unexpected instruction type in inverse"),
                }
            }
            InstructionType::LeftShiftArithmetic(x) => {
                let src = get_ireg_value!(x.src);
                let amount = get_ireg_value!(x.amount);

                let enc = src.data_type.clone();

                match (&src.value, &amount.value) {
                    (DataValueType::Integer(a), DataValueType::Integer(b)) => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Integer(a << b),
                            })),
                        );
                    }
                    _ => panic!("Unexpected instruction type in left shift arithmetic"),
                }
            }
            InstructionType::RightShiftArithmetic(x) => {
                let src = get_ireg_value!(x.src);
                let amount = get_ireg_value!(x.amount);

                let enc = src.data_type.clone();

                match (&src.value, &amount.value) {
                    (DataValueType::Integer(a), DataValueType::Integer(b)) => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Integer(a >> b),
                            })),
                        );
                    }
                    _ => panic!("Unexpected instruction type in right shift arithmetic"),
                }
            }
            InstructionType::RightShiftLogical(x) => {
                let src = get_ireg_value!(x.src);
                let amount = get_ireg_value!(x.amount);

                let enc = src.data_type.clone();

                match (&src.value, &amount.value) {
                    (DataValueType::Integer(a), DataValueType::Integer(b)) => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Integer((*a as u64 >> b) as i64),
                            })),
                        );
                    }
                    _ => panic!("Unexpected instruction type in right shift logical"),
                }
            }
            InstructionType::BitwiseAnd(x) => {
                let a = get_ireg_value!(x.src_a);
                let b = get_ireg_value!(x.src_b);

                let enc = a.data_type.clone();

                match (&a.value, &b.value) {
                    (DataValueType::Integer(a), DataValueType::Integer(b)) => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Integer(a & b),
                            })),
                        );
                    }
                    (DataValueType::Bool(a), DataValueType::Bool(b)) => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Bool(a & b),
                            })),
                        );
                    }
                    _ => panic!("Unexpected instruction type in bitwise and"),
                }
            }
            InstructionType::BitwiseOr(x) => {
                let a = get_ireg_value!(x.src_a);
                let b = get_ireg_value!(x.src_b);

                let enc = a.data_type.clone();

                match (&a.value, &b.value) {
                    (DataValueType::Integer(a), DataValueType::Integer(b)) => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Integer(a | b),
                            })),
                        );
                    }
                    (DataValueType::Bool(a), DataValueType::Bool(b)) => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Bool(a | b),
                            })),
                        );
                    }
                    _ => panic!("Unexpected instruction type in add"),
                }
            }
            InstructionType::BitwiseXor(x) => {
                let a = get_ireg_value!(x.src_a);
                let b = get_ireg_value!(x.src_b);

                let enc = a.data_type.clone();

                match (&a.value, &b.value) {
                    (DataValueType::Integer(a), DataValueType::Integer(b)) => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Integer(a ^ b),
                            })),
                        );
                    }
                    (DataValueType::Bool(a), DataValueType::Bool(b)) => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Bool(a ^ b),
                            })),
                        );
                    }
                    _ => panic!("Unexpected instruction type in bitwise xor"),
                }
            }
            InstructionType::BitwiseNot(x) => {
                let a = get_ireg_value!(x.src);

                let enc = a.data_type.clone();

                match &a.value {
                    DataValueType::Integer(a) => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Integer(!a),
                            })),
                        );
                    }
                    DataValueType::Bool(a) => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Bool(!a),
                            })),
                        );
                    }
                    _ => panic!("Unexpected instruction type in bitwise not"),
                }
            }
            InstructionType::Equal(x) => {
                let a = get_ireg_value!(x.src_l);
                let b = get_ireg_value!(x.src_r);

                let enc = DataTypeEncoding {
                    type_id: BuiltinType::Boolean as usize,
                    dimension: 0,
                    mutability: Mutability::Mutable,
                };

                match (&a.value, &b.value) {
                    (DataValueType::Integer(a), DataValueType::Integer(b)) => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Bool(a == b),
                            })),
                        );
                    }
                    (DataValueType::Decimal(a), DataValueType::Decimal(b)) => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Bool(a == b),
                            })),
                        );
                    }
                    (DataValueType::String(a), DataValueType::String(b)) => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Bool(a.eq(b)),
                            })),
                        );
                    }
                    (DataValueType::Bool(a), DataValueType::Bool(b)) => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Bool(a == b),
                            })),
                        );
                    }
                    _ => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Bool(false),
                            })),
                        );
                    }
                }
            }
            InstructionType::NotEqual(x) => {
                let a = get_ireg_value!(x.src_l);
                let b = get_ireg_value!(x.src_r);

                let enc = DataTypeEncoding {
                    type_id: BuiltinType::Boolean as usize,
                    dimension: 0,
                    mutability: Mutability::Mutable,
                };

                match (&a.value, &b.value) {
                    (DataValueType::Integer(a), DataValueType::Integer(b)) => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Bool(a != b),
                            })),
                        );
                    }
                    (DataValueType::Decimal(a), DataValueType::Decimal(b)) => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Bool(a != b),
                            })),
                        );
                    }
                    (DataValueType::String(a), DataValueType::String(b)) => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Bool(!a.eq(b)),
                            })),
                        );
                    }
                    (DataValueType::Bool(a), DataValueType::Bool(b)) => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Bool(a != b),
                            })),
                        );
                    }
                    _ => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Bool(true),
                            })),
                        );
                    }
                }
            }
            InstructionType::LessThan(x) => {
                let a = get_ireg_value!(x.src_l);
                let b = get_ireg_value!(x.src_r);

                let enc = DataTypeEncoding {
                    type_id: BuiltinType::Boolean as usize,
                    dimension: 0,
                    mutability: Mutability::Mutable,
                };

                match (&a.value, &b.value) {
                    (DataValueType::Integer(a), DataValueType::Integer(b)) => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Bool(a < b),
                            })),
                        );
                    }
                    (DataValueType::Decimal(a), DataValueType::Decimal(b)) => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Bool(a < b),
                            })),
                        );
                    }
                    _ => panic!("Unexpected instruction type in less than"),
                }
            }
            InstructionType::LessThanOrEqual(x) => {
                let a = get_ireg_value!(x.src_l);
                let b = get_ireg_value!(x.src_r);

                let enc = DataTypeEncoding {
                    type_id: BuiltinType::Boolean as usize,
                    dimension: 0,
                    mutability: Mutability::Mutable,
                };

                match (&a.value, &b.value) {
                    (DataValueType::Integer(a), DataValueType::Integer(b)) => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Bool(a <= b),
                            })),
                        );
                    }
                    (DataValueType::Decimal(a), DataValueType::Decimal(b)) => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Bool(a <= b),
                            })),
                        );
                    }
                    _ => panic!("Unexpected instruction type in less than or equal"),
                }
            }
            InstructionType::GreaterThan(x) => {
                let a = get_ireg_value!(x.src_l);
                let b = get_ireg_value!(x.src_r);

                let enc = DataTypeEncoding {
                    type_id: BuiltinType::Boolean as usize,
                    dimension: 0,
                    mutability: Mutability::Mutable,
                };

                match (&a.value, &b.value) {
                    (DataValueType::Integer(a), DataValueType::Integer(b)) => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Bool(a > b),
                            })),
                        );
                    }
                    (DataValueType::Decimal(a), DataValueType::Decimal(b)) => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Bool(a > b),
                            })),
                        );
                    }
                    _ => panic!("Unexpected instruction type in greater than"),
                }
            }
            InstructionType::GreaterThanOrEqual(x) => {
                let a = get_ireg_value!(x.src_l);
                let b = get_ireg_value!(x.src_r);

                let enc = DataTypeEncoding {
                    type_id: BuiltinType::Boolean as usize,
                    dimension: 0,
                    mutability: Mutability::Mutable,
                };

                match (&a.value, &b.value) {
                    (DataValueType::Integer(a), DataValueType::Integer(b)) => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Bool(a >= b),
                            })),
                        );
                    }
                    (DataValueType::Decimal(a), DataValueType::Decimal(b)) => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Bool(a >= b),
                            })),
                        );
                    }
                    _ => panic!("Unexpected instruction type in greater than or equal"),
                }
            }
            InstructionType::Label(_) => {}
            InstructionType::UnconditionalJump(x) => {
                current_instruction_index = package
                    .instructions
                    .binary_search_by_key(&x.target, |instr| instr.offset as i64)
                    .expect("Jump target offset not found in instructions");
                continue;
            }
            InstructionType::ConditionalJump(x) => {
                let cond = get_ireg_value!(x.condition);

                if let DataValueType::Bool(b) = cond.value {
                    if (x.condition_true && b) || (x.condition_true && !b) {
                        current_instruction_index = package
                            .instructions
                            .binary_search_by_key(&x.target, |instr| instr.offset as i64)
                            .expect("Jump target offset not found in instructions");
                        continue;
                    }
                } else {
                    panic!("Unexpected instruction type in conditional jump");
                }
            }
            InstructionType::PrepareFunctionCall(x) => {
                fcp = FuncCallPrepProps::new(x.function_id);
            }
            InstructionType::Argument(x) => {
                fcp.ireg_ids.push(x.src);
            }
            InstructionType::Invoke(x) => {
                let func_symbol = package
                    .symbols
                    .get(&fcp.func_id)
                    .expect("Function symbol not found");
                if let Symbol::Function(func) = func_symbol {
                    let mut arg_iregs = BTreeMap::new();
                    for (i, &ireg_id) in fcp.ireg_ids.iter().enumerate() {
                        let arg_value = iregs.get(&ireg_id).expect("No register found").clone();
                        arg_iregs.insert(i as u16, arg_value);
                    }
                    match exec_func(package, func, fcp.func_id, &mut arg_iregs) {
                        Ok(ret) => match ret {
                            Some(ret_value) => {
                                iregs.insert(x.dest, ret_value);
                            }
                            None => {}
                        },
                        Err(err) => return Err(err),
                    }
                } else {
                    panic!("Symbol is not a function");
                }
            }
            InstructionType::ReturnWithValue(rv) => {
                let return_value = iregs.get(&rv.src).expect("No register found").clone();
                return Ok(Some(return_value));
            }
            InstructionType::ReturnWithoutValue(_) => {
                return Ok(None);
            }
            InstructionType::Load(x) => {
                let constant = package
                    .constant_pools
                    .get(func.constant_pool as usize)
                    .expect("Constant pool not found")
                    .get(&x.constant_index)
                    .expect("Constant not found");

                iregs.insert(x.dest, Rc::new(RefCell::new(constant.clone().into())));
            }
            InstructionType::Copy(x) => {
                let src_value = iregs.get(&x.src).expect("No register found").clone();
                iregs.insert(x.dest, src_value);
            }
            InstructionType::Clone(x) => {
                let src_value = get_ireg_value!(x.src);
                let cloned_value = Rc::new(RefCell::new(DataValue {
                    data_type: src_value.data_type.clone(),
                    value: src_value.value.clone(),
                }));
                iregs.insert(x.dest, cloned_value);
            }
            InstructionType::LoadReference(ldr) => {
                let src_value = iregs.get(&ldr.src).expect("No register found").clone();
                iregs.insert(ldr.dest, src_value);
            }
            InstructionType::InitializeObject(init_obj) => {}
            InstructionType::LogicalAnd(x) => {
                let a = get_ireg_value!(x.src_a);
                let b = get_ireg_value!(x.src_b);

                let enc = a.data_type.clone();

                match (&a.value, &b.value) {
                    (DataValueType::Bool(a), DataValueType::Bool(b)) => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Bool(*a && *b),
                            })),
                        );
                    }
                    _ => panic!("Unexpected instruction type in divide"),
                }
            }
            InstructionType::LogicalOr(x) => {
                let a = get_ireg_value!(x.src_a);
                let b = get_ireg_value!(x.src_b);

                let enc = a.data_type.clone();

                match (&a.value, &b.value) {
                    (DataValueType::Bool(a), DataValueType::Bool(b)) => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Bool(*a || *b),
                            })),
                        );
                    }
                    _ => panic!("Unexpected instruction type in divide"),
                }
            }
            InstructionType::LogicalXor(x) => {
                let a = get_ireg_value!(x.src_a);
                let b = get_ireg_value!(x.src_b);

                let enc = a.data_type.clone();

                match (&a.value, &b.value) {
                    (DataValueType::Bool(a), DataValueType::Bool(b)) => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Bool(*a != *b),
                            })),
                        );
                    }
                    _ => panic!("Unexpected instruction type in divide"),
                }
            }
            InstructionType::LogicalNot(x) => {
                let a = get_ireg_value!(x.src);
                let enc = a.data_type.clone();

                match &a.value {
                    DataValueType::Bool(a) => {
                        iregs.insert(
                            x.dest,
                            Rc::new(RefCell::new(DataValue {
                                data_type: enc,
                                value: DataValueType::Bool(!*a),
                            })),
                        );
                    }
                    _ => panic!("Unexpected instruction type in divide"),
                }
            }
            InstructionType::LoadField(x) => {
                let src_value = get_ireg_value!(x.src);
                let complex = if let DataValueType::Complex(obj) = &src_value.value {
                    obj
                } else {
                    panic!("Unexpected instruction type in load field");
                };

                let mut cell = complex
                    .values
                    .get(&(x.field_id as usize))
                    .expect("Field not found")
                    .clone();
                let value = cell.borrow_mut().clone();
                iregs.insert(x.dest, Rc::new(RefCell::new(value)));
            }
            InstructionType::StoreField(x) => {
                let src_value = get_ireg_value!(x.src);
                let complex = if let DataValueType::Complex(obj) = &src_value.value {
                    obj
                } else {
                    panic!("Unexpected instruction type in load field");
                };

                let cell = complex
                    .values
                    .get(&(x.field_id as usize))
                    .expect("Field not found");
                let value = iregs.get(&x.src).expect("No register found").clone();
                cell.swap(&value);
            }
        }

        current_instruction_index += 1;
    }
}
