build:
	@dotnet build

build-ci:
	@dotnet build --configuration Release --no-restore

run:
	@dotnet run --project API.Rest

clean:
	@dotnet clean

deploy:
	@dotnet publish -c Release

tunnel:
	@ngrok http 4000

test:
	@ASPNETCORE_ENVIRONMENT=Test dotnet test

test-ci:
	@ASPNETCORE_ENVIRONMENT=Test dotnet test --no-restore --verbosity normal

local-tests:
	@curl -i -X POST http://localhost:4000/reset && echo
	@curl -i -X GET http://localhost:4000/balance?account_id=1234 && echo
	@curl -i -X POST http://localhost:4000/event -H "Content-Type: application/json" -d '{"type":"deposit", "destination":"100", "amount":10}' | jq .&& echo
	@curl -i -X POST http://localhost:4000/event -d '{"type":"deposit", "destination":"100", "amount":10}'| jq . && echo
	@curl -i -X GET http://localhost:4000/balance?account_id=100 && echo
	@curl -i -X POST http://localhost:4000/event -d '{"type":"withdraw", "origin":"200", "amount":10}'| jq . && echo
	@curl -i -X POST http://localhost:4000/event -d '{"type":"withdraw", "origin":"100", "amount":5}'| jq . && echo
	@curl -i -X POST http://localhost:4000/event -d '{"type":"transfer", "origin":"100", "amount":15, "destination":"300"}'| jq . && echo
	@curl -i -X POST http://localhost:4000/event -d '{"type":"transfer", "origin":"200", "amount":15, "destination":"300"}'| jq . && echo
