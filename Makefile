.PHONY: run-docker stop-docker executeSdkSample rebuild-container
run-docker:
	docker-compose up -d

stop-docker:
	docker-compose down

executeSdkSample:
	docker-compose exec bynder-sdk dotnet run --project Bynder/Sample/Bynder.Sample.csproj -- $(sample-name)

rebuild-container:
	docker-compose build
	docker-compose up -d

rebuild-project:
	docker-compose exec bynder-sdk dotnet build Bynder/Sample/Bynder.Sample.csproj