# Refactoring

This document is the authoritative source for how refactoring proposals and assessments are produced and evaluated for the Energy.Core library. GUIDELINES.md points here.

Energy.Core is a reusable class library that targets old .NET runtimes and the Compact Framework. Every refactoring must therefore be judged not only on code quality but also on backward compatibility, public API stability, and platform support. Read GUIDELINES.md first; it is the central source of development rules, and it defines the compatibility constraints that this document depends on.

## Scope

The purpose of a refactoring proposal is to improve correctness, reliability, maintainability, readability, and consistency while preserving observable behaviour and the public API wherever possible.

This document covers how to analyse the code, how to structure a proposal, how to classify risk, how to assess compatibility against every target framework, and how to write the mandatory assessment after the work is done. It does not change the workflow defined in WORKFLOW.md or the version rules in VERSIONING.md; it specialises them for refactoring.

## Definition

Refactoring is a behaviour-preserving change to internal structure. The same input must produce the same output, and the same observable behaviour must remain.

A change that alters observable behaviour is a feature or a fix, not a refactoring. It belongs in a change request under `docs/change/[version]/` and follows the cycle in WORKFLOW.md. Do not smuggle a behaviour change into a refactoring.

Where the library implements an external format, byte-for-byte output against the committed reference fixtures is part of the observable behaviour. A refactoring must keep that output identical.

## Refactor, Tidy, and Behaviour Change

Distinguish three kinds of edit and never mix them in a single step.

- Tidy steps are safe preparatory edits such as renames, dead-code removal, comment fixes, and formatting. They preserve behaviour and are low risk.
- Refactor steps such as extract, inline, move, split, and merge restructure code while preserving behaviour.
- Behaviour changes alter what the code does. They are out of scope here and route through a change request.

Keep tidy steps separate from structural steps so a regression is attributable to one step.

## General Rules

- Do not change observable behaviour unless the change addresses a defect, and a defect fix is a change request, not a refactoring.
- Do not introduce a breaking change without explicitly identifying and justifying it.
- Preserve compatibility with every supported target framework.
- Preserve the existing error-handling behaviour, including the `Energy.Core.Bug` diagnostics and the return-default-on-failure conversion patterns.
- Favour clarity, simplicity, and maintainability over unnecessary abstraction.
- Avoid speculative refactoring that provides no measurable benefit.
- Do not modify a working performance-critical path unless it is the explicit target of the proposal.
- Do not bump the version as part of a refactoring. A version bump is a separate release step. See VERSIONING.md.
- Every recommendation must include a clear rationale.

## Autonomous Analysis

When asked for a proposal, do not ask the user what to refactor. Analyse the recent changes, the current implementation, the existing guidelines, and good practice, then select the primary focus and document that analysis in the proposal.

The analysis must trace execution flow, map dependencies, and note side effects. It must cover code duplication, whether exact, functional, or intentional, and unused code, whether dead code, an incomplete refactor, or a bypassed legacy path. Verify that no indirect reference exists before recommending removal of any member.

## Rule of Three

Do not extract a shared abstraction until at least three concrete duplicates exist. Write the abstraction to match the real call sites, not a hypothetical future one.

Recommend extraction only when it improves maintainability without reducing readability.

## Proposal Document

A refactoring proposal contains the following sections in order. Each step in the plan must leave the codebase buildable and testable.

- Problem: the code smell, duplication, dead code, guideline violation, or structural issue. Include file paths and short quoted fragments. State which files the refactoring would touch. Document the analysis that led to this focus.
- Goal: the desired end state. What is the new shape of the types, members, or regions involved.
- Findings: a catalogue of the individual issues, each written in the finding structure below. Order findings by risk level, highest first.
- Plan: the steps in execution order for the accepted findings. Keep tidy steps separate from structural steps.
- Step Granularity: each step must be small enough that a regression it introduces is obvious from that step's build and test output. Split any step that touches many files or mixes a rename with an extraction.
- Risk: what could go wrong, which areas are most likely to regress, and which tests cover the affected code. Identify members with thin coverage that need characterisation tests first.
- Acceptance Criteria: concrete conditions for completion. Include identical observable behaviour, unchanged byte-for-byte output for the agreed fixtures, an unchanged public API unless a break is explicitly accepted, and a clean build and test run on every target framework.

## Finding Structure

Describe every identified issue with the following parts.

**Issue**

Describe the problem found in the code.

**Impact**

Explain the potential consequences, such as incorrect behaviour, runtime exceptions, resource leaks, reduced maintainability, reduced readability, performance degradation, thread-safety concerns, or API inconsistency.

**Recommendation**

Describe the proposed improvement and the rationale for it.

**Compatibility Assessment**

Evaluate the recommendation against every target framework and against the public API, using the values defined below.

**Risk Level**

Assign one of Critical, Major, Minor, or Suggestion, as defined below.

**Example**

Provide a short before-and-after code fragment demonstrating the issue and the proposed solution whenever applicable.

A finding written to this structure looks like this:

**Issue**

`Energy.Base.Text.Trim` calls `string.IsNullOrWhiteSpace`, which is unavailable on .NET 2.0, .NET 3.5, and the Compact Framework.

**Impact**

The library fails to compile on the legacy targets, so the affected build configurations break.

**Recommendation**

Expand the call to an explicit null and trim check, matching the rule in GUIDELINES.md.

**Compatibility Assessment**

- .NET Standard 2.0: Compatible
- .NET Framework 4.0: Compatible
- .NET Framework 3.5: Compatible
- .NET Framework 2.0: Compatible
- Compact Framework 3.5: Compatible
- Public API: Compatible

**Risk Level**

Critical

**Example**

```csharp
// Before
if (string.IsNullOrWhiteSpace(value)) { ... }

// After
if (value == null || value.Trim().Length == 0) { ... }
```

## Compatibility Assessment

Every recommendation must be assessed against the full target set and the public API. Use one of these values for each dimension.

- Compatible: the change builds and behaves identically on this target.
- Requires verification: the change is probably safe but must be confirmed by building or testing on this target before acceptance.
- Breaking change: the change alters behaviour or the public surface on this target.

Assess these dimensions for every finding.

- .NET Standard 2.0
- .NET Framework 4.0
- .NET Framework 3.5
- .NET Framework 2.0
- Compact Framework 3.5
- Public API

A recommendation is Compatible overall only when it compiles and behaves identically on the lowest common denominator, which is .NET Framework 2.0 and the Compact Framework 3.5. When in doubt, mark the dimension Requires verification rather than assuming success.

Any modification to a public class, interface, method, property, event, or enum must be marked explicitly as a potential breaking change, even when the modification looks cosmetic. Renames, signature changes, and accessibility changes on public members are breaking by default.

## Risk Level

Classify every finding with one of the following severities.

- Critical: issues that may cause data corruption, security vulnerabilities, resource leaks, application crashes, or undefined behaviour, including a build break on any target framework.
- Major: issues that significantly affect reliability, maintainability, performance, or public API quality.
- Minor: issues that affect readability, consistency, or style without significant technical risk.
- Suggestion: optional improvements that may enhance quality but are not required.

## Areas to Review

Review the code across the following areas. Each area lists what to look for, adapted to a legacy multi-target library.

**Correctness**

Look for missing argument validation, potential null reference issues, invalid assumptions about input, boundary and indexing errors, arithmetic overflow, division by zero, inconsistent state, and incorrect exception handling. Pay special attention to empty catch blocks, overly broad exception handling, and rethrow patterns that lose the stack trace, such as `throw exception` in place of a bare `throw`.

**Resource Management**

Look for undisposed `IDisposable` instances, stream and file handle leaks, database connection leaks, incorrect dispose patterns, and unnecessary finalizers. Resource ownership must be clear. Remember that Compact Framework builds cannot access `Stream.Dispose(bool)`, so streams are closed with `Close()`.

**Public API Design**

Review public types and members for naming consistency, design consistency, proper encapsulation, and accidental exposure of implementation details. Flag public fields that would be better as properties, but treat the change itself as a potential break. Keep summaries worded for external consumers, not for this project alone. Any change to a public member is a potential breaking change.

**Duplication and Maintainability**

Identify duplicate logic, duplicate algorithms, repeated validation, repeated object creation, and repeated conditional structures. Apply the Rule of Three before extracting. Extracted members follow the project naming and region conventions in GUIDELINES.md.

**Readability**

Review method size, class size, nesting depth, cyclomatic complexity, naming quality, and clarity of intent. Prefer code that is self-explanatory and minimises cognitive load.

**Performance**

Review for excessive allocations, object creation inside loops, unnecessary string concatenation, repeated reflection, inefficient collection usage, and excessive boxing and unboxing. Use `StringBuilder` for repeated concatenation and cache frequently used objects and delegates. Every performance recommendation must remain compatible with the target set.

**Thread Safety**

Public APIs must be thread-safe. Review static state, shared mutable state, synchronisation, and potential race conditions. Flag `lock(this)` and `lock(typeof(...))` and recommend a private lock object instead. Prefer holding all working state in per-call locals.

**Testability**

Look for hidden dependencies, excessive coupling, direct instantiation of dependencies, hard-coded external resources, time-dependent logic, and environment-dependent behaviour. Recommend improvements that increase isolation without adding unnecessary complexity. Tests must read committed fixtures, never paths under `work/`.

**Documentation**

Review XML documentation on public APIs for accuracy, outdated comments, and missing explanations of non-obvious logic. Comments should explain why something exists rather than restating what the code already does. Follow the XML comment formatting rules in GUIDELINES.md, including the `<br/><br/>` rule for multi-sentence summaries.

**Security**

Review for input validation gaps, injection vulnerabilities, unsafe file system operations, insecure cryptographic usage, and unsafe serialization. Flag any use of an outdated cryptographic algorithm.

## Legacy Platform Compatibility

Every recommendation must be evaluated against the supported platform set before acceptance. Do not introduce a feature that requires a newer language or framework capability than the lowest target supports unless it is explicitly approved and guarded with conditional compilation.

The following are unavailable or unsafe on one or more supported targets and must not appear in library code without an approved `#if` guard.

- LINQ, `dynamic`, `async`/`await`, the Task Parallel Library, concurrent collections, `Lazy<T>`, and modern reflection APIs.
- `nameof`, auto-property initializers, local functions, and `$@"..."` verbatim interpolated strings.
- `string.IsNullOrWhiteSpace`, `string.Contains`, and other members absent from the .NET 2.0 and Compact Framework base class libraries.
- `System.Reflection.Emit` and other runtime code generation, which break ahead-of-time compilation.

When referencing a type or static member declared in another type, use the fully qualified name, as required by GUIDELINES.md. If a recommendation depends on functionality that is unavailable on a supported platform, the finding must state that limitation in its Compatibility Assessment.

## Testing and Verification

Before refactoring a member, ensure it is covered by unit tests. If coverage is thin, add characterisation tests that capture the current behaviour first, so a regression introduced by the refactoring is visible.

After implementing a refactoring, run the canonical verification loop and repeat it until it is clean.

```
dotnet build Energy.Core/Energy.Core.csproj
dotnet test Energy.Core.Test/Energy.Core.Test.csproj
```

The implementation is complete only when the build and all tests pass with zero errors and zero warnings on every target framework. For any codec that implements an external format, confirm the output is still byte-for-byte identical to the committed reference fixtures. See TESTING.md for the full testing strategy.

Creating or updating a proposal or an assessment is a document-only change. It does not modify source and does not require the verification loop. The loop is required only once source, configuration, or test files change.

## Assessment

Creating an assessment after a refactoring is mandatory. It compares the result against the proposal and records the following.

- Proposal Summary: a brief recap of what was planned.
- Implementation Summary: what was actually implemented.
- Gap Analysis: what was missed, changed, or added relative to the proposal.
- Verification Results: the build and the full test run on every target framework, each with an explicit pass or fail.
- Metrics Comparison: before-and-after metrics where applicable, such as duplicated-block count, method size, or complexity of the affected members, with any growth explained.
- Output Fidelity: confirmation that the produced output for the agreed fixtures is unchanged.
- Conclusion: pass or fail, with recommendations.

## Directory Structure and Naming

Refactoring documents live under the current project version, consistent with the change and plan directories defined in WORKFLOW.md.

- Proposal: `docs/refactoring/[version]/[short-title]-proposal.md`
- Assessment: `docs/refactoring/[version]/[short-title]-assessment.md`

The `[version]` segment is the current project version. The `.csproj` files are the source of truth for that value, as stated in VERSIONING.md. A version bump does not move existing refactoring documents. The `[short-title]` is a kebab-case name describing the refactoring, matching the naming style already used for change requests and implementation plans.

## Anti-Patterns

The following are forbidden.

- Premature abstraction and speculative generality that anticipate needs the call sites do not yet have.
- Drive-by reformatting mixed into a structural step.
- A single step that mixes a rename with an extraction, or that touches many files at once.
- Test rewrites that hide a regression rather than expose it.
- Converting a failure into an inconclusive or passing test result.
- Time or duration estimates, which never appear in a proposal or assessment.

## Expected Outcome

A refactoring proposal must prioritise, in this order, correctness, reliability, compatibility, maintainability, readability, performance, and consistency.

Recommendations must be practical, evidence-based, and proportional to the value they provide.
