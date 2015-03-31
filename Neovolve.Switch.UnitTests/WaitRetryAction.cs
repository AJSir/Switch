namespace Neovolve.Switch.UnitTests
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Threading;

    /// <summary>
    /// The <see cref="WaitRetryAction"/>
    ///   class is used to provide the support for retrying an action until an expected result or exit condition is met.
    /// </summary>
    public static class WaitRetryAction
    {
        /// <summary>
        /// Deefins the default maximum number of attempts.
        /// </summary>
        public const Int32 DefaultMaxAttempts = 2147483647;

        /// <summary>
        /// Defines the default retry delay in milliseconds.
        /// </summary>
        public const Int32 DefaultMillisecondRetryDelay = 50;

        /// <summary>
        /// Defines the default timeout in milliseconds.
        /// </summary>
        public const Int32 DefaultMillisecondTimeout = 15000;

        /// <summary>
        /// Executes the specified execute action.
        /// </summary>
        /// <typeparam name="T">
        /// The type of object returned.
        /// </typeparam>
        /// <param name="executeAction">
        /// The execute action.
        /// </param>
        /// <param name="resultEvaluator">
        /// The result evaluator.
        /// </param>
        /// <returns>
        /// A <typeparamref name="T"/> instance.
        /// </returns>
        public static T Execute<T>(Func<T> executeAction, Func<T, Boolean> resultEvaluator)
        {
            return Execute(executeAction, resultEvaluator, DefaultMillisecondRetryDelay, DefaultMillisecondTimeout, DefaultMaxAttempts);
        }

        /// <summary>
        /// Executes the specified execute action.
        /// </summary>
        /// <typeparam name="T">
        /// The type of object returned.
        /// </typeparam>
        /// <param name="executeAction">
        /// The execute action.
        /// </param>
        /// <param name="resultEvaluator">
        /// The result evaluator.
        /// </param>
        /// <param name="millisecondRetryDelay">
        /// The millisecond retry delay.
        /// </param>
        /// <returns>
        /// A <typeparamref name="T"/> instance.
        /// </returns>
        public static T Execute<T>(Func<T> executeAction, Func<T, Boolean> resultEvaluator, Int32 millisecondRetryDelay)
        {
            return Execute(executeAction, resultEvaluator, millisecondRetryDelay, DefaultMillisecondTimeout, DefaultMaxAttempts);
        }

        /// <summary>
        /// Executes the specified execute action.
        /// </summary>
        /// <typeparam name="T">
        /// The type of object returned.
        /// </typeparam>
        /// <param name="executeAction">
        /// The execute action.
        /// </param>
        /// <param name="resultEvaluator">
        /// The result evaluator.
        /// </param>
        /// <param name="millisecondRetryDelay">
        /// The millisecond retry delay.
        /// </param>
        /// <param name="millisecondTimeout">
        /// The millisecond timeout.
        /// </param>
        /// <returns>
        /// A <typeparamref name="T"/> instance.
        /// </returns>
        public static T Execute<T>(Func<T> executeAction, Func<T, Boolean> resultEvaluator, Int32 millisecondRetryDelay, Int32 millisecondTimeout)
        {
            return Execute(executeAction, resultEvaluator, millisecondRetryDelay, millisecondTimeout, DefaultMaxAttempts);
        }

        /// <summary>
        /// Executes the specified execute action.
        /// </summary>
        /// <typeparam name="T">
        /// The type of object returned.
        /// </typeparam>
        /// <param name="executeAction">
        /// The execute action.
        /// </param>
        /// <param name="resultEvaluator">
        /// The result evaluator.
        /// </param>
        /// <param name="millisecondRetryDelay">
        /// The millisecond retry delay.
        /// </param>
        /// <param name="millisecondTimeout">
        /// The millisecond timeout.
        /// </param>
        /// <param name="maxAttempts">
        /// The max attempts.
        /// </param>
        /// <returns>
        /// A <typeparamref name="T"/> instance.
        /// </returns>
        public static T Execute<T>(
            Func<T> executeAction, Func<T, Boolean> resultEvaluator, Int32 millisecondRetryDelay, Int32 millisecondTimeout, Int32 maxAttempts)
        {
            Contract.Requires<ArgumentNullException>(executeAction != null, "The executeAction value is null.");
            Contract.Requires<ArgumentNullException>(resultEvaluator != null, "The resultEvaluator value is null.");

            Stopwatch timer = Stopwatch.StartNew();
            Int32 attemptsMade = 0;
            T lastResult;

            do
            {
                lastResult = executeAction();

                if (resultEvaluator(lastResult))
                {
                    Debug.WriteLine("WaitRetryAction found a valid result on attempt " + attemptsMade + " of " + maxAttempts);

                    return lastResult;
                }

                // Delay
                if (millisecondRetryDelay > 0)
                {
                    using (ManualResetEvent waitHandle = new ManualResetEvent(false))
                    {
                        waitHandle.WaitOne(millisecondRetryDelay);
                    }
                }

                attemptsMade++;
            }
 while (timer.ElapsedMilliseconds < millisecondTimeout && attemptsMade < maxAttempts);

            Boolean maximumAttemptsReached = attemptsMade < maxAttempts;

            if (maximumAttemptsReached)
            {
                Debug.WriteLine("WaitRetryAction failed to find a valid result and all " + maxAttempts + " attempts have been made");
            }
            else
            {
                Debug.WriteLine("WaitRetryAction failed to find a valid result and a " + millisecondTimeout + " millisecond timeout has now occurred");
            }

            return lastResult;
        }
    }
}