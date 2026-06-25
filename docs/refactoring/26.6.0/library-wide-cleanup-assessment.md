# Library-Wide Cleanup Refactoring Assessment

## Proposal Summary

The proposal at `docs/refactoring/26.6.1/library-wide-cleanup-proposal.md` identified nine findings across four groups: a forbidden API that escaped the project helper convention, a block of duplicated numeric-parse logic, a thread-safety anti-pattern, and several structural and tidy issues in the two largest files. The plan ordered eight steps, keeping tidy steps separate from structural ones, with a final optional step.

## Implementation Summary

Steps 1 through 6 were implemented and verified. Step 7 was closed without action by an explicit scope decision, and Step 8 was left as the optional follow-up the proposal described.

- Step 1, done: `Core/Tilde.cs` now calls `Energy.Base.Text.Contains(message, "{0}")` instead of `string.Contains`. The library no longer contains any direct `string.Contains` call.
- Step 2, done: the mislabeled `#region UInt32` at `Base/Cast.cs` line 1567 is renamed to `#region UInt64`, matching its `RealStringToUInt64` content.
- Step 3, done: `Core/Bug.cs` now locks on the existing private `_DefaultLock` object instead of `typeof(SuppressCodeList)`. The library no longer locks on any type object.
- Step 4, done: a private `TryParseTruncated` helper was extracted and the eight `StringToNullable*` integer conversions route their `#if !NETCF` branch through it. The `#if NETCF` branches were left unchanged on purpose.
- Step 5, done with corrected scope: characterisation tests were added for `Energy.Base.Money` and `Energy.Base.Variant`. `Energy.Base.Binary` was found to already have a test file, so no Binary tests were added.
- Step 6, done in part: the clearest commented-out dead-code blocks in `Base/Text.cs` were removed. Ambiguous comment lines were left in place.
- Step 7, closed without action: splitting `Cast.cs` and `Text.cs` into partial-class files was not carried out, by an explicit decision to keep the work inside the Energy.Core library and leave the Compact Framework projects untouched. See the gap analysis.
- Step 8, not started: enabling `TreatWarningsAsErrors` remains the optional, separate follow-up the proposal described.

## Gap Analysis

Three deviations from the proposal are recorded here.

The proposal's finding that `Energy.Base.Binary` had no test file was wrong. A test file with fourteen test methods already exists and covers the endian, bitwise, rotate, and shift helpers, as well as the bit reader and writer. The earlier search that produced the finding was run from the wrong working directory. Only `Money` and `Variant` were genuinely untested, and both now have characterisation tests. The proposal text overstated the coverage gap; the implementation corrected it rather than adding a redundant Binary test.

Step 4 was implemented for the `#if !NETCF` branch only, not for the Compact Framework branch. During implementation the Compact branches were found to use two different parse strategies, and they deliberately cast to `double` before `Math.Ceiling` and `Math.Floor`, which indicates that `Math.Ceiling(decimal)` and `Math.Floor(decimal)` are unavailable on that runtime. Unifying the Compact branch would change behaviour on a path that cannot be compiled or tested in this environment, so it was left byte-for-byte unchanged. The four buildable targets all compile the `#if !NETCF` branch, so the deduplication still applies to everything that can be verified here.

Step 7 was closed without action by an explicit scope decision to work only inside the Energy.Core library and not modify the Compact Framework or example projects. The three Compact Framework project files use explicit `<Compile Include>` lists of more than 160 entries each. The main `Energy.Core.csproj` is SDK-style and would pick up new partial-class files automatically, but the Compact projects would not, so splitting the files while leaving those projects untouched would drop the moved methods from the Compact build. Because the split is a maintainability-only change with no behavioural benefit, and keeping every supported target building takes priority, Step 7 is not carried forward as pending work. It can be revisited only as a deliberate, separately scoped change that updates the Compact projects in the same step, in an environment where their build can be verified.

## Verification Results

- Build, `dotnet build Energy.Core/Energy.Core.csproj`, all four targets `netstandard2.0`, `net40`, `net35`, `net20`: pass, zero errors, zero warnings.
- Test, `dotnet test Energy.Core.Test/Energy.Core.Test.csproj` on net48: 328 passed, 1 failed, 329 total.
- The single failing test is `Compression_ZX0_Big16KFixture`. It failed identically on the clean baseline before any change in this work, is caused by a committed fixture whose length does not match a hard-coded assertion, and is unrelated to the files touched here.
- Compact Framework build: not verified. The toolchain is not available in this environment. No Compact-only code path was modified, so no Compact behaviour change is expected.

## Metrics Comparison

- Direct `string.Contains` calls in library code: 1 before, 0 after.
- `lock(typeof(...))` occurrences in library code: 1 before, 0 after.
- Duplicated numeric truncate-and-range parse blocks on the buildable targets: 8 before, 1 shared helper after.
- Commented-out dead-code lines removed from `Base/Text.cs`: 24.
- Test methods: 321 before, 329 after, with 8 new characterisation tests for `Money` and `Variant`.
- `Base/Cast.cs` grew slightly, from 6481 to 6492 lines, because the shared helper and its documentation add more lines than the per-method savings on the non-Compact branch remove. The intent of Step 4 was to centralise the parse logic, not to shrink the file; the file-size reduction was the goal of the deferred Step 7.

## Output Fidelity

No produced output changed. The LZSS and ZX0 fixtures and all other committed fixtures decode and re-encode exactly as before, confirmed by the unchanged pass or fail state of every fixture test. The numeric conversion behaviour is preserved: the extracted helper leaves `_number` holding the truncated value that the previous code computed as `round`, the range check is performed on that same value, and the cast to the target type truncates toward zero exactly as before. The directly affected case `StringToNullableByte("-1")` still returns null, and decimal and scientific-notation inputs still truncate to their integer part.

## Conclusion

Pass for the implemented scope. Steps 1 through 6 are complete, verified on every buildable target with zero errors and zero warnings, and behaviour-preserving. The public API surface is unchanged. All changes are confined to the Energy.Core library; the Compact Framework and example projects were not modified.

Step 7, the partial-class split, is closed by decision and is not carried forward as pending work, to keep the change inside the library and preserve the Compact build. Two smaller items remain optional. Step 4 could be extended to the Compact Framework branch only in an environment where that branch can be built, where its two divergent parse paths can be unified and tested against the runtime's reduced Math support. Step 8, enabling `TreatWarningsAsErrors`, remains optional and should follow a confirmation that every target is warning-clean.
