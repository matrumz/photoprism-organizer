using PhotoPrismOrganizer.Common;
using PhotoPrismOrganizer.Common.Sdks.Compatibility;

using Serilog;

namespace PhotoPrism.Sdk;

internal partial class Client(
    ILogger logger,
    OperationRegistry operationRegistry
) : IClient
{
}
