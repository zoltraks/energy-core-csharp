# [Short Imperative Title] Implementation Plan

## Change Request Reference

This implementation plan is based on the change request at `docs/change/[version]/[filename].md`.

Create the plan in `docs/plan/[version]/`, named after the change request with an `-implementation` suffix. Use the current project version from VERSIONING.md. Do not include time estimates.

## Best Practices

Follow the coding standards in GUIDELINES.md.

Pay particular attention to the .NET compatibility rules: the library code must compile on the Compact Framework and on .NET 2.0, so avoid APIs and language features that are unavailable there.

## Documentation Updates

Update documentation before modifying source code:

- GUIDELINES.md or STRUCTURE.md when a rule or the project layout changes.
- The project-energy technical documentation when the public API changes.

## Step by Step Implementation

Define one bold-headed step per logical unit of change.

**[Step name]**

Describe what changes and why.

File to modify: `Energy.Core/[Namespace]/[Type].cs`.

    // Code fragment showing the structural change

## Implementation Order

Execute the steps above in a deterministic sequence. A typical order is:

- Update documentation
- Update or add types and interfaces
- Implement the core logic
- Add or update unit tests
- Run the verification loop

## Testing Strategy

Specify the unit tests for new functionality, edge cases, and error conditions. Where the change implements an external format, specify the reference-compatibility fixtures. See TESTING.md.

## Verification

The implementation is complete only when all steps pass with zero errors and zero warnings.

- Build: `dotnet build Energy.Core/Energy.Core.csproj`
- Test: `dotnet test Energy.Core.Test/Energy.Core.Test.csproj`
- Manually verify the specific behaviour that changed.
