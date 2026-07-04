# Fix Variant.Union type conversion bugs implementation plan

## Change Request Reference

This implementation plan is based on the change request at `docs/change/26.6.1/fix-variant-conversion-bugs.md`.

## Best Practices

Follow the coding standards in `docs/GUIDELINES.md`. The library must compile on .NET 2.0 and the Compact Framework, so use only casts and operators available on those runtimes.

## Documentation Updates

No guideline or project documentation changes are required. The change request documents the rationale.

## Step by Step Implementation

### Fix the `UInt32` getter

File to modify: `Energy.Core/Base/Variant.cs`.

```csharp
public UInt32 UInt32
{
    get { return Value == null ? (UInt32)0 : (UInt32)Value; }
    set { Value = value; }
}
```

### Fix the `Float` getter

File to modify: `Energy.Core/Base/Variant.cs`.

```csharp
public float Float
{
    get { return Value == null ? 0f : (float)Value; }
    set { Value = value; }
}
```

### Add regression tests

File to modify: `Energy.Core.Test/Base/Variant.cs`.

Add a new test method that verifies the two getters return the correct value and that null values return zero:

```csharp
[TestMethod]
public void Variant_Value_TypeAccessorCasts()
{
    Assert.AreEqual((uint)70000, new Energy.Base.Variant.Value((uint)70000).UInt32);
    Assert.AreEqual((uint)0, new Energy.Base.Variant.Value(null).UInt32);

    Assert.AreEqual(3.14f, new Energy.Base.Variant.Value(3.14f).Float, 0.001f);
    Assert.AreEqual(0f, new Energy.Base.Variant.Value(null).Float, 0.001f);
}
```

## Implementation Order

1. Add the regression test first and confirm it fails before the fix.
2. Fix the `UInt32` getter.
3. Fix the `Float` getter.
4. Run the verification loop.

## Testing Strategy

- The new regression test must fail before the fix and pass after the fix.
- Run the full `Energy.Core.Test` suite to ensure no other tests are affected.

## Verification

The implementation is complete only when all steps pass with zero errors and zero warnings.

- Build: `dotnet build Energy.Core/Energy.Core.csproj`
- Test: `dotnet test Energy.Core.Test/Energy.Core.Test.csproj`
