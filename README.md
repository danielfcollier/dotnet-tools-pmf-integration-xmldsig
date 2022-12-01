# REST API App

Requirements: .NET SDK 6.0

## Table of Contents

- [Demo Version](#demo-version)
- [Build and Run](#build-and-run)
- [Run Tests](#run-tests)
- [CI-CD](#ci-cd)
- [Configurations](#configurations)
- [References](#references)

## Demo Version

Run locally with `ngrok`:

```bash
make tunnel
```

## Build and Run the App

### Locally:

```bash
make build
make run
```

### Development:

```bash
make run
```

### Deploy:

```bash
make deploy
```

### With Docker:

```bash
docker build -t api-rest .
docker run -p 4000:4000 api-rest
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

```bash
dotnet new webapi -o API.Rest
```

https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-6.0&tabs=visual-studio

https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-6.0

https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-dotnet-test

### Ngrok

https://ngrok.com

### Base dockerfile created with:

https://learn.microsoft.com/en-us/dotnet/core/docker/build-container?tabs=linux

https://github.com/thingsboard/thingsboard-gateway/issues/897

### Test GitHub Actions Locally

https://docs.github.com/en/actions/automating-builds-and-tests/building-and-testing-net

https://github.com/nektos/act
