NUGET_API_KEY = Placeholder
NUGET_SOURCE = https://api.nuget.org/v3/index.json

.PHONY: all debug release clean publish

all: debug release

debug:
	dotnet build --configuration Debug

release:
	dotnet build --configuration Release

clean:
	dotnet clean
	rm -rf src/**/bin
	rm -rf src/**/obj

publish: clean release
	dotnet nuget push **/*.nupkg --api-key $(NUGET_API_KEY) --source $(NUGET_SOURCE)
