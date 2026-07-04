# Dead-Code Removal and Test Reliability Hardening Refactoring Proposal

## Problem

The Energy.Core library and Energy.Core.Test project have accumulated dead code, redundant checks, local resource leaks, and unreliable tests after the 26.6.0 library-wide cleanup. The previous refactoring fixed the most prominent issues: it removed direct `string.Contains` calls, replaced `lock(typeof(...))`, extracted the duplicated numeric truncate-and-range parse helper, corrected a mislabeled region, and removed some dead comments. That work intentionally left the two largest files (`Base/Cast.cs` and `Base/Text.cs`) unsplit to avoid touching the Compact Framework project files.

This proposal targets the next layer of behaviour-preserving cleanup without changing the public API or the Compact Framework build surface. The analysis traced the whole `Energy.Core` tree and the `Energy.Core.Test` project for the patterns listed in `docs/REFACTORING.md`.

Quantified observations:

- Four blocks of dead or commented-out code remain in `Core/Network.cs`, `Core/Bug.cs`, `Base/Path.cs`, and `Base/Money.cs`.
- Two local `MemoryStream` instances are created and returned or thrown away without disposal in `Base/Url.cs` and `Base/Class.cs`.
- Four `Binary.Compare` overloads contain the same redundant inner `left == null` check.
- Two `Variant.Union` type-conversion bugs were discovered during the analysis. They are defects, not refactorings, and are documented in a separate section for change-request follow-up.
- Five test reliability issues exist in `Energy.Core.Test`: time-dependent assertions, a fixed `Thread.Sleep`, an external HTTP dependency, a process environment variable mutation, and a test method with no assertions.

Files this refactoring would touch: `Core/Network.cs`, `Core/Bug.cs`, `Base/Path.cs`, `Base/Money.cs`, `Base/Url.cs`, `Base/Class.cs`, `Base/Binary.cs`, `Core/Tilde.cs`, plus test files `Energy.Core.Test/Base/Queue.cs`, `Energy.Core.Test/Base/File.cs`, `Energy.Core.Test/Base/Path.cs`, `Energy.Core.Test/Core/Web.cs`, `Energy.Core.Test/Base/Http.cs`, `Energy.Core.Test/Base/Class.cs`, and `Energy.Core.Test/Core/Tilde.cs`.

## Goal

The desired end state is a library and test project that still compile on every target framework, still pass the full test suite, and preserve every public signature and every observable result. Within that constraint, the work removes dead code, disposes local disposables, simplifies redundant checks, documents intentionally empty catch blocks, and hardens tests against timing, environment, and external-network dependencies.

## Findings

The findings are ordered by risk level, highest first. Every recommendation is a behaviour-preserving change unless explicitly noted as a breaking change.

### Remove the unreachable `throw` in `Core/Network.cs`

**Issue**

`Core/Network.cs` line 678 contains a `throw` statement immediately after `return false`, so it can never execute.

```csharp
return false;
throw;
```

**Impact**

Unreachable code is misleading during review and can trigger compiler warnings or analysis diagnostics. It adds no value and obscures the intended error-handling path.

**Recommendation**

Remove the `throw` statement.

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
return false;
throw;

// After
return false;
```

### Dispose the local `MemoryStream` in `Base/Url.Decode`

**Issue**

`Base/Url.Decode` creates a local `MemoryStream` at line 859, uses it to accumulate decoded bytes, and returns a string without disposing the stream.

```csharp
MemoryStream ms = new MemoryStream(text.Length);
// ... write bytes, read back, build string ...
string result = encoding.GetString(bytes, 0, bytes.Length);
return result;
```

**Impact**

A local `IDisposable` is left for finalization. While a `MemoryStream` holds only managed memory, this is still a resource-management defect and contradicts the guideline that resource ownership must be clear.

**Recommendation**

Wrap the `MemoryStream` in a `using` statement. The method is already `#if !NETCF`, so `using` is available on every buildable target.

**Compatibility Assessment**

- .NET Standard 2.0: Compatible
- .NET Framework 4.0: Compatible
- .NET Framework 3.5: Compatible
- .NET Framework 2.0: Compatible
- Compact Framework 3.5: Compatible (the whole method is guarded by `#if !NETCF`)
- Public API: Compatible

**Risk Level**

Major

**Example**

```csharp
// Before
MemoryStream ms = new MemoryStream(text.Length);
// ...
return result;

// After
using (MemoryStream ms = new MemoryStream(text.Length))
{
    // ...
    return result;
}
```

### Dispose the local `MemoryStream` in `Base/Class.GetObjectSize`

**Issue**

`Base/Class.GetObjectSize` creates a `MemoryStream` at line 1158 to measure the serialized size of an object, then returns the length without disposing the stream.

```csharp
BinaryFormatter bf = new BinaryFormatter();
MemoryStream ms = new MemoryStream();
byte[] Array;
bf.Serialize(ms, o);
Array = ms.ToArray();
return Array.Length;
```

**Impact**

Same as the `Url.Decode` finding: a local disposable is left for finalization. The code is already inside `#if !NETCF`.

**Recommendation**

Wrap the `MemoryStream` in a `using` statement.

**Compatibility Assessment**

- .NET Standard 2.0: Compatible
- .NET Framework 4.0: Compatible
- .NET Framework 3.5: Compatible
- .NET Framework 2.0: Compatible
- Compact Framework 3.5: Compatible (the code is guarded by `#if !NETCF`)
- Public API: Compatible

**Risk Level**

Major

**Example**

```csharp
// Before
BinaryFormatter bf = new BinaryFormatter();
MemoryStream ms = new MemoryStream();
// ...
return Array.Length;

// After
BinaryFormatter bf = new BinaryFormatter();
using (MemoryStream ms = new MemoryStream())
{
    // ...
    return Array.Length;
}
```

### Remove the commented-out ISO 4217 currency block in `Base/Money.cs`

**Issue**

`Base/Money.cs` contains approximately 170 lines of commented-out ISO 4217 currency definitions inside `Manager.ISO 4217` (lines 228-400+). The same currencies are already exposed as live `readonly` fields below the block.

```csharp
//public static Currency AED => new Currency("AED", "UAE Dirham", 784, 2);
//public static Currency AFN => new Currency("AFN", "Afghani", 971, 2, "؋");
// ... 170+ more lines
```

**Impact**

The commented block is dead code that obscures the live implementation, increases file size unnecessarily, and creates confusion about which definitions are authoritative.

**Recommendation**

Delete the commented-out block. The live definitions are sufficient; no replacement is needed.

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
// Before: 170 lines of commented currency definitions

// After: only the live definitions remain
public readonly Currency AED = new Currency("AED", "UAE Dirham", 784, 2);
// ...
```

### Remove the commented-out `Code` struct in `Core/Bug.cs`

**Issue**

`Core/Bug.cs` lines 168-234 contain an entire commented-out `Code` struct with constructors, implicit operators, and helper methods.

**Impact**

Dead code adds noise and makes the file harder to read. The struct has been commented out long enough that it is not part of the public API and has no references.

**Recommendation**

Delete the commented-out region.

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
//#region Code
//public struct Code
//{
//    ...
//}
//#endregion

// After: nothing remains
```

### Remove the commented-out `Split` overload in `Base/Path.cs`

**Issue**

`Base/Path.cs` lines 23-42 contain a commented-out overload of `Split` that duplicates the signature of the live method above it.

**Impact**

Dead code. The live `Split(string path)` delegates to `Split(path, null)` and is the correct implementation.

**Recommendation**

Delete the commented-out block.

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
//public static string[] Split(string path)
//{
//    ...
//}

// After: nothing remains
```

### Simplify the redundant null checks in `Base/Binary.Compare`

**Issue**

Each `Compare` overload in `Base/Binary.cs` checks `right == null` immediately after establishing that `left != null`. The inner `if (left == null)` is therefore unreachable.

```csharp
if (left == null)
{
    if (right == null)
        return 0;
    return -1;
}
if (right == null)
{
    if (left == null)  // redundant
        return 0;
    return 1;
}
```

**Impact**

The redundant check is a code smell. It appears in at least four overloads and makes the null-handling logic look more complex than it is.

**Recommendation**

Simplify the second branch to `return 1;` in each affected overload. Do not change the comparison logic itself.

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
if (right == null)
{
    if (left == null)
        return 0;
    return 1;
}

// After
if (right == null)
    return 1;
```

### Document the intentionally empty catch blocks in `Core/Tilde.cs`

**Issue**

`Core/Tilde.cs` contains several intentionally empty catch blocks around console operations (lines 429, 442, and related single-type catches). They swallow exceptions that can occur when the console is unavailable or restricted.

**Impact**

Empty catch blocks are a recognised anti-pattern. Without a comment, a future reader may mistake them for accidental omissions and add handling that changes behaviour.

**Recommendation**

Add a short comment inside each empty catch block explaining that the exception is intentionally ignored because the console may not be available. Do not add logging or rethrowing, because that would alter the observable behaviour of the console helper.

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
catch { }

// After
catch
{
    // Console may be unavailable; ignore and continue.
}
```

### Remove the external HTTP dependency in `Energy.Core.Test/Core/Web.cs`

**Issue**

`Energy.Core.Test/Core/Web.cs` calls `https://reqres.in` for `POST`, `PATCH`, and `PUT` assertions. The test fails if the machine is offline, the service is down, or the endpoint changes.

**Impact**

External-network dependencies make the test suite non-deterministic and unsuitable for isolated or CI environments.

**Recommendation**

Replace the live calls with a local, deterministic test server or mock `HttpWebRequest`/`HttpWebResponse` objects. If a live integration test is genuinely required, move it to a separately labelled integration test that is skipped by default.

**Compatibility Assessment**

- .NET Standard 2.0: Compatible
- .NET Framework 4.0: Compatible
- .NET Framework 3.5: Compatible
- .NET Framework 2.0: Compatible
- Compact Framework 3.5: N/A (tests do not target Compact Framework)
- Public API: N/A (test-only change)

**Risk Level**

Major

**Example**

```csharp
// Before
response = Energy.Core.Web.Post(url, body, "application/json");
Assert.IsNotNull(response);

// After: use a local loopback mock or a test-specific HttpListener
response = Energy.Core.Web.Post(localUrl, body, "application/json");
Assert.IsNotNull(response);
```

### Remove the time-dependent assertions in `Energy.Core.Test/Base/Queue.cs`

**Issue**

`Energy.Core.Test/Base/Queue.cs` lines 26-41 use `DateTime.Now` and `Thread.Sleep` to assert timing properties of `Queue.Pull`. The assertions can fail on slow or heavily loaded systems.

```csharp
start = DateTime.Now;
Assert.IsNull(q.Pull());
spend = DateTime.Now - start;
Assert.IsTrue(spend < TimeSpan.FromSeconds(0.1), ...);
```

**Impact**

Flaky tests reduce confidence in the test suite and waste CI time on false failures.

**Recommendation**

Keep the functional assertions (e.g. `Pull` returns `null` when empty, `Pull(timeout)` waits for an item). Remove the wall-clock timing assertions, or replace them with a deterministic test clock if the library supports one.

**Compatibility Assessment**

- .NET Standard 2.0: Compatible
- .NET Framework 4.0: Compatible
- .NET Framework 3.5: Compatible
- .NET Framework 2.0: Compatible
- Compact Framework 3.5: N/A
- Public API: N/A

**Risk Level**

Major

### Remove the `Thread.Sleep` timing in `Energy.Core.Test/Base/File.cs`

**Issue**

`Energy.Core.Test/Base/File.cs` line 44 sleeps for 10 milliseconds and then asserts that a file write-stamp change is detected. The comment in the test acknowledges that it may fail.

```csharp
System.Threading.Thread.Sleep(10);
Assert.IsTrue(clone.SetWriteStamp());
Assert.IsTrue(state.IsChanged(), "Running tests may fail here");
```

**Impact**

Timing-dependent file-system tests are flaky. A slow disk or scheduler can cause the timestamp to remain unchanged after 10 ms.

**Recommendation**

Replace the fixed sleep with a retry loop that polls the stamp until it changes or a reasonable timeout expires. Keep the functional assertion that the stamp eventually changes.

**Compatibility Assessment**

- .NET Standard 2.0: Compatible
- .NET Framework 4.0: Compatible
- .NET Framework 3.5: Compatible
- .NET Framework 2.0: Compatible
- Compact Framework 3.5: N/A
- Public API: N/A

**Risk Level**

Major

### Isolate the environment variable test in `Energy.Core.Test/Base/Path.cs`

**Issue**

`Energy.Core.Test/Base/Path.cs` lines 188-192 mutate the process environment variable `XXX-TEST` and then reset it. If the test fails between the set and the reset, the variable is left modified.

```csharp
System.Environment.SetEnvironmentVariable("XXX-TEST", "A:B:C");
result = Energy.Base.Path.Environment("XXX-TEST", ':');
// ...
System.Environment.SetEnvironmentVariable("XXX-TEST", null);
```

**Impact**

Modifying shared process state violates test isolation. Even with `[DoNotParallelize]` at the assembly level, the mutation can leak into later tests or processes.

**Recommendation**

Wrap the set/assert/reset in a `try`/`finally` so the variable is always restored. Alternatively, refactor the production API to accept an `IDictionary` of environment values for testability, though that is a larger change and should be a separate change request if it alters the public API.

**Compatibility Assessment**

- .NET Standard 2.0: Compatible
- .NET Framework 4.0: Compatible
- .NET Framework 3.5: Compatible
- .NET Framework 2.0: Compatible
- Compact Framework 3.5: N/A
- Public API: N/A (test-only change)

**Risk Level**

Major

### Complete or remove the empty test in `Energy.Core.Test/Base/Class.cs`

**Issue**

`Energy.Core.Test/Base/Class.cs` lines 145-150 contain a test method with a `TODO` comment and no assertions.

```csharp
[TestMethod]
public void ClassAssembly()
{
    var a = System.Reflection.Assembly.GetExecutingAssembly();
    var x = Energy.Base.Class.GetAssemblyFile(a);
    // TODO Something, something, etc...
}
```

**Impact**

A test without assertions always passes and gives a false sense of coverage.

**Recommendation**

Either add a meaningful assertion (for example, that the returned path is non-null and ends with the expected assembly file name) or remove the method.

**Compatibility Assessment**

- .NET Standard 2.0: Compatible
- .NET Framework 4.0: Compatible
- .NET Framework 3.5: Compatible
- .NET Framework 2.0: Compatible
- Compact Framework 3.5: N/A
- Public API: N/A

**Risk Level**

Major

### Correct the namespace in `Energy.Core.Test/Base/Http.cs`

**Issue**

The file `Energy.Core.Test/Base/Http.cs` declares namespace `Energy.Core.Test.Core` at line 7, but its directory is `Base/`. `docs/TESTING.md` requires test namespaces to mirror the production structure.

**Impact**

The mismatch makes it harder to locate the test for `Energy.Base.Http` and violates the project naming convention.

**Recommendation**

Change the namespace to `Energy.Core.Test.Base` and remove the auto-generated boilerplate (constructor, `TestContext`, and unused template comments) at lines 9-30.

**Compatibility Assessment**

- .NET Standard 2.0: Compatible
- .NET Framework 4.0: Compatible
- .NET Framework 3.5: Compatible
- .NET Framework 2.0: Compatible
- Compact Framework 3.5: N/A
- Public API: N/A

**Risk Level**

Minor

### Remove the commented-out tests in `Energy.Core.Test/Base/Path.cs`

**Issue**

`Energy.Core.Test/Base/Path.cs` lines 10-18 and 111-129 contain commented-out test methods with no explanation of why they were disabled.

**Impact**

Dead code in the test project reduces readability and makes it unclear whether the disabled tests are still valid.

**Recommendation**

Either restore the tests with proper isolation or delete them. If they are kept as comments, add a brief note explaining the reason.

**Compatibility Assessment**

- .NET Standard 2.0: Compatible
- .NET Framework 4.0: Compatible
- .NET Framework 3.5: Compatible
- .NET Framework 2.0: Compatible
- Compact Framework 3.5: N/A
- Public API: N/A

**Risk Level**

Minor

### Remove the tautological assertion in `Energy.Core.Test/Core/Tilde.cs`

**Issue**

`Energy.Core.Test/Core/Tilde.cs` lines 10-16 assert `x == true || x == false`, which is always true for any boolean.

```csharp
bool x = Energy.Core.Tilde.IsConsolePresent;
Assert.IsTrue(x == true || x == false);
```

**Impact**

The test does not verify the actual behaviour of `IsConsolePresent`. It only checks that the property returns a boolean, which is a compile-time guarantee.

**Recommendation**

Either replace the assertion with a meaningful check (for example, that the property does not throw and returns a consistent value across two calls) or remove the method.

**Compatibility Assessment**

- .NET Standard 2.0: Compatible
- .NET Framework 4.0: Compatible
- .NET Framework 3.5: Compatible
- .NET Framework 2.0: Compatible
- Compact Framework 3.5: N/A
- Public API: N/A

**Risk Level**

Minor

### Convert public mutable fields to properties in configuration classes

**Issue**

Several public configuration types expose mutable fields instead of properties. Examples include `Energy.Core.Shell.OptionStyle` (lines 20-60), `Energy.Base.Network.Client.Synchronisation` events (lines 604-608, 1183-1185), `Energy.Base.Plain.TableFormat` (lines 285-307), and `Energy.Query.Style` (lines 12-17). Public fields prevent the library from adding validation, change notification, or encapsulation later without a breaking change.

**Impact**

Public mutable fields reduce encapsulation and make the public API harder to evolve safely.

**Recommendation**

Convert the fields to auto-properties with public getters and setters. This is a breaking change for binary compatibility (field access becomes property access), so it must be deferred to a deliberate major-version or API-revision release, not included in this behaviour-preserving refactoring.

**Compatibility Assessment**

- .NET Standard 2.0: Breaking change (public API)
- .NET Framework 4.0: Breaking change (public API)
- .NET Framework 3.5: Breaking change (public API)
- .NET Framework 2.0: Breaking change (public API)
- Compact Framework 3.5: Breaking change (public API)
- Public API: Breaking change

**Risk Level**

Suggestion

**Example**

```csharp
// Before
public bool Slash = true;

// After
public bool Slash { get; set; }
```

## Defects Discovered During Analysis

The following issues are not refactorings because they alter observable behaviour. They should be tracked as separate change requests under `docs/change/26.6.1/` and implemented before or alongside this proposal if the user agrees.

### `Base/Variant.cs` `UInt32` property truncates to `UInt16`

**Issue**

`Base/Variant.cs` line 68 casts the value to `UInt16` inside the `UInt32` property.

```csharp
public UInt32 UInt32
{
    get { return Value == null ? (UInt16)0 : (UInt16)Value; }
    set { Value = value; }
}
```

**Impact**

Values larger than 65535 are silently truncated, producing incorrect results.

**Recommendation**

Change both casts to `UInt32`.

**Risk Level**

Critical

### `Base/Variant.cs` `Float` property casts to `char`

**Issue**

`Base/Variant.cs` line 98 casts the value to `char` inside the `Float` property.

```csharp
public float Float
{
    get { return Value == null ? (char)0 : (char)Value; }
    set { Value = value; }
}
```

**Impact**

The getter returns the character code of the boxed value instead of the floating-point value, which is completely incorrect for any float use case.

**Recommendation**

Change the casts to `0f` and `(float)Value`, or delegate to `Energy.Base.Cast.ObjectToFloat` if that helper exists.

**Risk Level**

Critical

## Plan

The plan is split into tidy steps and one structural step. Tidy steps remove dead code, comments, and redundant checks. The structural step is the test hardening, which changes test logic but not production behaviour. Each step leaves the codebase buildable and testable.

### Tidy Step 1: Remove dead code in `Core/Network.cs`, `Core/Bug.cs`, and `Base/Path.cs`

- Delete the unreachable `throw` at `Core/Network.cs` line 678.
- Delete the commented-out `Code` struct at `Core/Bug.cs` lines 168-234.
- Delete the commented-out `Split` overload at `Base/Path.cs` lines 23-42.

### Tidy Step 2: Remove dead code in `Base/Money.cs`

- Delete the commented-out ISO 4217 block at `Base/Money.cs` lines 228-400+.

### Tidy Step 3: Dispose local `MemoryStream` instances in `Base/Url.cs` and `Base/Class.cs`

- Wrap the `MemoryStream` in `Base/Url.Decode` in a `using` statement.
- Wrap the `MemoryStream` in `Base/Class.GetObjectSize` in a `using` statement.

### Tidy Step 4: Simplify redundant null checks in `Base/Binary.cs`

- Remove the redundant inner `if (left == null)` check in each `Compare` overload.

### Tidy Step 5: Document empty catch blocks in `Core/Tilde.cs`

- Add a short explanatory comment inside each intentionally empty catch block in the `ReadLine` region.

### Structural Step 6: Harden test reliability and isolation

- Remove or replace wall-clock timing assertions in `Energy.Core.Test/Base/Queue.cs`.
- Replace the fixed `Thread.Sleep` in `Energy.Core.Test/Base/File.cs` with a polling retry loop.
- Guard the environment variable mutation in `Energy.Core.Test/Base/Path.cs` with `try`/`finally`.
- Replace the external HTTP calls in `Energy.Core.Test/Core/Web.cs` with a local mock or loopback server.
- Complete or remove the empty `ClassAssembly` test in `Energy.Core.Test/Base/Class.cs`.
- Fix the namespace and remove boilerplate in `Energy.Core.Test/Base/Http.cs`.
- Remove or restore the commented-out tests in `Energy.Core.Test/Base/Path.cs`.
- Replace the tautological assertion in `Energy.Core.Test/Core/Tilde.cs` with a meaningful check or remove the method.

### Step 7: Verify the full loop

- Run `dotnet build Energy.Core/Energy.Core.csproj` and confirm zero errors and zero warnings.
- Run `dotnet test Energy.Core.Test/Energy.Core.Test.csproj` and confirm all tests pass.
- Confirm that the LZSS, ZX0, and other committed fixtures still decode and encode byte-for-byte.

## Risk

The lowest-risk changes are the dead-code removals and the redundant null-check simplifications. They are purely deletions and cannot change behaviour if the dead code is truly unreachable.

The resource-management changes are low risk but must be checked against the Compact Framework guard (`#if !NETCF`). Both affected methods are already behind that guard, so the `using` statement does not affect the Compact build.

The test-hardening changes carry the highest risk because they modify test logic. Removing the external HTTP dependency may require adding a test-only mock or local server, which is new infrastructure. The time-dependent tests need careful review to ensure that the functional assertions still cover the original intent.

The public-field-to-property conversion is explicitly deferred because it is a breaking API change.

## Acceptance Criteria

- `dotnet build Energy.Core/Energy.Core.csproj` succeeds with zero errors and zero warnings on every target framework.
- `dotnet test Energy.Core.Test/Energy.Core.Test.csproj` passes all tests.
- No public API signature changes, except for the deferred public-field proposal.
- The LZSS, ZX0, and other compression fixtures still produce byte-for-byte identical output.
- The dead code, unreachable code, and redundant checks identified in the findings are removed.
- The local `MemoryStream` instances in `Url.Decode` and `Class.GetObjectSize` are disposed deterministically.
- The test suite no longer depends on external network services, fixed `Thread.Sleep` durations, or unguarded environment variable mutations for the tests listed in the findings.
- The two `Variant.Union` defects are moved to separate change requests and not included in this refactoring.
