# Support for json serialization in domain models
## Context
We need json serialized models in different parts of the application:
 - Sending them to the UI
 - Storing them in files

## Decision
Json support was integrated directly in domain models.
Automated tests in RKTimeTrack.Application.Tests ensure that serialization works as expected.

## Status
Implemented

## Consequences
Dependency to json serialization in domain models