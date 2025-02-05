use std::fmt::Debug;
use std::rc::Rc;
use crate::models::descriptors::symbol::GroupSymbol;

pub struct GroupDetailViewModel {
    pub group: Rc<GroupSymbol>
}

impl Debug for GroupDetailViewModel {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        writeln!(f, "{}:", self.group.signature)?;
        writeln!(f, "= Fields:")?;
        for field_id in &self.group.field_ids {
            writeln!(f, "  0x{:016X}", field_id)?;
        }

        writeln!(f, "= Functions:")?;
        for function_id in &self.group.function_ids {
            writeln!(f, "  0x{:016X}", function_id)?;
        }

        writeln!(f, "= Constructors:")?;
        for constructor_id in &self.group.constructor_ids {
            writeln!(f, "  0x{:016X}", constructor_id)?;
        }

        writeln!(f, "= Destructors:")?;
        for destructor_id in &self.group.destructor_ids {
            writeln!(f, "  0x{:016X}", destructor_id)?;
        }

        writeln!(f, "= Subgroups:")?;
        for subgroup_id in &self.group.sub_group_ids {
            writeln!(f, "  {:016X}", subgroup_id)?;
        }

        Ok(())
    }
}

impl GroupDetailViewModel {
    pub fn new(group: Rc<GroupSymbol>) -> GroupDetailViewModel {
        GroupDetailViewModel {
            group
        }
    }
}

pub struct GroupListViewModel {
    pub groups: Vec<GroupDetailViewModel>
}

impl Debug for GroupListViewModel {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        writeln!(f, "=== Group list")?;
        for group in &self.groups {
            writeln!(f, "{:?}", group)?;
        }

        Ok(())
    }
}
