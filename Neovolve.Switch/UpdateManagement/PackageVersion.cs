namespace Neovolve.Switch.UpdateManagement
{
    using System;
    using System.Globalization;
    using System.Xml.Serialization;

    /// <summary>
    /// The <see cref="PackageVersion"/>
    ///   class is used to define the version of a package.
    /// </summary>
    public class PackageVersion : IComparable, IComparable<PackageVersion>, IEquatable<PackageVersion>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PackageVersion"/> class.
        /// </summary>
        public PackageVersion()
            : this(1, 0, 0, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageVersion"/> class.
        /// </summary>
        /// <param name="major">
        /// The major.
        /// </param>
        /// <param name="minor">
        /// The minor.
        /// </param>
        /// <param name="build">
        /// The build.
        /// </param>
        /// <param name="revision">
        /// The revision.
        /// </param>
        public PackageVersion(Int32 major, Int32 minor, Int32 build, Int32 revision)
        {
            Major = major;
            Minor = minor;
            Build = build;
            Revision = revision;
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="v1">
        /// The v1.
        /// </param>
        /// <param name="v2">
        /// The v2.
        /// </param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Boolean operator ==(PackageVersion v1, PackageVersion v2)
        {
            if (ReferenceEquals(v1, null))
            {
                return ReferenceEquals(v2, null);
            }

            return v1.Equals(v2);
        }

        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="v1">
        /// The v1.
        /// </param>
        /// <param name="v2">
        /// The v2.
        /// </param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Boolean operator >(PackageVersion v1, PackageVersion v2)
        {
            return v2 < v1;
        }

        /// <summary>
        /// Implements the operator &gt;=.
        /// </summary>
        /// <param name="v1">
        /// The v1.
        /// </param>
        /// <param name="v2">
        /// The v2.
        /// </param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Boolean operator >=(PackageVersion v1, PackageVersion v2)
        {
            return v2 <= v1;
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="v1">
        /// The v1.
        /// </param>
        /// <param name="v2">
        /// The v2.
        /// </param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Boolean operator !=(PackageVersion v1, PackageVersion v2)
        {
            return !(v1 == v2);
        }

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="v1">
        /// The v1.
        /// </param>
        /// <param name="v2">
        /// The v2.
        /// </param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Boolean operator <(PackageVersion v1, PackageVersion v2)
        {
            if (v1 == null)
            {
                throw new ArgumentNullException("v1");
            }

            return v1.CompareTo(v2) < 0;
        }

        /// <summary>
        /// Implements the operator &lt;=.
        /// </summary>
        /// <param name="v1">
        /// The v1.
        /// </param>
        /// <param name="v2">
        /// The v2.
        /// </param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Boolean operator <=(PackageVersion v1, PackageVersion v2)
        {
            if (v1 == null)
            {
                throw new ArgumentNullException("v1");
            }

            return v1.CompareTo(v2) <= 0;
        }

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="version">
        /// The version.
        /// </param>
        /// <returns>
        /// A <see cref="Int32"/> instance.
        /// </returns>
        public Int32 CompareTo(Object version)
        {
            if (version == null)
            {
                return 1;
            }

            PackageVersion version2 = version as PackageVersion;
            if (version2 == null)
            {
                throw new ArgumentException("version");
            }

            if (Major != version2.Major)
            {
                if (Major > version2.Major)
                {
                    return 1;
                }

                return -1;
            }

            if (Minor != version2.Minor)
            {
                if (Minor > version2.Minor)
                {
                    return 1;
                }

                return -1;
            }

            if (Build != version2.Build)
            {
                if (Build > version2.Build)
                {
                    return 1;
                }

                return -1;
            }

            if (Revision == version2.Revision)
            {
                return 0;
            }

            if (Revision > version2.Revision)
            {
                return 1;
            }

            return -1;
        }

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// A <see cref="Int32"/> instance.
        /// </returns>
        public Int32 CompareTo(PackageVersion value)
        {
            if (value == null)
            {
                return 1;
            }

            if (Major != value.Major)
            {
                if (Major > value.Major)
                {
                    return 1;
                }

                return -1;
            }

            if (Minor != value.Minor)
            {
                if (Minor > value.Minor)
                {
                    return 1;
                }

                return -1;
            }

            if (Build != value.Build)
            {
                if (Build > value.Build)
                {
                    return 1;
                }

                return -1;
            }

            if (Revision == value.Revision)
            {
                return 0;
            }

            if (Revision > value.Revision)
            {
                return 1;
            }

            return -1;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="System.Object"/> to compare with this instance.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override Boolean Equals(Object obj)
        {
            PackageVersion version = obj as PackageVersion;
            if (version == null)
            {
                return false;
            }

            return ((Major == version.Major) && (Minor == version.Minor)) && ((Build == version.Build) && (Revision == version.Revision));
        }

        /// <summary>
        /// Equalses the specified obj.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// A <see cref="Boolean"/> instance.
        /// </returns>
        public Boolean Equals(PackageVersion obj)
        {
            if (obj == null)
            {
                return false;
            }

            return ((Major == obj.Major) && (Minor == obj.Minor)) && ((Build == obj.Build) && (Revision == obj.Revision));
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override Int32 GetHashCode()
        {
            Int32 num = 0;
            num |= (Major & 15) << 0x1c;
            num |= (Minor & 0xff) << 20;
            num |= (Build & 0xff) << 12;
            return num | (Revision & 0xfff);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override String ToString()
        {
            if (Build == -1)
            {
                return ToString(2);
            }

            if (Revision == -1)
            {
                return ToString(3);
            }

            return ToString(4);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="fieldCount">
        /// The field count.
        /// </param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public String ToString(Int32 fieldCount)
        {
            switch (fieldCount)
            {
                case 0:
                    return String.Empty;

                case 1:
                    return Major.ToString(CultureInfo.InvariantCulture);

                case 2:
                    return Major + "." + Minor;
            }

            if (Build == -1)
            {
                throw new ArgumentOutOfRangeException("fieldCount");
            }

            if (fieldCount == 3)
            {
                return String.Concat(
                    new Object[]
                    {
                        Major, ".", Minor, ".", Build
                    });
            }

            if (Revision == -1)
            {
                throw new ArgumentOutOfRangeException("fieldCount");
            }

            if (fieldCount != 4)
            {
                throw new ArgumentOutOfRangeException("fieldCount");
            }

            return String.Concat(
                new Object[]
                {
                    Major, ".", Minor, ".", Build, ".", Revision
                });
        }

        /// <summary>
        /// Gets or sets the build.
        /// </summary>
        /// <value>
        /// The build.
        /// </value>
        [XmlAttribute]
        public Int32 Build
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the major.
        /// </summary>
        /// <value>
        /// The major.
        /// </value>
        [XmlAttribute]
        public Int32 Major
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the minor.
        /// </summary>
        /// <value>
        /// The minor.
        /// </value>
        [XmlAttribute]
        public Int32 Minor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the revision.
        /// </summary>
        /// <value>
        /// The revision.
        /// </value>
        [XmlAttribute]
        public Int32 Revision
        {
            get;
            set;
        }
    }
}