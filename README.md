# dotnet.Catalog

Dotnet Economy Catalog microservice

## Create and publish package

```powershell
$version="1.0.2"
$owner="Dotnet-Economy"
$gh_pat="[PAT HERE]"

dotnet pack src/dotnet.Catalog.Contracts/ --configuration Release -p:PackageVersion=$version -p:RepositoryUrl=https://github.com/$owner/dotnet.Catalog -o ../packages

dotnet nuget push ../packages/dotnet.Catalog.Contracts.$version.nupkg --api-key $gh_pat --source "github"
```
