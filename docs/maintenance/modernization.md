# Modernization strategy

## Goal

Keep SourceGrid viable on current Windows/.NET tooling while preserving the value of its established WinForms API and behavior.

Modernization is an incremental compatibility program, not a license to rewrite stable subsystems.

## Current baseline

SourceGrid 5.0 has already moved several repository/runtime concerns forward:

- SDK-style projects.
- `net48` and `net8.0-windows` multi-targeting.
- central package management.
- warnings-as-errors.
- current Windows platform metadata for the modern target.
- `Microsoft.Data.SqlClient` in the DevAge SQL command builder surface.
- removal of obsolete formatter-based exception serialization APIs.
- modernization of legacy crypto implementation details while retaining existing helper signatures/output behavior.

See `../migration/5.0.md` for user-facing compatibility details.

## Decision framework

Before modernizing an area, classify the change.

### Internal-only modernization

Examples: disposal improvements, supported API replacements, analyzer fixes, implementation simplification.

Preferred approach: preserve public behavior, add regression tests if semantics are delicate, and avoid migration churn.

### Public implementation dependency change

Example: replacing a dependency where dependency-specific types appear in public members.

Preferred approach: treat this as an API compatibility event. Review semantic versioning impact, update migration documentation, and add tests around the exposed contract.

### Removal of obsolete behavior

Examples: formatter serialization hooks that are no longer supportable/safe.

Preferred approach: remove deliberately, document exact affected types/members, retain ordinary constructors/behavior where possible, and advance major version when compatibility requires it.

### Platform-specific behavior

Prefer shared code first. When the two target frameworks genuinely require different implementations, isolate conditions narrowly and test both branches.

## Guardrails

1. Do not change public signatures as a side effect of cleanup.
2. Do not replace historical behavior based only on style preference.
3. Do not remove `net48` support accidentally by using modern-only APIs in shared code.
4. Do not add global analyzer suppressions for problems confined to generated/legacy artifacts.
5. Do not add new core dependencies on optional/sample binaries.
6. Do not assume a file is compiled because it exists; inspect project exclusions.
7. Do not use legacy documentation as evidence that an old dependency/runtime remains required.

## Compatibility tests

High-value modernization tests include:

- reflection tests for removed/preserved public members;
- SQL command-builder tests that verify exposed provider types;
- crypto regression vectors preserving historical output;
- platform/assembly metadata checks;
- resource loading on both frameworks;
- focus/selection/editing regressions for WinForms behavior.

Prefer tests that capture the external contract rather than private implementation details.

## Dependency modernization

Use central package management. For each dependency update, ask:

- Does it still support both target frameworks?
- Are any dependency types exposed publicly by SourceGrid?
- Did defaults change in ways that affect callers (for example connection security defaults)?
- Does the change affect sample-only infrastructure or the core package?

Keep optional backends optional.

## Documentation during modernization

Update:

- `AI_CONTEXT.md` when the stable architecture/system model changes.
- `docs/architecture/` when boundaries or important abstractions change.
- `docs/development/` when toolchain/build/test rules change.
- `docs/migration/` for user-visible breaking changes.

Historical material in `docs/legacy/` remains unchanged except for archival organization metadata.
