# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.2] - 2026-02-26

### Changed

- Merge Makefile target `push` into `publish`.
- Change the wording of **GDP0001**, **GDP0002**, and **GDP0003** diagnostics.

### Fixed

- Add `break` statement to **GDP0002** diagnostic code, fixing it potentially being reported more than once.
- Fix **GDP** diagnostics not showing in code editors.
- Fix `Node?` fields not being recognized by the `NodePathAttribute` analyzer.

## [1.0.1] - 2026-02-25

### Fixed

- Change namespaces to match the package name.

## [1.0.0] - 2026-02-25

- First release.

---

- [Version 1.0.2](#102---2026-02-26)
- [Version 1.0.1](#101---2026-02-25)
- [Version 1.0.0](#100---2026-02-25)
