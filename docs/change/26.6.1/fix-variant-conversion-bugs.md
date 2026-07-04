# Fix Variant.Union type conversion bugs

**Type:** Fix

## Summary

Correct the `UInt32` and `Float` property getters in `Energy.Base.Variant.Value`. The `UInt32` getter currently truncates to `UInt16`, and the `Float` getter casts through `char`, producing incorrect values. Both properties must return the boxed value cast to their declared type.

## Description

`Energy.Base.Variant.Value` exposes typed accessors for the boxed `Value` field. Two of these accessors have wrong casts:

- `UInt32` returns `(UInt16)Value`, so any value above 65535 is silently truncated.
- `Float` returns `(char)Value`, so the result is the character code of the boxed value instead of the floating-point value.

The fix is to change the casts to `(UInt32)` and `(float)` respectively, matching the declared return type and the pattern used by the other accessors.

## Compatibility

The change is compatible with every target framework. No new APIs or language features are used. The cast operators are available on .NET 2.0 and the Compact Framework.

The public API signature does not change; only the observable result of these two getters changes. This is a bug fix, not a breaking change.

## Use Cases

- A caller storing `70000` as a `uint` in a `Variant.Value` and reading `.UInt32` receives `70000` instead of `4464`.
- A caller storing `3.14f` and reading `.Float` receives `3.14f` instead of the character code of the boxed object.

## Hints

- Keep the null-handling pattern consistent with the neighbouring accessors.
- Add regression tests that fail before the fix and pass after it.

## Out of Scope

- Converting public mutable fields to properties.
- Refactoring other parts of `Energy.Base.Variant`.
