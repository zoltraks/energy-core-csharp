# Dead-Code Removal and Test Reliability Hardening Refactoring Assessment

## Proposal Summary

The proposal at `docs/refactoring/26.6.1/dead-code-and-test-hardening-proposal.md` targeted behaviour-preserving cleanup in the Energy.Core library and test reliability hardening in Energy.Core.Test. The planned work included removing dead code, disposing local `MemoryStream` instances, simplifying redundant null checks, documenting empty catch blocks, and replacing time-dependent, environment-dependent, and external-network tests with deterministic equivalents. The two `Variant.Union` type-conversion defects were explicitly excluded as behaviour-changing fixes requiring separate change requests.

## Implementation Summary

All steps in the implementation plan were completed.

- Tidy Step 1: Removed the unreachable `throw` in `Core/Network.cs`, the commented-out `Code` struct in `Core/Bug.cs`, and the commented-out `Split` overload in `Base/Path.cs`.
- Tidy Step 2: Removed approximately 170 lines of commented-out ISO 4217 currency definitions in `Base/Money.cs`.
- Tidy Step 3: Wrapped the local `MemoryStream` in `Base/Url.Decode` and `Base/Class.GetObjectSize` in `using` statements.
- Tidy Step 4: Simplified the redundant inner `left == null` check in all five `Base/Binary.Compare` overloads.
- Tidy Step 5: Added explanatory comments inside the intentionally empty catch blocks in `Core/Tilde.cs`.
- Structural Step 6: Hardened the test project:
  - Removed wall-clock timing assertions in `Energy.Core.Test/Base/Queue.cs`.
  - Replaced the fixed `Thread.Sleep(10)` in `Energy.Core.Test/Base/File.cs` with a polling retry loop that waits for the write stamp to change.
  - Wrapped the environment variable mutation in `Energy.Core.Test/Base/Path.cs` in `try`/`finally`.
  - Removed the commented-out tests in `Energy.Core.Test/Base/Path.cs`.
  - Replaced the external `https://reqres.in` dependency in `Energy.Core.Test/Core/Web.cs` with a local `HttpListener` on a dynamically chosen port.
  - Completed the `ClassAssembly` test in `Energy.Core.Test/Base/Class.cs` with a meaningful assertion.
  - Fixed the namespace and removed boilerplate in `Energy.Core.Test/Base/Http.cs`.
  - Replaced the tautological assertion in `Energy.Core.Test/Core/Tilde.cs` with a consistency check.

## Gap Analysis

No deviations from the proposal were introduced. The only implementation detail that required iteration was the `File.State` polling loop. The initial version set the write stamp once and then polled `IsChanged`, which failed because the file-system timestamp resolution can cause `SetWriteStamp` to produce the same timestamp if called immediately after creation. The loop was corrected to sleep before each `SetWriteStamp` attempt and retry until the stamp changes, matching the original test intent without the fixed 10 ms sleep.

The public-field-to-property conversion and the `Base/Variant.cs` defects were correctly left out of scope.

## Verification Results

- Build, `dotnet build Energy.Core/Energy.Core.csproj -c Release`, all four targets `netstandard2.0`, `net40`, `net35`, `net20`: pass, zero errors, zero warnings.
- Test, `dotnet test Energy.Core.Test/Energy.Core.Test.csproj` on net48: 341 passed, 0 failed, 0 skipped, 341 total.
- Compact Framework build: not verified. The toolchain is not available in this environment. No Compact-only code path was modified; the `MemoryStream` disposal changes are inside existing `#if !NETCF` guards.

## Metrics Comparison

- Commented-out dead code lines removed: approximately 180 across `Base/Money.cs`, `Core/Bug.cs`, and `Base/Path.cs`.
- Unreachable code statements removed: 1 in `Core/Network.cs`.
- Local `MemoryStream` instances now deterministically disposed: 2 (`Base/Url.Decode`, `Base/Class.GetObjectSize`).
- Redundant null-check branches removed: 5 in `Base/Binary.Compare`.
- Empty catch blocks documented: 8 in `Core/Tilde.cs`.
- Time-dependent test assertions removed: 3 in `Energy.Core.Test/Base/Queue.cs`.
- External network dependency removed: 1 in `Energy.Core.Test/Core/Web.cs`.
- Environment variable side effect now isolated: 1 in `Energy.Core.Test/Base/Path.cs`.
- Total diff: 15 files changed, 168 insertions, 447 deletions.

## Output Fidelity

No produced output changed. The LZSS, ZX0, and all other compression fixtures still decode and encode byte-for-byte, confirmed by the unchanged pass state of every fixture test. The `Url.Decode` method produces the same decoded string because the `using` statement only affects disposal; the `MemoryStream` read path is unchanged. The `Binary.Compare` simplification removes only a provably unreachable branch, so all comparison results are identical. The `Money.Manager` live `readonly` currency definitions are untouched.

## Post-Assessment Note

The two `Base/Variant.cs` type-conversion defects that were excluded from this refactoring as behaviour-changing fixes have been addressed separately under:

- `docs/change/26.6.1/fix-variant-conversion-bugs.md`
- `docs/plan/26.6.1/fix-variant-conversion-bugs-implementation.md`

The `UInt32` and `Float` getters were corrected, and regression tests were added. The full test suite now passes with 342 tests.

## Conclusion

Pass. The implemented changes are behaviour-preserving, the public API is unchanged, and every buildable target compiles with zero warnings and zero errors. The full test suite passes, including the hardened Web test that no longer depends on an external service. The Compact Framework build is not expected to change because the modified paths are either shared logic or guarded by `#if !NETCF`.
