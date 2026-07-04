# Dead-code removal and test reliability hardening implementation plan

## Change Request Reference

This implementation plan is based on the change request at `docs/change/26.6.1/dead-code-and-test-hardening.md` and the refactoring proposal at `docs/refactoring/26.6.1/dead-code-and-test-hardening-proposal.md`.

## Best Practices

Follow the coding standards in `docs/GUIDELINES.md` and the refactoring rules in `docs/REFACTORING.md`.

- Keep the public API unchanged.
- Do not modify the Compact Framework project files.
- Preserve existing error-handling behaviour, including the return-default-on-failure patterns.
- Use fully qualified references when calling static members declared in another type.
- Avoid language features or APIs that are unavailable on .NET 2.0 or the Compact Framework.
- Add or update tests for any changed path, but do not rewrite tests to hide regressions.

## Documentation Updates

No guideline or structural documentation changes are required. The refactoring proposal already documents the rationale. If any unexpected public API change becomes necessary during implementation, update `docs/GUIDELINES.md` or `docs/STRUCTURE.md` and the project-energy technical documentation before modifying source.

## Step by Step Implementation

### Tidy Step 1: Remove dead code in `Core/Network.cs`, `Core/Bug.cs`, and `Base/Path.cs`

Delete the unreachable code and commented-out blocks identified in the proposal.

File to modify: `Energy.Core/Core/Network.cs`.

```csharp
// Remove the unreachable throw at the end of the catch block.
return false;
```

File to modify: `Energy.Core/Core/Bug.cs`.

```csharp
// Remove the entire commented-out #region Code block (lines 168-234).
```

File to modify: `Energy.Core/Base/Path.cs`.

```csharp
// Remove the commented-out Split overload at lines 23-42.
```

### Tidy Step 2: Remove the commented-out currency block in `Base/Money.cs`

File to modify: `Energy.Core/Base/Money.cs`.

```csharp
// Remove the commented-out ISO 4217 block inside Manager.ISO 4217.
```

### Tidy Step 3: Dispose local `MemoryStream` instances in `Base/Url.cs` and `Base/Class.cs`

File to modify: `Energy.Core/Base/Url.cs`.

```csharp
using (MemoryStream ms = new MemoryStream(text.Length))
{
    // existing logic
    return result;
}
```

File to modify: `Energy.Core/Base/Class.cs`.

```csharp
using (MemoryStream ms = new MemoryStream())
{
    // existing logic
    return Array.Length;
}
```

### Tidy Step 4: Simplify redundant null checks in `Base/Binary.cs`

File to modify: `Energy.Core/Base/Binary.cs`.

```csharp
if (right == null)
    return 1;
```

Apply this to each `Compare` overload that has the redundant inner check.

### Tidy Step 5: Document empty catch blocks in `Core/Tilde.cs`

File to modify: `Energy.Core/Core/Tilde.cs`.

```csharp
catch
{
    // Console may be unavailable; ignore and continue.
}
```

### Structural Step 6: Harden test reliability and isolation

File to modify: `Energy.Core.Test/Base/Queue.cs`.

Remove the wall-clock timing assertions around `DateTime.Now` and `Thread.Sleep`. Keep the functional assertions that verify `Pull` returns `null` when empty and waits for an item.

File to modify: `Energy.Core.Test/Base/File.cs`.

Replace the fixed `Thread.Sleep(10)` with a polling retry loop that checks `state.IsChanged()` until it becomes true or a timeout expires.

File to modify: `Energy.Core.Test/Base/Path.cs`.

Wrap the environment variable mutation in `try`/`finally` so `XXX-TEST` is always restored, even if the assertion fails. Also decide whether to remove or restore the commented-out tests at lines 10-18 and 111-129.

File to modify: `Energy.Core.Test/Core/Web.cs`.

Replace the live `https://reqres.in` calls with a local mock, loopback server, or test-specific `HttpListener`. If that is not practical, replace the test with an integration test that is skipped by default.

File to modify: `Energy.Core.Test/Base/Class.cs`.

Complete the `ClassAssembly` test with a meaningful assertion or remove it.

File to modify: `Energy.Core.Test/Base/Http.cs`.

Change the namespace to `Energy.Core.Test.Base` and remove the auto-generated constructor, `TestContext`, and template comments.

File to modify: `Energy.Core.Test/Core/Tilde.cs`.

Replace the tautological assertion with a meaningful check (for example, that the property returns the same value on two consecutive calls) or remove the method.

## Implementation Order

1. Tidy Step 1: Remove dead code in `Core/Network.cs`, `Core/Bug.cs`, and `Base/Path.cs`.
2. Tidy Step 2: Remove dead code in `Base/Money.cs`.
3. Tidy Step 3: Dispose local `MemoryStream` instances in `Base/Url.cs` and `Base/Class.cs`.
4. Tidy Step 4: Simplify redundant null checks in `Base/Binary.cs`.
5. Tidy Step 5: Document empty catch blocks in `Core/Tilde.cs`.
6. Structural Step 6: Harden test reliability and isolation in the test project.
7. Run the verification loop after every step.

## Testing Strategy

- After each tidy step, build the library and run the full test suite. The existing tests must continue to pass.
- For the test-hardening step, verify that the modified tests still exercise the same production paths. The Web test must no longer require an external service; the Queue test must no longer assert wall-clock timing.
- Confirm that the LZSS, ZX0, and other compression fixtures still produce byte-for-byte identical output.

## Verification

The implementation is complete only when all steps pass with zero errors and zero warnings.

- Build: `dotnet build Energy.Core/Energy.Core.csproj`
- Test: `dotnet test Energy.Core.Test/Energy.Core.Test.csproj`
- Confirm no public API signatures changed.
- Confirm compression fixtures are unchanged byte-for-byte.
