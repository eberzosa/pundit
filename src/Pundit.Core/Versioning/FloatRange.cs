using System;
using System.Globalization;
using System.Linq;
using EBerzosa.Utils;

namespace EBerzosa.Pundit.Core.Versioning
{
    public class FloatRange : IEquatable<FloatRange>
    {
        /// <summary>
        /// The minimum version of the float range. This is null for cases such as *
        /// </summary>
        public PunditVersion MinVersion { get; }

        /// <summary>
        /// Defined float behavior
        /// </summary>
        public FloatBehaviour FloatBehaviour { get; }

        /// <summary>
        /// The original release label. Invalid labels are allowed here.
        /// </summary>
        public string OriginalReleasePrefix { get; }

        /// <summary>
        /// True if a min range exists.
        /// </summary>
        public bool HasMinVersion => MinVersion != null;

        /// <summary>
        /// The original version string
        /// </summary>
        public string OriginalVersion { get; }

        /// <summary>
        /// Create a floating range.
        /// </summary>
        /// <param name="floatBehavior">Section to float.</param>
        public FloatRange(FloatBehaviour floatBehavior)
            : this(floatBehavior, null, null)
        {
        }

        /// <summary>
        /// Create a floating range.
        /// </summary>
        /// <param name="floatBehavior">Section to float.</param>
        /// <param name="minVersion">Min version of the range.</param>
        public FloatRange(FloatBehaviour floatBehavior, PunditVersion minVersion)
            : this(floatBehavior, minVersion, null)
        {
        }

        /// <summary>
        /// Create a floating range.
        /// </summary>
        /// <param name="floatBehavior">Section to float.</param>
        /// <param name="minVersion">Min version of the range.</param>
        /// <param name="releasePrefix">The original release label. Invalid labels are allowed here.</param>
        public FloatRange(FloatBehaviour floatBehavior, PunditVersion minVersion, string releasePrefix)
            : this(floatBehavior, minVersion, releasePrefix, null)
        {
        }

        /// <summary>
        /// FloatRange
        /// </summary>
        /// <param name="floatBehaviour">Section to float.</param>
        /// <param name="minVersion">Min version of the range.</param>
        /// <param name="releasePrefix">The original release label. Invalid labels are allowed here.</param>
        /// <param name="originalVersion">The original version string.</param>
        private FloatRange(FloatBehaviour floatBehaviour, PunditVersion minVersion, string releasePrefix, string originalVersion)
        {
            FloatBehaviour = floatBehaviour;
            MinVersion = minVersion;
            OriginalReleasePrefix = releasePrefix;
            OriginalVersion = originalVersion ?? minVersion.OriginalVersion;

            if (OriginalReleasePrefix == null && minVersion != null && minVersion.IsPrerelease)
                OriginalReleasePrefix = minVersion.Release;
        }


        /// <summary>
        /// True if the given version falls into the floating range.
        /// </summary>
        public bool Satisfies(PunditVersion version)
        {
            Guard.NotNull(version, nameof(version));

            if (FloatBehaviour == FloatBehaviour.AbsoluteLatest)
                return true;

            if (FloatBehaviour == FloatBehaviour.None)
                return VersionComparer.VersionRelease.Equals(MinVersion, version);

            // Everything beyond this point requires a version
            if (FloatBehaviour == FloatBehaviour.Prerelease)
                return VersionComparer.Version.Equals(MinVersion, version) // Allow the stable version to match
                       && (!version.IsPrerelease || version.Release.StartsWith(OriginalReleasePrefix, StringComparison.OrdinalIgnoreCase));

            if (FloatBehaviour == FloatBehaviour.Revision || FloatBehaviour == FloatBehaviour.RevisionPrerelease)
                return MinVersion.Major == version.Major &&
                       MinVersion.Minor == version.Minor &&
                       MinVersion.Patch == version.Patch &&
                       CompareRelease(version, FloatBehaviour.Revision);

            if (FloatBehaviour == FloatBehaviour.Patch || FloatBehaviour == FloatBehaviour.PatchPrerelease)
                return MinVersion.Major == version.Major &&
                       MinVersion.Minor == version.Minor &&
                       CompareRelease(version, FloatBehaviour.Patch);

            if (FloatBehaviour == FloatBehaviour.Minor || FloatBehaviour == FloatBehaviour.MinorPrerelease)
                return MinVersion.Major == version.Major &&
                       CompareRelease(version, FloatBehaviour.Minor);

            if (FloatBehaviour == FloatBehaviour.Major || FloatBehaviour == FloatBehaviour.MajorPrerelease)
                return CompareRelease(version, FloatBehaviour.Major);

            return false;
        }

        /// <summary>
        /// Parse a floating version into a FloatRange
        /// </summary>
        public static FloatRange Parse(string versionString)
        {
            TryParse(versionString, out var range);
            return range;
        }

        /// <summary>
        /// Parse a floating version into a FloatRange
        /// </summary>
        public static bool TryParse(string versionString, out FloatRange range)
        {
            range = null;

            if (string.IsNullOrEmpty(versionString))
                return false;

            var metadataIndex = versionString.IndexOf('+');

            if (metadataIndex == 0)
                return false;

            var originalString = versionString;

            // Remove the metadata
            if (metadataIndex != -1)
                versionString.Remove(metadataIndex);

            if (versionString[0] != '*' && !char.IsDigit(versionString[0]))
                return false;

            var releaseIndex = versionString.IndexOf('-');

            if (releaseIndex == versionString.Length - 1)
                return false;

            var numberVersion = releaseIndex == -1 ? versionString : versionString.Substring(0, releaseIndex);
            var releaseVersion = releaseIndex == -1 ? null : versionString.Substring(releaseIndex + 1);

            var hasVersionStar = numberVersion[numberVersion.Length - 1] == '*';
            var hasReleaseStar = releaseVersion != null && releaseVersion[releaseVersion.Length - 1] == '*';
            var isPrereleaseOnly = hasReleaseStar &&
                                   (releaseVersion.Length == 1 || releaseVersion.Length > 1 && releaseVersion[releaseVersion.Length - 2] != '.');

            // *-beta* format is not allowed. Must be *-beta.* or x-beta*
            if (hasVersionStar && isPrereleaseOnly)
                return false;

            // There is a * in the middle in the version or release
            if (numberVersion.IndexOf('*', 0, numberVersion.Length - 1) != -1 ||
                (releaseVersion?.IndexOf('*', 0, releaseVersion.Length - 1) ?? -1) != -1)
                return false;

            // No * at all
            if (!hasVersionStar && !hasReleaseStar)
                return SetRangeVersion(versionString, FloatBehaviour.None, null, out range, originalString);

            if (hasVersionStar)
            {
                // Anything not *, .*
                if (numberVersion.Length > 1 && numberVersion[numberVersion.Length - 2] != '.')
                    return false;
            }

            var finalVersionString = ReplaceLastChar(numberVersion, '0', null)
                                     + (releaseVersion == null ? "" : '-' + ReplaceLastChar(releaseVersion, '0', new[] {'.', '-'}));

            var preRelease = releaseVersion == null ? null : ReplaceLastChar(releaseVersion, null, null);

            // x.y.z-*, x.y.z-ttt*
            if (!hasVersionStar && isPrereleaseOnly)
                return SetRangeVersion(finalVersionString, FloatBehaviour.Prerelease, preRelease, out range, originalString);

            var versionParts = numberVersion.Split('.').Length;

            FloatBehaviour behaviour;

            if (versionParts == 1)
                behaviour = hasReleaseStar ? FloatBehaviour.MajorPrerelease : FloatBehaviour.Major;
            else if (versionParts == 2)
                behaviour = hasReleaseStar ? FloatBehaviour.MinorPrerelease : FloatBehaviour.Minor;
            else if (versionParts == 3 && hasVersionStar)
                behaviour = hasReleaseStar ? FloatBehaviour.PatchPrerelease : FloatBehaviour.Patch;
            else if (versionParts == 4 || versionParts == 3 && !hasVersionStar)
                behaviour = hasReleaseStar ? FloatBehaviour.RevisionPrerelease : FloatBehaviour.Revision;
            else
                return false;

            return SetRangeVersion(finalVersionString, behaviour, preRelease, out range, originalString);
        }

        /// <summary>
        /// Create a floating version string in the format: 1.0.0-alpha-*
        /// </summary>
        public override string ToString()
        {
            if (FloatBehaviour == FloatBehaviour.None)
                return MinVersion.ToNormalizedString();

            if (FloatBehaviour == FloatBehaviour.Prerelease)
                return string.Format(CultureInfo.InvariantCulture, "{0:V}-{1}*", MinVersion, OriginalReleasePrefix);

            if (FloatBehaviour == FloatBehaviour.Revision)
                return string.Format(CultureInfo.InvariantCulture, "{0}.{1}.{2}.*", MinVersion.Major, MinVersion.Minor, MinVersion.Patch);

            if (FloatBehaviour == FloatBehaviour.Patch)
                return string.Format(CultureInfo.InvariantCulture, "{0}.{1}.*", MinVersion.Major, MinVersion.Minor);

            if (FloatBehaviour == FloatBehaviour.Minor)
                return string.Format(CultureInfo.InvariantCulture, "{0}.*", MinVersion.Major);

            if (FloatBehaviour == FloatBehaviour.Major)
                return "*";

            if (FloatBehaviour == FloatBehaviour.RevisionPrerelease)
                return string.Format(CultureInfo.InvariantCulture, "{0}.{1}.{2}-{3}*", MinVersion.Major, MinVersion.Minor, MinVersion.Patch,
                    OriginalReleasePrefix);

            if (FloatBehaviour == FloatBehaviour.PatchPrerelease)
                return string.Format(CultureInfo.InvariantCulture, "{0}.{1}.*-{2}*", MinVersion.Major, MinVersion.Minor, OriginalReleasePrefix);

            if (FloatBehaviour == FloatBehaviour.MinorPrerelease)
                return string.Format(CultureInfo.InvariantCulture, "{0}.*-{1}*", MinVersion.Major, OriginalReleasePrefix);

            if (FloatBehaviour == FloatBehaviour.MajorPrerelease)
                return string.Format(CultureInfo.InvariantCulture, "*-{0}*", OriginalReleasePrefix);

            if (FloatBehaviour == FloatBehaviour.AbsoluteLatest)
                return string.Format("*-*");
            return "";
        }

        public bool Equals(FloatRange other)
        {
            return other != null && FloatBehaviour == other.FloatBehaviour && VersionComparer.Default.Equals(MinVersion, other.MinVersion);
        }

        public override int GetHashCode()
        {
            var seed = 0x1505L;
            seed = ((seed << 5) + seed) ^ FloatBehaviour.GetHashCode();
            seed = ((seed << 5) + seed) ^ MinVersion.GetHashCode();

            return seed.GetHashCode();
        }

        private static bool SetRangeVersion(string versionString, FloatBehaviour behaviour, string release, out FloatRange floatVersion,
            string originalString)
        {
            floatVersion = PunditVersion.TryParse(versionString, out var version)
                ? new FloatRange(behaviour, version, release, originalString)
                : null;

            return floatVersion != null;
        }

        private static string ReplaceLastChar(string str, char? newChar, char[] onlyIfPreviousIs)
        {
            var pos = str.Length - 1;

            if (pos == 0)
                return newChar != null ? "0" : "";

            if (str[pos] != '*')
                return str;

            if (!newChar.HasValue || onlyIfPreviousIs != null && !onlyIfPreviousIs.Contains(str[pos - 1]))
                return str.Substring(0, pos);

            var tmp = str.ToCharArray();
            tmp[pos] = newChar.Value;

            return new string(tmp);
        }

        private bool CompareRelease(PunditVersion version, FloatBehaviour floatBehaviour)
        {
            if (FloatBehaviour == floatBehaviour)
                return !version.IsPrerelease;

            if (!version.IsPrerelease)
                return true;

            if (OriginalReleasePrefix.EndsWith("."))
                return (version.Release + ".").StartsWith(OriginalReleasePrefix, StringComparison.OrdinalIgnoreCase);

            return version.Release.StartsWith(OriginalReleasePrefix, StringComparison.OrdinalIgnoreCase);
        }
    }
}