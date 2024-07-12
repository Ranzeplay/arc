# Commit standard

## Commit message

### Format

```
[flags] <type> (<scope>) : <description>

[optional body]

[commands to automatic tools]
```

Description should be short and accurate,
if you have more to say, put them to the body.

#### Types

- `proj`: Indicates that the commit is crucial to the whole project, such as changing license.
- `feat`: Adding new features to the project.
- `fix`: Fix an existing issue.
- `chore`: Routine/automatic operations.
- `refactor`: Refactor an existing function for better using.
- `test`: Add/remove/modify rests.
- `docs`: Add/remove/modify documents.
- `ci` : Commits add by CI tools.
- `revert`: Revert to a previous commit
- `style`: Adjust coding style, doesn't affect any functions.

#### Flags

Add these flags before commit types.

- `[bc]`: Breaking changes.
- `[wip]`: Working in progress.

### Examples

```
proj (root) : Add LICENSE and COMMIT_STANDARD.md

```

```
fix (compiler/code_generator) : Fix array out-of-range issue.

Fixed issues: #132 #190
```
