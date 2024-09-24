---
title: Instruction set
---

# Instruction set

| Opcode | Mnemonic | Description                                                               |
| ------ | -------- | ------------------------------------------------------------------------- |
| 0x01   | decl     | Declare data                                                              |
| 0x02   | pushi    | Push instant value to stack                                               |
| 0x03   | pushc    | Push constant value to stack                                              |
| 0x04   | pushs    | Push value from data slot to stack                                        |
| 0x05   | popd     | Discard stack top                                                         |
| 0x06   | pops     | Pop stack top to data slot                                                |
| 0x07   | add      | Add 2 values on top of the stack                                          |
| 0x08   | sub      | Subtract 2 values on top of the stack                                     |
| 0x09   | mul      | Multiply 2 values on top of the stack                                     |
| 0x0a   | div      | Divide 2 values on top of the stack                                       |
| 0x0b   | mod      | Mod 2 values on top of the stack                                          |
| 0x0c   | logor    | Perform logical OR operation for 2 values on top of the stack             |
| 0x0d   | logand   | Perform logical AND operation for 2 values on top of the stack            |
| 0x0e   | lognot   | Perform logical NOT operation for the value on top of the stack           |
| 0x0f   | bitand   | Perform bitwise AND operation for 2 values on top of the stack            |
| 0x10   | bitor    | Perform bitwise OR operation for 2 values on top of the stack             |
| 0x11   | bitnot   | Perform bitwise NOT operation for the value on top of the stack           |
| 0x12   | inv      | Perform inverse operation for the value on top of the stack               |
| 0x13   | eqc      | Compare if the top 2 data on the stack have the same content              |
| 0x14   | eqr      | Compare if two objects on the top of the stack have the same reference    |
| 0x15   | clg      | Check if the top of the stack is more than the second top one             |
| 0x16   | clge     | Check if the top of the stack is more than or equal to the second top one |
| 0x17   | cls      | Check if the top of the stack is less than the second top one             |
| 0x18   | clse     | Check if the top of the stack is less than or equal to the second top one |
| 0x19   | invoke   | Invoke a function                                                         |
| 0x1a   | ret      | Return from a function                                                    |
| 0x1b   | throw    | Throw an exception                                                        |
| 0x1c   | btc      | Begin try-catch routine                                                   |
| 0x1d   | bt       | Begin try part                                                            |
| 0x1e   | bc       | Begin catch part                                                          |
| 0x1f   | bf       | Begin finally part                                                        |
| 0x20   | etc      | End try-catch routine                                                     |
| 0x21   | jmp      | Unconditional jump to relative location                                   |
| 0x22   | jmpc     | Conditional jump to relative location                                     |
| 0x23   | gtype    | Get object type on the top of the stack                                   |
| 0x24   | wall     | Wait for all async functions to complete                                  |
| 0x25   | tevt     | Trigger an event mentioned on the top of that stack                       |
| 0x26   | wevt     | Wait for the event mentioned on top of the stack                          |
| 0x27   | crt      | Call a runtime service                                                    |
| 0x28   | cir      | Create an isolated region                                                 |
| 0x29   | dir      | Destroy the isolated region                                               |
| 0x2a   | cln      | Clone the object on top of the stack                                      |
| 0x2b   | termp    | Terminate the whole program with exit code                                |
| 0x2c   | termef   | Terminate the current execution flow with exit code                       |
| 0x2d   | sef      | Suspend current execution flow                                            |
| 0x2e   | pefid    | Push execution flow ID on top of the stack                                |
| 0x2f   | cefid    | Continue execution flow by ID on the top of the stack                     |
| 0x30   | ctype    | Cast the object on top of the stack to another type                       |
