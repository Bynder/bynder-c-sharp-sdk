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
string GetAuthorisationUrl(string state, string scopes);
Task GetAccessTokenAsync(string code, string scopes);
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
Task UploadFileAsync(UploadQuery query);
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

## Sample

To see how to use the SDK, please check [ApiSample.cs](Bynder/Sample/ApiSample.cs)
