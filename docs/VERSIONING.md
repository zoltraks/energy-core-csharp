# Versioning Rules

This document is the authoritative source for how Energy.Core is versioned. GUIDELINES.md and other documents point here.

## Version Number Format

Energy.Core uses a calendar-driven format: **YY.M.R**

- **YY**: two-digit current year (for example, 26 for 2026).
- **M**: month number, written without a leading zero (1 for January, 12 for December).
- **R**: release counter within the current year and month, starting at 0.

Examples:

- `26.1.0` is the first release in January 2026.
- `26.1.1` is the second release in January 2026.
- `26.2.0` is the first release in February 2026.

## Source of Truth

The `.csproj` files are the authoritative source for the current version number.

This document defines the rules and process only. It does not list the current version, to avoid drift.

## Version Bump Process

Only bump the version when explicitly instructed to do so. A version bump is a release action, not part of implementing a feature or fix. Do not bump the version as a side effect of any other task.

When bumping the version, update all three version properties together in both project files:

**Energy.Core/Energy.Core.csproj**

- `<Version>`
- `<FileVersion>`
- `<AssemblyVersion>`

**Energy.Core.Test/Energy.Core.Test.csproj**

- `<Version>`
- `<FileVersion>`
- `<AssemblyVersion>`

Set all three to the same value in both projects.

Determine the value from the current date and the release counter:

- For the first release in a given month, set R to 0.
- For a later release in the same month, increment R by 1.
- Update YY and M to the current year and month when they change; R restarts at 0 whenever YY or M changes.

After updating the properties, build across all target frameworks and confirm zero errors and zero warnings:

```
dotnet build Energy.Core/Energy.Core.csproj
```

The version bump should be the last change before a release tag.

## NuGet Package

To produce a NuGet package for a release:

- Clone the latest existing `.nuspec` from `nuget/` (for example `Energy.Core.26.1.0.nuspec`), update the `<version>` element to the new value, and save it as `Energy.Core.YY.M.R.nuspec`.
- Build in Release configuration.
- Pack to the `nuget/` directory.
- Confirm `Energy.Core.YY.M.R.nupkg` exists and contains all target framework binaries and the XML documentation.

## Release Checklist

- All unit tests pass.
- Build succeeds with zero warnings.
- Documentation is updated if the public API changed.
- The project-energy technical documentation reflects any public API change.
