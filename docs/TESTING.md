# Testing Strategy

This document describes how Energy.Core is tested. GUIDELINES.md points here.

## Test Project

The unit tests live in `Energy.Core.Test`, an MSTest project.

- It uses the modern SDK-style project format and targets a current .NET Framework for tooling support.
- It is built and run with the standard .NET CLI:

```
dotnet test Energy.Core.Test/Energy.Core.Test.csproj
```

The test project may use modern .NET features, because it is never shipped to the legacy or Compact Framework runtimes. Only the library itself is constrained to old runtimes. See GUIDELINES.md for the library compatibility rules.

## Test Maintenance

After any code change or addition, the unit tests must be updated or created:

- New functionality requires tests covering public methods, edge cases, and error conditions.
- Bug fixes require a regression test that fails before the fix and passes after.
- API changes require updating existing tests to the new signatures and behaviour.
- Refactoring requires that all existing tests still pass and continue to cover the refactored paths.

No code change is complete until all tests pass.

## Test Naming Alignment

Test classes and methods must mirror the structure and naming of the components they exercise.

Match namespaces and keep names aligned with the production type and method so intent is obvious.

For example:

- The test class for `Energy.Base.Compression` lives in the `Energy.Core.Test.Base` namespace.
- A test for the `ZX0.Compress` behaviour is named so the production member it covers is obvious, for example `Compression_ZX0_Big16KFixture`.

## Test Grouping

Group related cases for the same function into a single test method with multiple cases, rather than one method per input.

This improves readability, reduces duplication, and makes the relationship between inputs and expected outputs clear.

## Assertions

Avoid wrapping assertions in `try/catch` blocks that convert failures into inconclusive or passing results. A real failure must surface as a failed test.

Prefer collection assertions such as `CollectionAssert.AreEqual` when comparing byte arrays, so a mismatch reports the differing position.

## Compatibility-Sensitive Code

When behaviour differs across target frameworks, add or adapt tests to cover each path that the library compiles differently with `#if` directives.

## Reference Compatibility

Where the library implements an external format, verify the output byte-for-byte against the reference tool, using committed fixtures.

For example, the ZX0 compressor output is checked against fixtures produced by the reference `zx0` tool, and decompression is checked against fixtures a reference `dzx0` tool can decode. See WORKFLOW.md for how such fixtures are produced and embedded.
