# XenforoResourceManagerAPI.Client

A strongly typed .NET wrapper for the XenForo Resource Manager API exposed by [SpigotMC](https://spigotmc.org/).

## Quick Start

```csharp
using XenforoResourceManagerAPI.Client;

var client = new XenforoResourceManagerApiClient();

var resources = await client.Resources.ListAsync(category: 4, page: 2);
var resource = await client.Resources.GetAsync(2);
var author = await client.Authors.FindAsync("simpleauthority");
```

## Endpoints

- `client.Resources.ListAsync(...)` calls `listResources`
- `client.Resources.GetAsync(...)` calls `getResource`
- `client.Resources.GetByAuthorAsync(...)` calls `getResourcesByAuthor`
- `client.ResourceCategories.ListAsync(...)` calls `listResourceCategories`
- `client.ResourceUpdates.GetAsync(...)` calls `getResourceUpdate`
- `client.ResourceUpdates.GetByResourceAsync(...)` calls `getResourceUpdates`
- `client.Authors.GetAsync(...)` calls `getAuthor`
- `client.Authors.FindAsync(...)` calls `findAuthor`
