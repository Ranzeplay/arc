---
title: On-stack data save-load operations
---

# On-stack data save-load operations

## Instruction format

```
<prefix> <source> <loc_id> <field_id>
```

- `<prefix>`: Instruction prefix or identifier
- `<source>`: Specify the source of data
	- `0x01`: From constant table
	- `0x02`: From accessible data slot
	- `0x03`: Data handle on the top of the stack
- `arg`: See below

### Arguments of loading data instruction

#### From constant table

The `<loc_id>` will be the *TableEntryId*.

The `<field_id>` indicates the *FieldId* in the symbol table. For base types, thay are always `0x00`.

#### From accessible data slot

The `<loc_id>` will be the *SlotId*.

The `<field_id>` indicates the *FieldId* in the symbol table. For base types, thay are always `0x00`.

## Load data

### Field already written in symbol table

The command prefix will be `PLACEHOLDER`.

### Field not written in the symbol table

You need to get the data handle before loading data.

## Save data

Data will be moved from the top of the stack to target location.

### Field already written in symbol table

The command prefix will be `PLACEHOLDER`.

### Field not written in the symbol table

You need to get the data handle before saving data.
