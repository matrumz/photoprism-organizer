# PhotoPrism.Sdk

This project is the PhotoPrism SDK for the PhotoPrism Organizer application.

It exposes simple actions and queries to interact with a PhotoPrism instance, such as:
- Adding labels to photos
- Removing keywords from photos
- Searching for photos by label/keyword/etc.

For tasks that are not supported by the PhotoPrism API, it will fall back to the less-optimal method of directly manipulating the database. As PhotoPrism continues to evolve, these implementations will be updated to use the best available method.

Goals:
- Provide a stable interface for interacting with any PhotoPrism instance, regardless of the instance's version or capabilities.
- Expose a .NET implementation of the PhotoPrism API.
- Extend the PhotoPrism API with additional, generic functionality that is not available in the official API.

## Namespaces

- `PhotoPrism.Sdk`: Contains the exposed SDK functionality via a client class.
- `PhotoPrism.Sdk.Database`: Contains the database access layer for direct database manipulation.
- `PhotoPrism.Sdk.Hosting`: Contains the functionality for hosting the SDK in a .NET Generic Host application.
- `PhotoPrism.Sdk.Rest`: Contains the REST API generated from the PhotoPrism OpenAPI specification.
