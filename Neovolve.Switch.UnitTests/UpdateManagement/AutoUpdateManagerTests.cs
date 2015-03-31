namespace Neovolve.Switch.UnitTests.UpdateManagement
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Switch.Extensibility.ResourceManagement;
    using Neovolve.Switch.Extensibility.Services;
    using Neovolve.Switch.UpdateManagement;
    using Rhino.Mocks;

    /// <summary>
    /// The <see cref="AutoUpdateManagerTests"/>
    ///   class is used to test the <see cref="AutoUpdateManager"/> class.
    /// </summary>
    [TestClass]
    public class AutoUpdateManagerTests
    {
        /// <summary>
        /// Runs test for does not execute updater when disabled.
        /// </summary>
        [TestMethod]
        public void DoesNotExecuteUpdaterWhenDisabledTest()
        {
            AutoUpdateSettings settings = new AutoUpdateSettings
                                          {
                                              Enabled = false
                                          };
            IDownloadManager downloadManager = MockRepository.GenerateStub<IDownloadManager>();
            ILogWriter logWriter = MockRepository.GenerateStub<ILogWriter>();
            IUserNotification userNotification = MockRepository.GenerateStub<IUserNotification>();
            IPackageInstaller installer = MockRepository.GenerateStub<IPackageInstaller>();
            AutoUpdater updater = MockRepository.GenerateStub<AutoUpdater>(downloadManager, logWriter, userNotification, settings, installer);
            AutoUpdateManager target = new AutoUpdateManager(settings, updater);

            target.OnStarting();

            WaitRetryAction.Execute(() => target.IsCompleted, x => x);

            updater.AssertWasNotCalled(x => x.Execute());
        }

        /// <summary>
        /// Runs test for does not execute updater when frequency is per day and time has not elapsed.
        /// </summary>
        [TestMethod]
        public void DoesNotExecuteUpdaterWhenFrequencyIsPerDayAndTimeHasNotElapsedTest()
        {
            AutoUpdateSettings settings = new AutoUpdateSettings
                                          {
                                              Enabled = true, 
                                              CheckFrequency = UpdateFrequency.EachDay, 
                                              LastChecked = DateTime.UtcNow.AddHours(-23)
                                          };
            IDownloadManager downloadManager = MockRepository.GenerateStub<IDownloadManager>();
            ILogWriter logWriter = MockRepository.GenerateStub<ILogWriter>();
            IUserNotification userNotification = MockRepository.GenerateStub<IUserNotification>();
            IPackageInstaller installer = MockRepository.GenerateStub<IPackageInstaller>();
            AutoUpdater updater = MockRepository.GenerateStub<AutoUpdater>(downloadManager, logWriter, userNotification, settings, installer);
            AutoUpdateManager target = new AutoUpdateManager(settings, updater);

            target.OnStarting();

            WaitRetryAction.Execute(() => target.IsCompleted, x => x);

            updater.AssertWasNotCalled(x => x.Execute());
        }

        /// <summary>
        /// Runs test for does not execute updater when frequency is per month and time has not elapsed.
        /// </summary>
        [TestMethod]
        public void DoesNotExecuteUpdaterWhenFrequencyIsPerMonthAndTimeHasNotElapsedTest()
        {
            AutoUpdateSettings settings = new AutoUpdateSettings
                                          {
                                              Enabled = true, 
                                              CheckFrequency = UpdateFrequency.EachMonth, 
                                              LastChecked = DateTime.UtcNow.AddDays(-25)
                                          };
            IDownloadManager downloadManager = MockRepository.GenerateStub<IDownloadManager>();
            ILogWriter logWriter = MockRepository.GenerateStub<ILogWriter>();
            IUserNotification userNotification = MockRepository.GenerateStub<IUserNotification>();
            IPackageInstaller installer = MockRepository.GenerateStub<IPackageInstaller>();
            AutoUpdater updater = MockRepository.GenerateStub<AutoUpdater>(downloadManager, logWriter, userNotification, settings, installer);
            AutoUpdateManager target = new AutoUpdateManager(settings, updater);

            target.OnStarting();

            WaitRetryAction.Execute(() => target.IsCompleted, x => x);

            updater.AssertWasNotCalled(x => x.Execute());
        }

        /// <summary>
        /// Runs test for does not execute updater when frequency is per week and time has not elapsed.
        /// </summary>
        [TestMethod]
        public void DoesNotExecuteUpdaterWhenFrequencyIsPerWeekAndTimeHasNotElapsedTest()
        {
            AutoUpdateSettings settings = new AutoUpdateSettings
                                          {
                                              Enabled = true, 
                                              CheckFrequency = UpdateFrequency.EachWeek, 
                                              LastChecked = DateTime.UtcNow.AddDays(-6)
                                          };
            IDownloadManager downloadManager = MockRepository.GenerateStub<IDownloadManager>();
            ILogWriter logWriter = MockRepository.GenerateStub<ILogWriter>();
            IUserNotification userNotification = MockRepository.GenerateStub<IUserNotification>();
            IPackageInstaller installer = MockRepository.GenerateStub<IPackageInstaller>();
            AutoUpdater updater = MockRepository.GenerateStub<AutoUpdater>(downloadManager, logWriter, userNotification, settings, installer);
            AutoUpdateManager target = new AutoUpdateManager(settings, updater);

            target.OnStarting();

            WaitRetryAction.Execute(() => target.IsCompleted, x => x);

            updater.AssertWasNotCalled(x => x.Execute());
        }

        /// <summary>
        /// Runs test for executes updater when frequency is on start.
        /// </summary>
        [TestMethod]
        public void ExecutesUpdaterWhenFrequencyIsOnStartTest()
        {
            AutoUpdateSettings settings = new AutoUpdateSettings
                                          {
                                              Enabled = true, 
                                              CheckFrequency = UpdateFrequency.OnStart, 
                                              LastChecked = DateTime.UtcNow.AddYears(-1)
                                          };
            IDownloadManager downloadManager = MockRepository.GenerateStub<IDownloadManager>();
            ILogWriter logWriter = MockRepository.GenerateStub<ILogWriter>();
            IUserNotification userNotification = MockRepository.GenerateStub<IUserNotification>();
            IPackageInstaller installer = MockRepository.GenerateStub<IPackageInstaller>();
            AutoUpdater updater = MockRepository.GenerateStub<AutoUpdater>(downloadManager, logWriter, userNotification, settings, installer);
            AutoUpdateManager target = new AutoUpdateManager(settings, updater);

            target.OnStarting();

            WaitRetryAction.Execute(() => target.IsCompleted, x => x);

            updater.AssertWasCalled(x => x.Execute());
        }

        /// <summary>
        /// Runs test for executes updater when frequency is per day and time has elapsed.
        /// </summary>
        [TestMethod]
        public void ExecutesUpdaterWhenFrequencyIsPerDayAndTimeHasElapsedTest()
        {
            AutoUpdateSettings settings = new AutoUpdateSettings
                                          {
                                              Enabled = true, 
                                              CheckFrequency = UpdateFrequency.EachDay, 
                                              LastChecked = DateTime.UtcNow.AddHours(-25)
                                          };
            IDownloadManager downloadManager = MockRepository.GenerateStub<IDownloadManager>();
            ILogWriter logWriter = MockRepository.GenerateStub<ILogWriter>();
            IUserNotification userNotification = MockRepository.GenerateStub<IUserNotification>();
            IPackageInstaller installer = MockRepository.GenerateStub<IPackageInstaller>();
            AutoUpdater updater = MockRepository.GenerateStub<AutoUpdater>(downloadManager, logWriter, userNotification, settings, installer);
            AutoUpdateManager target = new AutoUpdateManager(settings, updater);

            target.OnStarting();

            WaitRetryAction.Execute(() => target.IsCompleted, x => x);

            updater.AssertWasCalled(x => x.Execute());
        }

        /// <summary>
        /// Runs test for executes updater when frequency is per month and time has elapsed.
        /// </summary>
        [TestMethod]
        public void ExecutesUpdaterWhenFrequencyIsPerMonthAndTimeHasElapsedTest()
        {
            AutoUpdateSettings settings = new AutoUpdateSettings
                                          {
                                              Enabled = true, 
                                              CheckFrequency = UpdateFrequency.EachMonth, 
                                              LastChecked = DateTime.UtcNow.AddDays(-32)
                                          };
            IDownloadManager downloadManager = MockRepository.GenerateStub<IDownloadManager>();
            ILogWriter logWriter = MockRepository.GenerateStub<ILogWriter>();
            IUserNotification userNotification = MockRepository.GenerateStub<IUserNotification>();
            IPackageInstaller installer = MockRepository.GenerateStub<IPackageInstaller>();
            AutoUpdater updater = MockRepository.GenerateStub<AutoUpdater>(downloadManager, logWriter, userNotification, settings, installer);
            AutoUpdateManager target = new AutoUpdateManager(settings, updater);

            target.OnStarting();

            WaitRetryAction.Execute(() => target.IsCompleted, x => x);

            updater.AssertWasCalled(x => x.Execute());
        }

        /// <summary>
        /// Runs test for executes updater when frequency is per week and time has elapsed.
        /// </summary>
        [TestMethod]
        public void ExecutesUpdaterWhenFrequencyIsPerWeekAndTimeHasElapsedTest()
        {
            AutoUpdateSettings settings = new AutoUpdateSettings
                                          {
                                              Enabled = true, 
                                              CheckFrequency = UpdateFrequency.EachWeek, 
                                              LastChecked = DateTime.UtcNow.AddDays(-8)
                                          };
            IDownloadManager downloadManager = MockRepository.GenerateStub<IDownloadManager>();
            ILogWriter logWriter = MockRepository.GenerateStub<ILogWriter>();
            IUserNotification userNotification = MockRepository.GenerateStub<IUserNotification>();
            IPackageInstaller installer = MockRepository.GenerateStub<IPackageInstaller>();
            AutoUpdater updater = MockRepository.GenerateStub<AutoUpdater>(downloadManager, logWriter, userNotification, settings, installer);
            AutoUpdateManager target = new AutoUpdateManager(settings, updater);

            target.OnStarting();

            WaitRetryAction.Execute(() => target.IsCompleted, x => x);

            updater.AssertWasCalled(x => x.Execute());
        }

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