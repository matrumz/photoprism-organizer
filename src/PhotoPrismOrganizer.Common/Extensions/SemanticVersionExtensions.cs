namespace PhotoPrismOrganizer.Common.Extensions;

public static class SemanticVersionExtensions
{

    public static bool Between(
        this NuGet.Versioning.SemanticVersion version,
        NuGet.Versioning.SemanticVersion? since,
        NuGet.Versioning.SemanticVersion? until
    )
    {
        if (since is not null && version < since) return false;
        if (until is not null && version > until) return false;
        return true;
    }

}
