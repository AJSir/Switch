namespace Neovolve.Switch.Extensibility.UnitTests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Switch.Extensibility.Services;

    /// <summary>
    /// The <see cref="ServiceManagerTests"/>
    ///   class is used to test the <see cref="ServiceManager"/> class.
    /// </summary>
    [TestClass]
    public class ServiceManagerTests
    {
        /// <summary>
        /// Runs test for get service returns null for unknown resolution.
        /// </summary>
        [TestMethod]
        public void GetServiceReturnsNullForUnknownResolutionTest()
        {
            ITestMethodInvoker actual = ServiceManager.GetService<ITestMethodInvoker>();

            Assert.IsNull(actual, "GetService returned an incorrect value");
        }

        /// <summary>
        /// Runs test for get service returns service instance for known resolution.
        /// </summary>
        [TestMethod]
        public void GetServiceReturnsServiceInstanceForKnownResolutionTest()
        {
            IApplicationNotification actual = ServiceManager.GetService<IApplicationNotification>();

            Assert.IsNotNull(actual, "GetSevice returned an incorrect value");
        }

        /// <summary>
        /// Runs test for get services returns empty set for unknown resolution.
        /// </summary>
        [TestMethod]
        public void GetServicesReturnsEmptySetForUnknownResolutionTest()
        {
            IEnumerable<ITestMethodInvoker> actual = ServiceManager.GetServices<ITestMethodInvoker>();

            Assert.IsNotNull(actual, "GetServices returned an incorrect value");
            Assert.AreEqual(0, actual.Count(), "Count returned an incorrect value");
        }

        /// <summary>
        /// Runs test for get services returns set for known resolution.
        /// </summary>
        [TestMethod]
        public void GetServicesReturnsSetForKnownResolutionTest()
        {
            IEnumerable<IApplicationNotification> actual = ServiceManager.GetServices<IApplicationNotification>();

            Assert.IsNotNull(actual, "GetServices returned an incorrect value");
            Assert.AreNotEqual(0, actual.Count(), "Count returned an incorrect value");
        }

        /// <summary>
        /// Runs test for release service ignores non disposable types.
        /// </summary>
        [TestMethod]
        public void ReleaseServiceIgnoresNonDisposableTypesTest()
        {
            Object actual = new Object();

            ServiceManager.ReleaseService(actual);
        }

        /// <summary>
        /// Runs test for release service invokes dispose on disposable instances.
        /// </summary>
        [TestMethod]
        public void ReleaseServiceInvokesDisposeOnDisposableInstancesTest()
        {
            DisposableApplicationNotification actual = ServiceManager.GetService<IApplicationNotification>() as DisposableApplicationNotification;

            ServiceManager.ReleaseService(actual);

            Assert.IsTrue(actual.Disposed, "Disposed returned an incorrect value");
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