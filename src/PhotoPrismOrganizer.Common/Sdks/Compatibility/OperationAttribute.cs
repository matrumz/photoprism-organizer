using NuGet.Versioning;

using PhotoPrismOrganizer.Common.Extensions;

namespace PhotoPrismOrganizer.Common.Sdks.Compatibility;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class OperationAttribute(
    string operationKey,
    string? since = null,
    string? until = null,
    int priority = 0
) : Attribute
{

    public string OperationKey => operationKey;
    public string? Since => since;
    public string? Until => until;
    public int Priority => priority;

    public bool IsApplicable(SemanticVersion currentVersion) =>
        currentVersion.Between(since.ParseSemanticVersionOrNull(), until.ParseSemanticVersionOrNull());

}
