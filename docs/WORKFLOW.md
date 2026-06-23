# Development Workflow

This document describes how to work on the Energy.Core library.

It mirrors the process used by sibling projects, adapted for a class library that targets old .NET runtimes and the Compact Framework. Read GUIDELINES.md first; it is the central source of development rules.

## Standard Workflow Cycle

**Analysis**

- Use search tools to find relevant code.
- Read README.md, GUIDELINES.md, and STRUCTURE.md.
- Understand the existing class and namespace patterns before adding new ones.

**Research**

When researching an external format, algorithm, or specification:

- Read the current implementation to identify gaps and known limitations.
- Find the authoritative specification.
- Capture research notes describing the specification and any compatibility findings.
- Do not modify source code during research-only work.

**Planning**

- For any non-trivial change, write a change request and an implementation plan before touching source. See the Version-Based Implementation Cycle below.
- Wait for user approval when required.

**Implementation**

- Write code that compiles on every target framework, including the Compact Framework. See the compatibility rules in GUIDELINES.md.
- Maintain existing code style and naming conventions.
- Keep the public API surface meaningful for consumers, since this is a reusable library.

**Building**

- Build the library across all target frameworks:

```
dotnet build Energy.Core/Energy.Core.csproj
```

- Ensure zero errors and zero warnings.
- This step applies to code changes only. Documentation-only updates do not require a build.

**Testing**

- Run the unit tests:

```
dotnet test Energy.Core.Test/Energy.Core.Test.csproj
```

- Fix any regression immediately. No code change is complete until all tests pass.
- See TESTING.md for the full testing strategy.
- This step applies to code changes only. Documentation-only updates do not require running tests.

**Bug Fixing and Diagnostics**

- If a bug is hard to locate, add temporary diagnostics.
- Reproduce the issue with a test case.
- Fix the issue.
- Remove all temporary diagnostics once resolved.

**Verification**

- Final review of changes.
- Verify copyright compliance against COPYRIGHTS.md.

## Version-Based Implementation Cycle

When implementing a new feature or fix:

- Create a change request in `docs/change/[version]/[short-title].md` describing what and why. Use the template in `docs/template/change-request-template.md`.
- Create an implementation plan in `docs/plan/[version]/[short-title]-implementation.md` before modifying source code. Use the template in `docs/template/implementation-plan-template.md`.
- Implement the change, update or add tests, and run the build and test loop.
- The version value follows the rules in VERSIONING.md.

Source code may only be modified after both documents exist, and only when implementation has been explicitly requested.

## Artifact Naming Conventions

- Change requests: `docs/change/[version]/[short-title].md`
- Implementation plans: `docs/plan/[version]/[short-title]-implementation.md`

## Temporary Files

- Use the `work/` directory for transient files such as generated fixtures or scratch programs.
- Do not commit generated artifacts from `work/`.
- Delete temporary files you create once the task is complete.
- The reference ZX0 tools (`zx0.exe`, `dzx0.exe`) and similar provided utilities under `work/` are kept on purpose. Do not delete them.

## Test Fixtures

- Unit tests must read committed fixtures embedded in the test project's `Resources` directory.
- Never make a test depend on files under `work/` or any path outside the test project.
- When a fixture is produced with an external tool, generate it under `work/`, then copy the input and expected output into `Energy.Core.Test/Resources/` and register them as embedded resources.

## Commit Guidance

- Do not commit automatically. The user commits when ready.
- Do not use prefixes like `feat:`, `fix:`, `refactor:`, or `docs:` in commit messages.
- Keep commit messages short and descriptive.
