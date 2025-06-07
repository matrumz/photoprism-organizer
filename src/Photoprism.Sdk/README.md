# Photoprism.Sdk

This project is the Photoprism SDK for the Photoprism Organizer application.

It exposes simple actions and queries to interact with a Photoprism instance, such as:
- Adding labels to photos
- Removing keywords from photos
- Searching for photos by label/keyword/etc.

For tasks that are not supported by the Photoprism API, it will fall back to the less-optimal method of directly manipulating the database. As Photoprism continues to evolve, these implementations will be updated to use the best available method.

Goals:
- Provide a stable interface for interacting with any Photoprism instance, regardless of the instance's version or capabilities.
- Expose a .NET implementation of the Photoprism API.
- Extend the Photoprism API with additional, generic functionality that is not available in the official API.

## Namespaces

- `Photoprism.Sdk`: Contains the exposed SDK functionality via a client class.
- `Photoprism.Sdk.Database`: Contains the database access layer for direct database manipulation.
- `Photoprism.Sdk.Hosting`: Contains the functionality for hosting the SDK in a .NET Generic Host application.
- `Photoprism.Sdk.Rest`: Contains the REST API generated from the Photoprism OpenAPI specification.
