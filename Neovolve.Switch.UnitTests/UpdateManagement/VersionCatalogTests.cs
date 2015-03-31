namespace Neovolve.Switch.UnitTests.UpdateManagement
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Switch.UpdateManagement;

    /// <summary>
    /// The version catalog tests.
    /// </summary>
    [TestClass]
    public class VersionCatalogTests
    {
        /// <summary>
        /// Runs test for can deserialize.
        /// </summary>
        [TestMethod]
        public void CanDeserializeTest()
        {
            const String XmlData =
                @"<?xml version=""1.0"" encoding=""utf-8""?>
<VersionCatalog xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <Packages>
    <PackageDescription>
      <PackageAddress>http://switch.codeplex.com</PackageAddress>
      <PackageType>Release</PackageType>
      <PackageVersion Build=""0"" Major=""2"" Minor=""0"" Revision=""1"" />
      <Released>2011-06-12T13:02:26.1653071Z</Released>
      <ReleaseNotesAddress>http://switch.codeplex.com/releasenotes</ReleaseNotesAddress>
    </PackageDescription>
  </Packages>
</VersionCatalog>";
            XmlSerializer serializer = new XmlSerializer(typeof(VersionCatalog));

            TextReader reader = new StringReader(XmlData);
            VersionCatalog actual = serializer.Deserialize(reader) as VersionCatalog;

            Assert.IsNotNull(actual, "VersionCatalog failed to deserialize");
            Assert.AreEqual(1, actual.Packages.Count, "Count returned an incorrect value");
        }

        /// <summary>
        /// Runs test for can serialize and deserialize the same value.
        /// </summary>
        [TestMethod]
        public void CanSerializeAndDeserializeTheSameValueTest()
        {
            VersionCatalog target = GenerateCatalog();
            XmlSerializer serializer = new XmlSerializer(typeof(VersionCatalog));
            StringBuilder builder = new StringBuilder();

            using (TextWriter writer = new StringWriter(builder))
            {
                serializer.Serialize(writer, target);
            }

            String xmlData = builder.ToString();
            TextReader reader = new StringReader(xmlData);
            VersionCatalog actual = serializer.Deserialize(reader) as VersionCatalog;

            Assert.AreEqual(target.Packages.Count, actual.Packages.Count, "Count returned an incorrect value");
            Assert.AreEqual(target.Packages[0].PackageAddress, actual.Packages[0].PackageAddress, "PackageAddress returned an incorrect value");
            Assert.AreEqual(target.Packages[0].PackageType, actual.Packages[0].PackageType, "PackageType returned an incorrect value");
            Assert.AreEqual(target.Packages[0].PackageVersion, actual.Packages[0].PackageVersion, "PackageVersion returned an incorrect value");
            Assert.AreEqual(target.Packages[0].Released, actual.Packages[0].Released, "Released returned an incorrect value");
            Assert.AreEqual(
                target.Packages[0].ReleaseNotesAddress, actual.Packages[0].ReleaseNotesAddress, "ReleaseNotesAddress returned an incorrect value");
        }

        /// <summary>
        /// Runs test for can serialize.
        /// </summary>
        [TestMethod]
        public void CanSerializeTest()
        {
            VersionCatalog target = GenerateCatalog();
            XmlSerializer serializer = new XmlSerializer(typeof(VersionCatalog));
            StringBuilder builder = new StringBuilder();

            using (TextWriter writer = new StringWriter(builder))
            {
                serializer.Serialize(writer, target);
            }

            String actual = builder.ToString();

            Trace.WriteLine(actual);

            Assert.IsFalse(String.IsNullOrWhiteSpace(actual), "VersionCatalog could not be serialized");
        }

        #region Static Helper Methods

        /// <summary>
        /// Generates the catalog.
        /// </summary>
        /// <returns>
        /// A <see cref="VersionCatalog"/> instance.
        /// </returns>
        private static VersionCatalog GenerateCatalog()
        {
            PackageDescription package = new PackageDescription
                                         {
                                             PackageAddress = Guid.NewGuid().ToString(), 
                                             PackageType = PackageType.Alpha, 
                                             PackageVersion = new PackageVersion(2, 0, 0, 1), 
                                             Released = DateTime.UtcNow, 
                                             ReleaseNotesAddress = Guid.NewGuid().ToString()
                                         };
            List<PackageDescription> packages = new List<PackageDescription>
                                                {
                                                    package
                                                };

            return new VersionCatalog(packages);
        }

        #endregion

        /// <summary>
        /// Gets or sets the test context which provides
        ///   information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext
        {
            get;
            set;
        }
    }
}