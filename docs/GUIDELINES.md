# Energy Core Library - Code Generation Guidelines

## Project Overview

Energy.Core is a .NET class library providing type conversions, utilities, database abstraction, object mapping, and application framework functionality for wide-range deployment scenarios.

**IMPORTANT**: Before making any code changes, always read README.md in the project root and follow all guidelines and rules specified there unless specifically instructed to do something differently.

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
- **Visual Studio 2019** - Required for Energy.Core.Test project compatibility
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
- **Fully qualified references** - When referencing classes or static members declared in another type, use their full namespace (for example `Energy.Base.Binary.BitReader` instead of relying on `using` aliases). This keeps intent explicit and avoids namespace conflicts across multi-targeted builds.

### .NET Compatibility

- **Avoid .NET 4.0+ features** in main library code for .NET 2.0/3.5/Compact Framework compatibility
- **Use compatible alternatives**: Replace `string.IsNullOrWhiteSpace()` with `string == null || string.Trim() == ""`
- **No `string.IsNullOrWhiteSpace` usage**: The method is unavailable on .NET 2.0/3.5/CF, so never call it in production code—always expand to explicit null/trim checks as shown above.
- **Avoid auto-property initializers**: Syntax such as `public bool Enabled { get; set; } = true;` is not supported on .NET 2.0/3.5/CF. Assign defaults via constructors or explicit initialization logic instead.
- **Avoid the `nameof` operator**: Older compilers do not support `nameof(...)`. Use literal parameter names in exception constructors and logging.
- **Modern features allowed in test projects**: Test projects can use newer .NET features
- **Conditional compilation**: Use `#if` directives when necessary for platform-specific code
- **No verbatim strings with "$"**: Avoid `$@"..."` interpolated verbatim strings - use string.Format() or regular concatenation instead
- **No string.Contains()**: Replace with `string.IndexOf() != -1` for .NET 2.0/3.5 compatibility
- **No local functions**: Avoid declaring functions inside other methods - use private methods instead

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
- When a `<summary>` comment needs multiple sentences, separate each sentence with `<br/><br/>` (e.g., `First sentence.<br/><br/>Second sentence.`) to keep formatting consistent across viewers.
- **Markdown formatting**: Always include exactly one blank line after section headings (###, ##, etc.) to improve readability in plain text viewers. Do not add multiple blank lines between sections. Also include a blank line before code blocks (``` or ```csharp) for proper spacing.
- **XML comments**: In `<summary>` blocks, omit the trailing period for single-sentence descriptions. For multiple sentences, end each sentence with a period and separate them using `<br/><br/>`.

### Method Organization

- **Use #region/#endregion** to group related methods
- **Group overloaded methods** in the same region with the function name as region name
- **Region naming**: Use the function/method name as the region name (e.g., "#region HslToColor")
- **Logical grouping**: Organize regions by functionality, not just by accessibility
- **Consistent ordering**: Place regions in a logical order within the class

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

### Test Project Configuration

- **Energy.Core.Test** project requires **Visual Studio 2019** minimum
- Uses MSTest framework with modern SDK-style project format
- Targets .NET Framework 4.8 for maximum compatibility
- All 263 tests must pass successfully before release

### Unit Test Maintenance

**After any code change or addition, unit tests must be updated or created:**

- **New functionality**: Create comprehensive unit tests covering all public methods, edge cases, and error conditions
- **Bug fixes**: Add regression tests to prevent the bug from reoccurring
- **API changes**: Update existing tests to reflect new method signatures, parameters, or return values
- **Refactoring**: Ensure all existing tests still pass and cover the refactored code paths
- **Platform-specific code**: Add tests for each target framework where behavior differs
- **Performance changes**: Update benchmark tests to validate performance improvements or regressions

**Test Execution Requirements:**
- **After each code change**: Run all unit tests immediately to verify no regressions
- **Fix all failing tests**: No code change is complete until all tests pass
- **Test validation**: Ensure new functionality works as expected before committing
- **Continuous testing**: Run tests frequently during development to catch issues early

**Test Coverage Requirements:**
- All public methods must have corresponding unit tests
- Test coverage should remain at or above current levels
- Critical paths and error handling must be thoroughly tested
- Tests must be maintainable and clearly document the expected behavior

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
