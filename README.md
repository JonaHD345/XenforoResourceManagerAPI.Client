# XenforoResourceManagerAPI.Client

[![NuGet Version](https://img.shields.io/nuget/v/XenforoResourceManagerAPI.Client.svg)](https://www.nuget.org/packages/XenforoResourceManagerAPI.Client/)

A strongly typed .NET wrapper for the XenForo Resource Manager API exposed by [SpigotMC](https://spigotmc.org/). Built with **.NET Standard 2.1**, this library provides a simple and convenient way to read resource, category, update, and author information directly from your C# application.

## Features

- **Resources**: List resources with optional category and page filters, retrieve a single resource, and list resources created by an author.
- **Resource Categories**: Retrieve all available resource categories.
- **Resource Updates**: Retrieve a specific resource update or list all updates for a resource.
- **Authors**: Retrieve author details by user id or find an author by exact username.
- **Simple Public Access**: Uses the public SpigotMC API endpoints without authentication.
- **Configurable Transport**: Supports custom `HttpClient`, base address, and JSON serializer settings.

---

## Installation

Add the project reference to your application or install the package from [NuGet](https://www.nuget.org/packages/XenforoResourceManagerAPI.Client/):

```bash
dotnet add package XenforoResourceManagerAPI.Client
```

---

## Quick Start

### 1. Initialize the Client
The SpigotMC API is currently exposed through public GET endpoints, so no API token is required:

```csharp
using XenforoResourceManagerAPI.Client;

var client = new XenforoResourceManagerApiClient();
```

### 2. Read Resource Data
Here is a simple example of how to list resources, load a single resource, and find an author:

```csharp
var resources = await client.Resources.ListAsync(category: 4, page: 2);

foreach (var resource in resources)
{
    Console.WriteLine($"{resource.Title} (ID: {resource.Id})");
}

var hubKick = await client.Resources.GetAsync(2);
Console.WriteLine($"Current version: {hubKick.CurrentVersion}");

var author = await client.Authors.FindAsync("simpleauthority");
Console.WriteLine($"Author ID: {author.Id}");
```

---

## Modules & API Reference

### Resources (**client.Resources**)
Provides endpoints to retrieve resource metadata.

```csharp
// List resources, optionally filtered by category and page
var resources = await client.Resources.ListAsync(category: 4, page: 2);

// Retrieve a resource by id
var resource = await client.Resources.GetAsync(2);
Console.WriteLine($"Title: {resource.Title}");
Console.WriteLine($"Downloads: {resource.Stats?.Downloads}");

// Retrieve resources created by an author
var authorResources = await client.Resources.GetByAuthorAsync(authorId: 100356, page: 1);
```

### Resource Categories (**client.ResourceCategories**)
Retrieves the resource categories available in the API.

```csharp
var categories = await client.ResourceCategories.ListAsync();

foreach (var category in categories)
{
    Console.WriteLine($"{category.Id}: {category.Title}");
}
```

### Resource Updates (**client.ResourceUpdates**)
Provides endpoints to retrieve update posts for resources.

```csharp
// Retrieve a specific resource update by update id
var update = await client.ResourceUpdates.GetAsync(352711);
Console.WriteLine($"Update: {update.Title}");

// Retrieve updates for a resource
var updates = await client.ResourceUpdates.GetByResourceAsync(resourceId: 2, page: 1);

foreach (var resourceUpdate in updates)
{
    Console.WriteLine($"{resourceUpdate.ResourceVersion}: {resourceUpdate.Title}");
}
```

### Authors (**client.Authors**)
Provides endpoints to retrieve author metadata.

```csharp
// Retrieve an author by user id
var author = await client.Authors.GetAsync(1);
Console.WriteLine($"Username: {author.Username}");

// Find an author by exact username
var exactMatch = await client.Authors.FindAsync("simpleauthority");
Console.WriteLine($"Resource count: {exactMatch.ResourceCount}");
```

---

## Configuration Options

Use **XenforoResourceManagerApiClientOptions** to customize the client. This is useful for environments where a custom **HttpClient** should be used, for example when integrating into ASP.NET Core via **IHttpClientFactory**:

```csharp
var options = new XenforoResourceManagerApiClientOptions
{
    BaseAddress = new Uri("https://api.spigotmc.org/simple/0.2/"),
    JsonSerializerSettings = new JsonSerializerSettings { /* Custom JSON settings */ }
};

// Instantiate with a custom HttpClient and options
var client = new XenforoResourceManagerApiClient(myCustomHttpClient, options);
```

### Dependency Injection Integration (ASP.NET Core)

Register the client in your **Program.cs**:

```csharp
builder.Services.AddHttpClient<XenforoResourceManagerApiClient>((httpClient, sp) =>
{
    var options = new XenforoResourceManagerApiClientOptions();
    return new XenforoResourceManagerApiClient(httpClient, options);
});
```

---

## Error Handling

API calls return the deserialized response model directly, for example `Resource`, `Author`, `ResourceUpdate`, or `List<Resource>`.

If an HTTP communication error occurs, the API returns an empty response, or the response cannot be deserialized, the client throws a **XenforoResourceManagerApiException**.

```csharp
try
{
    var resource = await client.Resources.GetAsync(2);
    Console.WriteLine($"Resource: {resource.Title}");
}
catch (XenforoResourceManagerApiException ex)
{
    Console.WriteLine($"API Error: {ex.Message}");
    if (ex.StatusCode.HasValue)
    {
        Console.WriteLine($"HTTP Status Code: {ex.StatusCode}");
        Console.WriteLine($"Response Content: {ex.ResponseContent}");
    }
}
```

---

## Endpoint Mapping

| Client method | API action |
|---------------|------------|
| `client.Resources.ListAsync(...)` | `listResources` |
| `client.Resources.GetAsync(...)` | `getResource` |
| `client.Resources.GetByAuthorAsync(...)` | `getResourcesByAuthor` |
| `client.ResourceCategories.ListAsync(...)` | `listResourceCategories` |
| `client.ResourceUpdates.GetAsync(...)` | `getResourceUpdate` |
| `client.ResourceUpdates.GetByResourceAsync(...)` | `getResourceUpdates` |
| `client.Authors.GetAsync(...)` | `getAuthor` |
| `client.Authors.FindAsync(...)` | `findAuthor` |

---

## License

This project is licensed under the **MIT License**. See the LICENSE.txt file for details.

---

## Disclaimer

This is an unofficial .NET wrapper for the XenForo Resource Manager API exposed by SpigotMC. The project is not affiliated with SpigotMC, XenForo, or their owners. All brand names and logos are property of their respective owners.
