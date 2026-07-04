# Dead-code removal and test reliability hardening

**Type:** Refactor

## Summary

Remove dead code, unreachable code, and redundant checks from the Energy.Core library, dispose local disposable resources, and harden the Energy.Core.Test unit tests against timing, environment, and external-network dependencies. The work is behaviour-preserving and does not change the public API.

## Description

The Energy.Core library and Energy.Core.Test project still contain several maintainability issues after the 26.6.0 library-wide cleanup. The most important ones are:

- Dead and commented-out code in `Core/Network.cs`, `Core/Bug.cs`, `Base/Path.cs`, and `Base/Money.cs`.
- Local `MemoryStream` instances in `Base/Url.cs` and `Base/Class.cs` that are created but not disposed.
- Redundant inner null checks in the `Base/Binary.Compare` overloads.
- Undocumented empty `catch` blocks in `Core/Tilde.cs`.
- Unreliable tests that depend on `DateTime.Now`, `Thread.Sleep`, an external HTTP service, or process environment variables.
- A test method with no assertions, a test namespace mismatch, commented-out tests, and a tautological assertion.

This change removes or hardens all of the above. It does not split any files, does not change the Compact Framework project files, and does not convert public mutable fields to properties. Those conversions are explicitly deferred because they would break the public API.

Two type-conversion defects in `Base/Variant.cs` were discovered during the analysis. They are not included here because they change observable behaviour; they will be handled as separate change requests.

## Compatibility

All library changes compile on every supported target framework:

- .NET Standard 2.0
- .NET Framework 4.0
- .NET Framework 3.5
- .NET Framework 2.0
- Compact Framework 3.5

No new APIs or language features are introduced. The `MemoryStream` disposal changes are inside existing `#if !NETCF` guards, so the Compact Framework build is unaffected. The public API surface is unchanged.

## Use Cases

- Developers reviewing `Base/Money.cs` no longer scroll through 170 lines of commented-out currency definitions.
- The test suite passes reliably on CI machines that have no external internet access or variable load.
- Static analysis and future warning-as-error builds are not tripped by unreachable code or undisposed local disposables.

## Hints

- Keep each tidy step separate so a regression is attributable to one step.
- Run the build and test loop after every step.
- Do not change the public API or the Compact Framework project files.

## Out of Scope

- Converting public mutable fields to properties. That is a breaking API change and is deferred to a deliberate major-version release.
- Splitting `Base/Cast.cs` or `Base/Text.cs` into partial-class files. That was deferred in 26.6.0 because it requires updating the Compact Framework project files.
- The `Base/Variant.cs` `UInt32` and `Float` conversion defects. They are behaviour changes and must be handled separately.
