---
title: Instruction set
---

# Instruction set

| Opcode | Mnemonic | Description                                                            |
| ------ | -------- | ---------------------------------------------------------------------- |
|        | decl     | Declare data                                                           |
|        | pushi    | Push instant value to stack                                            |
|        | pushc    | Push constant value to stack                                           |
|        | pushs    | Push value from data slot to stack                                     |
|        | popd     | Discard stack top                                                      |
|        | pops     | Pop stack top to data slot                                             |
|        | add      | Add 2 values on top of the stack                                       |
|        | sub      | Subtract 2 values on top of the stack                                  |
|        | mul      | Multiply 2 values on top of the stack                                  |
|        | div      | Divide 2 values on top of the stack                                    |
|        | mod      | Mod 2 values on top of the stack                                       |
|        | logor    | Perform logical OR operation for 2 values on top of the stack          |
|        | logand   | Perform logical AND operation for 2 values on top of the stack         |
|        | lognot   | Perform logical NOT operation for the value on top of the stack        |
|        | bitand   | Perform bitwise AND operation for 2 values on top of the stack         |
|        | bitor    | Perform bitwise OR operation for 2 values on top of the stack          |
|        | bitnot   | Perform bitwise NOT operation for the value on top of the stack        |
|        | inv      | Perform inverse operation for the value on top of the stack            |
|        | eqc      | Compare if the top 2 data on the stack have the same content           |
|        | eqr      | Compare if two objects on the top of the stack have the same reference |
|        | clg      | Check if the top of the stack is more than the second top one          |
|        | clge     | Check if the top of the stack is more or equal than the second top one |
|        | cls      | Check if the top of the stack is less than the second top one          |
|        | clse     | Check if the top of the stack is less or equal than the second top one |
|        | invoke   | Invoke a function                                                      |
|        | ret      | Return from a function                                                 |
|        | throw    | Throw an exception                                                     |
|        | btc      | Begin try-catch routine                                                |
|        | bt       | Begin try part                                                         |
|        | bc       | Begin catch part                                                       |
|        | bf       | Begin finally part                                                     |
|        | etc      | End try-catch routine                                                  |
|        | jmp      | Unconditional jump to relative location                                |
|        | jmpc     | Conditional jump to relative location                                  |
|        | gtype    | Get object type on the top of the stack                                |
|        | wall     | Wait for all async functions to complete                               |
|        | tevt     | Trigger an event mentioned on the top of that stack                    |
|        | wevt     | Wait for the event mentioned on top of the stack                       |
|        | crt      | Call a runtime service                                                 |
|        | cir      | Create an isolated region                                              |
|        | dir      | Destroy the isolated region                                            |
|        | cln      | Clone the object on top of the stack                                   |
|        | termp    | Terminate the whole program with exit code                             |
|        | termef   | Terminate the current execution flow with exit code                    |
|        | sef      | Suspend current execution flow                                         |
|        | pefid    | Push execution flow ID on top of the stack                             |
|        | cefid    | Continue execution flow by ID on the top of the stack                  |
|        | ctype    | Cast the object on top of the stack to another type                    |
