# Bynder C# SDK

![Tests](https://github.com/Bynder/bynder-c-sharp-sdk/workflows/Tests/badge.svg)
![Publish](https://github.com/Bynder/bynder-c-sharp-sdk/workflows/Publish/badge.svg)
[![Coverage Status](https://coveralls.io/repos/github/Bynder/bynder-c-sharp-sdk/badge.svg?branch=master)](https://coveralls.io/github/Bynder/bynder-c-sharp-sdk?branch=master)
![Nuget](https://img.shields.io/nuget/v/Bynder.Sdk)
![Nuget](https://img.shields.io/nuget/dt/Bynder.Sdk?color=orange)

The main goal of this SDK is to speed up the integration of Bynder customers who use C# making it easier to connect to the Bynder API (http://docs.bynder.apiary.io) and executing requests on it.

## Nuget Package

You can download and use Bynder SDK from Nuget. https://www.nuget.org/packages/Bynder.Sdk/

## Current status

At the moment this SDK provides a library with the following methods:

### OAuth operations

```c#
string GetAuthorisationUrl(string state);
Task GetAccessTokenAsync();
Task GetAccessTokenAsync(string code);
```

### Asset management operations

```c#
Task<IList<Brand>> GetBrandsAsync();
Task<Uri> GetDownloadFileUrlAsync(DownloadMediaQuery query);
Task<IDictionary<string, Metaproperty>> GetMetapropertiesAsync();
Task<Metaproperty> GetMetapropertyAsync(MetapropertiesQuery query);
Task<List<String>> GetMetapropertyDependenciesAsync(MetapropertiesQuery query);
Task<Media> GetMediaInfoAsync(MediaInformationQuery query);
Task<IList<Media>> GetMediaListAsync(MediaQuery query);
Task<SaveMediaResponse> UploadFileAsync(UploadQuery query);
Task<Status> ModifyMediaAsync(ModifyMediaQuery query);
Task<List<Tag>> GetTagsAsync(TagsQuery query);
Task<Status> AddTagToMediaAsync(AddTagToMediaQuery query);
```

### Collection management operations

```c#
Task<IList<Collection>> GetCollectionsAsync(GetCollectionsQuery query);
Task<Collection> GetCollectionAsync(string id);
Task<Status> CreateCollectionAsync(CreateCollectionQuery query);
Task<Status> DeleteCollectionAsync(string id);
Task<IList<string>> GetMediaAsync(GetMediaQuery query);
Task<Status> AddMediaAsync(AddMediaQuery query);
Task<Status> RemoveMediaAsync(RemoveMediaQuery query);
Task<Status> ShareCollectionAsync(ShareQuery query);
```

### Sample Files Functionality Testing

Classes within `Sample` contain code to execute corresponding functionalities. The purpose is to demonstrate how methods 
are called and provide a convenient method to execute functions.

Executing the code in each file will prompt you in the terminal to enter in various IDs (media, collection, brand, tag, etc) to pass into each of the functions being run.

Within `Bynder/Sample` create an `Config.json` file or modify with the correct values. This file will be referenced from sample files. Depending on the sample file, you will need to have the correct scopes granted. 

Example `Config.json` file content:

```json
{
  "base_url": "https://example.bynder.com",
  "client_id": "your oauth app client id",
  "client_secret": "your oauth app client secret",
  "redirect_uri": "your oauth app redirect uri",
  "scopes": "offline asset:read asset:write collection:read collection:write asset.usage:read asset.usage:write meta.assetbank:read meta.assetbank:write meta.workflow:read"
}
```
Within each sample file, OAuth credentials are read in from `Config.json`. 
This will prompt the browser to open to retrieve an access code and then redirected to the redirect URI. 
Access code is then provided to terminal prompt to retrieve an access token for API calls afterward.


#### Setting up .NET (dotnet) and building the project

Make sure you have .NET 5.0 set up and installed. If you are developing on a Mac OS, the fastest method is to download the C# Dev Kit in Visual Studio Code and download .NET 5.0 from https://dotnet.microsoft.com/en-us/download/dotnet/5.0.

From `Bynder/Sample` directory, the project can be built using command from `Bynder.Sample.csproj`:
```bash
dotnet build
```


#### Brands Sample

Execute `BrandsSample.cs` file with command

```bash
dotnet run -- BrandsSample
```

Methods Used:
* GetBrandsAsync()


#### Collections Sample

Execute `CollectionsSample.cs` file with command

```bash
dotnet run -- CollectionsSample
```

Methods Used:
* GetCollectionsAsync(GetCollectionsQuery)
* GetCollectionAsync(collectionId)
* CreateCollectionAsync(CreateCollectionQuery)
* ShareCollectionAsync(ShareQuery)
* DeleteCollectionAsync(collectionId)
* AddMediaAsync(AddMediaQuery)
* GetMediaAsync(GetMediaQuery)
* RemoveMediaAsync(RemoveMediaQuery)


#### Media Sample

Execute `MediaSample.cs` file with command

```bash
dotnet run -- MediaSample
```

Methods Used:

* GetMediaListAsync(MediaQuery)
* GetMediaInfoAsync(MediaInformationQuery)
* GetDownloadFileUrlAsync(DownloadMediaQuery)
* ModifyMediaAsync(ModifyMediaQuery)

#### Metaproperties Sample

Execute `MetapropertiesSample.cs` file with command

```bash
dotnet run -- MetapropertiesSample
```

Methods Used:
* GetMetapropertiesAsync()
* GetMetapropertyAsync(MetapropertyQuery)
* GetMetapropertyDependenciesAsync(GetMetapropertyDependenciesAsync)

#### Tags Sample

Execute `TagsSample.cs` file with command

```bash
dotnet run -- TagsSample
```

Methods Used:
* GetTagsAsync(GetTagsQuery)
* AddTagToMediaAsync(AddTagToMediaQuery)

#### Upload Sample

Execute `UploadSample.cs` file with command

```bash
dotnet run -- UploadSample
```

Methods Used:
* UploadFileAsync(UploadQuery)

#### Asset Usage Sample

Execute `AssetUsage.cs` file with command

```bash
dotnet run -- AssetUsage
```
Methods Used:
* CreateAssetUsage(AssetUsageQuery)
* DeleteAssetUsage(AssetUsageQuery)
