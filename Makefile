build:
	@dotnet build

build-ci:
	@dotnet build --configuration Release --no-restore

run:
	@dotnet run --project PMF.Invoice.Service

clean:
	@dotnet clean

deploy:
	@dotnet publish -c Release

test:
	@ASPNETCORE_ENVIRONMENT=Test dotnet test

test-ci:
	@ASPNETCORE_ENVIRONMENT=Test dotnet test --no-restore --verbosity normal
