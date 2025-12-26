# Energy Core Library - Code Generation Guidelines

## Project Overview
Energy.Core is a .NET class library providing type conversions, utilities, database abstraction, object mapping, and application framework functionality for wide-range deployment scenarios.

## Target Frameworks
- **.NET Standard 2.0** - Modern cross-platform support
- **.NET Framework 4.0** - Windows desktop applications
- **.NET Framework 3.5** - Legacy Windows support
- **.NET Framework 2.0** - Maximum compatibility
- **Compact Framework 3.5** - Windows CE/Mobile devices

## Compilation Requirements

### Visual Studio Compatibility
- **Visual Studio 2008** - Required for Compact Framework support
- **Visual Studio 2017+** - Modern development and .NET Standard
- **Visual Studio 2022** - Latest features and performance

### AOT Compilation Support
- Code must be **Ahead-of-Time (AOT) compatible**
- Avoid runtime code generation and reflection-heavy patterns
- Use generic constraints instead of reflection where possible
- Minimize dynamic type usage and late binding

## Code Structure

### Namespace Organization
```
Energy.Base        - Base utility classes and core functionality
Energy.Core        - Main library functions and application framework
Energy.Attribute   - Custom attributes for metadata
Energy.Enumeration - Enumerations and constants
Energy.Interface   - Interface definitions
Energy.Source      - Database connection and data access
Energy.Query      - SQL query building and dialects
```

### Conditional Compilation Directives
```csharp
#if NETSTANDARD     // .NET Standard 2.0 specific code
#elif NET40         // .NET Framework 4.0 specific code
#elif NET35         // .NET Framework 3.5 specific code
#elif NET20         // .NET Framework 2.0 specific code
#elif NETCF         // Compact Framework specific code
#endif
```

## Coding Standards

### General Principles
- **AOT-friendly** - Avoid runtime code generation
- **Cross-platform** - Windows, Linux, macOS compatibility
- **Backward compatible** - Support .NET 2.0 APIs
- **Memory efficient** - Minimal allocations, value types where appropriate
- **Thread-safe** - Public APIs must be thread-safe

### Type Safety
- Use strongly-typed collections over ArrayList
- Prefer generic methods with constraints
- Implement explicit interface conversions when needed
- Use nullable types appropriately for value types

### Performance Considerations
- Minimize boxing/unboxing operations
- Use StringBuilder for string concatenation
- Cache frequently used objects and delegates
- Avoid reflection in hot paths

### Compact Framework Limitations
- No LINQ (use manual iteration)
- Limited generic support
- No Expression Trees
- Reduced BCL availability
- Memory constraints (64MB typical)

## API Design

### Method Naming
- Use **PascalCase** for public methods
- Use **camelCase** for private methods
- Prefix boolean methods with **Is/Has/Can**
- Use descriptive names avoiding abbreviations

### Error Handling
- Return default values for conversion failures
- Use Energy.Base.Cast for safe type conversions
- Implement graceful degradation for unsupported platforms
- Provide clear exception messages for developer errors

### Documentation
- XML documentation comments required for public APIs
- Include parameter descriptions and return value explanations
- Add examples for complex functionality
- Document platform-specific limitations

## Platform-Specific Guidelines

### Compact Framework
```csharp
#if NETCF
    // Compact Framework specific implementation
    // Use manual loops instead of LINQ
    // Limited API availability
    // Memory-conscious design
#else
    // Full framework implementation
    // Can use modern APIs
#endif
```

### AOT Compilation
- Avoid `System.Reflection.Emit`
- Limit dynamic assembly loading
- Use compiled delegates over reflection
- Pre-generate serialization code if needed

## Testing Requirements
- Unit tests for all public APIs
- Integration tests for database functionality
- Platform-specific test suites
- Performance benchmarks for critical paths

## Build Configuration
- **Debug**: Full debugging info, no optimization
- **Release**: Optimized, documentation generation
- **Platform-specific builds** for each target framework
- **Conditional symbols** for feature detection

## Dependencies
- **Minimal external dependencies** for maximum compatibility
- **NuGet packages** only when absolutely necessary
- **Version-specific packages** for different target frameworks
- **Conditional compilation** for optional features
