test:
	docker run --rm -it \
		-e NUGET_PACKAGES=/app/packages \
		-v `pwd`:/app \
		microsoft/dotnet:2.2-sdk-alpine \
			sh -c "cd /app/Bynder/Test; dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover"

clean:
	rm -rf \
		Bynder/Sdk/bin \
		Bynder/Sdk/obj \
		Bynder/Sample/bin \
		Bynder/Sample/obj \
		Bynder/Test/bin \
		Bynder/Test/obj

dev:
	docker run --rm -it \
		-e NUGET_PACKAGES=/app/packages \
		-v `pwd`:/app \
		microsoft/dotnet:2.2-sdk-alpine \
			sh -c "cd /app; dotnet restore; exec sh"
