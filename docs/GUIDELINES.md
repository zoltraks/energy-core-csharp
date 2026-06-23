# Energy Core Library - Code Generation Guidelines

## Project Overview

Energy.Core is a .NET class library providing type conversions, utilities, database abstraction, object mapping, and application framework functionality for wide-range deployment scenarios.

**IMPORTANT**: Before making any code changes, always read README.md in the project root and follow all guidelines and rules specified there unless specifically instructed to do something differently.

## Related Documents

This file is the central source of development rules. The following documents cover specific areas:

- `WORKFLOW.md`: development process and version-based implementation cycle.
- `VERSIONING.md`: version numbering and bump process (authoritative).
- `TESTING.md`: testing strategy and verification loop.
- `STRUCTURE.md`: project and solution layout.
- `COPYRIGHTS.md`: copyright and license requirements.
- `template/`: templates for change requests and implementation plans.

Before any code change, read README.md, this file, and STRUCTURE.md. For a non-trivial change, follow the version-based cycle in WORKFLOW.md: author a change request and an implementation plan before modifying source.

## Documentation Guidelines

These rules apply to the markdown documents in this `docs/` directory and to the project-energy technical documentation. They keep the documentation durable and plain-text friendly.

Prefer durable content over restating the code. Keep decisions and their rationale, constraints that are not inferable from the code, intended behaviour and invariants, and the rules that govern how to work on the project. Remove content that simply mirrors the source tree, such as exhaustive file lists or signatures that duplicate the code, because it goes stale the moment the code changes.

Write short sentences. Separate sentences or list groups with a blank line for readability in simple text viewers. Put exactly one blank line before and after a list. Avoid nested lists; keep a single level. Prefer plain ASCII characters and standard double quotes. Do not number section titles, so the structure stays easy to change.

When editing an existing, non-empty document, follow the style already used in that document rather than imposing these rules on it.

## Copyright and License

**ALL code changes must comply with the requirements documented in `COPYRIGHTS.md`.**

Before adding any code, verify it is original work or properly licensed and attributed. Third-party dependencies must be compatible with the project's LGPL-3.0 license.

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
- **Dispose streams using `Close()`**: Compact Framework builds in Visual Studio 2008 cannot access `Stream.Dispose(bool)`. Call `stream.Close()` (which invokes the public dispose path) unless a specific type offers its own public dispose method.
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
- When a multi-line XML comment (summary, param, or returns) needs `<br/><br/>`, place the tag on its own `///` line between the sentences, as in:
  ```
  /// <summary>
  /// First sentence.
  /// <br/><br/>
  /// Second sentence.
  /// </summary>
  ```
- Always follow the `<br/><br/>` rule for any multi-sentence XML documentation comment (including metadata helpers) so command-line help output and generated docs render consistently.
- Public APIs in `Energy.Core` are meant for consumption by other projects; avoid wording summaries as if a class or method serves only this project (e.g., prefer "File system related helper functions" instead of "for Energy.Core utilities").
- **Markdown formatting**: Always include exactly one blank line after section headings (###, ##, etc.) to improve readability in plain text viewers. Do not add multiple blank lines between sections. Also include a blank line before code blocks (``` or ```csharp) for proper spacing.
- **XML comments**: In `<summary>` blocks, omit the trailing period for single-sentence descriptions. For multiple sentences, end each sentence with a period and separate them using `<br/><br/>`.
- **XML `<param>`/`<returns>` formatting**: If the description is a single sentence, keep it on one line between the tags **without a trailing period** (e.g., `/// <param name="value">Color string to parse</param>`). When the description needs multiple sentences, expand it so the sentences live on separate `///` lines (no leading spaces before the text), insert `/// <br/><br/>` between them, and keep the entire block between the opening and closing tag, matching the pattern shown in the specification.
- **Region naming**: When grouping a method inside `#region`/`#endregion`, name the region exactly after the method (for example, `#region IsHex` instead of `#region Hex`) so that the region name always matches the method it contains.

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

## Testing

See TESTING.md for the full testing strategy. Key rules:

- Every public method must have unit tests covering edge cases and error conditions.
- After any code change, run `dotnet test Energy.Core.Test/Energy.Core.Test.csproj`; all tests must pass before the change is complete.
- Test class and method names mirror the production type and method they exercise.
- Where the library implements an external format, verify output byte-for-byte against the reference tool using committed fixtures.

## Build Configuration

- **Debug**: Full debugging info, no optimization
- **Release**: Optimized, documentation generation
- **Platform-specific builds** for each target framework
- **Conditional symbols** for feature detection

## Version Management

See VERSIONING.md for the authoritative version numbering rules, bump process, and NuGet packaging steps.

In short: the format is YY.M.RR; the `.csproj` files are the source of truth for the current version; update `<Version>`, `<FileVersion>`, and `<AssemblyVersion>` together to the same value.

## Dependencies

- **Minimal external dependencies** for maximum compatibility
- **NuGet packages** only when absolutely necessary
- **Version-specific packages** for different target frameworks
- **Conditional compilation** for optional features
