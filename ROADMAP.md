# Roadmap

This roadmap outlines the development plan for the Arc programming language, including key milestones and language features.

## Work in Progress

- IDE & editor integration (Visual Studio Code)
  - Syntax highlighting
  - Code completion
  - Debugging support
- Standard library
  - Elementary data structures (lists, maps, sets, etc.)
- Group inheritence and implementation
- Primitive types
  - Byte

## Reviewing

- Compiler type checking

## Planned

### Core Language Features

- First-class function
  - Lambda expression
  - Function type
- Foreign Function Interface (FFI)
- Module system
- Standard library
  - Operating system interfaces
  - Networking
  - File system operations
  - String manipulation
  - Algorithms (sorting, searching, etc.)
  - Concurrency primitives
- Concurrency model
- Generics constraints
- Type inference
- Exception handling
- Syntactic sugar
  - Guard statement
  - Assignment on declaration

### Maintenance

- Set up CI/CD pipeline
- Code formatting and linting tools
- Version control best practices
- Documentation standards
- Code review process
- Renew documentation website: detach from fumadocs

### Ecosystem Development

- Package management system
- Debugging tools
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
- Enum support
- Generics support
- Reference/value data
- Code generator error handling

## Long-term Goals

- Cross-platform support
- Compiler bootstrapping (self-hosting)
- Performance benchmarking and optimization
- Optimize grammar and syntax for better coding experience
- Domain-specific features for target use cases
- REPL (Read-Eval-Print Loop) support via interpreter

This roadmap is subject to change as the project evolves and priorities shift.
