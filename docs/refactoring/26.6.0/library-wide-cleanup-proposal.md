# Library-Wide Cleanup Refactoring Proposal

## Problem

Energy.Core has grown to roughly 57,000 lines of library code across more than 160 source files. The growth has left a number of behaviour-preserving structural and consistency issues that this proposal addresses without changing any observable behaviour or the public API.

The analysis traced the largest and most central files, then searched the whole `Energy.Core` tree for systemic patterns. The findings fall into four groups: a forbidden API that escaped the project's own helper convention, a large block of duplicated numeric-parse logic, a thread-safety anti-pattern, and several structural and tidy issues in the two largest files.

Quantified observations from the analysis:

- `Base/Cast.cs` is 6,481 lines in a single file, with 267 public static methods and a single `Object` region spanning roughly 1,687 lines. `Base/Text.cs` is 5,273 lines.
- The numeric conversion methods in `Base/Cast.cs` repeat the same truncate-and-range-check parse block, split across `#if !NETCF` and `#if NETCF`, about twelve times.
- `Core/Bug.cs` locks on `typeof(SuppressCodeList)` at line 101, while the same file already uses a private lock object `_LastLock` elsewhere.
- `Core/Tilde.cs` line 1594 calls `string.Contains` directly, the only such call in the library; all 60 other call sites use the `Energy.Base.Text.Contains` helper.
- `Base/Cast.cs` line 1567 has a `#region UInt32` that actually contains `UInt64` methods such as `RealStringToUInt64`.
- `Base/Text.cs` carries 311 line-comment lines, a portion of which is commented-out dead code.
- `Energy.Base.Binary`, `Energy.Base.Money`, and `Energy.Base.Variant` have no test files, which raises the risk of refactoring them.

Files this refactoring would touch: `Base/Cast.cs`, `Base/Text.cs`, `Core/Bug.cs`, `Core/Tilde.cs`, plus new partial-class files split out from the two largest files, and new test files for the uncovered classes.

## Goal

The desired end state preserves every public signature and every observable result, while removing the forbidden API, collapsing the duplicated parse logic into one private helper, replacing the type lock with a private lock object, correcting the mislabeled region, removing dead code, and splitting the two largest files into partial-class files organised by responsibility.

After the work, the `dmsc/lzss-sap` and ZX0 fixtures and every other committed fixture still decode and re-encode byte for byte, the full test suite passes on every target framework, and the public API surface is unchanged.

## Findings

The findings are ordered by risk level, highest first.

### Replace the direct string.Contains call

**Issue**

`Core/Tilde.cs` line 1594 calls `message.Contains("{0}")` directly. GUIDELINES.md forbids `string.Contains` in library code, and every other call site in the library uses the `Energy.Base.Text.Contains` helper.

**Impact**

The call violates the project's stated compatibility rule and is the single inconsistency among 60 sibling call sites. While `string.Contains(string)` compiles on the current targets, the rule exists to keep one canonical helper, and the lone exception is a maintainability and consistency defect.

**Recommendation**

Replace the direct call with `Energy.Base.Text.Contains(message, "{0}")`, matching the rest of the codebase.

**Compatibility Assessment**

- .NET Standard 2.0: Compatible
- .NET Framework 4.0: Compatible
- .NET Framework 3.5: Compatible
- .NET Framework 2.0: Compatible
- Compact Framework 3.5: Compatible
- Public API: Compatible

**Risk Level**

Major

**Example**

```csharp
// Before
if (message.Contains("{0}"))

// After
if (Energy.Base.Text.Contains(message, "{0}"))
```

### Extract the duplicated numeric truncate-and-range parse

**Issue**

The integer conversion methods in `Base/Cast.cs` repeat the same block about twelve times: parse the text, truncate toward zero with `value < 0 ? Math.Ceiling(value) : Math.Floor(value)`, check the result against the target type's range, and return null on failure. Each occurrence is duplicated again across a `#if !NETCF` path that uses `decimal.TryParse` and a `#if NETCF` path that uses `double.Parse` inside a try and catch.

**Impact**

The duplication multiplies maintenance cost and risk. A fix or a behaviour clarification must be applied in a dozen places and kept consistent across both platform paths, which is error prone. It also inflates `Base/Cast.cs` and obscures the per-type logic.

**Recommendation**

Extract one private static helper that parses a string to a truncated `decimal` (or `double` on the Compact Framework) and reports success, then have each typed conversion call it and apply only its own range check. The Rule of Three is satisfied many times over. Keep the `#if NETCF` divergence inside the single helper so the per-type methods become uniform.

**Compatibility Assessment**

- .NET Standard 2.0: Compatible
- .NET Framework 4.0: Compatible
- .NET Framework 3.5: Compatible
- .NET Framework 2.0: Compatible
- Compact Framework 3.5: Requires verification
- Public API: Compatible

**Risk Level**

Major

**Example**

```csharp
// Before, repeated per type with its own #if NETCF twin
decimal _number;
if (!decimal.TryParse(value, numberStyles, CultureInfo.InvariantCulture, out _number))
    return null;
var round = _number < 0 ? Math.Ceiling(_number) : Math.Floor(_number);
if (round < byte.MinValue || round > byte.MaxValue)
    return null;
return (byte?)_number;

// After
decimal _number;
if (!TryParseTruncated(value, numberStyles, out _number))
    return null;
if (_number < byte.MinValue || _number > byte.MaxValue)
    return null;
return (byte?)_number;
```

### Replace the lock on a type object

**Issue**

`Core/Bug.cs` line 101 uses `lock (typeof(SuppressCodeList))`. Locking on a `Type` instance shares a process-wide lock that any other code can also take, which risks a deadlock and is the anti-pattern called out in REFACTORING.md and GUIDELINES.md.

**Impact**

A shared type lock can serialise or deadlock against unrelated code that happens to lock the same type. Because `Energy.Core.Bug` is a diagnostics path used throughout the library, contention here is hard to diagnose.

**Recommendation**

Introduce a private static readonly object and lock on it, matching the existing `_LastLock` pattern already used at lines 561 and 568 of the same file.

**Compatibility Assessment**

- .NET Standard 2.0: Compatible
- .NET Framework 4.0: Compatible
- .NET Framework 3.5: Compatible
- .NET Framework 2.0: Compatible
- Compact Framework 3.5: Compatible
- Public API: Compatible

**Risk Level**

Major

**Example**

```csharp
// Before
lock (typeof(SuppressCodeList)) { ... }

// After
private static readonly object _SuppressCodeListLock = new object();
lock (_SuppressCodeListLock) { ... }
```

### Split the two largest files into partial-class files

**Issue**

`Base/Cast.cs` is 6,481 lines with 267 public methods and a single region of roughly 1,687 lines, and `Base/Text.cs` is 5,273 lines. Files of this size are hard to navigate, review, and reason about.

**Impact**

Large files increase cognitive load, make code review slower, and raise the chance that related logic drifts out of sync. They do not affect behaviour, so this is a maintainability issue only.

**Recommendation**

Split each class into partial-class files grouped by responsibility, for example `Cast.Numeric.cs`, `Cast.DateTime.cs`, `Cast.Object.cs`, and `Cast.Json.cs`, keeping the existing regions intact. Partial classes are a compile-time construct, so the resulting type and its public API are identical. Move code without editing it, one responsibility group per step.

**Compatibility Assessment**

- .NET Standard 2.0: Compatible
- .NET Framework 4.0: Compatible
- .NET Framework 3.5: Compatible
- .NET Framework 2.0: Compatible
- Compact Framework 3.5: Requires verification
- Public API: Compatible

**Risk Level**

Major

**Example**

```csharp
// Cast.Numeric.cs
namespace Energy.Base
{
    public static partial class Cast
    {
        // numeric conversion regions moved here verbatim
    }
}
```

A prerequisite for the split is changing the declaration from `public static class Cast` to `public static partial class Cast`. The Compact Framework project files must include the new files, which is why the Compact dimension is marked Requires verification.

### Correct the mislabeled region

**Issue**

`Base/Cast.cs` line 1567 opens `#region UInt32`, but the region contains `UInt64` logic such as `RealStringToUInt64` and `ulong` results. GUIDELINES.md requires a region name to match the member it contains. A correct `#region UInt32` already exists at line 1081.

**Impact**

The wrong name misleads readers and breaks the region-naming convention. It is a readability and consistency defect with no behavioural effect.

**Recommendation**

Rename the region at line 1567 to `UInt64`.

**Compatibility Assessment**

- .NET Standard 2.0: Compatible
- .NET Framework 4.0: Compatible
- .NET Framework 3.5: Compatible
- .NET Framework 2.0: Compatible
- Compact Framework 3.5: Compatible
- Public API: Compatible

**Risk Level**

Minor

**Example**

```csharp
// Before
#region UInt32
private static ulong RealStringToUInt64(string value, ...)

// After
#region UInt64
private static ulong RealStringToUInt64(string value, ...)
```

### Make terminal exception handling consistent

**Issue**

There are about 80 `catch (Exception)` sites across the library. Their terminal behaviour is inconsistent: some call `Energy.Core.Bug.Catch`, some call `System.Diagnostics.Debug.WriteLine`, and a few swallow the exception with no diagnostic at all, for example the `FormatException` handlers at `Core/Network.cs` line 50 and `Base/ByteArrayBuilder.cs` line 846.

**Impact**

Silent swallows lose information that would help diagnose a fault, and the mix of three terminal styles makes the error policy unpredictable. Deliberate control-flow catches such as `ThreadAbortException` and `PlatformNotSupportedException` are legitimate and should stay.

**Recommendation**

Route diagnostics through `Energy.Core.Bug.Catch` where a diagnostic is appropriate, give each genuinely silent catch a short comment explaining why it is intentional, and leave deliberate control-flow catches unchanged. Do not alter which exceptions are caught or the resulting return values, so behaviour is preserved.

**Compatibility Assessment**

- .NET Standard 2.0: Compatible
- .NET Framework 4.0: Compatible
- .NET Framework 3.5: Compatible
- .NET Framework 2.0: Compatible
- Compact Framework 3.5: Compatible
- Public API: Compatible

**Risk Level**

Minor

**Example**

```csharp
// Before
catch (FormatException)
{
}

// After
catch (FormatException formatException)
{
    Energy.Core.Bug.Catch(formatException);
}
```

### Remove commented-out dead code

**Issue**

`Base/Text.cs` contains 311 line-comment lines, a portion of which is commented-out code rather than explanatory comments. Smaller pockets exist in other files.

**Impact**

Dead code in comments adds noise, goes stale, and misleads readers into thinking it is relevant. It has no behavioural effect.

**Recommendation**

Remove commented-out code while preserving comments that explain intent or rationale. Verify against git history that the removed code is not a needed reference before deleting.

**Compatibility Assessment**

- .NET Standard 2.0: Compatible
- .NET Framework 4.0: Compatible
- .NET Framework 3.5: Compatible
- .NET Framework 2.0: Compatible
- Compact Framework 3.5: Compatible
- Public API: Compatible

**Risk Level**

Minor

### Add characterisation tests for uncovered classes

**Issue**

`Energy.Base.Binary` at 1,410 lines, `Energy.Base.Money` at 701 lines, and `Energy.Base.Variant` have no test files, although the library has 323 test methods overall.

**Impact**

Without tests, any future refactoring of these classes cannot prove behaviour preservation, so the safety net the rest of the library enjoys is missing here.

**Recommendation**

Add characterisation tests that capture the current behaviour of the public methods of these classes before any structural change touches them. This finding is also a prerequisite for safely refactoring those files later.

**Compatibility Assessment**

- .NET Standard 2.0: Compatible
- .NET Framework 4.0: Compatible
- .NET Framework 3.5: Compatible
- .NET Framework 2.0: Compatible
- Compact Framework 3.5: Compatible
- Public API: Compatible

**Risk Level**

Minor

### Enforce zero warnings in the build

**Issue**

WORKFLOW.md requires zero warnings, but `Energy.Core.csproj` does not set `TreatWarningsAsErrors`, so the rule is enforced only by discipline.

**Impact**

A warning can slip in unnoticed between reviews. Turning warnings into errors makes the existing rule mechanical, but it may surface latent warnings that need fixing first.

**Recommendation**

Consider enabling `TreatWarningsAsErrors` per configuration after the codebase is confirmed warning-clean on every target. Treat this as a separate, optional follow-up because it can surface pre-existing warnings.

**Compatibility Assessment**

- .NET Standard 2.0: Requires verification
- .NET Framework 4.0: Requires verification
- .NET Framework 3.5: Requires verification
- .NET Framework 2.0: Requires verification
- Compact Framework 3.5: Requires verification
- Public API: Compatible

**Risk Level**

Suggestion

## Plan

Execute the steps in this order. Each step leaves the codebase buildable and testable. Tidy steps are kept separate from structural steps.

- Step 1, tidy: replace the direct `string.Contains` call in `Core/Tilde.cs`.
- Step 2, tidy: rename the mislabeled `UInt32` region in `Base/Cast.cs` to `UInt64`.
- Step 3, structural: replace the type lock in `Core/Bug.cs` with a private lock object.
- Step 4, structural: extract the numeric truncate-and-range parse helper in `Base/Cast.cs`, converting one conversion method at a time so each change is individually verifiable.
- Step 5, tidy: add characterisation tests for `Binary`, `Money`, and `Variant`.
- Step 6, tidy: remove commented-out dead code from `Base/Text.cs` and other files.
- Step 7, structural: introduce the `partial` modifier on `Cast`, then move one responsibility group at a time into a new partial-class file; repeat for `Text`.
- Step 8, suggestion, optional and separate: evaluate enabling `TreatWarningsAsErrors` once the build is confirmed warning-clean.

## Step Granularity

Each step must be small enough that a regression it introduces is obvious from that step's build and test output.

Steps 4 and 7 are split internally so that a single conversion method or a single responsibility group moves at a time. No step may mix a rename with an extraction, and no step may move code and edit it in the same commit.

## Risk

The numeric helper extraction in Step 4 is the highest-behavioural-risk change, because the `#if NETCF` and non-NETCF paths differ in parse type and overflow handling. The `Energy.Base.Cast` tests cover many of these conversions, so a regression should surface, but the Compact Framework path is harder to exercise and must be built and checked explicitly.

The file split in Step 7 carries mechanical risk from moving thousands of lines and from registering the new files in the Compact Framework project files, which are separate from `Energy.Core.csproj`. The mitigation is to move verbatim, one group per step, and to keep the public API and the build output identical.

`Binary`, `Money`, and `Variant` have no tests, so Step 5 must precede any later structural change to those classes. They are not split in this proposal for that reason.

## Acceptance Criteria

- The build succeeds with zero errors and zero warnings on `netstandard2.0`, `net40`, `net35`, and `net20`, and on the Compact Framework project files.
- The full test suite passes, and the new characterisation tests for `Binary`, `Money`, and `Variant` pass.
- Every committed reference fixture, including the LZSS and ZX0 fixtures, still decodes and re-encodes byte for byte.
- The public API surface is unchanged. No public class, interface, method, property, event, or enum is renamed, removed, or has its signature changed.
- No observable behaviour changes for any input, including the error-return values produced on failure.
