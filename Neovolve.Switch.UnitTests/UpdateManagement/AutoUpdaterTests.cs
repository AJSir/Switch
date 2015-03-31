namespace Neovolve.Switch.UnitTests.UpdateManagement
{
    using System;
    using System.IO;
    using System.Xml.Serialization;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Switch.Extensibility.ResourceManagement;
    using Neovolve.Switch.Extensibility.Services;
    using Neovolve.Switch.UpdateManagement;
    using Rhino.Mocks;
    using Rhino.Mocks.Constraints;

    /// <summary>
    /// The <see cref="AutoUpdaterTests"/>
    ///   class is used to test the <see cref="AutoUpdater"/> class.
    /// </summary>
    [TestClass]
    public class AutoUpdaterTests
    {
        /// <summary>
        /// Runs test for execute does not notify user when new version does not release type filter.
        /// </summary>
        [TestMethod]
        public void ExecuteDoesNotNotifyUserWhenNewVersionDoesNotReleaseTypeFilterTest()
        {
            AutoUpdateSettings settings = new AutoUpdateSettings();
            IDownloadManager downloadManager = MockRepository.GenerateStub<IDownloadManager>();
            ILogWriter logWriter = MockRepository.GenerateStub<ILogWriter>();
            IUserNotification userNotification = MockRepository.GenerateStub<IUserNotification>();
            IPackageInstaller installer = MockRepository.GenerateStub<IPackageInstaller>();
            AutoUpdater target = new AutoUpdater(downloadManager, logWriter, userNotification, settings, installer);
            VersionCatalog catalog = new VersionCatalog();

            AddPackage(catalog, PackageType.Beta, 2, 1);

            using (Stream catalogStream = GenerateCatalogStream(catalog))
            {
                downloadManager.Stub(x => x.Download(settings.VersionMetadataAddress)).Return(catalogStream);

                target.Execute();
            }

            userNotification.AssertWasNotCalled(x => x.ShowMessage(null), opt => opt.IgnoreArguments());
        }

        /// <summary>
        /// Runs test for execute does not notify user when no new version exists.
        /// </summary>
        [TestMethod]
        public void ExecuteDoesNotNotifyUserWhenNoNewVersionExistsTest()
        {
            AutoUpdateSettings settings = new AutoUpdateSettings();
            IDownloadManager downloadManager = MockRepository.GenerateStub<IDownloadManager>();
            ILogWriter logWriter = MockRepository.GenerateStub<ILogWriter>();
            IUserNotification userNotification = MockRepository.GenerateStub<IUserNotification>();
            IPackageInstaller installer = MockRepository.GenerateStub<IPackageInstaller>();
            AutoUpdater target = new AutoUpdater(downloadManager, logWriter, userNotification, settings, installer);
            VersionCatalog catalog = new VersionCatalog();

            AddPackage(catalog);

            using (Stream catalogStream = GenerateCatalogStream(catalog))
            {
                downloadManager.Stub(x => x.Download(settings.VersionMetadataAddress)).Return(catalogStream);

                target.Execute();
            }

            userNotification.AssertWasNotCalled(x => x.ShowMessage(null), opt => opt.IgnoreArguments());
        }

        /// <summary>
        /// Runs test for execute does not notify user when version catalog is not available.
        /// </summary>
        [TestMethod]
        public void ExecuteDoesNotNotifyUserWhenVersionCatalogIsNotAvailableTest()
        {
            AutoUpdateSettings settings = new AutoUpdateSettings();
            IDownloadManager downloadManager = MockRepository.GenerateStub<IDownloadManager>();
            ILogWriter logWriter = MockRepository.GenerateStub<ILogWriter>();
            IUserNotification userNotification = MockRepository.GenerateStub<IUserNotification>();
            IPackageInstaller installer = MockRepository.GenerateStub<IPackageInstaller>();
            AutoUpdater target = new AutoUpdater(downloadManager, logWriter, userNotification, settings, installer);

            target.Execute();

            userNotification.AssertWasNotCalled(x => x.ShowMessage(null), opt => opt.IgnoreArguments());
        }

        /// <summary>
        /// The execute downloads package with user approval test.
        /// </summary>
        [TestMethod]
        public void ExecuteDownloadsPackageWithUserApprovalTest()
        {
            AutoUpdateSettings settings = new AutoUpdateSettings();
            IDownloadManager downloadManager = MockRepository.GenerateStub<IDownloadManager>();
            ILogWriter logWriter = MockRepository.GenerateStub<ILogWriter>();
            IUserNotification userNotification = MockRepository.GenerateStub<IUserNotification>();
            IPackageInstaller installer = MockRepository.GenerateStub<IPackageInstaller>();
            AutoUpdater target = new AutoUpdater(downloadManager, logWriter, userNotification, settings, installer);
            VersionCatalog catalog = new VersionCatalog();

            AddPackage(catalog, PackageType.Release, 2, 1);

            userNotification.Stub(x => x.AskQuestion(null)).IgnoreArguments().Return(true);

            using (Stream catalogStream = GenerateCatalogStream(catalog))
            {
                downloadManager.Stub(x => x.Download(settings.VersionMetadataAddress)).Return(catalogStream);
                downloadManager.Stub(x => x.Download(new Uri(catalog.Packages[0].PackageAddress))).Return(catalogStream);

                target.Execute();
            }

            downloadManager.AssertWasCalled(x => x.Download(new Uri(catalog.Packages[0].PackageAddress)));
        }

        /// <summary>
        /// Runs test for execute downloads relative package address as relative to the catalog address.
        /// </summary>
        [TestMethod]
        public void ExecuteDownloadsRelativePackageAddressAsRelativeToTheCatalogAddressTest()
        {
            AutoUpdateSettings settings = new AutoUpdateSettings();
            IDownloadManager downloadManager = MockRepository.GenerateStub<IDownloadManager>();
            ILogWriter logWriter = MockRepository.GenerateStub<ILogWriter>();
            IUserNotification userNotification = MockRepository.GenerateStub<IUserNotification>();
            IPackageInstaller installer = MockRepository.GenerateStub<IPackageInstaller>();
            AutoUpdater target = new AutoUpdater(downloadManager, logWriter, userNotification, settings, installer);
            VersionCatalog catalog = new VersionCatalog();

            AddPackage(catalog, PackageType.Release, 2, 1);

            catalog.Packages[0].PackageAddress = "someresource.zip";

            String baseAddress = settings.VersionMetadataAddress.ToString();
            Int32 lastPartIndex = baseAddress.LastIndexOf("/");

            if (lastPartIndex > -1)
            {
                baseAddress = baseAddress.Substring(0, lastPartIndex + 1);
            }

            Uri expectedAddress = new Uri(baseAddress + catalog.Packages[0].PackageAddress);

            userNotification.Stub(x => x.AskQuestion(null)).IgnoreArguments().Return(true);

            using (Stream catalogStream = GenerateCatalogStream(catalog))
            {
                downloadManager.Stub(x => x.Download(settings.VersionMetadataAddress)).Return(catalogStream);
                downloadManager.Stub(x => x.Download(expectedAddress)).Return(catalogStream);

                target.Execute();
            }

            downloadManager.AssertWasCalled(x => x.Download(expectedAddress));
        }

        /// <summary>
        /// Runs test for execute installs package with user approval.
        /// </summary>
        [TestMethod]
        public void ExecuteInstallsPackageWithUserApprovalTest()
        {
            AutoUpdateSettings settings = new AutoUpdateSettings();
            IDownloadManager downloadManager = MockRepository.GenerateStub<IDownloadManager>();
            ILogWriter logWriter = MockRepository.GenerateStub<ILogWriter>();
            IUserNotification userNotification = MockRepository.GenerateStub<IUserNotification>();
            IPackageInstaller installer = MockRepository.GenerateStub<IPackageInstaller>();
            AutoUpdater target = new AutoUpdater(downloadManager, logWriter, userNotification, settings, installer);
            VersionCatalog catalog = new VersionCatalog();

            AddPackage(catalog, PackageType.Release, 2, 1);

            userNotification.Stub(x => x.AskQuestion(null)).IgnoreArguments().Return(true);

            using (Stream catalogStream = GenerateCatalogStream(catalog))
            {
                downloadManager.Stub(x => x.Download(settings.VersionMetadataAddress)).Return(catalogStream);
                downloadManager.Stub(x => x.Download(new Uri(catalog.Packages[0].PackageAddress))).Return(catalogStream);

                target.Execute();

                installer.AssertWasCalled(x => x.InstallPackage(null, null), opt => opt.Constraints(Is.NotNull(), Is.Equal(catalogStream)));
            }
        }

        /// <summary>
        /// Runs test for execute notifies user of latest version when mulitple versions are available.
        /// </summary>
        [TestMethod]
        public void ExecuteNotifiesUserOfLatestVersionWhenMulitpleVersionsAreAvailableTest()
        {
            AutoUpdateSettings settings = new AutoUpdateSettings();
            IDownloadManager downloadManager = MockRepository.GenerateStub<IDownloadManager>();
            ILogWriter logWriter = MockRepository.GenerateStub<ILogWriter>();
            IUserNotification userNotification = MockRepository.GenerateStub<IUserNotification>();
            IPackageInstaller installer = MockRepository.GenerateStub<IPackageInstaller>();
            AutoUpdater target = new AutoUpdater(downloadManager, logWriter, userNotification, settings, installer);
            VersionCatalog catalog = new VersionCatalog();

            AddPackage(catalog, PackageType.Release, 2, 1);
            AddPackage(catalog, PackageType.Beta, 2, 1, 0, 1);
            AddPackage(catalog, PackageType.Alpha, 2, 1, 0, 2);

            using (Stream catalogStream = GenerateCatalogStream(catalog))
            {
                downloadManager.Stub(x => x.Download(settings.VersionMetadataAddress)).Return(catalogStream);

                target.Execute();
            }

            userNotification.AssertWasNotCalled(x => x.ShowMessage(null), opt => opt.Constraints(new Contains("2.1.0.2 Alpha")));
        }

        /// <summary>
        /// Runs test for execute notifies user of new available version.
        /// </summary>
        [TestMethod]
        public void ExecuteNotifiesUserOfNewAvailableVersionTest()
        {
            AutoUpdateSettings settings = new AutoUpdateSettings();
            IDownloadManager downloadManager = MockRepository.GenerateStub<IDownloadManager>();
            ILogWriter logWriter = MockRepository.GenerateStub<ILogWriter>();
            IUserNotification userNotification = MockRepository.GenerateStub<IUserNotification>();
            IPackageInstaller installer = MockRepository.GenerateStub<IPackageInstaller>();
            AutoUpdater target = new AutoUpdater(downloadManager, logWriter, userNotification, settings, installer);
            VersionCatalog catalog = new VersionCatalog();

            AddPackage(catalog, PackageType.Release, 2, 1);

            using (Stream catalogStream = GenerateCatalogStream(catalog))
            {
                downloadManager.Stub(x => x.Download(settings.VersionMetadataAddress)).Return(catalogStream);

                target.Execute();
            }

            userNotification.AssertWasNotCalled(x => x.ShowMessage(null), opt => opt.Constraints(new Contains("2.1.0.0")));
        }

        /// <summary>
        /// Runs test for execute obtains version catalog from download manager.
        /// </summary>
        [TestMethod]
        public void ExecuteObtainsVersionCatalogFromDownloadManagerTest()
        {
            AutoUpdateSettings settings = new AutoUpdateSettings();
            IDownloadManager downloadManager = MockRepository.GenerateStub<IDownloadManager>();
            ILogWriter logWriter = MockRepository.GenerateStub<ILogWriter>();
            IUserNotification userNotification = MockRepository.GenerateStub<IUserNotification>();
            IPackageInstaller installer = MockRepository.GenerateStub<IPackageInstaller>();
            AutoUpdater target = new AutoUpdater(downloadManager, logWriter, userNotification, settings, installer);

            target.Execute();

            downloadManager.AssertWasCalled(x => x.Download(settings.VersionMetadataAddress));
        }

        /// <summary>
        /// Runs test for execute user notification of new available version includes non release type name.
        /// </summary>
        [TestMethod]
        public void ExecuteUserNotificationOfNewAvailableVersionIncludesNonReleaseTypeNameTest()
        {
            AutoUpdateSettings settings = new AutoUpdateSettings
                                          {
                                              PackageType = PackageType.Alpha
                                          };
            IDownloadManager downloadManager = MockRepository.GenerateStub<IDownloadManager>();
            ILogWriter logWriter = MockRepository.GenerateStub<ILogWriter>();
            IUserNotification userNotification = MockRepository.GenerateStub<IUserNotification>();
            IPackageInstaller installer = MockRepository.GenerateStub<IPackageInstaller>();
            AutoUpdater target = new AutoUpdater(downloadManager, logWriter, userNotification, settings, installer);
            VersionCatalog catalog = new VersionCatalog();

            AddPackage(catalog, PackageType.Beta, 2, 1);

            using (Stream catalogStream = GenerateCatalogStream(catalog))
            {
                downloadManager.Stub(x => x.Download(settings.VersionMetadataAddress)).Return(catalogStream);

                target.Execute();
            }

            userNotification.AssertWasNotCalled(x => x.ShowMessage(null), opt => opt.Constraints(new Contains("2.1.0.0 Beta")));
        }

        #region Static Helper Methods

        /// <summary>
        /// Adds the package.
        /// </summary>
        /// <param name="catalog">
        /// The catalog.
        /// </param>
        /// <param name="packageType">
        /// Type of the package.
        /// </param>
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
        private static void AddPackage(
            VersionCatalog catalog, 
            PackageType packageType = PackageType.Release, 
            Int32 major = 1, 
            Int32 minor = 0, 
            Int32 build = 0, 
            Int32 revision = 0)
        {
            PackageDescription package = new PackageDescription
                                         {
                                             PackageAddress = "http://switch.codeplex.com/release", 
                                             PackageType = packageType, 
                                             PackageVersion = new PackageVersion(major, minor, build, revision), 
                                             Released = DateTime.UtcNow, 
                                             ReleaseNotesAddress = "http://switch.codeplex.com/doco"
                                         };
            catalog.Packages.Add(package);
        }

        /// <summary>
        /// Generates the catalog stream.
        /// </summary>
        /// <param name="catalog">
        /// The catalog.
        /// </param>
        /// <returns>
        /// A <see cref="Stream"/> instance.
        /// </returns>
        private static Stream GenerateCatalogStream(VersionCatalog catalog)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(VersionCatalog));
            MemoryStream stream = new MemoryStream();

            serializer.Serialize(stream, catalog);
            stream.Position = 0;

            return stream;
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