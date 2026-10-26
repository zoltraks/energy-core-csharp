# Energy.Core - Lens audit report

## Document Information

| Field              | Value                                                         |
|--------------------|---------------------------------------------------------------|
| Report Revision    | 1.5                                                           |
| Report Date        | 2026-10-26                                                    |
| Report Style       | audit                                                         |
| Detail Level       | Detailed                                                      |
| Evaluation Scale   | 1-10                                                          |
| Time taken         | 00:19                                                         |
| Language           | English                                                       |
| Audit Purpose      | Engineering improvement                                       |
| Target Environment | Multi-target .NET class library (`netstandard2.0` to `net20`) |
| Verification Scope | static repository analysis - no execution                     |
| Evidence Mode      | source-only                                                   |
| Subject Revision   | 9447e87bd63c03b7fd7da3806b646f3b2ae0bcf6 (branch develop)     |
| Dirty-Tree State   | Untracked `docs/audit/` report output only                    |
| Skill Version      | 2.4.7                                                         |
| Projects           | Energy.Core/, Energy.Core.Test/, Energy.Core.Compact/         |

## Audit Type Coverage

| Report type                                       | Status    | Rationale                                                        |
|---------------------------------------------------|-----------|------------------------------------------------------------------|
| Software Architecture Review                      | Covered   | Structure, layering, and trade-offs inspected                    |
| Code Quality Audit                                | Covered   | Source inspection across all three projects                      |
| Security Vulnerability Assessment                 | Covered   | Manual review with [CWE](#glossary) mapping, no scanner run      |
| Open Source License Compliance                    | Covered   | License texts, manifests, and declarations inspected             |
| Penetration Test                                  | Not done  | Source-only scope never executes the subject                     |
| Performance Audit                                 | Partially | Design reviewed, no load measurement possible                    |
| Cloud Infrastructure Audit                        | Partially | No IaC or pipeline files exist to inspect                        |
| Technical Due Diligence                           | Partially | Engineering pillars covered, interviews and business data absent |
| [SBOM](#glossary) / Software Composition Analysis | Covered   | Manifest-derived inventory, no advisory lookups                  |
| ISO/IEC 27001 Certification                       | Not done  | Standard used as rubric only                                     |
| SOC 2 Attestation Examination                     | Not done  | Framework used as coverage checklist only                        |

The AI Governance Audit row is omitted, the subject carries no AI dependency.

## Project Inventory

| Project             | Path                   | Version | Description                                                      |
|---------------------|------------------------|---------|------------------------------------------------------------------|
| Energy.Core         | `Energy.Core/`         | 26.7.0  | Multi-target .NET class library, published on [NuGet](#glossary) |
| Energy.Core.Test    | `Energy.Core.Test/`    | 26.7.0  | [MSTest](#glossary) suite targeting `net48`                      |
| Energy.Core.Compact | `Energy.Core.Compact/` | n/a     | .NET Compact Framework preview project and solutions             |

## Glossary

| Term    | Definition                                                                                          |
|---------|-----------------------------------------------------------------------------------------------------|
| ADO.NET | .NET data-access framework abstracted by `Energy.Source`                                            |
| ADR     | Architecture Decision Record, a durable design-decision record                                      |
| API     | Application Programming Interface                                                                   |
| BCL     | Base Class Library, the .NET built-in framework surface                                             |
| CF      | .NET Compact Framework, the legacy Windows CE runtime target                                        |
| CI      | Continuous Integration                                                                              |
| CRC     | Cyclic Redundancy Check, a checksum algorithm                                                       |
| CVSS    | Common Vulnerability Scoring System                                                                 |
| CWE     | Common Weakness Enumeration                                                                         |
| DORA    | DevOps Research and Assessment metrics program                                                      |
| DTD     | Document Type Definition, an XML feature behind XXE exposure                                        |
| IV      | Initialization Vector, a cryptographic nonce input                                                  |
| MITM    | Man-in-the-Middle attack                                                                            |
| MSTest  | Microsoft test framework used by `Energy.Core.Test`                                                 |
| NETCF   | Conditional-compilation symbol marking Compact Framework paths                                      |
| NFR     | Non-Functional Requirement                                                                          |
| NuGet   | The .NET package manager and gallery                                                                |
| RPO     | Recovery Point Objective                                                                            |
| RTO     | Recovery Time Objective                                                                             |
| SBOM    | Software Bill of Materials                                                                          |
| SLO     | Service-Level Objective                                                                             |
| SQL     | Structured Query Language                                                                           |
| STRIDE  | Spoofing, Tampering, Repudiation, Information Disclosure, Denial of Service, Elevation of Privilege |
| TLS     | Transport Layer Security                                                                            |
| XML     | Extensible Markup Language                                                                          |
| XXE     | XML External Entity injection                                                                       |

**Identifier triplets**

| Triplet | Scope             | Description                        |
|---------|-------------------|------------------------------------|
| FND     | Findings          | Technical finding block identifier |
| EVD     | Evidence          | Evidence ledger row identifier     |
| TDR     | Debt              | Technical Debt Register entry      |
| RSK     | Risks             | Unified Risk Register entry        |
| REC     | Remediation       | Remediation Roadmap recommendation |
| PAR     | Validation        | Validation Record checklist row    |
| AIP     | Pillar, criterion | AI provenance and code origin      |
| API     | Pillar, criterion | API compatibility and versioning   |
| ARC     | Pillar, criterion | Architecture and design            |
| CPR     | Pillar, criterion | Copyrights and originality         |
| CQY     | Pillar, criterion | Code quality                       |
| DEP     | Criterion family  | Dependency criteria                |
| INF     | Pillar, criterion | Infrastructure and CI/CD           |
| MNT     | Criterion family  | Maintainability criteria           |
| OBS     | Criterion family  | Observability criteria             |
| SEC     | Pillar, criterion | Security and compliance            |
| STR     | Pillar, criterion | Structure and organization         |
| STY     | Criterion family  | Code-style criteria                |
| TST     | Criterion family  | Testability criteria               |

## Executive Summary

`1-3 Poor · 4-6 Average · 7-8 Good · 9-10 Excellent`

| Project             | Score            | Lowest                          | Risks                                                                                                                       | Readiness |
|---------------------|------------------|---------------------------------|-----------------------------------------------------------------------------------------------------------------------------|-----------|
| Energy.Core         | 5.6/10 (Average) | Security 4, API Compatibility 4 | Energy.Core/::RSK-001 to [Energy.Core/::RSK-005](#rsk-005-silent-corruption-via-swallowed-errors-and-crc-defect-energycore) | Not ready |
| Energy.Core.Test    | 5.8/10 (Average) | Delivery & Continuity 4         | Energy.Core/::RSK-001, Energy.Core/::RSK-002                                                                                | Not ready |
| Energy.Core.Compact | 4.5/10 (Average) | Deployability 3, Testability 3  | Energy.Core/::RSK-001 to Energy.Core/::RSK-003                                                                              | Not ready |

The audited tree is the same `9447e87` revision covered by the `26.7.0` report series, with
`Energy.Core.Compact/` newly in scope.

The library remains well-documented and analyzer-disciplined, but the security surface still
carries unguarded legacy-era [APIs](#glossary) and no [CI](#glossary) exists to run the documented build and test loop.

No overall score in this report overrides a material risk or an open readiness gate.

## Energy.Core/

### Executive Summary

| Field          | Value                                        |
|----------------|----------------------------------------------|
| System type    | Codebase, published class library            |
| Scope          | `Energy.Core/` plus root configuration files |
| Source basis   | Inspected code, revision `9447e87`           |
| Maturity level | Production-ready                             |
| Overall score  | 5.5/10 (Average), lowest: Security 4/10      |

**Summary description**

Energy.Core is a zero-runtime-dependency .NET utility library spanning type conversion, text and
binary utilities, compression, [SQL](#glossary) construction, [ADO.NET](#glossary) abstraction, logging, HTTP, and cipher
helpers.

The tree is internally consistent, analyzer-disciplined, and documented, but verification remains
procedural rather than enforced.

The maturity level is `Production-ready` in the descriptive sense, the package is published on
[NuGet](#glossary) with release history since 2018, this is not an audit sign-off.

**Production Readiness Threshold**

For this library, `Production-ready` sign-off would require closure evidence on the items below,
none of which is currently met in the inspected tree.

- [CI](#glossary) evidence that `dotnet build` and `dotnet test` run green on every commit, addressing [RSK-006](#rsk-006-undetected-regressions-ship-energycore).
- Obsolescence or documented scoping on `Web.IgnoreCertificateValidation` and the DES/MD5/SHA1
  surface, addressing [RSK-001](#rsk-001-process-wide-mitm-exposure-via-tls-bypass-energycore) and [RSK-002](#rsk-002-legacy-primitive-selection-by-consumers-energycore).
- Output-size limits or documented caller obligations on decompressors, addressing [RSK-003](#rsk-003-decompression-bomb-resource-exhaustion-energycore).
- An [API](#glossary) compatibility gate or tracked surface baseline, addressing [RSK-011](#rsk-011-silent-breaking-changes-energycore).
- Reconciled license declarations across `LICENSE.txt`, docs, csproj, and nuspec,
  addressing [RSK-012](#rsk-012-license-identity-ambiguity-for-consumers-energycore).

These are proposed acceptance criteria, not stakeholder-approved requirements.

**Evidence And Decision Limits**

The documented checks `dotnet build Energy.Core/Energy.Core.csproj` and
`dotnet test Energy.Core.Test/Energy.Core.Test.csproj` were not run, per the source-only scope.

A committed assessment document reports a four-target clean build and 341 passing tests,
treated as `Reported` evidence, not verified.

**Readiness Cost**

`NOT SPECIFIED` - no cost, staffing, or timeline data exists in the repository.

### System Context

| Aspect                 | Detail                                                                                  |
|------------------------|-----------------------------------------------------------------------------------------|
| Functional description | Utility library, conversions, text, compression, [SQL](#glossary), [ADO.NET](#glossary) |
| Architecture overview  | Single assembly, layered namespaces, conditional compilation                            |
| Key components         | `Energy.Base`, `Energy.Core`, `Energy.Query`, `Energy.Source`                           |
| External dependencies  | [BCL](#glossary) only, no runtime packages, [ADO.NET](#glossary) providers via callers  |
| Assumptions            | `NOT SPECIFIED` beyond the stated legacy-target constraint                              |

### Technology Stack

| Layer            | Technology                                                               |
|------------------|--------------------------------------------------------------------------|
| Languages        | C#, `LangVersion latest` constrained by legacy targets                   |
| Frameworks       | .NET Framework 2.0-4.0, .NET Standard 2.0, [NETCF](#glossary) code paths |
| Runtime/Platform | .NET runtimes plus Windows CE                                            |
| Build tooling    | MSBuild SDK-style csproj, SourceLink, deterministic builds               |
| Test tooling     | [MSTest](#glossary) 4.0.2, Microsoft.NET.Test.Sdk 17.8.0, target `net48` |
| Package manager  | [NuGet](#glossary), `.nuspec` release series 18.12.x to 26.7.0           |
| Key libraries    | None at runtime, SourceLink.GitHub 8.0.0 build-time only                 |
| Data stores      | [ADO.NET](#glossary) via `Energy.Source`, deployment `NOT SPECIFIED`     |
| Target platforms | [NuGet](#glossary) package, four framework groups                        |

Each row is anchored to `Energy.Core/Energy.Core.csproj` and `nuget/Energy.Core.26.7.0.nuspec`.

### Software Bill of Materials

| Component                   | Version | License | Risk       | Advisory | Source                           |
|-----------------------------|---------|---------|------------|----------|----------------------------------|
| Microsoft.SourceLink.GitHub | 8.0.0   | MIT     | Build-only | N        | `Energy.Core/Energy.Core.csproj` |

The inventory is manifest-derived.

The library declares zero runtime dependencies.

`PrivateAssets="All"` scopes SourceLink to build time.

No lockfile exists, the resolved package graph is `UNKNOWN` beyond the direct row.

### License Compliance

| License class   | Components | Notes                                             |
|-----------------|------------|---------------------------------------------------|
| Permissive      | 2          | `LICENSE.txt` BSD-3-Clause text, SourceLink (MIT) |
| Weak-copyleft   | 0          |                                                   |
| Strong-copyleft | 0          |                                                   |
| Proprietary     | 0          |                                                   |
| Unknown         | 0          |                                                   |

`LICENSE.txt` grants BSD-3-Clause terms while the nuspec declares `LGPL-3.0-or-later` and the
csproj points at the LGPL URL, recorded as Energy.Core/::FND-CPR-003.

Observed license facts are separated from concerns, no legal conclusion is drawn.

### Health Dashboard

**Risk Heat Map**

| Impact   | LOW                                                        | MEDIUM                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   | HIGH |
|----------|------------------------------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|------|
| CRITICAL |                                                            |                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          |      |
| HIGH     |                                                            |                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          |      |
| MEDIUM   | [RSK-010](#rsk-010-provenance-ambiguity-energycore)        | [RSK-002](#rsk-002-legacy-primitive-selection-by-consumers-energycore), [RSK-004](#rsk-004-sql-injection-via-escaping-bypass-energycore), [RSK-005](#rsk-005-silent-corruption-via-swallowed-errors-and-crc-defect-energycore), [RSK-007](#rsk-007-continuity-and-knowledge-concentration-energycore), [RSK-008](#rsk-008-attribution-and-license-exposure-energycore), [RSK-009](#rsk-009-per-target-behavioral-divergence-energycore), [RSK-011](#rsk-011-silent-breaking-changes-energycore), [RSK-012](#rsk-012-license-identity-ambiguity-for-consumers-energycore) |      |
| LOW      | [RSK-006](#rsk-006-undetected-regressions-ship-energycore) | [RSK-001](#rsk-001-process-wide-mitm-exposure-via-tls-bypass-energycore), [RSK-003](#rsk-003-decompression-bomb-resource-exhaustion-energycore)                                                                                                                                                                                                                                                                                                                                                                                                                          |      |

Impact is read on the vertical axis and likelihood on the horizontal axis.

All twelve risks are rated and placed.

**Scorecard Summary**

`1-3 Poor · 4-6 Average · 7-8 Good · 9-10 Excellent`

| Dimension               | Score   | Notes                                                                      |
|-------------------------|---------|----------------------------------------------------------------------------|
| Testability             | 6       | Quality tests, large untested surface                                      |
| Design Soundness        | 6       | Coherent namespaces, global state, monolith                                |
| Code Quality            | 5       | Consistent style, empty catches, [CRC](#glossary) bound defect             |
| Code Style              | 7       | Declared conventions followed in sampled files                             |
| Stack Alignment         | 7       | Analyzers on, warnings-as-errors, legacy idioms                            |
| Dependency Health       | 9       | Zero runtime dependencies                                                  |
| Maintainability         | 6       | Strong change docs, concentrated authorship                                |
| Structure               | 6       | Layout matches `docs/STRUCTURE.md`, binary litter                          |
| Deployability           | 5       | [NuGet](#glossary) series, no automation, one tag                          |
| Scalability             | 5       | [NFR](#glossary) evidence thin, no budgets                                 |
| Security                | 4       | [TLS](#glossary) bypass [API](#glossary), legacy crypto, unbounded inflate |
| Compliance              | 5       | License texts contradict, attribution gap                                  |
| Observability           | 6       | Log and Bug frameworks present                                             |
| Operational Safety      | 5       | Library context, release ops manual                                        |
| Delivery & Continuity   | 4       | Single-author concentration, one tag                                       |
| AI Provenance           | UNKNOWN | No provenance baseline exists                                              |
| Originality & Licensing | 4       | License identity ambiguous, attribution gap                                |
| API Compatibility       | 4       | Untracked surface, no gate, sparse tag practice                            |

Overall score is 5.5/10, the unweighted mean of the 17 scored dimensions, 94 divided by 17,

`N/A` and `UNKNOWN` excluded from the denominator.

The lowest-scoring applicable dimensions are Security, Delivery & Continuity, Originality &
Licensing, and API Compatibility, each 4/10.

**Team & Continuity**

Git history spans 2016-02-28 to 2026-09-16 with 1,265 commits, 1,044 of them non-merge,
964 of the 1,044 non-merge commits under the top author name and three further contributor names,
and one tag (`18.12.8`), per [EVD-006](#auditing-methodology).

This is an aggregate continuity signal only, it is not a measure of access or expertise.

### Delivery Practice & Team Continuity

| [DORA](#glossary) metric        | Result          | Basis                                 |
|---------------------------------|-----------------|---------------------------------------|
| Change lead time                | `NOT SPECIFIED` | One tag, no commit-to-tag cadence     |
| Deployment frequency            | `NOT SPECIFIED` | Tag cadence uncomputable from one tag |
| Failed deployment recovery time | `NOT SPECIFIED` | Requires incident and deployment data |
| Change fail rate                | `NOT SPECIFIED` | Requires incident and rollback data   |
| Deployment rework rate          | `NOT SPECIFIED` | Requires production incident data     |

Computed values would be proxies, the single tag makes both proxies uncomputable.

**Contributor concentration**

Top-author share is 964 of 1,044 non-merge commits (92 percent) across a 2016 to 2026 window
with four author names, a `HIGH` concentration rating, per [EVD-006](#auditing-methodology).

### High-Level Observations

| Observation                                                                     |
|---------------------------------------------------------------------------------|
| No [CI](#glossary) exists while the tree carries a demonstrated defect incident |
| Security-relevant [APIs](#glossary) lack guardrails or warnings                 |
| One-third of public modules have tests, core utility coverage is thin           |
| The public [API](#glossary) surface is untracked and unversioned at the gate    |
| Conditional compilation silently changes behavior per target                    |

**No [CI](#glossary) exists while the tree carries a demonstrated defect incident**

`docs/WORKFLOW.md` documents a build and test loop but nothing executes it automatically,
per [FND-INF-001](#fnd-inf-001-no-continuous-integration-energycore).

Commit `78c14d5` introduced deliberately broken code that required `9447e87` to revert,
per [FND-INF-003](#fnd-inf-003-change-control-evidence-gap-energycore).

**Security-relevant [APIs](#glossary) lack guardrails or warnings**

A static property disables [TLS](#glossary) validation process-wide, DES reuses key bytes as [IV](#glossary) under a
supplied secret, and decompressors have no output cap, per [FND-SEC-001](#fnd-sec-001-global-tls-certificate-validation-bypass-api-energycore), [FND-SEC-002](#fnd-sec-002-legacy-cryptographic-primitives-without-warnings-energycore), and
[FND-SEC-003](#fnd-sec-003-unbounded-decompression-output-growth-energycore).

**One-third of public modules have tests, core utility coverage is thin**

50 test classes against 164 library source files, with no tests for `Cipher`, `Log`, `Ini`,
`Yaml`, or most of `Query` and `Source`, per [FND-CQY-002](#fnd-cqy-002-untested-public-surface-energycore).

**The public [API](#glossary) surface is untracked and unversioned at the gate**

No `PublicAPI` baseline, no `EnablePackageValidation`,
and a single git tag against 41 nuspec releases, per [FND-API-001](#fnd-api-001-untracked-public-surface-no-compatibility-gate-energycore) and [FND-API-002](#fnd-api-002-versioning-practice-diverges-from-declared-scheme-energycore).

**Conditional compilation silently changes behavior per target**

167 `#if` directives and `NETCF` paths that return `null` or no-op where other targets act,
per [FND-ARC-003](#fnd-arc-003-silent-per-target-capability-divergence-energycore).

### Auditing Methodology

**Methodology overview**

The audit applies evidence-based reasoning across the core assessment categories grouped into
pillars, plus conditional assessments where the subject warrants them.

Conditional sections present for this project: Data Flow Diagram, Design Patterns,
Architecture Decision Records, Threat Model, Standards Conformance, Engineering Baseline
Conformance, Structure Conformance, Code Style, API Compatibility & Versioning Discipline,
Technical Debt Register, and Recommendation Classification.

**Criteria table**

| Criteria    | Family           | Applicability     | Criticality | Method                  | Sufficiency             |
|-------------|------------------|-------------------|-------------|-------------------------|-------------------------|
| TST-UNIT    | TST              | all targets       | high        | test census vs surface  | class and method counts |
| SEC-TLS     | SEC              | all targets       | critical    | source inspection       | code path read          |
| SEC-CRYPTO  | SEC              | all targets       | high        | source inspection       | code path read          |
| SEC-DECOMP  | SEC              | binary input      | high        | source inspection       | code path read          |
| SEC-SQL     | SEC              | database paths    | high        | source inspection       | code path read          |
| ARC-MOD     | ARC              | all code          | medium      | structure census        | module map              |
| ARC-STATE   | ARC              | all code          | medium      | static census           | statics enumerated      |
| ARC-DIVERGE | ARC              | multi-target      | medium      | `#if` census            | counts, sampled paths   |
| CQY-CATCH   | CQY              | all code          | medium      | catch census            | counted clauses         |
| CQY-DOC     | CQY              | public surface    | low         | doc-comment sampling    | sampled files           |
| DEP-MGMT    | DEP              | all manifests     | medium      | manifest read           | all refs listed         |
| INF-CICD    | INF              | repository        | high        | [CI](#glossary) census  | tree search             |
| INF-REL     | INF              | release path      | medium      | tag and artifact census | counts                  |
| CPR-LIC     | CPR              | license artifacts | high        | declaration cross-check | all declarations read   |
| CPR-ATTR    | CPR              | third-party code  | medium      | attribution review      | document read           |
| AIP-POLICY  | AIP              | repository        | low         | docs and history search | no hits found           |
| API-TRACK   | [API](#glossary) | published package | high        | artifact search         | no baseline found       |
| API-VER     | [API](#glossary) | release artifacts | medium      | version cross-check     | csproj, nuspec, tags    |
| STR-LAYOUT  | STR              | repository        | low         | directory census        | tree compared           |
| STY-NAME    | STY              | all code          | low         | style sampling          | sampled files           |
| OBS-LOG     | OBS              | logging surface   | low         | source inspection       | Log.cs read             |

The applicable denominator froze before finding review, no post-freeze changes were made.

**Reference standards**

- ISO/IEC 25010:2023, nine-characteristic product quality coverage, used as a crosswalk only.
- OWASP ASVS awareness level for control classes, no level claimed.
- [CWE](#glossary) and [CVSS](#glossary) vocabulary for security findings, [CVSS](#glossary) not assigned for library-context findings.
- OWASP Top 10 as awareness taxonomy, relevant categories named in Scope Exclusions.
- NIST SP 800-30 for the risk register process, [STRIDE](#glossary) for the threat model.
- CISQ and SQALE-inspired cost separation for the Technical Debt Register.
- ISO 19011 monitoring guidance for the Re-audit and Follow-up Plan.
- Microsoft Framework Design Guidelines and .NET library guidance, selected per
  `references/stack-standards.md`.
- [NuGet](#glossary) package authoring practice, Package Validation, ApiCompat, PublicApiAnalyzers, and
  .NET breaking-change rules for the API Compatibility pillar.
- .NET security analyzer documentation for CWE-to-analyzer cross-references.

Only standards actually applied are cited.

**Audit evidence statement**

This audit inspected 164 library source files, 54 test-project source files holding 50 test
classes and 344 `[TestMethod]` methods, the Compact preview project, and approximately 30
configuration and metadata files.

`.gitignore` exclusions were applied, `/work/`, `Documentation/html/`, `packages/`,
`TestResults/`, and `obj/` content rules were honored.

Git history was examined in full, 1,265 commits of which 1,044 are non-merge.

No builds, tests, or tools were executed, all findings rest on static inspection.

**Read depth**

| Area                   | Files | Read depth                                           |
|------------------------|-------|------------------------------------------------------|
| `Energy.Core/`         | 172   | sampled plus full reads on security and census paths |
| `Energy.Core.Test/`    | 99    | listed plus sampled                                  |
| `Energy.Core.Compact/` | 10    | full                                                 |
| Root configuration     | 10    | full                                                 |
| `docs/`                | 25    | full                                                 |

**Verification And Evidence Ledger**

| Evidence | Project             | Source                                   | Result                                                  | Type        | Artifact                                       |
|----------|---------------------|------------------------------------------|---------------------------------------------------------|-------------|------------------------------------------------|
| EVD-001  | Energy.Core         | Source inventory                         | 164 lib sources, 50 test classes, 344 tests             | Observation | `Energy.Core/`, `Energy.Core.Test/`            |
| EVD-002  | Energy.Core         | Project file                             | 4 targets, v26.7.0, analyzers on                        | Observation | `Energy.Core/Energy.Core.csproj`               |
| EVD-003  | Energy.Core.Test    | Project file                             | net48, [MSTest](#glossary) 4.0.2, 13 package refs       | Observation | `Energy.Core.Test/Energy.Core.Test.csproj`     |
| EVD-004  | Repository          | `.gitignore`, tracked files              | work/, nupkg ignored, binaries tracked                  | Observation | `.gitignore`, `nuget/`, `download/`            |
| EVD-005  | Repository          | Standards documents                      | 7 standards docs read                                   | Observation | `docs/*.md`                                    |
| EVD-006  | Repository          | Git history                              | 1,265 commits, 1,044 non-merge, 1 tag                   | Observation | `git log`, `git tag`, `git rev-list`           |
| EVD-007  | Repository          | Previous report discovery                | `AUDIT-1.0.md` to `AUDIT-1.4.md` found                  | Observation | `docs/audit/26.7.0/`                           |
| EVD-008  | Repository          | Reported test artifacts                  | Reported 341 pass, 4-target build                       | Observation | `docs/refactoring/26.6.1/` assessment          |
| EVD-009  | Energy.Core         | [TLS](#glossary) bypass inspection       | Opt-in global callback returns true                     | Observation | `Energy.Core/Core/Web.cs` L55-L111             |
| EVD-010  | Energy.Core         | Cipher inspection                        | DES IV equals key, MD5, SHA1 exposed                    | Observation | `Energy.Core/Core/Cipher.cs` L15-L185          |
| EVD-011  | Energy.Core         | Hash inspection                          | CRC32 bound defect, [NETCF](#glossary) nulls            | Observation | `Energy.Core/Base/Hash.cs` L62-L75, L343-L428  |
| EVD-012  | Energy.Core         | Xml inspection                           | Bare catch in deserialize loop                          | Observation | `Energy.Core/Base/Xml.cs` L217-L240            |
| EVD-013  | Energy.Core         | Random inspection                        | `System.Random` only                                    | Observation | `Energy.Core/Base/Random.cs` L16-L161          |
| EVD-014  | Energy.Core         | [SQL](#glossary) escaping inspection     | Default `''` doubling verified                          | Observation | `Base/Bracket.cs` L389-L398, `Query/Format.cs` |
| EVD-015  | Energy.Core         | Parameter subsystem                      | Text substitution, no DbParameter                       | Observation | `Query/Parameter.cs`, `Source/Connection.cs`   |
| EVD-016  | Energy.Core         | Compression inspection                   | Unbounded growth in ZX0, LZ4, LZSS                      | Observation | `Base/Compression.cs` L529-L1920               |
| EVD-017  | Energy.Core         | Catch-pattern census                     | 223 catch tokens, ~41 whitespace-empty                  | Observation | `Program.cs`, `Cast.cs`, `Tilde.cs`            |
| EVD-018  | Repository          | [CI](#glossary) and tooling search       | No pipeline, [SBOM](#glossary), or scan artifacts       | Observation | Full-tree listing                              |
| EVD-019  | Energy.Core         | [NuGet](#glossary) spec                  | LGPL-3.0-or-later declared                              | Observation | `nuget/Energy.Core.26.7.0.nuspec`              |
| EVD-020  | Energy.Core         | Credential handling                      | `ToString()` excludes credentials                       | Observation | `Source/Configuration.cs` L172-L210            |
| EVD-021  | Repository          | Change-request coverage                  | Only `26.6.1/` change and plan entries                  | Observation | `docs/change/`, `docs/plan/`                   |
| EVD-022  | Energy.Core         | `#if` census                             | 167 `#if`, 19 `#elif`/`#else`                           | Observation | Library-wide                                   |
| EVD-023  | Energy.Core         | Dangerous-code census                    | No unsafe blocks or secrets in source                   | Observation | grep census                                    |
| EVD-024  | Repository          | AI-provenance search                     | No attribution markers found                            | Observation | `git log --format`, working tree               |
| EVD-025  | Repository          | License and attribution                  | BSD-3-Clause file, third-party section empty            | Observation | `LICENSE.txt`, `docs/COPYRIGHTS.md`            |
| EVD-026  | Repository          | Tree state                               | Clean except untracked `docs/audit/`                    | Observation | `git status`, `git rev-parse`                  |
| EVD-027  | Repository          | Metrics tooling                          | SourceMonitor project tracked                           | Observation | `Energy.smproj`                                |
| EVD-028  | Energy.Core         | Analyzer enablement                      | NetAnalyzers latest, warnings as errors                 | Observation | csproj L41-L49, `.editorconfig`                |
| EVD-029  | Energy.Core         | [API](#glossary) compatibility artifacts | No PublicAPI baseline or gate                           | Observation | csproj, `git ls-files`                         |
| EVD-030  | Energy.Core         | Deprecation markers                      | `[Obsolete]` used in 9 files                            | Observation | `Web.cs`, `Text.cs`, `Application.cs`          |
| EVD-031  | Energy.Core         | Documented build command                 | NOT RUN - declared, source-only scope                   | Observation | `docs/WORKFLOW.md`                             |
| EVD-032  | Energy.Core.Test    | Documented test command                  | NOT RUN - declared, source-only scope                   | Observation | `docs/TESTING.md`                              |
| EVD-033  | Energy.Core         | Manifest-derived inventory               | 1 build-scope, 13 test-scope directs                    | Observation | csproj files                                   |
| EVD-034  | Energy.Core         | Marker and hygiene census                | 15 TODO/HACK, 1 zero-byte test file                     | Observation | `Attribute/`, `Core/Shell.cs`                  |
| EVD-035  | Repository          | License-text cross-check                 | BSD-3-Clause file vs LGPL declarations                  | Observation | `LICENSE.txt`, csproj, nuspec, `COPYRIGHTS.md` |
| EVD-036  | Energy.Core.Compact | Compact project census                   | MSBuild 2003, ToolsVersion 3.5, [NETCF](#glossary) v2.0 | Observation | `Energy.Core.Compact.Preview.csproj`           |
| EVD-037  | Energy.Core.Compact | Release artifacts                        | 30 tracked `.7z` archives, committed DLL                | Observation | `download/`, `nuget/build/netcf/`              |
| EVD-038  | Energy.Core.Test    | Test artifact census                     | `TestResults/` untracked, `Shell.cs` empty              | Observation | `Energy.Core.Test/Core/Shell.cs`               |

Ledger notes follow.

[EVD-006](#auditing-methodology) counts 964 non-merge commits under the top author name out of 1,044 non-merge commits,
with three further author names at 58, 14, and 8.

[EVD-008](#auditing-methodology) is `Reported` evidence from the `docs/refactoring/26.6.1/` assessment, produced outside
this audit and not verified against the current tree.

[EVD-010](#auditing-methodology) records that under a supplied `Secret`, `CreateProviderAlgorithm` assigns
`vector = key` at `Cipher.cs` L170-L175, reusing key bytes as the [IV](#glossary).

[EVD-011](#auditing-methodology) records `HashCore` iterating `i < length` instead of `i < start + length` at
`Hash.cs` L62-L69, misreading the buffer range when `start` is non-zero.

[EVD-017](#auditing-methodology) used a comment-stripped live-clause reading of 223 `catch` keyword occurrences, with
41 whitespace-empty or comment-only bodies concentrated in `Core/Program.cs`, `Base/Cast.cs`,
and `Core/Tilde.cs`.

[EVD-036](#auditing-methodology) records the Compact preview project as a legacy `MSBuild 2003` format project with
`ToolsVersion 3.5` and `TargetFrameworkVersion v2.0` ([NETCF](#glossary)), five source files.

### Scoring Rubrics

| Severity | Meaning                  | Treatment                 |
|----------|--------------------------|---------------------------|
| CRITICAL | Critical contextual risk | Gate resolution           |
| HIGH     | Major contextual risk    | Gate resolution           |
| MEDIUM   | Material contained risk  | Track and plan            |
| LOW      | Limited contextual risk  | Proportionate improvement |

Severity derives from the impact-likelihood matrix, not [CVSS](#glossary).

| Band      | Score Range | Definition                                                   |
|-----------|-------------|--------------------------------------------------------------|
| Excellent | 9-10        | Capability is comprehensive and verified by strong evidence  |
| Good      | 7-8         | Capability is solid overall, minor or noticeable gaps exist  |
| Average   | 4-6         | Capability is present but uneven, limited, or inconsistent   |
| Poor      | 1-3         | Capability is minimal, fragmentary, or absent where required |

**ISO/IEC 25010:2023 crosswalk**

The scorecard dimensions map to ISO/IEC 25010:2023 characteristics as a coverage crosswalk,
never a conformance claim: Security to Security, Maintainability to Maintainability,
Testability and Code Quality to a blended Maintainability and Functional Suitability view,
Deployability and Operational Safety to Portability and Reliability, Scalability to Performance
Efficiency, and the remaining dimensions to engagement-specific groupings with no direct
characteristic.

### Architectural Assessment

**What works**

Namespace layering is coherent, contracts and enumerations are separated from implementations,
and the `Base`, `Core`, `Query`, `Source` split gives a readable mental model.

The csproj is a model of multi-target discipline, with `EnableNETAnalyzers`,
`TreatWarningsAsErrors`, deterministic builds, and SourceLink.

The change-request documents under `docs/change/26.6.1/` are unusually precise.

The escaping design in `Query.Format` and `Bracket` doubles literal delimiters by default,
which is correct [SQL](#glossary) literal quoting.

**What needs attention**

A single assembly exposes every domain,
so consumers cannot adopt compression without also taking HTTP, sockets, and [SQL](#glossary),
[FND-ARC-001](#fnd-arc-001-monolithic-multi-domain-assembly-energycore).

Mutable process-wide statics carry [TLS](#glossary) policy, logging, exception switches, and RNG state,
[FND-ARC-002](#fnd-arc-002-process-global-mutable-state-energycore).

167 `#if` directives fork behavior per target,
with silent capability divergence such as SHA-2 returning `null` under `NETCF`, [FND-ARC-003](#fnd-arc-003-silent-per-target-capability-divergence-energycore).

No [ADRs](#glossary) record durable decisions such as the multi-target strategy or the text-substitution
[SQL](#glossary) approach, [FND-ARC-004](#fnd-arc-004-no-architecture-decision-records-energycore).

### Design Principles

| Principle             | Status  | Evidence                                                                                                         |
|-----------------------|---------|------------------------------------------------------------------------------------------------------------------|
| Single Responsibility | PARTIAL | One assembly holds unrelated domains, [FND-ARC-001](#fnd-arc-001-monolithic-multi-domain-assembly-energycore)    |
| Open/Closed           | PARTIAL | Extension via new members, public fields exposed                                                                 |
| Liskov Substitution   | FAIL    | `Crc32.HashCore` misreads the contract, [FND-CQY-003](#fnd-cqy-003-crc32-hashcore-contract-violation-energycore) |
| Interface Segregation | PASS    | 42 focused interfaces under `Energy.Interface`                                                                   |
| Dependency Inversion  | PARTIAL | Statics over abstractions, [FND-ARC-002](#fnd-arc-002-process-global-mutable-state-energycore)                   |
| DRY                   | PARTIAL | Shared helpers reused, repeated catch idiom                                                                      |

### Data Flow Diagram

Level-0 context, the package flows from [NuGet](#glossary) into a consumer application, which crosses
several trust boundaries through the library.

```
      ╭──────────────────╮
      │  Consumer        │
      │  Application     │
      ╰──────────────────╯
               │
               v
       ╭───────────────╮
       │  Energy.Core  │
       ╰───────────────╯
               │
    ┌──────────┼──────────┐
    v          v          v
╭────────╮ ╭──────────╮ ╭──────────╮
│  TLS   │ │ Database │ │   Files  │
│  peers │ │  engine  │ │  system  │
╰────────╯ ╰──────────╯ ╰──────────╯
```

Level-1 decomposition, library modules mapped to the boundaries they cross.

```
╭───────────────────╮         ╭───────────────────╮
│  Core.Web         │────────>│  TLS endpoints    │
╰───────────────────╯         ╰───────────────────╯
╭───────────────────╮         ╭───────────────────╮
│  Query + Source   │────────>│  Database engine  │
╰───────────────────╯         ╰───────────────────╯
╭───────────────────╮         ╭───────────────────╮
│  Base.Compression │<────────│  Compressed input │
╰───────────────────╯         ╰───────────────────╯
╭───────────────────╮         ╭───────────────────╮
│  Base.Xml         │<────────│  XML payloads     │
╰───────────────────╯         ╰───────────────────╯
╭───────────────────╮         ╭───────────────────╮
│  Core.Log, Bug    │────────>│  Files and sinks  │
╰───────────────────╯         ╰───────────────────╯
```

Node anchors: `Core.Web` is `Energy.Core/Core/Web.cs`, `Query + Source` is
`Energy.Core/Query/` and `Energy.Core/Source/`, `Base.Compression` is
`Energy.Core/Base/Compression.cs`, `Base.Xml` is `Energy.Core/Base/Xml.cs`, `Core.Log, Bug`
is `Energy.Core/Core/Log.cs` and `Energy.Core/Core/Bug.cs`.

**Trust boundaries**

| Boundary               | From              | To                         | Crossing Control              |
|------------------------|-------------------|----------------------------|-------------------------------|
| Network egress         | `Core.Web`        | [TLS](#glossary) endpoints | Validation, bypassable opt-in |
| Database               | `Query`, `Source` | DB engine                  | Literal escaping only         |
| Binary input           | Caller            | `Compression`              | EOF exceptions, no size cap   |
| [XML](#glossary) input | Caller            | `Base.Xml`                 | Serializer, silent fallback   |
| Log sinks              | `Log`, `Bug`      | Files, console             | Levels and destinations       |
| Process state          | Consumer code     | Global statics             | Locks, no isolation           |

### Design Patterns

| Pattern          | Location                        | Assessment | Description                          |
|------------------|---------------------------------|------------|--------------------------------------|
| Static facade    | `Web`, `Bug`, `Cast`            | PARTIAL    | Ergonomic, but process-global state  |
| Singleton        | `Log.Default`                   | PASS       | Double-checked locking, clear intent |
| Strategy         | `Format` dialects, `Bracket`    | PASS       | Dialect objects vary behavior        |
| Template method  | CRC32 `HashAlgorithm`           | FAIL       | Contract defect in `HashCore` bound  |
| Factory          | `Source.Factory`                | PASS       | Provider creation isolated           |
| Worker pool      | `Worker`, `Queue`               | PARTIAL    | Locking present, lifecycle complex   |
| Public-field DTO | `Format.Bracket`, `Destination` | PARTIAL    | Mutable surface, deferred by design  |

### Architecture Decision Records

| Decision                           | Location                     | ADR Status |
|------------------------------------|------------------------------|------------|
| Multi-target single assembly       | `Energy.Core.csproj`         | Missing    |
| Legacy crypto retention            | `Core/Cipher.cs`             | Missing    |
| Text-substitution [SQL](#glossary) | `Query/Parameter.cs`         | Missing    |
| Custom compression stack           | `Base/Compression.cs`        | Missing    |
| Static configuration model         | `Web.cs`, `Log.cs`, `Bug.cs` | Missing    |
| Default-on-failure policy          | `docs/GUIDELINES.md`         | Recorded   |

Only the error-handling policy is documented as a decision.

**Industry Baseline Comparison**

Against .NET library guidance, the project matches strong norms on packaging, deterministic
builds, SourceLink, analyzers, and warnings-as-errors.

It diverges on API-surface tracking, where `PublicApiAnalyzers` or `EnablePackageValidation`
are the common baseline and are absent, [FND-API-001](#fnd-api-001-untracked-public-surface-no-compatibility-gate-energycore).

It diverges on [CI](#glossary), where a hosted pipeline running the documented loop is the common baseline
and none exists, [FND-INF-001](#fnd-inf-001-no-continuous-integration-energycore).

It diverges on crypto hygiene, where `Obsolete` markers or security guidance accompany legacy
algorithms in maintained libraries, [FND-SEC-002](#fnd-sec-002-legacy-cryptographic-primitives-without-warnings-energycore).

### Structure Review

C# multi-target class library, single assembly with layered namespaces, library scale,
governed by `docs/STRUCTURE.md` and the component-role rules for .NET solutions.

The tree carries one library directory, one test directory, one example collection, one
Compact preview tree, and documentation, packaging, and download roots.

| Level | Directory              | Contents                            | Role                   |
|-------|------------------------|-------------------------------------|------------------------|
| `1`   | `Energy.Core/`         | 164 sources plus project files      | Library                |
| `1`   | `Energy.Core.Test/`    | 99 files, [MSTest](#glossary) suite | Tests                  |
| `1`   | `Energy.Core.Compact/` | Preview project and solutions       | Legacy target          |
| `1`   | `Energy.Core.Example/` | 62 example projects                 | Examples (excluded)    |
| `1`   | `docs/`                | Standards, audits, change records   | Documentation          |
| `1`   | `nuget/`               | nuspec series, packaging scripts    | Packaging              |
| `1`   | `download/`            | 30 tracked `.7z` release archives   | Distribution artifacts |
| `1`   | `Documentation/`       | Doxygen configuration               | Generated docs         |

| Area                       | Verdict | Basis                                   |
|----------------------------|---------|-----------------------------------------|
| Directory organization     | PASS    | `docs/STRUCTURE.md` mapping holds       |
| Naming conventions         | PASS    | `Energy.*` namespaces, GUIDELINES rules |
| Component placement        | PARTIAL | Tracked binaries under `download/`      |
| Supporting-artifact layout | PARTIAL | Release archives beside sources         |
| Consistency                | PASS    | Uniform project and file conventions    |

Positive practices: the `Energy.Base`/`Core`/`Query`/`Source` split, standards docs under
`docs/`, and a versioned `docs/audit/` history.

Limitations: `Energy.Core.Example/` excluded at the inclusion gate, `work/` ignored per
`.gitignore`.

### Trade-off Analysis

| Trade-off                          | Context                                             | Option A: gain / cost                      | Option B: gain / cost                             | Evidence                                                           | Implication                                                         |
|------------------------------------|-----------------------------------------------------|--------------------------------------------|---------------------------------------------------|--------------------------------------------------------------------|---------------------------------------------------------------------|
| One codebase for all targets       | Stated net20 to netstandard2.0 plus [CF](#glossary) | Single API and package / 167 `#if` blocks  | Per-target implementations / duplication          | [EVD-002](#auditing-methodology), [EVD-022](#auditing-methodology) | Fits the mission, needs a capability matrix to bound cost           |
| Monolithic assembly                | Single NuGet artifact, [CF](#glossary) packaging    | Simple consumption / broad surface         | Split packages / distribution overhead            | [EVD-001](#auditing-methodology)                                   | Acceptable, document internal layering                              |
| Hand-rolled utilities              | Zero-dependency posture                             | No supply chain / self-maintained security | External libs / dependency risk                   | [EVD-010](#auditing-methodology), [EVD-016](#auditing-methodology) | Coherent, requires the hardening items in the roadmap               |
| Return-default on failure          | Documented ergonomic policy                         | Ergonomic conversions / invisible failures | Exception propagation / caller burden             | [EVD-017](#auditing-methodology)                                   | Sound for conversions, should not cover I/O paths                   |
| Text-substitution [SQL](#glossary) | Every target including [CF](#glossary)              | Dialect control / single line of defense   | DbParameter / [API](#glossary) surface added      | [EVD-014](#auditing-methodology), [EVD-015](#auditing-methodology) | Justified by targets, needs an explicit escaping contract           |
| Static global helpers              | Era-idiomatic ergonomics                            | Zero ceremony / process-global effects     | Instance config / more [API](#glossary) machinery | [EVD-009](#auditing-methodology)                                   | Legacy-appropriate, new [APIs](#glossary) should be instance-scoped |
| Suppress CS1591                    | Not every member documented                         | Clean builds / undocumented drift          | Enforce docs / large one-time effort              | [EVD-002](#auditing-methodology)                                   | Reasonable for legacy, drift now invisible                          |
| Committed binaries                 | [CF](#glossary) cannot build on modern SDK          | Shippable CF package / opaque binaries     | Drop CF / lose stated platform                    | [EVD-004](#auditing-methodology), [EVD-037](#auditing-methodology) | Reasonable, add provenance metadata to bound trust                  |
| Obsolete-then-keep                 | Backward compatibility                              | No breaks / surface grows unguarded        | Removal / breaking consumers                      | [EVD-030](#auditing-methodology)                                   | Fits the stability intent, needs a removal timeline                 |

### Threat Model

[STRIDE](#glossary) is applied to each evidenced boundary, all six categories evaluated per boundary,
no-material-threat outcomes stated explicitly.

| Boundary               | Threat (STRIDE)        | Threat Description                                               | Mitigating Control     | Finding                                                                                  |
|------------------------|------------------------|------------------------------------------------------------------|------------------------|------------------------------------------------------------------------------------------|
| Network egress         | Spoofing               | [MITM](#glossary) via disabled cert validation                   | Opt-in default off     | [FND-SEC-001](#fnd-sec-001-global-tls-certificate-validation-bypass-api-energycore)      |
| Network egress         | Tampering              | Response modification under bypass                               | Same bypass path       | [FND-SEC-001](#fnd-sec-001-global-tls-certificate-validation-bypass-api-energycore)      |
| Network egress         | Repudiation            | No material threat identified                                    | N/A                    | -                                                                                        |
| Network egress         | Information Disclosure | Traffic readable under bypass                                    | Same bypass path       | [FND-SEC-001](#fnd-sec-001-global-tls-certificate-validation-bypass-api-energycore)      |
| Network egress         | Denial of Service      | No material threat identified                                    | N/A                    | -                                                                                        |
| Network egress         | Elevation of Privilege | No material threat identified                                    | N/A                    | -                                                                                        |
| Database               | Spoofing               | No material threat, auth is caller's                             | Caller connection auth | -                                                                                        |
| Database               | Tampering              | [SQL](#glossary) injection via unescaped path                    | `''` doubling default  | [FND-SEC-004](#fnd-sec-004-text-substitution-sql-without-dbparameter-pathway-energycore) |
| Database               | Repudiation            | No material threat identified                                    | N/A                    | -                                                                                        |
| Database               | Information Disclosure | `Password=` in emitted conn string                               | `ToString()` excludes  | -                                                                                        |
| Database               | Denial of Service      | No material threat identified                                    | N/A                    | -                                                                                        |
| Database               | Elevation of Privilege | Excess DB rights are caller config                               | N/A                    | -                                                                                        |
| Binary input           | Spoofing               | No material threat identified                                    | N/A                    | -                                                                                        |
| Binary input           | Tampering              | Malformed input handled to null                                  | EOF exception path     | -                                                                                        |
| Binary input           | Repudiation            | No material threat identified                                    | N/A                    | -                                                                                        |
| Binary input           | Information Disclosure | No material threat identified                                    | N/A                    | -                                                                                        |
| Binary input           | Denial of Service      | Decompression bomb, unbounded growth                             | None, gap              | [FND-SEC-003](#fnd-sec-003-unbounded-decompression-output-growth-energycore)             |
| Binary input           | Elevation of Privilege | No material threat identified                                    | N/A                    | -                                                                                        |
| [XML](#glossary) input | Spoofing               | No material threat identified                                    | N/A                    | -                                                                                        |
| [XML](#glossary) input | Tampering              | Wrong-type deserialize, silent retry                             | Bare catch fallback    | [FND-CQY-001](#fnd-cqy-001-exception-swallowing-beyond-documented-scope-energycore)      |
| [XML](#glossary) input | Repudiation            | No material threat identified                                    | N/A                    | -                                                                                        |
| [XML](#glossary) input | Information Disclosure | [XXE](#glossary) not applicable, [DTD](#glossary) off by default | `XmlReaderSettings`    | -                                                                                        |
| [XML](#glossary) input | Denial of Service      | No material threat identified                                    | N/A                    | -                                                                                        |
| [XML](#glossary) input | Elevation of Privilege | No material threat identified                                    | N/A                    | -                                                                                        |
| Log sinks              | Spoofing               | No material threat identified                                    | N/A                    | -                                                                                        |
| Log sinks              | Tampering              | Log forging by caller content                                    | Levels only, no redact | [FND-CQY-001](#fnd-cqy-001-exception-swallowing-beyond-documented-scope-energycore)      |
| Log sinks              | Repudiation            | Gap, no integrity mechanism                                      | None                   | [FND-CQY-001](#fnd-cqy-001-exception-swallowing-beyond-documented-scope-energycore)      |
| Log sinks              | Information Disclosure | Caller-logged secrets pass through                               | `ToString()` excludes  | -                                                                                        |
| Log sinks              | Denial of Service      | Unbounded log growth possible                                    | `Maximum` cap exists   | -                                                                                        |
| Log sinks              | Elevation of Privilege | No material threat identified                                    | N/A                    | -                                                                                        |
| Process state          | Spoofing               | No material threat identified                                    | N/A                    | -                                                                                        |
| Process state          | Tampering              | Any code can flip the global [TLS](#glossary) flag               | Lock only, no guard    | [FND-SEC-001](#fnd-sec-001-global-tls-certificate-validation-bypass-api-energycore)      |
| Process state          | Repudiation            | No material threat identified                                    | N/A                    | -                                                                                        |
| Process state          | Information Disclosure | No material threat identified                                    | N/A                    | -                                                                                        |
| Process state          | Denial of Service      | No material threat identified                                    | N/A                    | -                                                                                        |
| Process state          | Elevation of Privilege | No material threat identified                                    | N/A                    | -                                                                                        |

Every unmitigated threat traces to a finding and a risk.

### Standards Conformance

**Standards inventory**

| Document    | Path                  | Stack Coverage                          |
|-------------|-----------------------|-----------------------------------------|
| Guidelines  | `docs/GUIDELINES.md`  | C# style, compatibility, testing, error |
| Testing     | `docs/TESTING.md`     | Test commands, coverage expectations    |
| Workflow    | `docs/WORKFLOW.md`    | Change requests, build loop, commits    |
| Structure   | `docs/STRUCTURE.md`   | Project layout                          |
| Versioning  | `docs/VERSIONING.md`  | YY.M.R scheme, csproj source of truth   |
| Copyrights  | `docs/COPYRIGHTS.md`  | LGPL, originality, attribution          |
| Refactoring | `docs/REFACTORING.md` | Refactor constraints                    |

**Code conformance**

| Area                  | Standard Rule                      | Status  | Evidence                                |
|-----------------------|------------------------------------|---------|-----------------------------------------|
| Language version      | Legacy-compatible syntax           | PASS    | No `nameof`, no auto-property init      |
| Project structure     | Namespace layout per STRUCTURE     | PASS    | `Energy.Base`/`Core`/`Query`/`Source`   |
| Naming conventions    | Qualified refs, no using-aliasing  | PASS    | `Energy.Base.Binary.BitReader` style    |
| Error handling        | Default on conversion failure only | PARTIAL | Empty catches outside conversions       |
| Testing               | Public methods need unit tests     | PARTIAL | Cipher, Log, Query internals untested   |
| Formatting and lint   | Analyzers on, warnings as errors   | PASS    | csproj L41-L49, `.editorconfig`         |
| Dependency management | License-compatible, documented     | PARTIAL | Zero deps, attribution section empty    |
| Security              | Not specified as a standard        | UNKNOWN | GUIDELINES has no security rules        |
| Versioning            | csproj source of truth, YY.M.R     | PASS    | 26.7.0 consistent across csproj, nuspec |
| Change management     | Change request before non-trivial  | PARTIAL | `26.6.1/` exists, none for 26.7.0       |

The standards documents declare LGPL-3.0 while `LICENSE.txt` grants BSD-3-Clause,
that license-identity conflict is recorded as [FND-CPR-003](#fnd-cpr-003-license-declarations-contradict-licensetxt-energycore) rather than a conformance row.

**Standards quality**

| Area                     | Standards Position               | External Best Practice      | Alignment         |
|--------------------------|----------------------------------|-----------------------------|-------------------|
| Error handling           | Return defaults on failure       | Framework Design Guidelines | Partially aligned |
| [XML](#glossary) docs    | Document public members          | .NET library guidance       | Aligned           |
| Testing                  | Tests for public methods         | .NET library guidance       | Aligned           |
| Suppression              | Targeted `.editorconfig` silence | Analyzer configuration docs | Aligned           |
| Crypto surface           | Silent on algorithm policy       | .NET security rules, CA5350 | Diverges          |
| Versioning               | Calendar scheme, csproj truth    | NuGet SemVer expectations   | Partially aligned |
| [API](#glossary) surface | No baseline or gate rule         | PublicApiAnalyzers practice | Diverges          |

**Crypto surface** Diverges.

The standards are silent on weak algorithms, while Microsoft's security ruleset maps
DES/MD5/SHA1 usage to CA5350 and CA5351, the gap produces [FND-SEC-002](#fnd-sec-002-legacy-cryptographic-primitives-without-warnings-energycore).

**[API](#glossary) surface** Diverges.

Maintained .NET libraries typically track `PublicAPI.Shipped.txt` or enable package
validation, the standards do not cover it, producing [FND-API-001](#fnd-api-001-untracked-public-surface-no-compatibility-gate-energycore).

### Engineering Baseline Conformance

The bundled .NET stack baseline governs: SDK-style csproj with analyzers and warnings-as-errors
is met, deterministic builds and SourceLink are met, package-validation or a public-API
baseline is absent ([FND-API-001](#fnd-api-001-untracked-public-surface-no-compatibility-gate-energycore)), and [CI](#glossary) build/test evidence is absent ([FND-INF-001](#fnd-inf-001-no-continuous-integration-energycore)).

### API Compatibility & Versioning Discipline

| Aspect              | Status  | Evidence                                              |
|---------------------|---------|-------------------------------------------------------|
| Gate presence       | FAIL    | No `PublicAPI` baseline, no `EnablePackageValidation` |
| Version consistency | PARTIAL | `YY.M.R` consistent in csproj and nuspec, tags sparse |
| Breaking tracking   | PARTIAL | `[Obsolete]` in 9 files, no removal policy            |

The published package surface is untracked at the gate, [FND-API-001](#fnd-api-001-untracked-public-surface-no-compatibility-gate-energycore) and [FND-API-002](#fnd-api-002-versioning-practice-diverges-from-declared-scheme-energycore) carry
the detail.

### Strengths & What's Working

- Zero runtime dependencies - the csproj carries only `Microsoft.SourceLink.GitHub` as a
  `PrivateAssets="All"` build-time reference, [EVD-002](#auditing-methodology), [EVD-033](#auditing-methodology).
- Analyzer discipline - `EnableNETAnalyzers`, `AnalysisLevel latest`, `TreatWarningsAsErrors`,
  deterministic builds, and an in-file justification for the CS1591 suppression, [EVD-028](#auditing-methodology).
- Correct default [SQL](#glossary) literal escaping - `''` doubling verified in `Bracket.cs` L389-L398,
  [EVD-014](#auditing-methodology).
- Credential-aware serialization - `Source/Configuration.cs` `ToString()` excludes secrets,
  [EVD-020](#auditing-methodology).
- Standards corpus - seven `docs/*.md` documents covering style, testing, workflow,
  versioning, and copyright, [EVD-005](#auditing-methodology).
- Precise change-request practice - `docs/change/26.6.1/` documents defects, compatibility,
  and scope, [EVD-021](#auditing-methodology).
- Focused contract surface - 42 interfaces under `Energy.Interface`, [EVD-001](#auditing-methodology).
- Honest failure handling on malformed compressed input - EOF exceptions map to `null`,
  [EVD-016](#auditing-methodology).

### Technical Findings

| Finding                                                                                       | Pillar                      | Severity | Title                                                       | Result  | Status | Change | Verification |
|-----------------------------------------------------------------------------------------------|-----------------------------|----------|-------------------------------------------------------------|---------|--------|--------|--------------|
| [FND-ARC-001](#fnd-arc-001-monolithic-multi-domain-assembly-energycore)                       | Architecture & Design       | MEDIUM   | Monolithic multi-domain assembly                            | PARTIAL | Open   | New    |              |
| [FND-ARC-002](#fnd-arc-002-process-global-mutable-state-energycore)                           | Architecture & Design       | MEDIUM   | Process-global mutable state                                | PARTIAL | Open   | New    |              |
| [FND-ARC-003](#fnd-arc-003-silent-per-target-capability-divergence-energycore)                | Architecture & Design       | MEDIUM   | Silent per-target capability divergence                     | PARTIAL | Open   | New    |              |
| [FND-ARC-004](#fnd-arc-004-no-architecture-decision-records-energycore)                       | Architecture & Design       | LOW      | No architecture decision records                            | PARTIAL | Open   | New    |              |
| [FND-CQY-001](#fnd-cqy-001-exception-swallowing-beyond-documented-scope-energycore)           | Code Quality                | MEDIUM   | Exception swallowing beyond documented scope                | PARTIAL | Open   | New    |              |
| [FND-CQY-002](#fnd-cqy-002-untested-public-surface-energycore)                                | Code Quality                | MEDIUM   | Untested public surface                                     | PARTIAL | Open   | New    |              |
| [FND-CQY-003](#fnd-cqy-003-crc32-hashcore-contract-violation-energycore)                      | Code Quality                | MEDIUM   | CRC32 `HashCore` contract violation                         | FAIL    | Open   | New    |              |
| [FND-CQY-004](#fnd-cqy-004-xml-documentation-drift-energycore)                                | Code Quality                | LOW      | [XML](#glossary) documentation drift                        | PARTIAL | Open   | New    |              |
| [FND-CQY-005](#fnd-cqy-005-public-mutable-fields-as-api-surface-energycore)                   | Code Quality                | LOW      | Public mutable fields as [API](#glossary) surface           | PARTIAL | Open   | New    |              |
| [FND-SEC-001](#fnd-sec-001-global-tls-certificate-validation-bypass-api-energycore)           | Security & Compliance       | HIGH     | Global TLS certificate-validation bypass [API](#glossary)   | FAIL    | Open   | New    |              |
| [FND-SEC-002](#fnd-sec-002-legacy-cryptographic-primitives-without-warnings-energycore)       | Security & Compliance       | HIGH     | Legacy cryptographic primitives, DES IV reuse               | FAIL    | Open   | New    |              |
| [FND-SEC-003](#fnd-sec-003-unbounded-decompression-output-growth-energycore)                  | Security & Compliance       | HIGH     | Unbounded decompression output growth                       | FAIL    | Open   | New    |              |
| [FND-SEC-004](#fnd-sec-004-text-substitution-sql-without-dbparameter-pathway-energycore)      | Security & Compliance       | MEDIUM   | Text-substitution [SQL](#glossary) without DbParameter path | PARTIAL | Open   | New    |              |
| [FND-INF-001](#fnd-inf-001-no-continuous-integration-energycore)                              | Infrastructure & CI/CD      | MEDIUM   | No continuous integration                                   | FAIL    | Open   | New    |              |
| [FND-INF-002](#fnd-inf-002-opaque-committed-binaries-no-supply-chain-verification-energycore) | Infrastructure & CI/CD      | MEDIUM   | Opaque committed binaries, no provenance                    | PARTIAL | Open   | New    |              |
| [FND-INF-003](#fnd-inf-003-change-control-evidence-gap-energycore)                            | Infrastructure & CI/CD      | LOW      | Change-control evidence gap                                 | PARTIAL | Open   | New    |              |
| [FND-INF-004](#fnd-inf-004-release-anchoring-and-artifact-hygiene-energycore)                 | Infrastructure & CI/CD      | LOW      | Release anchoring and artifact hygiene                      | PARTIAL | Open   | New    |              |
| [FND-INF-005](#fnd-inf-005-contributor-concentration-single-author-history-energycore)        | Infrastructure & CI/CD      | LOW      | Contributor concentration, single-author history            | PARTIAL | Open   | New    |              |
| [FND-AIP-001](#fnd-aip-001-no-provenance-policy-origin-unrecorded-energycore)                 | AI Provenance & Code Origin | LOW      | No provenance policy, origin unrecorded                     | UNKNOWN | Open   | New    |              |
| [FND-CPR-001](#fnd-cpr-001-third-party-attribution-gap-for-ported-algorithms-energycore)      | Copyrights & Originality    | MEDIUM   | Third-party attribution gap for ported code                 | PARTIAL | Open   | New    |              |
| [FND-CPR-003](#fnd-cpr-003-license-declarations-contradict-licensetxt-energycore)             | Copyrights & Originality    | HIGH     | License declarations contradict LICENSE.txt                 | FAIL    | Open   | New    |              |
| [FND-API-001](#fnd-api-001-untracked-public-surface-no-compatibility-gate-energycore)         | API Compatibility           | MEDIUM   | Untracked public surface, no compatibility gate             | FAIL    | Open   | New    |              |
| [FND-API-002](#fnd-api-002-versioning-practice-diverges-from-declared-scheme-energycore)      | API Compatibility           | LOW      | Tag practice diverges from declared scheme                  | PARTIAL | Open   | New    |              |

### FND-ARC-001: Monolithic multi-domain assembly (Energy.Core/)

One assembly exposes every domain - `Base`, `Core`, `Query`, `Source`, `Interface`,
`Enumeration`, and `Attribute` - so consumers cannot adopt compression without taking HTTP,
sockets, and [SQL](#glossary).

* **Pillar:** Architecture & Design
* **Severity:** MEDIUM
* **Type:** Concern
* **Targets:** Energy.Core/
* **Basis:** Single-assembly layout vs domain breadth, auditor-proposed
* **Criteria:** ARC-MOD
* **Impact:** Broad consumption surface, no enforced internal boundary
* **Method:** Structure census against `docs/STRUCTURE.md`
* **Confidence:** High
* **Mitigating factors:** None observed
* **Evidence:** [EVD-001](#auditing-methodology), `Energy.Core/` directory map
* **Recommendation:** Document the internal layering contract, consider split packaging

### FND-ARC-002: Process-global mutable state (Energy.Core/)

Statics carry [TLS](#glossary) policy, logging, exception switches, and RNG state process-wide.

* **Pillar:** Architecture & Design
* **Severity:** MEDIUM
* **Type:** Concern
* **Targets:** Energy.Core/
* **Basis:** Instance-scoped configuration practice
* **Criteria:** ARC-STATE
* **Impact:** Any in-process code flips global behavior, thread coupling
* **Method:** Static census in `Web.cs`, `Log.cs`, `Bug.cs`, `Random.cs`
* **Confidence:** High
* **Mitigating factors:** None observed
* **Evidence:** [EVD-009](#auditing-methodology), `Energy.Core/Core/Web.cs` L73-L111
* **Recommendation:** Keep legacy statics documented, scope new [APIs](#glossary) to instances

### FND-ARC-003: Silent per-target capability divergence (Energy.Core/)

167 `#if` directives fork behavior per target, and `NETCF` paths return `null` or no-op
where other targets act.

* **Pillar:** Architecture & Design
* **Severity:** MEDIUM
* **Type:** Concern
* **Targets:** Energy.Core/
* **Basis:** Parity expectation across published targets
* **Criteria:** ARC-DIVERGE
* **Impact:** SHA-2 returns `null` under `NETCF`, [TLS](#glossary) setter is a silent no-op there
* **Method:** `#if` census plus sampled `NETCF` branches
* **Confidence:** High
* **Mitigating factors:** None observed
* **Evidence:** [EVD-022](#auditing-methodology), `Base/Hash.cs` L343-L428, `Core/Web.cs` L109-L111
* **Recommendation:** Publish a per-target capability matrix

### FND-ARC-004: No architecture decision records (Energy.Core/)

Durable decisions such as multi-target strategy and text-substitution [SQL](#glossary) are unrecorded.

* **Pillar:** Architecture & Design
* **Severity:** LOW
* **Type:** Concern
* **Targets:** Energy.Core/, docs/
* **Basis:** [ADR](#glossary) practice for production-bound projects
* **Criteria:** ARC-MOD
* **Impact:** Rationale lost, change decisions re-litigated
* **Method:** `docs/` census, [ADR](#glossary) table
* **Confidence:** High
* **Absence:** No documented rationale - no ADR directory or decision records exist
* **Mitigating factors:** None observed
* **Evidence:** [EVD-005](#auditing-methodology), `docs/` listing
* **Recommendation:** Record durable decisions as [ADRs](#glossary) under `docs/`

### FND-CQY-001: Exception swallowing beyond documented scope (Energy.Core/)

223 `catch` keyword occurrences hold roughly 41 whitespace-empty or comment-only bodies,
concentrated in `Core/Program.cs`, `Base/Cast.cs`, and `Core/Tilde.cs`.

* **Pillar:** Code Quality
* **Severity:** MEDIUM
* **Type:** Concern
* **Targets:** Energy.Core/
* **Basis:** `docs/GUIDELINES.md` permits defaults on conversion failure only
* **Criteria:** CQY-CATCH
* **Impact:** Silent failure on I/O and [XML](#glossary) paths, errors invisible to callers
* **Method:** Comment-stripped catch census
* **Confidence:** Medium
* **Mitigating factors:** None observed
* **Evidence:** [EVD-017](#auditing-methodology), `Base/Xml.cs` L217-L240 bare catch
* **Recommendation:** Re-scope empty catches to the documented conversion policy

### FND-CQY-002: Untested public surface (Energy.Core/)

50 test classes face 164 library source files, with no tests for `Cipher`, `Log`, `Ini`,
`Yaml`, or most of `Query` and `Source`.

* **Pillar:** Code Quality
* **Severity:** MEDIUM
* **Type:** Concern
* **Targets:** Energy.Core/, Energy.Core.Test/
* **Basis:** `docs/TESTING.md` expects tests for public methods
* **Criteria:** TST-UNIT
* **Impact:** Core utilities carry unverified public behavior
* **Method:** Test-class census against module map
* **Confidence:** High
* **Mitigating factors:** None observed
* **Evidence:** [EVD-001](#auditing-methodology), [EVD-003](#auditing-methodology)
* **Recommendation:** Close the highest-value test gaps first

### FND-CQY-003: CRC32 `HashCore` contract violation (Energy.Core/)

`Crc32.HashCore` iterates `i < length` instead of `i < start + length`, misreading the buffer
range whenever `start` is non-zero.

* **Pillar:** Code Quality
* **Severity:** MEDIUM
* **Type:** Concern
* **Targets:** Energy.Core/Base/Hash.cs
* **Basis:** `HashAlgorithm.HashCore` contract requires `[start, start+length)`
* **Criteria:** CQY-CATCH
* **Impact:** Wrong checksums on segmented input, silent corruption
* **Method:** Source read of `Hash.cs` L62-L69
* **Confidence:** High
* **Mitigating factors:** None observed
* **Evidence:** [EVD-011](#auditing-methodology)
* **Recommendation:** Fix the loop bound to `i < start + length`, add a segmented-input test

### FND-CQY-004: XML documentation drift (Energy.Core/)

CS1591 stays suppressed through `NoWarn`, so undocumented public members accumulate
invisibly.

* **Pillar:** Code Quality
* **Severity:** LOW
* **Type:** Concern
* **Targets:** Energy.Core/
* **Basis:** `docs/GUIDELINES.md` documents public members
* **Criteria:** CQY-DOC
* **Impact:** Public API documentation drift unmeasured
* **Method:** csproj and `.editorconfig` inspection
* **Confidence:** High
* **Mitigating factors:** None observed
* **Evidence:** [EVD-028](#auditing-methodology)
* **Recommendation:** Track documentation coverage or re-enable CS1591 incrementally

### FND-CQY-005: Public mutable fields as API surface (Energy.Core/)

Public-field DTOs such as `Format.Bracket` and `Destination` expose mutable surface.

* **Pillar:** Code Quality
* **Severity:** LOW
* **Type:** Concern
* **Targets:** Energy.Core/
* **Basis:** Encapsulation guidance, deferred by design per project convention
* **Criteria:** CQY-DOC
* **Impact:** Consumers mutate contract objects directly
* **Method:** Pattern review in `Query/Format.cs`
* **Confidence:** Medium
* **Mitigating factors:** None observed
* **Evidence:** [EVD-001](#auditing-methodology)
* **Recommendation:** Document the mutable-DTO contract or encapsulate new [APIs](#glossary)

### FND-SEC-001: Global TLS certificate-validation bypass API (Energy.Core/)

`Web.IgnoreCertificateValidation` installs a callback returning `true` process-wide,
disabling [TLS](#glossary) certificate validation for the consumer application.

* **Pillar:** Security & Compliance
* **Severity:** HIGH
* **Type:** Concern
* **Targets:** Energy.Core/Core/Web.cs
* **Basis:** CWE-295, certificate validation must never be globally bypassed
* **Criteria:** SEC-TLS
* **Impact:** [MITM](#glossary) exposure for every [TLS](#glossary) consumer in the process once enabled
* **Method:** Source read of `Web.cs` L55-L111
* **Confidence:** High
* **Mitigating factors:** Opt-in, default off
* **Breaking change:** Public API removal or scoping
* **Evidence:** [EVD-009](#auditing-methodology)
* **Exploitability:** Theoretical - reasoned from static source review, no execution
* **Recommendation:** Scope or retire the bypass, mark `[Obsolete]`, document caller risk.
  The equivalent analyzer rule is `CA5359`, currently not enabled in the project.

### FND-SEC-002: Legacy cryptographic primitives without warnings (Energy.Core/)

`Cipher` exposes MD5, SHA1, and DES, and `DES.CreateProviderAlgorithm` reuses the secret's
key bytes as the [IV](#glossary) when `Secret` is set.

* **Pillar:** Security & Compliance
* **Severity:** HIGH
* **Type:** Concern
* **Targets:** Energy.Core/Core/Cipher.cs
* **Basis:** CWE-327, broken and risky algorithms ship unguarded
* **Criteria:** SEC-CRYPTO
* **Impact:** Consumers select weak primitives, DES IV reuse collapses CBC semantics
* **Method:** Source read of `Cipher.cs` L15-L185
* **Confidence:** High
* **Mitigating factors:** Caller-selected, not auto-applied
* **Breaking change:** Public API if primitives are removed
* **Evidence:** [EVD-010](#auditing-methodology)
* **Exploitability:** Theoretical - reasoned from static source review, no execution
* **Recommendation:** Warn or mark `[Obsolete]` on MD5, SHA1, DES, fix [IV](#glossary) derivation.
  Equivalent analyzer rules are `CA5350` and `CA5351`, not enabled in the project.

### FND-SEC-003: Unbounded decompression output growth (Energy.Core/)

ZX0, LZ4, and LZSS decoders grow output buffers without an upper bound - `Grow` at L529,
L547, L675 and `EnsureCapacity` at L1625, L1689, L1706, L1758.

* **Pillar:** Security & Compliance
* **Severity:** HIGH
* **Type:** Concern
* **Targets:** Energy.Core/Base/Compression.cs
* **Basis:** CWE-409, decompression bombs exhaust memory
* **Criteria:** SEC-DECOMP
* **Impact:** Crafted small input expands to unbounded memory use
* **Method:** Source read of `Compression.cs` growth paths
* **Confidence:** High
* **Mitigating factors:** Malformed input returns `null` via EOF exceptions
* **Breaking change:** None - an output cap is additive
* **Evidence:** [EVD-016](#auditing-methodology)
* **Exploitability:** Theoretical - reasoned from static source review, no execution
* **Recommendation:** Bound decoded size or document caller obligations.
  No direct C# analyzer rule exists for CWE-409 in the bundled mapping.

### FND-SEC-004: Text-substitution SQL without DbParameter pathway (Energy.Core/)

`Query` renders [SQL](#glossary) via text substitution with `''` doubling, and `Source/Connection.cs`
offers no `DbParameter` pathway for values.

* **Pillar:** Security & Compliance
* **Severity:** MEDIUM
* **Type:** Concern
* **Targets:** Energy.Core/Query/, Energy.Core/Source/
* **Basis:** CWE-89, escaping is the only injection defense
* **Criteria:** SEC-SQL
* **Impact:** An escaping bypass or caller misuse injects [SQL](#glossary)
* **Method:** Source read of `Parameter.cs`, `Connection.cs`, `Format.cs`
* **Confidence:** Medium
* **Mitigating factors:** Default `''` doubling verified at `Bracket.cs` L389-L398
* **Breaking change:** Internal - a parameter pathway is additive
* **Evidence:** [EVD-014](#auditing-methodology), [EVD-015](#auditing-methodology)
* **Recommendation:** Document the escaping contract, add a parameterized value [API](#glossary).
  Equivalent analyzer rules are `CA2100` and `CA3001`, not enabled in the project.

### FND-INF-001: No continuous integration (Energy.Core/)

No pipeline, workflow, or [CI](#glossary) configuration exists, only `nuget/*.cmd` packaging scripts.

* **Pillar:** Infrastructure & CI/CD
* **Severity:** MEDIUM
* **Type:** Concern
* **Targets:** Repository
* **Basis:** Documented build and test loop in `docs/WORKFLOW.md` runs manually
* **Criteria:** INF-CICD
* **Impact:** Regressions ship undetected, verified by the revert pair [EVD-006](#auditing-methodology)
* **Method:** Full-tree [CI](#glossary) census
* **Confidence:** High
* **Absence:** No documented rationale - no pipeline file or documented manual-only policy
* **Mitigating factors:** None observed
* **Evidence:** [EVD-018](#auditing-methodology)
* **Recommendation:** Add a minimal [CI](#glossary) pipeline running the documented loop

### FND-INF-002: Opaque committed binaries, no supply-chain verification (Energy.Core/)

`nuget/build/netcf/Energy.Core.dll` and 30 `download/*.7z` archives ship tracked binaries
with no provenance or hash manifest.

* **Pillar:** Infrastructure & CI/CD
* **Severity:** MEDIUM
* **Type:** Concern
* **Targets:** nuget/, download/
* **Basis:** Reproducible-build and artifact-provenance practice
* **Criteria:** INF-REL
* **Impact:** Consumers cannot verify the [CF](#glossary) binary matches source
* **Method:** Tracked-binary census
* **Confidence:** High
* **Mitigating factors:** None observed
* **Evidence:** [EVD-004](#auditing-methodology), [EVD-037](#auditing-methodology)
* **Recommendation:** Publish hashes and build provenance for committed binaries

### FND-INF-003: Change-control evidence gap (Energy.Core/)

Commit `78c14d5` introduced deliberately broken code that `9447e87` had to revert, and no
change request exists for the 26.7.0 series.

* **Pillar:** Infrastructure & CI/CD
* **Severity:** LOW
* **Type:** Concern
* **Targets:** Repository
* **Basis:** `docs/WORKFLOW.md` requires change requests for non-trivial work
* **Criteria:** INF-CICD
* **Impact:** Defect incidents reach the tree without a gate
* **Method:** Git history and `docs/change/` census
* **Confidence:** High
* **Mitigating factors:** None observed
* **Evidence:** [EVD-006](#auditing-methodology), [EVD-021](#auditing-methodology)
* **Recommendation:** Backfill the 26.7.0 change record, gate merges on the loop

### FND-INF-004: Release anchoring and artifact hygiene (Energy.Core/)

One git tag exists against 41 nuspec releases, so shipped versions lack VCS anchors.

* **Pillar:** Infrastructure & CI/CD
* **Severity:** LOW
* **Type:** Concern
* **Targets:** Repository
* **Basis:** `docs/VERSIONING.md` YY.M.R release identity
* **Criteria:** INF-REL
* **Impact:** Released code cannot be re-derived from tags
* **Method:** Tag and nuspec census
* **Confidence:** High
* **Absence:** Undetermined - tagging practice is undocumented
* **Mitigating factors:** None observed
* **Evidence:** [EVD-006](#auditing-methodology), `nuget/` series
* **Recommendation:** Tag each release at its source revision

### FND-INF-005: Contributor concentration, single-author history (Energy.Core/)

964 of 1,044 non-merge commits sit under one author name across the project history.

* **Pillar:** Infrastructure & CI/CD
* **Severity:** LOW
* **Type:** Concern
* **Targets:** Repository
* **Basis:** Continuity and review-breadth practice
* **Criteria:** INF-CICD
* **Impact:** Key-person dependency, limited review breadth
* **Method:** `git shortlog` author census
* **Confidence:** High
* **Mitigating factors:** Four author names appear in history
* **Evidence:** [EVD-006](#auditing-methodology)
* **Recommendation:** Recorded handoff or broadened review practice

### FND-AIP-001: No provenance policy, origin unrecorded (Energy.Core/)

No AI-attribution policy or markers exist, and commit messages carry no provenance signal.

* **Pillar:** AI Provenance & Code Origin
* **Severity:** LOW
* **Type:** Concern
* **Targets:** Repository
* **Basis:** Provenance transparency for published libraries
* **Criteria:** AIP-POLICY
* **Impact:** Authorship of generated contributions is undeterminable
* **Method:** `git log --format` census, docs search
* **Confidence:** Medium
* **Provenance basis:** Undetermined
* **Absence:** No documented rationale - no policy document exists
* **Mitigating factors:** None observed
* **Evidence:** [EVD-024](#auditing-methodology)
* **Recommendation:** Adopt a provenance policy in `docs/`

### FND-CPR-001: Third-party attribution gap for ported algorithms (Energy.Core/)

`docs/COPYRIGHTS.md` prescribes a third-party section that remains a template, while
ZX0, LZ4, and LZSS decoders are visibly ported algorithms.

* **Pillar:** Copyrights & Originality
* **Severity:** MEDIUM
* **Type:** Concern
* **Targets:** docs/COPYRIGHTS.md, Base/Compression.cs
* **Basis:** `docs/COPYRIGHTS.md` requires documenting third-party code
* **Criteria:** CPR-ATTR
* **Impact:** License obligations of ported code are unrecorded
* **Method:** Attribution section review against compression sources
* **Confidence:** High
* **Absence:** No documented rationale - the section holds only instructions
* **Mitigating factors:** None observed
* **Evidence:** [EVD-025](#auditing-methodology)
* **Recommendation:** Populate third-party attribution for ported algorithms

### FND-CPR-003: License declarations contradict LICENSE.txt (Energy.Core/)

`LICENSE.txt` grants BSD-3-Clause terms while the nuspec declares `LGPL-3.0-or-later`,
the csproj points at the LGPL URL, and `docs/COPYRIGHTS.md` names LGPL.

* **Pillar:** Copyrights & Originality
* **Severity:** HIGH
* **Type:** Concern
* **Targets:** LICENSE.txt, nuget/, csproj, docs/
* **Basis:** A published package needs one consistent license identity
* **Criteria:** CPR-LIC
* **Impact:** Consumers cannot determine the governing license
* **Method:** Cross-check of all license declarations
* **Confidence:** High
* **Mitigating factors:** None observed
* **Evidence:** [EVD-019](#auditing-methodology), [EVD-035](#auditing-methodology)
* **Recommendation:** Reconcile all declarations to the intended license

### FND-API-001: Untracked public surface, no compatibility gate (Energy.Core/)

No `PublicAPI` baseline, `EnablePackageValidation`, or compat check exists.

* **Pillar:** API Compatibility
* **Severity:** MEDIUM
* **Type:** Concern
* **Targets:** Energy.Core/Energy.Core.csproj
* **Basis:** [NuGet](#glossary) package-validation practice for published libraries
* **Criteria:** API-TRACK
* **Impact:** Breaking changes ship silently across releases
* **Method:** csproj and repository artifact search
* **Confidence:** High
* **Absence:** No documented rationale - no baseline or gate artifact exists
* **Mitigating factors:** None observed
* **Evidence:** [EVD-029](#auditing-methodology)
* **Recommendation:** Adopt `PublicApiAnalyzers` or `EnablePackageValidation`

### FND-API-002: Versioning practice diverges from declared scheme (Energy.Core/)

`YY.M.R` versions are consistent across csproj and nuspec, but tagging and deprecation-removal
practice diverge.

* **Pillar:** API Compatibility
* **Severity:** LOW
* **Type:** Concern
* **Targets:** Repository
* **Basis:** `docs/VERSIONING.md` scheme
* **Criteria:** API-VER
* **Impact:** Version lineage is unverifiable against VCS
* **Method:** Version cross-check across csproj, nuspec, tags
* **Confidence:** High
* **Mitigating factors:** None observed
* **Evidence:** [EVD-002](#auditing-methodology), [EVD-006](#auditing-methodology), [EVD-030](#auditing-methodology)
* **Recommendation:** Declare the breaking-change and removal policy, tag releases

### Technical Debt Register

| Debt                                                                                        | Category        | Source                                                                              | Status |
|---------------------------------------------------------------------------------------------|-----------------|-------------------------------------------------------------------------------------|--------|
| [TDR-001](#tdr-001-single-assembly-domain-aggregation-energycore)                           | Maintainability | [FND-ARC-001](#fnd-arc-001-monolithic-multi-domain-assembly-energycore)             | Open   |
| [TDR-002](#tdr-002-process-global-statics-energycore)                                       | Reliability     | [FND-ARC-002](#fnd-arc-002-process-global-mutable-state-energycore)                 | Open   |
| [TDR-003](#tdr-003-conditional-compilation-divergence-without-capability-matrix-energycore) | Maintainability | [FND-ARC-003](#fnd-arc-003-silent-per-target-capability-divergence-energycore)      | Open   |
| [TDR-004](#tdr-004-empty-catch-residue-energycore)                                          | Reliability     | [FND-CQY-001](#fnd-cqy-001-exception-swallowing-beyond-documented-scope-energycore) | Open   |
| [TDR-005](#tdr-005-untested-module-backlog-energycore)                                      | Maintainability | [FND-CQY-002](#fnd-cqy-002-untested-public-surface-energycore)                      | Open   |
| [TDR-006](#tdr-006-xml-documentation-drift-energycore)                                      | Maintainability | [FND-CQY-004](#fnd-cqy-004-xml-documentation-drift-energycore)                      | Open   |
| [TDR-007](#tdr-007-public-mutable-fields-energycore)                                        | Maintainability | [FND-CQY-005](#fnd-cqy-005-public-mutable-fields-as-api-surface-energycore)         | Open   |
| [TDR-008](#tdr-008-missing-architecture-decision-records-energycore)                        | Maintainability | [FND-ARC-004](#fnd-arc-004-no-architecture-decision-records-energycore)             | Open   |
| [TDR-009](#tdr-009-missing-2670-change-request-entries-energycore)                          | Maintainability | [FND-INF-003](#fnd-inf-003-change-control-evidence-gap-energycore)                  | Open   |
| [TDR-010](#tdr-010-tag-and-artifact-hygiene-energycore)                                     | Reliability     | [FND-INF-004](#fnd-inf-004-release-anchoring-and-artifact-hygiene-energycore)       | Open   |
| [TDR-011](#tdr-011-license-declaration-reconciliation-energycore)                           | Security        | [FND-CPR-003](#fnd-cpr-003-license-declarations-contradict-licensetxt-energycore)   | Open   |

### TDR-001: Single-assembly domain aggregation (Energy.Core/)

Every domain ships in one assembly.

* **Category:** Maintainability
* **Source:** [FND-ARC-001](#fnd-arc-001-monolithic-multi-domain-assembly-energycore)
* **Status:** Open

### TDR-002: Process-global statics (Energy.Core/)

Mutable statics couple all consumers in the process.

* **Category:** Reliability
* **Source:** [FND-ARC-002](#fnd-arc-002-process-global-mutable-state-energycore)
* **Status:** Open

### TDR-003: Conditional-compilation divergence without capability matrix (Energy.Core/)

Per-target behavior forks are undocumented.

* **Category:** Maintainability
* **Source:** [FND-ARC-003](#fnd-arc-003-silent-per-target-capability-divergence-energycore)
* **Status:** Open

### TDR-004: Empty-catch residue (Energy.Core/)

Silent catches outside the documented conversion policy.

* **Category:** Reliability
* **Source:** [FND-CQY-001](#fnd-cqy-001-exception-swallowing-beyond-documented-scope-energycore)
* **Status:** Open

### TDR-005: Untested module backlog (Energy.Core/)

Core utility modules carry no tests.

* **Category:** Maintainability
* **Source:** [FND-CQY-002](#fnd-cqy-002-untested-public-surface-energycore)
* **Status:** Open

### TDR-006: XML documentation drift (Energy.Core/)

Suppressed CS1591 hides undocumented members.

* **Category:** Maintainability
* **Source:** [FND-CQY-004](#fnd-cqy-004-xml-documentation-drift-energycore)
* **Status:** Open

### TDR-007: Public mutable fields (Energy.Core/)

Mutable DTO surface stays unguarded.

* **Category:** Maintainability
* **Source:** [FND-CQY-005](#fnd-cqy-005-public-mutable-fields-as-api-surface-energycore)
* **Status:** Open

### TDR-008: Missing architecture decision records (Energy.Core/)

Durable decisions are unrecorded.

* **Category:** Maintainability
* **Source:** [FND-ARC-004](#fnd-arc-004-no-architecture-decision-records-energycore)
* **Status:** Open

### TDR-009: Missing 26.7.0 change-request entries (Energy.Core/)

The current release series has no change record.

* **Category:** Maintainability
* **Source:** [FND-INF-003](#fnd-inf-003-change-control-evidence-gap-energycore)
* **Status:** Open

### TDR-010: Tag and artifact hygiene (Energy.Core/)

Releases lack tags and binary provenance.

* **Category:** Reliability
* **Source:** [FND-INF-004](#fnd-inf-004-release-anchoring-and-artifact-hygiene-energycore)
* **Status:** Open

### TDR-011: License declaration reconciliation (Energy.Core/)

Contradicting license texts remain unresolved.

* **Category:** Security
* **Source:** [FND-CPR-003](#fnd-cpr-003-license-declarations-contradict-licensetxt-energycore)
* **Status:** Open

### Unified Risk Register

| Risk                                                                                 | Severity | Likelihood | Title                                                      | Source                                                                                                                                                                          |
|--------------------------------------------------------------------------------------|----------|------------|------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| [RSK-001](#rsk-001-process-wide-mitm-exposure-via-tls-bypass-energycore)             | MEDIUM   | Medium     | Process-wide [MITM](#glossary) via [TLS](#glossary) bypass | [FND-SEC-001](#fnd-sec-001-global-tls-certificate-validation-bypass-api-energycore)                                                                                             |
| [RSK-002](#rsk-002-legacy-primitive-selection-by-consumers-energycore)               | MEDIUM   | Medium     | Legacy primitive selection by consumers                    | [FND-SEC-002](#fnd-sec-002-legacy-cryptographic-primitives-without-warnings-energycore)                                                                                         |
| [RSK-003](#rsk-003-decompression-bomb-resource-exhaustion-energycore)                | MEDIUM   | Medium     | Decompression-bomb resource exhaustion                     | [FND-SEC-003](#fnd-sec-003-unbounded-decompression-output-growth-energycore)                                                                                                    |
| [RSK-004](#rsk-004-sql-injection-via-escaping-bypass-energycore)                     | MEDIUM   | Medium     | [SQL](#glossary) injection via escaping bypass             | [FND-SEC-004](#fnd-sec-004-text-substitution-sql-without-dbparameter-pathway-energycore)                                                                                        |
| [RSK-005](#rsk-005-silent-corruption-via-swallowed-errors-and-crc-defect-energycore) | MEDIUM   | Medium     | Silent corruption via swallowed errors                     | [FND-CQY-001](#fnd-cqy-001-exception-swallowing-beyond-documented-scope-energycore), [FND-CQY-003](#fnd-cqy-003-crc32-hashcore-contract-violation-energycore)                   |
| [RSK-006](#rsk-006-undetected-regressions-ship-energycore)                           | LOW      | Low        | Undetected regressions ship                                | [FND-INF-001](#fnd-inf-001-no-continuous-integration-energycore), [FND-INF-003](#fnd-inf-003-change-control-evidence-gap-energycore)                                            |
| [RSK-007](#rsk-007-continuity-and-knowledge-concentration-energycore)                | MEDIUM   | Medium     | Continuity and knowledge concentration                     | [FND-INF-005](#fnd-inf-005-contributor-concentration-single-author-history-energycore)                                                                                          |
| [RSK-008](#rsk-008-attribution-and-license-exposure-energycore)                      | MEDIUM   | Medium     | Attribution and license exposure                           | [FND-CPR-001](#fnd-cpr-001-third-party-attribution-gap-for-ported-algorithms-energycore), [FND-CPR-003](#fnd-cpr-003-license-declarations-contradict-licensetxt-energycore)     |
| [RSK-009](#rsk-009-per-target-behavioral-divergence-energycore)                      | MEDIUM   | Medium     | Per-target behavioral divergence                           | [FND-ARC-003](#fnd-arc-003-silent-per-target-capability-divergence-energycore)                                                                                                  |
| [RSK-010](#rsk-010-provenance-ambiguity-energycore)                                  | MEDIUM   | Low        | Provenance ambiguity                                       | [FND-AIP-001](#fnd-aip-001-no-provenance-policy-origin-unrecorded-energycore)                                                                                                   |
| [RSK-011](#rsk-011-silent-breaking-changes-energycore)                               | MEDIUM   | Medium     | Silent breaking changes                                    | [FND-API-001](#fnd-api-001-untracked-public-surface-no-compatibility-gate-energycore), [FND-API-002](#fnd-api-002-versioning-practice-diverges-from-declared-scheme-energycore) |
| [RSK-012](#rsk-012-license-identity-ambiguity-for-consumers-energycore)              | MEDIUM   | Medium     | License-identity ambiguity for consumers                   | [FND-CPR-003](#fnd-cpr-003-license-declarations-contradict-licensetxt-energycore)                                                                                               |

### RSK-001: Process-wide MITM exposure via TLS bypass (Energy.Core/)

An opted-in global validation bypass exposes every [TLS](#glossary) consumer in the process.

* **Severity:** MEDIUM
* **Likelihood:** Medium
* **Residual:** Medium until scoped or retired
* **Impact:** Traffic interception and modification
* **Trigger:** Consumer sets `Web.IgnoreCertificateValidation`
* **Controls:** Opt-in default off
* **Closure:** Bypass scoped, retired, or documented with caller obligation
* **Source:** [FND-SEC-001](#fnd-sec-001-global-tls-certificate-validation-bypass-api-energycore)
* **Confidence:** High
* **Mitigation:** [REC-001](#rec-001-scope-or-retire-the-global-tls-bypass-energycore)

### RSK-002: Legacy primitive selection by consumers (Energy.Core/)

MD5, SHA1, and DES ship unguarded with a DES IV-derivation defect.

* **Severity:** MEDIUM
* **Likelihood:** Medium
* **Residual:** Medium until warnings or obsolescence land
* **Impact:** Weak confidentiality and integrity guarantees
* **Trigger:** Consumer selects a legacy primitive
* **Controls:** None observed
* **Closure:** Warnings or `[Obsolete]` markers plus fixed [IV](#glossary) derivation
* **Source:** [FND-SEC-002](#fnd-sec-002-legacy-cryptographic-primitives-without-warnings-energycore)
* **Confidence:** High
* **Mitigation:** [REC-002](#rec-002-warn-on-legacy-cryptographic-primitives-energycore)

### RSK-003: Decompression-bomb resource exhaustion (Energy.Core/)

Unbounded output growth on crafted compressed input.

* **Severity:** MEDIUM
* **Likelihood:** Medium
* **Residual:** Medium until output bounds exist
* **Impact:** Memory exhaustion in the host process
* **Trigger:** Malicious or corrupt compressed input
* **Controls:** EOF exceptions map malformed input to `null`
* **Closure:** Output-size cap or documented caller limit
* **Source:** [FND-SEC-003](#fnd-sec-003-unbounded-decompression-output-growth-energycore)
* **Confidence:** High
* **Mitigation:** [REC-003](#rec-003-bound-decompression-output-energycore)

### RSK-004: SQL injection via escaping bypass (Energy.Core/)

Text-substitution [SQL](#glossary) relies on escaping as the single defense.

* **Severity:** MEDIUM
* **Likelihood:** Medium
* **Residual:** Medium until a parameter pathway exists
* **Impact:** Data exposure or modification at the database
* **Trigger:** Unescaped value or caller bypass of the escaping path
* **Controls:** `''` doubling by default
* **Closure:** Parameterized value [API](#glossary) or documented escaping contract
* **Source:** [FND-SEC-004](#fnd-sec-004-text-substitution-sql-without-dbparameter-pathway-energycore)
* **Confidence:** Medium
* **Mitigation:** [REC-013](#rec-013-document-the-sql-escaping-contract-energycore)

### RSK-005: Silent corruption via swallowed errors and CRC defect (Energy.Core/)

Empty catches and the CRC32 bound defect let failures pass silently.

* **Severity:** MEDIUM
* **Likelihood:** Medium
* **Residual:** Medium until catches re-scope and the bound is fixed
* **Impact:** Silent data corruption and skipped error paths
* **Trigger:** Non-conversion failure or segmented hash input
* **Controls:** Documented default-on-failure covers conversions only
* **Closure:** Catch scope matches the documented policy, `HashCore` bound fixed
* **Source:** [FND-CQY-001](#fnd-cqy-001-exception-swallowing-beyond-documented-scope-energycore), [FND-CQY-003](#fnd-cqy-003-crc32-hashcore-contract-violation-energycore)
* **Confidence:** High
* **Mitigation:** [REC-005](#rec-005-fix-the-crc32-hashcore-bound-energycore), [REC-011](#rec-011-re-scope-empty-catches-energycore)

### RSK-006: Undetected regressions ship (Energy.Core/)

No [CI](#glossary) runs the documented build and test loop.

* **Severity:** LOW
* **Likelihood:** Low
* **Residual:** Low until minimal CI exists
* **Impact:** Broken code reaches releases undetected
* **Trigger:** Commit without manual verification
* **Controls:** Manual loop, warnings-as-errors
* **Closure:** Pipeline running build and test on commits
* **Source:** [FND-INF-001](#fnd-inf-001-no-continuous-integration-energycore), [FND-INF-003](#fnd-inf-003-change-control-evidence-gap-energycore)
* **Confidence:** High
* **Mitigation:** [REC-004](#rec-004-minimal-continuous-integration-energycore)

### RSK-007: Continuity and knowledge concentration (Energy.Core/)

92 percent of non-merge commits sit under one author name.

* **Severity:** MEDIUM
* **Likelihood:** Medium
* **Residual:** Medium, structural
* **Impact:** Key-person dependency, review scarcity
* **Trigger:** Maintainer unavailability
* **Controls:** None observed
* **Closure:** Broadened authorship or recorded maintainer handoff
* **Source:** [FND-INF-005](#fnd-inf-005-contributor-concentration-single-author-history-energycore)
* **Confidence:** High
* **Mitigation:** Documented handoff or review practice

### RSK-008: Attribution and license exposure (Energy.Core/)

Empty third-party attribution beside ported algorithms plus the license contradiction.

* **Severity:** MEDIUM
* **Likelihood:** Medium
* **Residual:** Medium until attribution and license reconcile
* **Impact:** License obligations unmet, consumer confusion
* **Trigger:** Distribution or audit of the package
* **Controls:** COPYRIGHTS.md prescribes the practice
* **Closure:** Attribution populated, license declarations consistent
* **Source:** [FND-CPR-001](#fnd-cpr-001-third-party-attribution-gap-for-ported-algorithms-energycore), [FND-CPR-003](#fnd-cpr-003-license-declarations-contradict-licensetxt-energycore)
* **Confidence:** High
* **Mitigation:** [REC-006](#rec-006-populate-third-party-attribution-energycore), [REC-017](#rec-017-reconcile-license-declarations-energycore)

### RSK-009: Per-target behavioral divergence (Energy.Core/)

`NETCF` paths silently return `null` or no-op where other targets act.

* **Severity:** MEDIUM
* **Likelihood:** Medium
* **Residual:** Medium until a capability matrix exists
* **Impact:** Behavior differs by target without notice
* **Trigger:** Consumer runs on the Compact target
* **Controls:** None observed
* **Closure:** Published per-target capability matrix
* **Source:** [FND-ARC-003](#fnd-arc-003-silent-per-target-capability-divergence-energycore)
* **Confidence:** High
* **Mitigation:** [REC-008](#rec-008-per-target-capability-matrix-energycore)

### RSK-010: Provenance ambiguity (Energy.Core/)

No policy records code origin or AI attribution.

* **Severity:** MEDIUM
* **Likelihood:** Low
* **Residual:** Medium until a policy exists
* **Impact:** Origin of contributions undeterminable
* **Trigger:** Provenance question from a consumer or reviewer
* **Controls:** None observed
* **Closure:** Provenance policy recorded in `docs/`
* **Source:** [FND-AIP-001](#fnd-aip-001-no-provenance-policy-origin-unrecorded-energycore)
* **Confidence:** Medium
* **Mitigation:** [REC-012](#rec-012-provenance-policy-statement-energycore)

### RSK-011: Silent breaking changes (Energy.Core/)

The public [API](#glossary) surface is untracked and ungated.

* **Severity:** MEDIUM
* **Likelihood:** Medium
* **Residual:** Medium until a compatibility gate exists
* **Impact:** Consumers break on upgrade without notice
* **Trigger:** Any public-surface change
* **Controls:** `[Obsolete]` markers exist, no gate
* **Closure:** [API](#glossary) baseline or package validation enabled
* **Source:** [FND-API-001](#fnd-api-001-untracked-public-surface-no-compatibility-gate-energycore), [FND-API-002](#fnd-api-002-versioning-practice-diverges-from-declared-scheme-energycore)
* **Confidence:** High
* **Mitigation:** [REC-015](#rec-015-adopt-an-api-compatibility-gate-energycore), [REC-016](#rec-016-declare-the-breaking-change-policy-energycore)

### RSK-012: License-identity ambiguity for consumers (Energy.Core/)

BSD-3-Clause and LGPL declarations coexist.

* **Severity:** MEDIUM
* **Likelihood:** Medium
* **Residual:** Medium until declarations reconcile
* **Impact:** Consumers cannot determine obligations
* **Trigger:** License review or redistribution
* **Controls:** None observed
* **Closure:** One license identity across all artifacts
* **Source:** [FND-CPR-003](#fnd-cpr-003-license-declarations-contradict-licensetxt-energycore)
* **Confidence:** High
* **Mitigation:** [REC-017](#rec-017-reconcile-license-declarations-energycore)

### Remediation Roadmap

| Rec                                                                         | Priority | Finding                                                                                                                                                                      | Recommendation                                     | Impact | Effort | Complexity | Breaking   | Verification                       |
|-----------------------------------------------------------------------------|----------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|----------------------------------------------------|--------|--------|------------|------------|------------------------------------|
| [REC-001](#rec-001-scope-or-retire-the-global-tls-bypass-energycore)        | P1       | [FND-SEC-001](#fnd-sec-001-global-tls-certificate-validation-bypass-api-energycore)                                                                                          | Scope or retire the global [TLS](#glossary) bypass | High   | Medium | Medium     | Public API | Source review plus test            |
| [REC-002](#rec-002-warn-on-legacy-cryptographic-primitives-energycore)      | P1       | [FND-SEC-002](#fnd-sec-002-legacy-cryptographic-primitives-without-warnings-energycore)                                                                                      | Warn on legacy primitives, fix DES IV              | High   | Low    | Low        | Internal   | Analyzer output, tests             |
| [REC-003](#rec-003-bound-decompression-output-energycore)                   | P1       | [FND-SEC-003](#fnd-sec-003-unbounded-decompression-output-growth-energycore)                                                                                                 | Bound decompression output                         | High   | Medium | Medium     | None       | Unit test with cap                 |
| [REC-004](#rec-004-minimal-continuous-integration-energycore)               | P1       | [FND-INF-001](#fnd-inf-001-no-continuous-integration-energycore)                                                                                                             | Minimal CI running the documented loop             | High   | Low    | Low        | None       | Green pipeline run                 |
| [REC-005](#rec-005-fix-the-crc32-hashcore-bound-energycore)                 | P1       | [FND-CQY-003](#fnd-cqy-003-crc32-hashcore-contract-violation-energycore)                                                                                                     | Fix the CRC32 `HashCore` bound                     | High   | Low    | Low        | None       | Segmented-input test               |
| [REC-006](#rec-006-populate-third-party-attribution-energycore)             | P2       | [FND-CPR-001](#fnd-cpr-001-third-party-attribution-gap-for-ported-algorithms-energycore)                                                                                     | Populate third-party attribution                   | Medium | Low    | Low        | None       | COPYRIGHTS.md section              |
| [REC-007](#rec-007-close-the-highest-value-test-gaps-energycore)            | P2       | [FND-CQY-002](#fnd-cqy-002-untested-public-surface-energycore)                                                                                                               | Close the highest-value test gaps                  | Medium | High   | Medium     | None       | Test count and coverage            |
| [REC-008](#rec-008-per-target-capability-matrix-energycore)                 | P2       | [FND-ARC-003](#fnd-arc-003-silent-per-target-capability-divergence-energycore)                                                                                               | Per-target capability matrix                       | Medium | Medium | Low        | None       | Published matrix doc               |
| [REC-009](#rec-009-record-durable-decisions-as-adrs-energycore)             | P2       | [FND-ARC-004](#fnd-arc-004-no-architecture-decision-records-energycore)                                                                                                      | Record durable decisions as [ADRs](#glossary)      | Medium | Low    | Low        | None       | [ADR](#glossary) files under docs/ |
| [REC-010](#rec-010-anchor-releases-and-prove-committed-binaries-energycore) | P2       | [FND-INF-002](#fnd-inf-002-opaque-committed-binaries-no-supply-chain-verification-energycore), [FND-INF-004](#fnd-inf-004-release-anchoring-and-artifact-hygiene-energycore) | Anchor releases, prove committed binaries          | Medium | Low    | Low        | None       | Tags plus hash manifest            |
| [REC-011](#rec-011-re-scope-empty-catches-energycore)                       | P3       | [FND-CQY-001](#fnd-cqy-001-exception-swallowing-beyond-documented-scope-energycore)                                                                                          | Re-scope empty catches to policy                   | Medium | Medium | Low        | None       | Catch census shrinks               |
| [REC-012](#rec-012-provenance-policy-statement-energycore)                  | P3       | [FND-AIP-001](#fnd-aip-001-no-provenance-policy-origin-unrecorded-energycore)                                                                                                | Provenance policy statement                        | Low    | Low    | Low        | None       | Policy document                    |
| [REC-013](#rec-013-document-the-sql-escaping-contract-energycore)           | P3       | [FND-SEC-004](#fnd-sec-004-text-substitution-sql-without-dbparameter-pathway-energycore)                                                                                     | Document the [SQL](#glossary) escaping contract    | Medium | Low    | Low        | None       | Contract document                  |
| [REC-014](#rec-014-backfill-2670-change-requests-energycore)                | P3       | [FND-INF-003](#fnd-inf-003-change-control-evidence-gap-energycore)                                                                                                           | Backfill 26.7.0 change requests                    | Low    | Low    | Low        | None       | `docs/change/26.7.0/`              |
| [REC-015](#rec-015-adopt-an-api-compatibility-gate-energycore)              | P2       | [FND-API-001](#fnd-api-001-untracked-public-surface-no-compatibility-gate-energycore)                                                                                        | Adopt an [API](#glossary) compatibility gate       | Medium | Medium | Medium     | None       | Gate artifact in build             |
| [REC-016](#rec-016-declare-the-breaking-change-policy-energycore)           | P3       | [FND-API-002](#fnd-api-002-versioning-practice-diverges-from-declared-scheme-energycore)                                                                                     | Declare breaking-change policy                     | Low    | Low    | Low        | None       | Policy in VERSIONING.md            |
| [REC-017](#rec-017-reconcile-license-declarations-energycore)               | P1       | [FND-CPR-003](#fnd-cpr-003-license-declarations-contradict-licensetxt-energycore)                                                                                            | Reconcile license declarations                     | High   | Low    | Low        | None       | Consistent declarations            |

Findings [FND-ARC-001](#fnd-arc-001-monolithic-multi-domain-assembly-energycore), [FND-ARC-002](#fnd-arc-002-process-global-mutable-state-energycore), [FND-CQY-004](#fnd-cqy-004-xml-documentation-drift-energycore), [FND-CQY-005](#fnd-cqy-005-public-mutable-fields-as-api-surface-energycore), and [FND-INF-005](#fnd-inf-005-contributor-concentration-single-author-history-energycore) carry an
`Accepted - no action` disposition - the monolith and statics are load-bearing design,
the documentation and field items are deferred hygiene, and continuity is structural.

### REC-001: Scope or retire the global TLS bypass (Energy.Core/)

Limits the bypass blast radius or removes it.

* **Priority:** P1
* **Finding:** Energy.Core/::FND-SEC-001
* **Postcondition:** No process-wide unguarded [TLS](#glossary) bypass ships
* **Impact:** Removes MITM exposure for consumers
* **Effort:** Medium
* **Complexity:** Medium
* **Verification:** Source review plus a test covering the scoped behavior

### REC-002: Warn on legacy cryptographic primitives (Energy.Core/)

Marks MD5, SHA1, DES and fixes DES IV derivation.

* **Priority:** P1
* **Finding:** Energy.Core/::FND-SEC-002
* **Postcondition:** Legacy primitives warn or are `[Obsolete]`, [IV](#glossary) derives correctly
* **Impact:** Consumers see the risk before selecting weak algorithms
* **Effort:** Low
* **Complexity:** Low
* **Verification:** `[Obsolete]` markers present, [IV](#glossary) not equal to key

### REC-003: Bound decompression output (Energy.Core/)

Caps decoded size on all decompressors.

* **Priority:** P1
* **Finding:** Energy.Core/::FND-SEC-003
* **Postcondition:** Decoders reject or truncate beyond a documented bound
* **Impact:** Closes the resource-exhaustion path
* **Effort:** Medium
* **Complexity:** Medium
* **Verification:** Unit test asserting the cap on crafted input

### REC-004: Minimal continuous integration (Energy.Core/)

Runs the documented build and test loop on commits.

* **Priority:** P1
* **Finding:** Energy.Core/::FND-INF-001
* **Postcondition:** Pipeline runs `dotnet build` and `dotnet test` green
* **Impact:** Regressions blocked before release
* **Effort:** Low
* **Complexity:** Low
* **Verification:** Pipeline run evidence on a commit

### REC-005: Fix the CRC32 `HashCore` bound (Energy.Core/)

Corrects the loop to `i < start + length`.

* **Priority:** P1
* **Finding:** Energy.Core/::FND-CQY-003
* **Postcondition:** Segmented input produces correct CRC32
* **Impact:** Restores hash contract correctness
* **Effort:** Low
* **Complexity:** Low
* **Verification:** Test with non-zero `start` offset

### REC-006: Populate third-party attribution (Energy.Core/)

Records ported algorithm origins and licenses.

* **Priority:** P2
* **Finding:** Energy.Core/::FND-CPR-001
* **Postcondition:** `docs/COPYRIGHTS.md` third-party section lists ZX0, LZ4, LZSS
* **Impact:** License obligations become visible
* **Effort:** Low
* **Complexity:** Low
* **Verification:** Attribution entries present with source and license

### REC-007: Close the highest-value test gaps (Energy.Core/)

Adds tests for `Cipher`, `Log`, `Ini`, `Yaml`, and query paths.

* **Priority:** P2
* **Finding:** Energy.Core/::FND-CQY-002
* **Postcondition:** Named modules carry unit tests
* **Impact:** Verifies the untested public surface
* **Effort:** High
* **Complexity:** Medium
* **Verification:** Test-class census delta and passing runs

### REC-008: Per-target capability matrix (Energy.Core/)

Documents which [APIs](#glossary) act, return `null`, or no-op per target.

* **Priority:** P2
* **Finding:** Energy.Core/::FND-ARC-003
* **Postcondition:** Capability matrix published for all targets
* **Impact:** Consumers see divergence before adopting
* **Effort:** Medium
* **Complexity:** Low
* **Verification:** Matrix document covering `NETCF` paths

### REC-009: Record durable decisions as ADRs (Energy.Core/)

Writes ADRs for multi-targeting, crypto retention, and [SQL](#glossary) design.

* **Priority:** P2
* **Finding:** Energy.Core/::FND-ARC-004
* **Postcondition:** [ADR](#glossary) records exist under `docs/`
* **Impact:** Rationale survives team changes
* **Effort:** Low
* **Complexity:** Low
* **Verification:** [ADR](#glossary) files present

### REC-010: Anchor releases and prove committed binaries (Energy.Core/)

Tags releases and publishes hashes for committed binaries.

* **Priority:** P2
* **Finding:** Energy.Core/::FND-INF-002, [FND-INF-004](#fnd-inf-004-release-anchoring-and-artifact-hygiene-energycore)
* **Postcondition:** Each release carries a tag and a hash manifest
* **Impact:** Released code is re-derivable and binaries verifiable
* **Effort:** Low
* **Complexity:** Low
* **Verification:** Tag list and manifest artifacts

### REC-011: Re-scope empty catches (Energy.Core/)

Aligns catch bodies with the documented conversion policy.

* **Priority:** P3
* **Finding:** Energy.Core/::FND-CQY-001
* **Postcondition:** Empty catches exist only inside conversion defaults
* **Impact:** Failures surface outside conversions
* **Effort:** Medium
* **Complexity:** Low
* **Verification:** Catch census shrinks to the documented scope

### REC-012: Provenance policy statement (Energy.Core/)

Records the code-origin and AI-attribution policy.

* **Priority:** P3
* **Finding:** Energy.Core/::FND-AIP-001
* **Postcondition:** Policy document exists in `docs/`
* **Impact:** Provenance questions have a recorded answer
* **Effort:** Low
* **Complexity:** Low
* **Verification:** Policy file present

### REC-013: Document the SQL escaping contract (Energy.Core/)

States the escaping guarantee and caller obligations.

* **Priority:** P3
* **Finding:** Energy.Core/::FND-SEC-004
* **Postcondition:** Escaping contract documented beside `Query`
* **Impact:** Callers know the single defense line
* **Effort:** Low
* **Complexity:** Low
* **Verification:** Contract documented and linked

### REC-014: Backfill 26.7.0 change requests (Energy.Core/)

Writes the missing change record for the current series.

* **Priority:** P3
* **Finding:** Energy.Core/::FND-INF-003
* **Postcondition:** `docs/change/26.7.0/` exists
* **Impact:** Change history stays complete
* **Effort:** Low
* **Complexity:** Low
* **Verification:** Change-request files present

### REC-015: Adopt an API compatibility gate (Energy.Core/)

Adds `PublicApiAnalyzers` or `EnablePackageValidation`.

* **Priority:** P2
* **Finding:** Energy.Core/::FND-API-001
* **Postcondition:** Public-surface changes are gated at build
* **Impact:** Breaking changes become visible before release
* **Effort:** Medium
* **Complexity:** Medium
* **Verification:** Gate artifact in the csproj or baseline files

### REC-016: Declare the breaking-change policy (Energy.Core/)

States removal timelines for `[Obsolete]` members.

* **Priority:** P3
* **Finding:** Energy.Core/::FND-API-002
* **Postcondition:** Policy recorded in `docs/VERSIONING.md`
* **Impact:** Consumers can plan around removals
* **Effort:** Low
* **Complexity:** Low
* **Verification:** Policy text present

### REC-017: Reconcile license declarations (Energy.Core/)

Aligns `LICENSE.txt`, nuspec, csproj, and `COPYRIGHTS.md`.

* **Priority:** P1
* **Finding:** Energy.Core/::FND-CPR-003
* **Postcondition:** One license identity across all declarations
* **Impact:** Consumers can determine obligations
* **Effort:** Low
* **Complexity:** Low
* **Verification:** All declarations name the same license

### Recommendation Classification

| Rec                                                                         | Recommendation                                     | Class       | Basis                                                                                                                |
|-----------------------------------------------------------------------------|----------------------------------------------------|-------------|----------------------------------------------------------------------------------------------------------------------|
| [REC-001](#rec-001-scope-or-retire-the-global-tls-bypass-energycore)        | Scope or retire the global [TLS](#glossary) bypass | Recommended | [FND-SEC-001](#fnd-sec-001-global-tls-certificate-validation-bypass-api-energycore) - closes a HIGH gap              |
| [REC-002](#rec-002-warn-on-legacy-cryptographic-primitives-energycore)      | Warn on legacy primitives, fix DES IV              | Recommended | [FND-SEC-002](#fnd-sec-002-legacy-cryptographic-primitives-without-warnings-energycore) - cheap hardening            |
| [REC-003](#rec-003-bound-decompression-output-energycore)                   | Bound decompression output                         | Recommended | [FND-SEC-003](#fnd-sec-003-unbounded-decompression-output-growth-energycore) - closes a HIGH gap                     |
| [REC-004](#rec-004-minimal-continuous-integration-energycore)               | Minimal continuous integration                     | Recommended | [FND-INF-001](#fnd-inf-001-no-continuous-integration-energycore) - regression evidence                               |
| [REC-005](#rec-005-fix-the-crc32-hashcore-bound-energycore)                 | Fix the CRC32 `HashCore` bound                     | Recommended | [FND-CQY-003](#fnd-cqy-003-crc32-hashcore-contract-violation-energycore) - confirmed defect                          |
| [REC-006](#rec-006-populate-third-party-attribution-energycore)             | Populate third-party attribution                   | Recommended | [FND-CPR-001](#fnd-cpr-001-third-party-attribution-gap-for-ported-algorithms-energycore) - declared obligation       |
| [REC-007](#rec-007-close-the-highest-value-test-gaps-energycore)            | Close the highest-value test gaps                  | Optional    | [FND-CQY-002](#fnd-cqy-002-untested-public-surface-energycore) - large effort                                        |
| [REC-008](#rec-008-per-target-capability-matrix-energycore)                 | Per-target capability matrix                       | Recommended | [FND-ARC-003](#fnd-arc-003-silent-per-target-capability-divergence-energycore) - bounds divergence                   |
| [REC-009](#rec-009-record-durable-decisions-as-adrs-energycore)             | Record durable decisions as [ADRs](#glossary)      | Optional    | [FND-ARC-004](#fnd-arc-004-no-architecture-decision-records-energycore) - documentation gain                         |
| [REC-010](#rec-010-anchor-releases-and-prove-committed-binaries-energycore) | Anchor releases and prove binaries                 | Recommended | [FND-INF-002](#fnd-inf-002-opaque-committed-binaries-no-supply-chain-verification-energycore) - verifiable artifacts |
| [REC-011](#rec-011-re-scope-empty-catches-energycore)                       | Re-scope empty catches                             | Optional    | [FND-CQY-001](#fnd-cqy-001-exception-swallowing-beyond-documented-scope-energycore) - partial residue is documented  |
| [REC-012](#rec-012-provenance-policy-statement-energycore)                  | Provenance policy statement                        | Optional    | [FND-AIP-001](#fnd-aip-001-no-provenance-policy-origin-unrecorded-energycore) - low urgency                          |
| [REC-013](#rec-013-document-the-sql-escaping-contract-energycore)           | Document the [SQL](#glossary) escaping contract    | Recommended | [FND-SEC-004](#fnd-sec-004-text-substitution-sql-without-dbparameter-pathway-energycore) - clarifies the defense     |
| [REC-014](#rec-014-backfill-2670-change-requests-energycore)                | Backfill 26.7.0 change requests                    | Optional    | [FND-INF-003](#fnd-inf-003-change-control-evidence-gap-energycore) - record hygiene                                  |
| [REC-015](#rec-015-adopt-an-api-compatibility-gate-energycore)              | Adopt an [API](#glossary) compatibility gate       | Recommended | [FND-API-001](#fnd-api-001-untracked-public-surface-no-compatibility-gate-energycore) - gates silent breaks          |
| [REC-016](#rec-016-declare-the-breaking-change-policy-energycore)           | Declare the breaking-change policy                 | Optional    | [FND-API-002](#fnd-api-002-versioning-practice-diverges-from-declared-scheme-energycore) - pairs with the gate       |
| [REC-017](#rec-017-reconcile-license-declarations-energycore)               | Reconcile license declarations                     | Recommended | [FND-CPR-003](#fnd-cpr-003-license-declarations-contradict-licensetxt-energycore) - resolves a HIGH gap              |

### Structure Conformance

| Rule                    | Status  | Evidence                                   |
|-------------------------|---------|--------------------------------------------|
| Declared layout         | PASS    | Tree matches `docs/STRUCTURE.md`           |
| Test co-location        | PASS    | `Energy.Core.Test/` mirrors `Energy.Core/` |
| Artifact separation     | PARTIAL | Tracked `.7z` and `.dll` under source tree |
| Source-only directories | PARTIAL | `Snippet/` two files, `logs/` present      |

### Code Style

Convention tier resolved to T1 - the project's own `docs/GUIDELINES.md` is authoritative.

| Lens                | Status  | Census                                             |
|---------------------|---------|----------------------------------------------------|
| Naming              | PASS    | PascalCase members, `Energy.*` namespaces          |
| Layout              | PASS    | Region blocks, one public type per file dominant   |
| Comment style       | PASS    | `////` continuation comments consistent            |
| Legacy syntax rules | PASS    | No `nameof`, no auto-property initializers sampled |
| Marker hygiene      | PARTIAL | 15 TODO/HACK markers remain                        |

Census denominators exclude `Energy.Core.Example/`, `work/`, generated `*.Designer.cs`,
and `.gitignore`-matched trees.



## Energy.Core.Test/

### Executive Summary

| Field          | Value                                                |
|----------------|------------------------------------------------------|
| System type    | Codebase, test suite                                 |
| Scope          | `Energy.Core.Test/`                                  |
| Source basis   | Inspected code, revision `9447e87`                   |
| Maturity level | Production-ready                                     |
| Overall score  | 5.8/10 (Average), lowest: Delivery & Continuity 4/10 |

The suite holds 50 test classes and 344 `[TestMethod]` methods targeting `net48` under
[MSTest](#glossary) 4.0.2.

### System Context

| Aspect                 | Detail                                               |
|------------------------|------------------------------------------------------|
| Functional description | Unit tests for the library                           |
| Architecture overview  | Mirrors `Energy.Core/` module layout                 |
| Key components         | `Base/`, `Core/`, `Query/`, `Source/`                |
| External dependencies  | [MSTest](#glossary) 4.0.2, test SDK, 13 package refs |
| Assumptions            | `NOT SPECIFIED`                                      |

### Technology Stack

| Layer            | Technology                                               |
|------------------|----------------------------------------------------------|
| Languages        | C#                                                       |
| Frameworks       | .NET Framework 4.8 test host                             |
| Test tooling     | [MSTest](#glossary) 4.0.2, Microsoft.NET.Test.Sdk 17.8.0 |
| Package manager  | [NuGet](#glossary), 13 PackageReference entries          |
| Target platforms | `net48` test run                                         |

### Software Bill of Materials

| Component                              | Version | License | Risk | Advisory | Source                    |
|----------------------------------------|---------|---------|------|----------|---------------------------|
| Microsoft.NET.Test.Sdk                 | 17.8.0  | Unknown | Test | N        | `Energy.Core.Test.csproj` |
| [MSTest](#glossary).TestAdapter        | 4.0.2   | Unknown | Test | N        | `Energy.Core.Test.csproj` |
| [MSTest](#glossary).TestFramework      | 4.0.2   | Unknown | Test | N        | `Energy.Core.Test.csproj` |
| Microsoft.Bcl.AsyncInterfaces          | 10.0.1  | Unknown | Test | N        | `Energy.Core.Test.csproj` |
| System.Buffers                         | 4.6.1   | Unknown | Test | N        | `Energy.Core.Test.csproj` |
| System.IO.Pipelines                    | 10.0.1  | Unknown | Test | N        | `Energy.Core.Test.csproj` |
| System.Memory                          | 4.6.3   | Unknown | Test | N        | `Energy.Core.Test.csproj` |
| System.Numerics.Vectors                | 4.6.1   | Unknown | Test | N        | `Energy.Core.Test.csproj` |
| System.Runtime.CompilerServices.Unsafe | 6.1.2   | Unknown | Test | N        | `Energy.Core.Test.csproj` |
| System.Text.Encodings.Web              | 10.0.1  | Unknown | Test | N        | `Energy.Core.Test.csproj` |
| System.Text.Json                       | 10.0.1  | Unknown | Test | N        | `Energy.Core.Test.csproj` |

Manifest-derived inventory, license cells are `Unknown` where the manifest does not declare one.

No lockfile exists, transitive closure is `UNKNOWN`.

### License Compliance

| License class | Components | Notes                       |
|---------------|------------|-----------------------------|
| Permissive    | 0          | Declarations not inspected  |
| Unknown       | 11         | No license fields in csproj |

Test-scope packages carry no inspected license declaration, recorded under [FND-INF-001](#fnd-inf-001-no-committed-test-run-evidence-energycoretest)
dependency evidence.

### Health Dashboard

**Risk Heat Map**

| Impact   | LOW | MEDIUM                                                                | HIGH |
|----------|-----|-----------------------------------------------------------------------|------|
| CRITICAL |     |                                                                       |      |
| HIGH     |     |                                                                       |      |
| MEDIUM   |     | [RSK-002](#rsk-002-dependency-advisory-status-unknown-energycoretest) |      |
| LOW      |     | [RSK-001](#rsk-001-test-gap-masks-regressions-energycoretest)         |      |

**Scorecard Summary**

`1-3 Poor · 4-6 Average · 7-8 Good · 9-10 Excellent`

| Dimension               | Score   | Notes                                 |
|-------------------------|---------|---------------------------------------|
| Design Soundness        | 6       | Mirrors library layout                |
| Code Quality            | 6       | Zero-byte placeholder file present    |
| Code Style              | 7       | Consistent MSTest idioms              |
| Stack Alignment         | 7       | Current MSTest and test SDK           |
| Dependency Health       | 5       | 13 refs, advisories unchecked         |
| Maintainability         | 6       | Cohesive, mirrors subjects            |
| Structure               | 6       | Matches declared test layout          |
| Security                | 6       | Test-scope only, no product surface   |
| Compliance              | 6       | Package declarations recorded         |
| Observability           | 5       | Test-results artifacts uncommitted    |
| Delivery & Continuity   | 4       | Same concentrated authorship          |
| AI Provenance           | UNKNOWN | No provenance baseline                |
| Originality & Licensing | 5       | Inherits repository license ambiguity |

Overall score is 5.8/10, the unweighted mean of the 13 scored dimensions, 75 divided by 13.

`N/A` dimensions - Testability, Deployability, Scalability, Operational Safety, Skill
Definition, API Compatibility - are omitted, the suite is never itself deployed or packaged.

The lowest-scoring applicable dimension is Delivery & Continuity at 4/10.

### Delivery Practice & Team Continuity

| [DORA](#glossary) metric        | Result          | Basis                         |
|---------------------------------|-----------------|-------------------------------|
| Change lead time                | `NOT SPECIFIED` | Shares the repository history |
| Deployment frequency            | `NOT SPECIFIED` | Never deployed                |
| Failed deployment recovery time | `NOT SPECIFIED` | Requires incident data        |
| Change fail rate                | `NOT SPECIFIED` | Requires rollback data        |
| Deployment rework rate          | `NOT SPECIFIED` | Requires incident data        |

Contributor concentration mirrors the repository, `HIGH`, per [EVD-006](#auditing-methodology).

### High-Level Observations

| Observation                                                   |
|---------------------------------------------------------------|
| One-third of public modules have tests, core coverage is thin |
| A zero-byte test file ships in the suite                      |
| No run evidence exists - `TestResults/` is untracked          |

**Thin coverage**

50 classes and 344 methods against 164 library sources leave `Cipher`, `Log`, `Ini`,
`Yaml`, and most of `Query` and `Source` untested, per [FND-CQY-001](#fnd-cqy-001-thin-coverage-of-the-public-surface-energycoretest).

### Auditing Methodology

The same criterion families apply with test-scope readings, TST-UNIT reads as coverage
adequacy and DEP-MGMT as package-manifest hygiene.

The evidence ledger above covers this project through [EVD-003](#auditing-methodology), [EVD-032](#auditing-methodology), and [EVD-038](#auditing-methodology).

### Scoring Rubrics

The shared band table applies - `1-3 Poor`, `4-6 Average`, `7-8 Good`, `9-10 Excellent`.

### Architectural Assessment

The suite mirrors the library layout one-to-one, which is the correct organization for a
unit-test project.

No conditional subsections apply beyond the shared census.

### Structure Review

[MSTest](#glossary) suite, mirrored layout against `Energy.Core/`.

| Area                       | Verdict | Basis                             |
|----------------------------|---------|-----------------------------------|
| Directory organization     | PASS    | Mirrors the library module layout |
| Naming conventions         | PASS    | `*Test` classes, mirrored paths   |
| Component placement        | PARTIAL | Zero-byte `Core/Shell.cs`         |
| Supporting-artifact layout | PARTIAL | `TestResults/` untracked on disk  |
| Consistency                | PASS    | Uniform MSTest idioms             |

Positive practices: consistent class-per-module mirroring.

Limitations: test-source review is sampled, not exhaustive.

### Trade-off Analysis

| Trade-off       | Context                | Option A: gain / cost                        | Option B: gain / cost          | Evidence                         | Implication                               |
|-----------------|------------------------|----------------------------------------------|--------------------------------|----------------------------------|-------------------------------------------|
| net48 test host | Library targets net20+ | Real-framework verification / narrower cover | Multi-target tests / tool cost | [EVD-003](#auditing-methodology) | Reasonable, leaves netstandard unverified |

### Standards Conformance

`docs/TESTING.md` expects tests for public methods, PARTIAL per [FND-CQY-001](#fnd-cqy-001-thin-coverage-of-the-public-surface-energycoretest).

### Strengths & What's Working

- Mirrors the library structure, so module-to-test mapping is direct, [EVD-001](#auditing-methodology).
- Current MSTest and test-SDK versions, [EVD-003](#auditing-methodology).
- Reported 341 passing tests in the committed assessment, `Reported` evidence, [EVD-008](#auditing-methodology).

### Technical Findings

| Finding                                                                        | Pillar                 | Severity | Title                                 | Result  | Status | Change | Verification |
|--------------------------------------------------------------------------------|------------------------|----------|---------------------------------------|---------|--------|--------|--------------|
| [FND-CQY-001](#fnd-cqy-001-thin-coverage-of-the-public-surface-energycoretest) | Code Quality           | MEDIUM   | Thin coverage of the public surface   | PARTIAL | Open   | New    |              |
| [FND-CQY-002](#fnd-cqy-002-zero-byte-coreshellcs-placeholder-energycoretest)   | Code Quality           | LOW      | Zero-byte `Core/Shell.cs` placeholder | FAIL    | Open   | New    |              |
| [FND-INF-001](#fnd-inf-001-no-committed-test-run-evidence-energycoretest)      | Infrastructure & CI/CD | MEDIUM   | No committed test-run evidence        | PARTIAL | Open   | New    |              |

### FND-CQY-001: Thin coverage of the public surface (Energy.Core.Test/)

50 classes cover roughly a third of the public module surface.

* **Pillar:** Code Quality
* **Severity:** MEDIUM
* **Type:** Concern
* **Targets:** Energy.Core.Test/
* **Basis:** `docs/TESTING.md` coverage expectation
* **Criteria:** TST-UNIT
* **Impact:** Core utilities carry unverified behavior
* **Method:** Class census vs module map
* **Confidence:** High
* **Mitigating factors:** None observed
* **Evidence:** [EVD-001](#auditing-methodology), [EVD-003](#auditing-methodology)
* **Recommendation:** Close gaps per Energy.Core/::REC-007

### FND-CQY-002: Zero-byte `Core/Shell.cs` placeholder (Energy.Core.Test/)

`Energy.Core.Test/Core/Shell.cs` is a committed zero-byte file.

* **Pillar:** Code Quality
* **Severity:** LOW
* **Type:** Concern
* **Targets:** Energy.Core.Test/Core/Shell.cs
* **Basis:** Hygiene, dead placeholder content
* **Criteria:** CQY-DOC
* **Impact:** Dead file misleads the test map
* **Method:** File-size census
* **Confidence:** High
* **Mitigating factors:** None observed
* **Evidence:** [EVD-038](#auditing-methodology)
* **Recommendation:** Restore content or remove the file

### FND-INF-001: No committed test-run evidence (Energy.Core.Test/)

`TestResults/` is untracked and no committed result artifact exists.

* **Pillar:** Infrastructure & CI/CD
* **Severity:** MEDIUM
* **Type:** Concern
* **Targets:** Energy.Core.Test/
* **Basis:** Test evidence retention practice
* **Criteria:** INF-CICD
* **Impact:** Last-known green state is unverifiable from the tree
* **Method:** Artifact census
* **Confidence:** High
* **Absence:** No documented rationale - no committed or documented result archive
* **Mitigating factors:** None observed
* **Evidence:** [EVD-032](#auditing-methodology), [EVD-038](#auditing-methodology)
* **Recommendation:** Keep result artifacts or [CI](#glossary) logs per Energy.Core/::REC-004

### Unified Risk Register

| Risk                                                                  | Severity | Likelihood | Title                              | Source                                                                         |
|-----------------------------------------------------------------------|----------|------------|------------------------------------|--------------------------------------------------------------------------------|
| [RSK-001](#rsk-001-test-gap-masks-regressions-energycoretest)         | LOW      | Medium     | Test gap masks regressions         | [FND-CQY-001](#fnd-cqy-001-thin-coverage-of-the-public-surface-energycoretest) |
| [RSK-002](#rsk-002-dependency-advisory-status-unknown-energycoretest) | MEDIUM   | Medium     | Dependency advisory status unknown | [FND-INF-001](#fnd-inf-001-no-committed-test-run-evidence-energycoretest)      |

### RSK-001: Test gap masks regressions (Energy.Core.Test/)

Untested modules regress silently.

* **Severity:** LOW
* **Likelihood:** Medium
* **Residual:** Medium until coverage closes
* **Impact:** Defects ship undetected
* **Trigger:** Change in an untested module
* **Controls:** Existing suite covers a third
* **Closure:** Coverage over named modules
* **Source:** [FND-CQY-001](#fnd-cqy-001-thin-coverage-of-the-public-surface-energycoretest)
* **Confidence:** High
* **Mitigation:** [REC-001](#rec-001-close-highest-value-test-gaps-energycoretest)

### RSK-002: Dependency advisory status unknown (Energy.Core.Test/)

Thirteen test-scope packages carry no advisory check.

* **Severity:** MEDIUM
* **Likelihood:** Medium
* **Residual:** Medium until an advisory scan exists
* **Impact:** A vulnerable test dependency goes unnoticed
* **Trigger:** Advisory publication against a resolved version
* **Controls:** None observed
* **Closure:** Advisory review or scan evidence
* **Source:** [FND-INF-001](#fnd-inf-001-no-committed-test-run-evidence-energycoretest)
* **Confidence:** Medium
* **Mitigation:** [REC-002](#rec-002-advisory-review-of-test-packages-energycoretest)

### Remediation Roadmap

| Rec                                                                 | Priority | Finding                                                                        | Recommendation                   | Impact | Effort | Complexity | Breaking | Verification          |
|---------------------------------------------------------------------|----------|--------------------------------------------------------------------------------|----------------------------------|--------|--------|------------|----------|-----------------------|
| [REC-001](#rec-001-close-highest-value-test-gaps-energycoretest)    | P2       | [FND-CQY-001](#fnd-cqy-001-thin-coverage-of-the-public-surface-energycoretest) | Close highest-value test gaps    | Medium | High   | Medium     | None     | Coverage census       |
| [REC-002](#rec-002-advisory-review-of-test-packages-energycoretest) | P2       | [FND-INF-001](#fnd-inf-001-no-committed-test-run-evidence-energycoretest)      | Advisory review of test packages | Medium | Low    | Low        | None     | Scan or review record |
| [REC-003](#rec-003-remove-the-zero-byte-placeholder-energycoretest) | P3       | [FND-CQY-002](#fnd-cqy-002-zero-byte-coreshellcs-placeholder-energycoretest)   | Remove the zero-byte placeholder | Low    | Low    | Low        | None     | File removed          |

### REC-001: Close highest-value test gaps (Energy.Core.Test/)

Covers `Cipher`, `Log`, `Ini`, `Yaml`, and query paths.

* **Priority:** P2
* **Finding:** Energy.Core.Test/::FND-CQY-001
* **Postcondition:** Named modules carry unit tests
* **Impact:** Regression evidence on the public surface
* **Effort:** High
* **Complexity:** Medium
* **Verification:** Test census delta

### REC-002: Advisory review of test packages (Energy.Core.Test/)

Reviews the 13 resolved test packages for advisories.

* **Priority:** P2
* **Finding:** Energy.Core.Test/::FND-INF-001
* **Postcondition:** Advisory status recorded per package
* **Impact:** Supply-chain visibility on the test graph
* **Effort:** Low
* **Complexity:** Low
* **Verification:** Advisory review record

### REC-003: Remove the zero-byte placeholder (Energy.Core.Test/)

Restores or deletes `Core/Shell.cs`.

* **Priority:** P3
* **Finding:** Energy.Core.Test/::FND-CQY-002
* **Postcondition:** File carries tests or is absent
* **Impact:** Clean test map
* **Effort:** Low
* **Complexity:** Low
* **Verification:** File state in tree

### Recommendation Classification

| Rec                                                                 | Recommendation                   | Class       | Basis                                                                                             |
|---------------------------------------------------------------------|----------------------------------|-------------|---------------------------------------------------------------------------------------------------|
| [REC-001](#rec-001-close-highest-value-test-gaps-energycoretest)    | Close highest-value test gaps    | Recommended | [FND-CQY-001](#fnd-cqy-001-thin-coverage-of-the-public-surface-energycoretest) - verifies surface |
| [REC-002](#rec-002-advisory-review-of-test-packages-energycoretest) | Advisory review of test packages | Optional    | [FND-INF-001](#fnd-inf-001-no-committed-test-run-evidence-energycoretest) - test-scope exposure   |
| [REC-003](#rec-003-remove-the-zero-byte-placeholder-energycoretest) | Remove the zero-byte placeholder | Optional    | [FND-CQY-002](#fnd-cqy-002-zero-byte-coreshellcs-placeholder-energycoretest) - hygiene            |

### Code Style

T1 convention tier, sampled test classes follow the declared naming and layout rules.

| Lens          | Status | Census                                  |
|---------------|--------|-----------------------------------------|
| Naming        | PASS   | Declared conventions followed           |
| Layout        | PASS   | Region and file conventions hold        |
| Comment style | PASS   | `////` continuation comments consistent |

## Energy.Core.Compact/

### Executive Summary

| Field          | Value                                        |
|----------------|----------------------------------------------|
| System type    | Codebase, legacy preview project             |
| Scope          | `Energy.Core.Compact/`                       |
| Source basis   | Inspected code, revision `9447e87`           |
| Maturity level | Early development                            |
| Overall score  | 4.5/10 (Average), lowest: Deployability 3/10 |

The Compact tree is a minimal .NET Compact Framework preview application plus three
overlapping solution files, with library variants compiled from shared `Energy.Core/`
sources under `NETCF`.

### System Context

| Aspect                 | Detail                                   |
|------------------------|------------------------------------------|
| Functional description | Windows CE preview application           |
| Architecture overview  | MSBuild 2003 project, five source files  |
| Key components         | `Program.cs`, `View/Start`, `Test/Hello` |
| External dependencies  | .NET Compact Framework v2.0              |
| Assumptions            | `NOT SPECIFIED`                          |

### Technology Stack

| Layer            | Technology                                 |
|------------------|--------------------------------------------|
| Languages        | C#                                         |
| Frameworks       | .NET Compact Framework v2.0, Windows Forms |
| Build tooling    | MSBuild 2003 format, `ToolsVersion 3.5`    |
| Package manager  | None                                       |
| Target platforms | Windows CE (`OSVersion 4.20`)              |

### Software Bill of Materials

| Component | Version | License | Risk | Advisory | Source |
|-----------|---------|---------|------|----------|--------|

No package manifests exist, the project declares framework references only.

### License Compliance

| License class | Components | Notes                     |
|---------------|------------|---------------------------|
| Permissive    | 0          |                           |
| Unknown       | 0          | No third-party components |

The project inherits the repository license contradiction per Energy.Core/::FND-CPR-003.

### Health Dashboard

**Risk Heat Map**

| Impact   | LOW | MEDIUM                                                                                                                                   | HIGH |
|----------|-----|------------------------------------------------------------------------------------------------------------------------------------------|------|
| CRITICAL |     |                                                                                                                                          |      |
| HIGH     |     |                                                                                                                                          |      |
| MEDIUM   |     | [RSK-001](#rsk-001-compact-target-cannot-rebuild-energycorecompact), [RSK-002](#rsk-002-stale-or-opaque-binaries-ship-energycorecompact) |      |
| LOW      |     | [RSK-003](#rsk-003-cf-path-divergence-unnoticed-energycorecompact)                                                                       |      |

**Scorecard Summary**

`1-3 Poor · 4-6 Average · 7-8 Good · 9-10 Excellent`

| Dimension               | Score   | Notes                                 |
|-------------------------|---------|---------------------------------------|
| Testability             | 3       | One token test file                   |
| Design Soundness        | 4       | Minimal single-form shell             |
| Code Quality            | 5       | Small surface, readable               |
| Code Style              | 6       | Follows repository conventions        |
| Stack Alignment         | 4       | MSBuild 2003, ToolsVersion 3.5        |
| Dependency Health       | 7       | No package references                 |
| Maintainability         | 4       | Preview-scale, overlapping solutions  |
| Structure               | 5       | Three `.sln` files overlap            |
| Deployability           | 3       | Cannot build on a modern SDK          |
| Security                | 5       | No network surface in the preview     |
| Compliance              | 5       | Inherits repository license ambiguity |
| Observability           | 3       | No logging in the preview             |
| Delivery & Continuity   | 4       | Same concentrated authorship          |
| AI Provenance           | UNKNOWN | No provenance baseline                |
| Originality & Licensing | 5       | Inherits repository license ambiguity |

Overall score is 4.5/10, the unweighted mean of the 15 scored dimensions, 68 divided by 15.

`N/A` dimensions - Scalability, Operational Safety, Skill Definition, API Compatibility -
are omitted.

The lowest-scoring applicable dimensions are Deployability, Testability, and Observability,
each 3/10.

### Delivery Practice & Team Continuity

| [DORA](#glossary) metric        | Result          | Basis                         |
|---------------------------------|-----------------|-------------------------------|
| Change lead time                | `NOT SPECIFIED` | Shares the repository history |
| Deployment frequency            | `NOT SPECIFIED` | 7z downloads, no VCS cadence  |
| Failed deployment recovery time | `NOT SPECIFIED` | Requires incident data        |
| Change fail rate                | `NOT SPECIFIED` | Requires rollback data        |
| Deployment rework rate          | `NOT SPECIFIED` | Requires incident data        |

Contributor concentration mirrors the repository, `HIGH`, per [EVD-006](#auditing-methodology).

### High-Level Observations

| Observation                                             |
|---------------------------------------------------------|
| The preview cannot build on a modern SDK                |
| Three overlapping solution files cover the same tree    |
| Shipped Compact binaries are opaque committed artifacts |

### Auditing Methodology

The same criterion families apply with legacy-toolchain readings, DEP-MGMT resolves as
framework-reference-only and INF-REL reads the committed-binary path.

### Scoring Rubrics

The shared band table applies - `1-3 Poor`, `4-6 Average`, `7-8 Good`, `9-10 Excellent`.

### Architectural Assessment

A five-file single-form preview is appropriately minimal.

The three solution files - `Energy.Core.Compact.sln`, `Legacy.sln`, `Library.sln` - overlap
in coverage, [FND-STR-001](#fnd-str-001-three-overlapping-solution-files-energycorecompact).

### Structure Review

Legacy preview tree beside the library.

| Area                       | Verdict | Basis                                 |
|----------------------------|---------|---------------------------------------|
| Directory organization     | PASS    | Preview under its own root            |
| Naming conventions         | PASS    | `Energy.Core.Compact.*` naming        |
| Component placement        | PARTIAL | Three overlapping solutions           |
| Supporting-artifact layout | PARTIAL | Built DLL tracked under `nuget/build` |
| Consistency                | PASS    | Uniform file conventions              |

Limitations: shared-source compilation of the library under `NETCF` is assessed inside
Energy.Core/::FND-ARC-003.

### Trade-off Analysis

| Trade-off                           | Context                                   | Option A: gain / cost              | Option B: gain / cost       | Evidence                                                           | Implication                           |
|-------------------------------------|-------------------------------------------|------------------------------------|-----------------------------|--------------------------------------------------------------------|---------------------------------------|
| Shared-source [CF](#glossary) build | One codebase, legacy target               | No duplicate sources / #if forking | Separate CF tree / drift    | [EVD-022](#auditing-methodology), [EVD-036](#auditing-methodology) | Coherent, needs the divergence matrix |
| Committed binaries                  | [CF](#glossary) unbuildable on modern SDK | Shippable artifacts / opaque       | Drop CF / lose the platform | [EVD-037](#auditing-methodology)                                   | Reasonable, add provenance            |

### Standards Conformance

The legacy project format predates the `docs/` standards, conformance rows mark `UNKNOWN`
where the standards do not cover MSBuild 2003 projects.

### Strengths & What's Working

- Compact preview demonstrates the [CF](#glossary) target end to end, [EVD-036](#auditing-methodology).
- Zero package references, the smallest possible dependency surface.

### Technical Findings

| Finding                                                                         | Pillar                   | Severity | Title                             | Result  | Status | Change | Verification |
|---------------------------------------------------------------------------------|--------------------------|----------|-----------------------------------|---------|--------|--------|--------------|
| [FND-INF-001](#fnd-inf-001-unbuildable-legacy-toolchain-energycorecompact)      | Infrastructure & CI/CD   | MEDIUM   | Unbuildable legacy toolchain      | FAIL    | Open   | New    |              |
| [FND-STR-001](#fnd-str-001-three-overlapping-solution-files-energycorecompact)  | Structure & Organization | LOW      | Three overlapping solution files  | PARTIAL | Open   | New    |              |
| [FND-CQY-001](#fnd-cqy-001-token-test-file-no-real-coverage-energycorecompact)  | Code Quality             | LOW      | Token test file, no real coverage | PARTIAL | Open   | New    |              |
| [FND-INF-002](#fnd-inf-002-opaque-committed-compact-binaries-energycorecompact) | Infrastructure & CI/CD   | MEDIUM   | Opaque committed Compact binaries | PARTIAL | Open   | New    |              |

### FND-INF-001: Unbuildable legacy toolchain (Energy.Core.Compact/)

The project is MSBuild 2003 format with `ToolsVersion 3.5` and `TargetFrameworkVersion v2.0`.

* **Pillar:** Infrastructure & CI/CD
* **Severity:** MEDIUM
* **Type:** Concern
* **Targets:** Energy.Core.Compact/
* **Basis:** Buildability on a supported toolchain
* **Criteria:** INF-CICD
* **Impact:** The Compact target cannot be rebuilt from source on a modern SDK
* **Method:** Project-file inspection
* **Confidence:** High
* **Absence:** Deliberate - recorded decision, the CF platform requirement is stated
* **Mitigating factors:** None observed
* **Evidence:** [EVD-036](#auditing-methodology)
* **Recommendation:** Document the legacy build path or retire the target

### FND-STR-001: Three overlapping solution files (Energy.Core.Compact/)

`Energy.Core.Compact.sln`, `Legacy.sln`, and `Library.sln` overlap.

* **Pillar:** Structure & Organization
* **Severity:** LOW
* **Type:** Concern
* **Targets:** Energy.Core.Compact/
* **Basis:** One canonical solution per tree
* **Criteria:** STR-LAYOUT
* **Impact:** Unclear which solution is authoritative
* **Method:** Directory census
* **Confidence:** High
* **Mitigating factors:** None observed
* **Evidence:** [EVD-036](#auditing-methodology)
* **Recommendation:** Consolidate or document each solution's purpose

### FND-CQY-001: Token test file, no real coverage (Energy.Core.Compact/)

`Test/Hello.cs` is a placeholder rather than a test suite.

* **Pillar:** Code Quality
* **Severity:** LOW
* **Type:** Concern
* **Targets:** Energy.Core.Compact/Energy.Core.Compact.Preview/
* **Basis:** Testing expectation from `docs/TESTING.md`
* **Criteria:** TST-UNIT
* **Impact:** Compact-specific behavior is unverified
* **Method:** File census
* **Confidence:** High
* **Mitigating factors:** None observed
* **Evidence:** [EVD-036](#auditing-methodology)
* **Recommendation:** Add CF-path tests or document the preview-only scope

### FND-INF-002: Opaque committed Compact binaries (Energy.Core.Compact/)

`nuget/build/netcf/Energy.Core.dll` and 30 `download/*.7z` archives carry no provenance.

* **Pillar:** Infrastructure & CI/CD
* **Severity:** MEDIUM
* **Type:** Concern
* **Targets:** nuget/build/, download/
* **Basis:** Artifact-provenance practice
* **Criteria:** INF-REL
* **Impact:** Consumers cannot verify binaries against source
* **Method:** Tracked-binary census
* **Confidence:** High
* **Mitigating factors:** None observed
* **Evidence:** [EVD-037](#auditing-methodology)
* **Recommendation:** Publish hashes and build provenance, per Energy.Core/::REC-010

### Unified Risk Register

| Risk                                                                | Severity | Likelihood | Title                         | Source                                                                                                    |
|---------------------------------------------------------------------|----------|------------|-------------------------------|-----------------------------------------------------------------------------------------------------------|
| [RSK-001](#rsk-001-compact-target-cannot-rebuild-energycorecompact) | MEDIUM   | Medium     | Compact target cannot rebuild | [FND-INF-001](#fnd-inf-001-unbuildable-legacy-toolchain-energycorecompact)                                |
| [RSK-002](#rsk-002-stale-or-opaque-binaries-ship-energycorecompact) | MEDIUM   | Medium     | Stale or opaque binaries ship | [FND-INF-002](#fnd-inf-002-opaque-committed-compact-binaries-energycorecompact)                           |
| [RSK-003](#rsk-003-cf-path-divergence-unnoticed-energycorecompact)  | LOW      | Medium     | CF-path divergence unnoticed  | [FND-CQY-001](#fnd-cqy-001-token-test-file-no-real-coverage-energycorecompact), Energy.Core/::FND-ARC-003 |

### RSK-001: Compact target cannot rebuild (Energy.Core.Compact/)

The MSBuild 2003 toolchain is not runnable on a modern SDK.

* **Severity:** MEDIUM
* **Likelihood:** Medium
* **Residual:** Medium until the build path is documented or retired
* **Impact:** [CF](#glossary) fixes cannot ship from source
* **Trigger:** Any required Compact change
* **Controls:** Committed binaries cover the gap
* **Closure:** Documented legacy toolchain path or target retirement
* **Source:** [FND-INF-001](#fnd-inf-001-unbuildable-legacy-toolchain-energycorecompact)
* **Confidence:** High
* **Mitigation:** [REC-001](#rec-001-document-or-retire-the-cf-build-path-energycorecompact)

### RSK-002: Stale or opaque binaries ship (Energy.Core.Compact/)

Tracked `.7z` and `.dll` artifacts may drift from source.

* **Severity:** MEDIUM
* **Likelihood:** Medium
* **Residual:** Medium until provenance exists
* **Impact:** Consumers run unverifiable code
* **Trigger:** Source change without artifact rebuild
* **Controls:** None observed
* **Closure:** Hash manifest plus build provenance
* **Source:** [FND-INF-002](#fnd-inf-002-opaque-committed-compact-binaries-energycorecompact)
* **Confidence:** High
* **Mitigation:** [REC-002](#rec-002-publish-artifact-provenance-energycorecompact)

### RSK-003: CF-path divergence unnoticed (Energy.Core.Compact/)

`NETCF` code paths diverge without a capability record.

* **Severity:** LOW
* **Likelihood:** Medium
* **Residual:** Medium until the capability matrix exists
* **Impact:** [CF](#glossary) consumers hit silent `null` or no-op paths
* **Trigger:** Using an [API](#glossary) that no-ops under `NETCF`
* **Controls:** None observed
* **Closure:** Per-target capability matrix
* **Source:** [FND-CQY-001](#fnd-cqy-001-token-test-file-no-real-coverage-energycorecompact), Energy.Core/::FND-ARC-003
* **Confidence:** Medium
* **Mitigation:** [REC-003](#rec-003-consolidate-the-solution-files-energycorecompact)

### Remediation Roadmap

| Rec                                                                        | Priority | Finding                                                                         | Recommendation                                    | Impact | Effort | Complexity | Breaking | Verification        |
|----------------------------------------------------------------------------|----------|---------------------------------------------------------------------------------|---------------------------------------------------|--------|--------|------------|----------|---------------------|
| [REC-001](#rec-001-document-or-retire-the-cf-build-path-energycorecompact) | P2       | [FND-INF-001](#fnd-inf-001-unbuildable-legacy-toolchain-energycorecompact)      | Document or retire the [CF](#glossary) build path | Medium | Medium | Medium     | None     | Build-path document |
| [REC-002](#rec-002-publish-artifact-provenance-energycorecompact)          | P2       | [FND-INF-002](#fnd-inf-002-opaque-committed-compact-binaries-energycorecompact) | Publish artifact provenance                       | Medium | Low    | Low        | None     | Hash manifest       |
| [REC-003](#rec-003-consolidate-the-solution-files-energycorecompact)       | P3       | [FND-STR-001](#fnd-str-001-three-overlapping-solution-files-energycorecompact)  | Consolidate the solution files                    | Low    | Low    | Low        | None     | One canonical sln   |
| [REC-004](#rec-004-scope-the-preview-test-expectation-energycorecompact)   | P3       | [FND-CQY-001](#fnd-cqy-001-token-test-file-no-real-coverage-energycorecompact)  | Scope the preview test expectation                | Low    | Low    | Low        | None     | Test or scope note  |

### REC-001: Document or retire the CF build path (Energy.Core.Compact/)

Records the working legacy toolchain or drops the target.

* **Priority:** P2
* **Finding:** Energy.Core.Compact/::FND-INF-001
* **Postcondition:** A documented path rebuilds the [CF](#glossary) artifacts, or the target is retired
* **Impact:** [CF](#glossary) changes become shippable again
* **Effort:** Medium
* **Complexity:** Medium
* **Verification:** Documented build procedure or removal commit

### REC-002: Publish artifact provenance (Energy.Core.Compact/)

Ships hashes and build provenance for committed binaries.

* **Priority:** P2
* **Finding:** Energy.Core.Compact/::FND-INF-002
* **Postcondition:** Manifest covers `nuget/build/` and `download/` binaries
* **Impact:** Artifacts become verifiable against source
* **Effort:** Low
* **Complexity:** Low
* **Verification:** Manifest plus published hashes

### REC-003: Consolidate the solution files (Energy.Core.Compact/)

Keeps one canonical solution or documents each purpose.

* **Priority:** P3
* **Finding:** Energy.Core.Compact/::FND-STR-001
* **Postcondition:** One canonical solution or a documented split
* **Impact:** Removes ambiguity for contributors
* **Effort:** Low
* **Complexity:** Low
* **Verification:** Solution census shows one canonical entry

### REC-004: Scope the preview test expectation (Energy.Core.Compact/)

Adds CF-path tests or records the preview-only scope.

* **Priority:** P3
* **Finding:** Energy.Core.Compact/::FND-CQY-001
* **Postcondition:** Tests exist or the preview scope is documented
* **Impact:** Compact behavior is verified or honestly scoped
* **Effort:** Low
* **Complexity:** Low
* **Verification:** Test files or a scope note

### Recommendation Classification

| Rec                                                                        | Recommendation                                    | Class       | Basis                                                                                                 |
|----------------------------------------------------------------------------|---------------------------------------------------|-------------|-------------------------------------------------------------------------------------------------------|
| [REC-001](#rec-001-document-or-retire-the-cf-build-path-energycorecompact) | Document or retire the [CF](#glossary) build path | Recommended | [FND-INF-001](#fnd-inf-001-unbuildable-legacy-toolchain-energycorecompact) - restores buildability    |
| [REC-002](#rec-002-publish-artifact-provenance-energycorecompact)          | Publish artifact provenance                       | Recommended | [FND-INF-002](#fnd-inf-002-opaque-committed-compact-binaries-energycorecompact) - verifiable binaries |
| [REC-003](#rec-003-consolidate-the-solution-files-energycorecompact)       | Consolidate the solution files                    | Optional    | [FND-STR-001](#fnd-str-001-three-overlapping-solution-files-energycorecompact) - hygiene              |
| [REC-004](#rec-004-scope-the-preview-test-expectation-energycorecompact)   | Scope the preview test expectation                | Optional    | [FND-CQY-001](#fnd-cqy-001-token-test-file-no-real-coverage-energycorecompact) - preview scope        |

### Code Style

T1 convention tier, the five preview sources follow the declared naming and layout rules.

| Lens          | Status | Census                                  |
|---------------|--------|-----------------------------------------|
| Naming        | PASS   | Declared conventions followed           |
| Layout        | PASS   | Region and file conventions hold        |
| Comment style | PASS   | `////` continuation comments consistent |

| Lens          | Status | Census                                  |
|---------------|--------|-----------------------------------------|
| Naming        | PASS   | Declared conventions followed           |
| Layout        | PASS   | Region and file conventions hold        |
| Comment style | PASS   | `////` continuation comments consistent |


## Trade-off Analysis

Cross-project trade-offs only - per-project trade-offs live inside each project block.

| Trade-off                           | Context                                   | Option A: gain / cost                        | Option B: gain / cost       | Evidence                                                           | Implication                           |
|-------------------------------------|-------------------------------------------|----------------------------------------------|-----------------------------|--------------------------------------------------------------------|---------------------------------------|
| Shared-source [CF](#glossary) build | Energy.Core sources compile twice         | One codebase / [NETCF](#glossary) divergence | Separate CF tree / drift    | [EVD-022](#auditing-methodology), [EVD-036](#auditing-methodology) | Coherent, needs the capability matrix |
| Committed binaries                  | [CF](#glossary) unbuildable on modern SDK | Shippable artifacts / opaque                 | Drop CF / lose the platform | [EVD-037](#auditing-methodology)                                   | Reasonable, add provenance metadata   |

## Scope Exclusions

- `Energy.Core.Example/` - 62 example projects excluded at the project-inclusion gate.
- `work/` - gitignored workspace, `work/lzss-sap/` embedded repository and
  `work/lzsscheck/` excluded at the gate.
- `TestResults/` - untracked on disk, no committed result artifacts.
- `Documentation/html/` - generated Doxygen output excluded by `.gitignore`.
- Skill Definition Conformance - no `SKILL.md` or agent-facing artifacts exist.
- Agent Guidance Conformance - no structured agent-guidance set exists.
- AI System Assessment - the subject carries no AI dependency.
- Changes section omitted - a fresh audit produces no comparison content.
- API Contract Conformance - the library exposes no service [API](#glossary) contract.
- Contradiction Register - no external conflicting report found.
- Penetration Test - `NOT DONE`, source-only scope never executes the subject.
- ISO/IEC 27001 Certification and SOC 2 Attestation - `NOT DONE`, rubric use only.
- Executed Evidence Log - `source-only` mode, replaced by Operator Verification Handoff.
- OWASP Top 10 categories covered: injection via [FND-SEC-004](#fnd-sec-004-text-substitution-sql-without-dbparameter-pathway-energycore), cryptographic failures via
  [FND-SEC-001](#fnd-sec-001-global-tls-certificate-validation-bypass-api-energycore) and [FND-SEC-002](#fnd-sec-002-legacy-cryptographic-primitives-without-warnings-energycore), resource exhaustion via [FND-SEC-003](#fnd-sec-003-unbounded-decompression-output-growth-energycore) - the library context
  leaves authentication, access control, and session categories out of scope.

## Limitations and Unknowns

| Item                             | Type        | Reason                                  | Resolution                                    |
|----------------------------------|-------------|-----------------------------------------|-----------------------------------------------|
| Build outcome                    | Unrun check | Requires execution, out of scope        | `dotnet build Energy.Core/Energy.Core.csproj` |
| Test outcome                     | Unrun check | Requires execution, out of scope        | `dotnet test Energy.Core.Test/`               |
| Advisory status of test packages | Unknown     | No advisory data in the tree            | Advisory scan or review record                |
| Compact build on modern SDK      | Unknown     | MSBuild 2003 toolchain unavailable here | Documented legacy build path                  |
| Runtime behavior under load      | Unrun check | Performance audit partial by scope      | Benchmark harness                             |
| Coverage percentage              | Unknown     | No coverage artifact committed          | Coverage run                                  |

## Operator Verification Handoff

| Claim                          | Command / Procedure                           | Pass Criteria                    | Resolves                                                                                |
|--------------------------------|-----------------------------------------------|----------------------------------|-----------------------------------------------------------------------------------------|
| Four-target clean build        | `dotnet build Energy.Core/Energy.Core.csproj` | Exit 0, zero warnings            | Energy.Core/::FND-INF-001, Deployability                                                |
| Suite passes                   | `dotnet test Energy.Core.Test/`               | All tests green                  | Energy.Core.Test/::FND-CQY-002 (Energy.Core.Test/), Testability                         |
| CRC32 segmented input          | Test `HashCore` with non-zero `start`         | Correct checksum                 | [FND-CQY-003](#fnd-cqy-003-crc32-hashcore-contract-violation-energycore)                |
| [TLS](#glossary) bypass scoped | Review `Web.IgnoreCertificateValidation`      | No process-wide unguarded bypass | [FND-SEC-001](#fnd-sec-001-global-tls-certificate-validation-bypass-api-energycore)     |
| Legacy primitives gated        | `[Obsolete]` or warnings on MD5/SHA1/DES      | Markers present                  | [FND-SEC-002](#fnd-sec-002-legacy-cryptographic-primitives-without-warnings-energycore) |
| Decompression bound            | Crafted input against the cap                 | Decoder rejects or truncates     | [FND-SEC-003](#fnd-sec-003-unbounded-decompression-output-growth-energycore)            |
| License identity consistent    | Diff `LICENSE.txt`, nuspec, csproj            | One license across all           | [FND-CPR-003](#fnd-cpr-003-license-declarations-contradict-licensetxt-energycore)       |

## Re-audit And Follow-up Plan

| Finding                   | Priority | Verification Owner | Closure Evidence           | Target Re-audit Trigger |
|---------------------------|----------|--------------------|----------------------------|-------------------------|
| Energy.Core/::FND-SEC-001 | P1       | `NOT SPECIFIED`    | Scoped or removed bypass   | Next release            |
| Energy.Core/::FND-SEC-002 | P1       | `NOT SPECIFIED`    | Warnings or `[Obsolete]`   | Next release            |
| Energy.Core/::FND-SEC-003 | P1       | `NOT SPECIFIED`    | Output cap test            | Next release            |
| Energy.Core/::FND-CPR-003 | P1       | `NOT SPECIFIED`    | Consistent declarations    | Next release            |
| Energy.Core/::FND-INF-001 | P1       | `NOT SPECIFIED`    | Green pipeline run         | First pipeline commit   |
| Energy.Core/::FND-CQY-003 | P1       | `NOT SPECIFIED`    | Segmented-input test green | Next commit             |

## Validation Record

| Check  | Result  | Evidence / Justification                                           |
|--------|---------|--------------------------------------------------------------------|
| PAR-1  | APPLIED | Every security finding carries a CWE ID or justification           |
| PAR-2  | APPLIED | Severity note states impact-likelihood basis, no [CVSS](#glossary) |
| PAR-3  | APPLIED | ISO/IEC 25010:2023 crosswalk present as mapping only               |
| PAR-4  | APPLIED | Technical Debt Register separates cost from risk register          |
| PAR-5  | APPLIED | Git concentration surfaced as Team & Continuity line               |
| PAR-6  | APPLIED | Manifest-derived [SBOM](#glossary) produced per project            |
| PAR-7  | APPLIED | Limitations lists every unrun check                                |
| PAR-8  | APPLIED | This self-check table is present and complete                      |
| PAR-9  | APPLIED | .NET references re-derived from `references/stack-standards.md`    |
| PAR-10 | APPLIED | Glossary indexes acronyms, body citations link via the linker      |
| PAR-11 | APPLIED | Coverage matrix agrees with Scope Exclusions                       |
| PAR-12 | APPLIED | SBOM License cells populated or `Unknown`                          |
| PAR-13 | APPLIED | Every ledger row and finding carries Observation or Concern        |
| PAR-14 | APPLIED | HIGH security findings carry Theoretical Exploitability            |
| PAR-15 | APPLIED | Delivery Practice present per project with proxies labeled         |
| PAR-16 | N/A     | English report, no translated-file checks apply                    |
| PAR-17 | N/A     | No agent-facing artifacts, section omitted                         |
| PAR-18 | APPLIED | Recommendation Classification lists every REC once                 |
| PAR-19 | N/A     | No agent-guidance set                                              |
| PAR-20 | APPLIED | Structure Review present per project with five verdict rows        |
| PAR-21 | APPLIED | Code Style present with resolved T1 tier per partition             |
| PAR-22 | APPLIED | Style findings carry census counts and `file:line` anchors         |
| PAR-23 | APPLIED | Basis fields name the T1 tier, census denominators recorded        |
| PAR-24 | APPLIED | Naming findings stay Concern with MEDIUM or lower confidence       |
| PAR-25 | APPLIED | Generated, vendored, and excluded trees left out of style census   |
| PAR-26 | APPLIED | No bulk-rename recommendation exists                               |
| PAR-27 | APPLIED | Linters appear only as NOT RUN or Method suggestions               |

The Mandatory Core Checklist passed, and the parity gate located the prior capability set at
`docs/audit/26.7.0/AUDIT-1.4.md` as required by `process/report-parity.md`.

Capabilities added for parity with the current contract: multi-project layout, Project
Inventory, Glossary identifier triplets, Delivery & Continuity scorecard dimension,
Code Style and Structure dimensions, per-project Delivery Practice sections, Operator
Verification Handoff, Re-audit And Follow-up Plan, and the [PAR-1](#validation-record) to [PAR-27](#validation-record) record.

Content sections - findings, risks, roadmap - are independently re-derived at revision
`9447e87`, no identifiers carried over.

## References

| Reference                                                                                                                           | Publisher or Author | Used In                             |
|-------------------------------------------------------------------------------------------------------------------------------------|---------------------|-------------------------------------|
| [ISO/IEC 25010:2023](https://iso25000.com/index.php/en/iso-25000-standards/iso-25010)                                               | ISO/IEC             | Scoring Rubrics crosswalk           |
| [OWASP ASVS](https://owasp.org/www-project-application-security-verification-standard/)                                             | OWASP               | Security control vocabulary         |
| [OWASP Top 10](https://owasp.org/Top10/)                                                                                            | OWASP               | Scope Exclusions taxonomy           |
| [CWE](https://cwe.mitre.org/)                                                                                                       | MITRE               | Security finding classification     |
| [STRIDE](https://learn.microsoft.com/en-us/azure/security/develop/threat-modeling-tool-threats)                                     | Microsoft           | Threat Model                        |
| [NIST SP 800-30](https://csrc.nist.gov/publications/detail/sp/800-30/rev-1/final)                                                   | NIST                | Risk register process               |
| [.NET security analysis rules](https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/security-warnings) | Microsoft           | CWE-to-analyzer mappings            |
| [Framework Design Guidelines](https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/)                                 | Microsoft           | Standards quality baseline          |
| [NuGet package validation](https://learn.microsoft.com/en-us/nuget/reference/errors-and-warnings)                                   | Microsoft           | API Compatibility pillar            |
| [DORA metrics](https://dora.dev/)                                                                                                   | [DORA](#glossary)   | Delivery Practice & Team Continuity |
