NUGET_API_KEY = Placeholder
NUGET_SOURCE = https://api.nuget.org/v3/index.json

.PHONY: all publish debug release push clean

all: debug release
publish: clean release push

debug:
	dotnet build --configuration Debug

release:
	dotnet build --configuration Release

push:
	dotnet nuget push **/*.nupkg --api-key $(NUGET_API_KEY) --source $(NUGET_SOURCE)

clean:
	dotnet clean
	rm -rf src/**/bin
	rm -rf src/**/obj
