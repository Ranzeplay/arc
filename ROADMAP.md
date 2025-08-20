# Roadmap

This roadmap outlines the development plan for the Arc programming language, including key milestones and language features.

## Work in Progress

- Enum support
- Generics support
- Reference/value data
- Code generator error handling

## Reviewing

- Compiler type checking

## Planned

### Core Language Features

- Foreign Function Interface (FFI)
- Module system
- Standard library design
  - Operating system interfaces
  - Networking
  - File system operations
  - String manipulation
  - Elementary data structures (lists, maps, sets, etc.)
  - Algorithms (sorting, searching, etc.)
  - Concurrency primitives
- Concurrency model
- Generics constraints
- Type inference
- Error handling
- Primitive types
  - Byte
- Syntactic sugar
  - Guard statement
  - Assignment on declaration

### Maintenance

- Set up CI/CD pipeline
- Code formatting and linting tools
- Version control best practices
- Documentation standards
- Code review process

### Ecosystem Development

- Package management system
- Debugging tools
- IDE integration
  - Syntax highlighting
  - Code completion
  - Debugging support
- Documentation generation tools
- Community building
  - Website and forums
  - Example projects
  - Tutorials and guides
  - Package registry

## Finished & Inactive

- Grammar and syntax design
- Initial lexer implementation
  - Tokenization
  - Early error detection
- Basic parser functionality
  - Abstract Syntax Tree (AST) representation
  - Parsing error handling
- Basic package generator
  - AST to instruction translation
  - Scope tree construction
  - Symbol table management
  - Constant pool management
  - Package output generation
- Instruction set design
- Initial code generation backend
- Documentation website setup
- Primitive type system design
- Group type support
- Basic memory management

## Long-term Goals

- Cross-platform support
- Compiler bootstrapping (self-hosting)
- Performance benchmarking and optimization
- Optimize grammar and syntax for better coding experience
- Domain-specific features for target use cases
- REPL (Read-Eval-Print Loop) support via interpreter

This roadmap is subject to change as the project evolves and priorities shift.
