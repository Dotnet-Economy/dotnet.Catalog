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

## Build the docker image

```powershell
$env:GH_OWNER="Dotnet-Economy"
$env:GH_PAT="[PAT HERE]"
docker build --secret id=GH_OWNER --secret id=GH_PAT -t dotnet.catalog:$version .
```

## Run the docker image

```powershell
docker run -it --rm -p 5000:5000 --name catalog -e MongoDbSettings__Host=mongo -e RabbitMQSettings__Host=rabbitmq --network dotnetinfra_default dotnet.catalog:$version
```
