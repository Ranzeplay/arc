---
title: On-stack data save-load operations
---

# On-stack data save-load operations

## Instruction format

```
<prefix> <source> <loc_id> <field_id> <args>
```

- `<prefix>`: Instruction prefix or identifier
- `<source>`: Specify the source of data
	- `0x01`: From constant table
	- `0x02`: From accessible data slot
	- `0x03`: Data handle on the top of the stack
- `<args>`: Additional data may need to be provided

### Arguments of loading data instruction

#### From constant table

The `<loc_id>` will be the *TableEntryId*.

The `<field_id>` indicates the *FieldId* in the symbol table. For base types, thay are always `0x00`.

#### From accessible data slot

The `<loc_id>` will be the *SlotId*.

The `<field_id>` indicates the *FieldId* in the symbol table. For base types, thay are always `0x00`.

## Load data

The command prefix will be `0x37`.

### Field already written in symbol table

Only `loc_id` and `field_id` should be filled.

### Field not written in the symbol table

You need to get the data handle before loading data. Additional things need to be provided in `args`.

## Save data

The command prefix will be `0x38`.

Data will be moved from the top of the stack to target location.

### Field already written in symbol table

Only `loc_id` and `field_id` should be filled.

### Field not written in the symbol table

You need to get the data handle before loading data. Additional things need to be provided in `args`.
