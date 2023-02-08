# PMF Invoice Integration Service

Requirements: .NET SDK 6.0

## Table of Contents

- [Build and Run](#build-and-run)
- [Run Tests](#run-tests)
- [CI-CD](#ci-cd)
- [References](#references)

## Build and Run

```bash
make build
make run
```

### Deploy:

```bash
make deploy
```

### With Docker:

```bash
docker build -t tools-pmf-invoices .
docker run -p 4000:4000 tools-pmf-invoices
```

## Run Tests

```bash
make test
```

## CI-CD

### GitHub Actions

Tests are configured to run on multiple OS and .NET SDK versions to make sure the app is compatible across many platforms.

#### Test locally with `act`

```bash
act -j tests
```

### Deployment to Production Branch

If tests are passing, the CI with GitHub Actions pushes the changes to a production branch (`prod`).

## References

### Base template created with:

https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-dotnet-test

### Base dockerfile created with:

https://learn.microsoft.com/en-us/dotnet/core/docker/build-container?tabs=linux

https://github.com/thingsboard/thingsboard-gateway/issues/897

### Test GitHub Actions Locally

https://docs.github.com/en/actions/automating-builds-and-tests/building-and-testing-net

https://github.com/nektos/act
