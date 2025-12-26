# Project Structure

This document describes the structure of the Energy.Core project.

## Main Library

### Energy.Core Directory

The `Energy.Core` directory contains the source code for the library.

### Project Files

- **Energy.Core.csproj** - The main C# source code project file for the library
- **Energy.Core.Compact.Legacy.csproj** - Old style C# project file for Compact Framework 2.0
- **Energy.Core.Compact.csproj** - C# project file for Compact Framework 3.5

## Solution Files

### Library Solution

- **Energy.Core.sln** - Solution file for the library only

### Test Solution

- **Energy.Core.Test.sln** - Test solution which includes the unit test project

## Test Project

- **Energy.Core.Test** - Unit test project for the library

## Other Directories

### Examples

- **Energy.Core.Example** - Contains example projects demonstrating library usage

### Documentation

- **docs** - Documentation files
- **nuget** - NuGet package specifications and media files

### Build Artifacts

- **download** - Archived build artifacts
- **packages** - NuGet package cache directory
