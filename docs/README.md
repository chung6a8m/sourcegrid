# SourceGrid documentation

This directory contains the current technical documentation for SourceGrid plus a clearly separated historical archive.

## Start here

- [Architecture overview](architecture/overview.md) — system boundaries and dependency direction.
- [Codebase map](architecture/codebase-map.md) — where major responsibilities live.
- [Grid and cell model](architecture/grid-model.md) — `Grid`, `GridVirtual`, cells, models, views, editors, controllers, selection, and spans.
- [Getting started](development/getting-started.md) — contributor setup and first workflow.
- [Build and test](development/build-and-test.md) — supported targets and validation commands.
- [Coding guidelines](development/coding-guidelines.md) — repository-specific engineering guidance.
- [Modernization strategy](maintenance/modernization.md) — how to evolve the legacy codebase safely.
- [Migrating to SourceGrid 5.0](migration/5.0.md) — compatibility-impacting changes in 5.0.

## Directory policy

| Directory | Status | Purpose |
| --- | --- | --- |
| `architecture/` | Canonical | Current architecture and conceptual model. |
| `development/` | Canonical | Setup, build/test, and implementation guidance. |
| `maintenance/` | Canonical | Modernization and long-term maintenance guidance. |
| `migration/` | Canonical | Version-specific migration notes and breaking changes. |
| `plans/` | Working | Approved implementation plans for future or in-progress work. |
| `legacy/` | Historical | Archived documentation/assets preserved for reference. |

`docs/legacy/` must not be used to determine current frameworks, dependencies, hosting locations, build commands, or supported behavior.

## Sources of truth

For current behavior, use evidence in this order:

1. Current implementation and project configuration.
2. Automated tests that describe intended behavior.
3. Canonical documentation in this directory.
4. Version migration guides for compatibility history.
5. Legacy documentation for historical context only.

If the sources disagree, investigate before changing behavior. A mismatch can indicate either stale documentation or a regression.
