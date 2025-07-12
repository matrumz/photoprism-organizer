using System.Diagnostics.CodeAnalysis;

namespace PhotoPrismOrganizer.Common;

public record SemanticVersion(
    int Major,
    int Minor = 0,
    int Patch = 0
) : IComparable<SemanticVersion>
{

    public SemanticVersion(string version)
        : this(Parse(version))
    {
    }

    /// <summary>
    /// Parses a semantic version string in the format "v?(Major)(.Minor)?(.Patch)?".
    /// </summary>
    /// <param name="version"></param>
    /// <returns></returns>
    public static SemanticVersion Parse(string version)
    {
        if (string.IsNullOrWhiteSpace(version))
            throw new ArgumentException($"Argument cannot be null or empty.", nameof(version));
        if (version.StartsWith("v", StringComparison.OrdinalIgnoreCase))
            version = version[1..];
        var parts = version.Split('.');
        return new(
            int.Parse(parts[0]),
            parts.Length > 1 ? int.Parse(parts[1]) : 0,
            parts.Length > 2 ? int.Parse(parts[2]) : 0
        );
    }

    /// <summary>
    /// Parses a semantic version string or returns a parsed default version if the string is invalid.
    /// </summary>
    /// <param name="version"></param>
    /// <param name="defaultVersion"></param>
    /// <returns></returns>
    public static SemanticVersion Parse(string? version, string defaultVersion) =>
        IsValid(version)
            ? Parse(version)
            : new SemanticVersion(defaultVersion);

    /// <summary>
    /// Determines whether the specified string is a valid semantic version.
    /// </summary>
    /// <param name="version">The version string to validate.</param>
    /// <returns>true if the version string is valid; otherwise, false.</returns>
    public static bool IsValid([NotNullWhen(true)] string? version)
    {
        if (string.IsNullOrWhiteSpace(version))
            return false;
        try
        {
            Parse(version);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Compares this instance with another SemanticVersion instance.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(SemanticVersion? other) =>
        other is null
            ? 1
            : (Major, Minor, Patch).CompareTo((other.Major, other.Minor, other.Patch));

    /// <summary>
    /// Checks if this version is compatible with the specified range.
    /// </summary>
    /// <param name="since">(inclusive)</param>
    /// <param name="until">(exclusive)</param>
    /// <returns></returns>
    public bool Between(SemanticVersion? since, SemanticVersion? until) =>
        (since == null || this >= since) && (until == null || this < until);

    public static bool operator >=(SemanticVersion v1, SemanticVersion v2) => v1.CompareTo(v2) >= 0;
    public static bool operator <=(SemanticVersion v1, SemanticVersion v2) => v1.CompareTo(v2) <= 0;
    public static bool operator >(SemanticVersion v1, SemanticVersion v2) => v1.CompareTo(v2) > 0;
    public static bool operator <(SemanticVersion v1, SemanticVersion v2) => v1.CompareTo(v2) < 0;

}

public static class StringExtensions
{

    public static bool IsValidSemanticVersion(this string? version) =>
        SemanticVersion.IsValid(version);

    public static bool IsValidSemanticVersionOrNull(this string? version) =>
        version is null || SemanticVersion.IsValid(version);

    public static SemanticVersion ParseSemanticVersion(this string version) =>
        SemanticVersion.Parse(version);

    public static SemanticVersion? ParseSemanticVersionOrNull(this string? version) =>
        version is null ? null : SemanticVersion.Parse(version);

    public static SemanticVersion ParseSemanticVersionOrDefault(this string? version, string defaultVersion) =>
        SemanticVersion.Parse(version, defaultVersion);

}
