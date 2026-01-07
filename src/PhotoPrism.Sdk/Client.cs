using Microsoft.Extensions.Logging;

using PhotoPrismOrganizer.Common;
using PhotoPrismOrganizer.Common.Sdks.Compatibility;

namespace PhotoPrism.Sdk;

internal partial class Client(
    ILogger<Client> logger,
    OperationRegistry operationRegistry
) : IClient
{
}
