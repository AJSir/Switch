namespace Neovolve.Switch
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Threading;
    using Neovolve.Toolkit.Threading;

    /// <summary>
    /// The <see cref="SafeObservableCollection&lt;T&gt;"/>
    ///   class is used to provide a thread safe implementation of the <see cref="ObservableCollection{T}"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of object held by the collection.
    /// </typeparam>
    [DebuggerDisplay("Count = {Count}")]
    [ComVisible(false)]
    public class SafeObservableCollection<T> : ObservableCollection<T>
    {
        /// <summary>
        /// The reader writer lock.
        /// </summary>
        private readonly ReaderWriterLockSlim _syncLock = new ReaderWriterLockSlim();

        /// <summary>
        /// The dispatcher.
        /// </summary>
        private Dispatcher _dispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeObservableCollection{T}"/> class.
        /// </summary>
        public SafeObservableCollection()
            : this(Enumerable.Empty<T>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeObservableCollection{T}"/> class.
        /// </summary>
        /// <param name="dispatcher">
        /// The dispatcher.
        /// </param>
        public SafeObservableCollection(Dispatcher dispatcher)
            : this(Enumerable.Empty<T>(), dispatcher)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeObservableCollection{T}"/> class.
        /// </summary>
        /// <param name="collection">
        /// The collection.
        /// </param>
        public SafeObservableCollection(IEnumerable<T> collection)
            : this(collection, Dispatcher.CurrentDispatcher)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeObservableCollection{T}"/> class.
        /// </summary>
        /// <param name="collection">
        /// The collection.
        /// </param>
        /// <param name="dispatcher">
        /// The dispatcher.
        /// </param>
        public SafeObservableCollection(IEnumerable<T> collection, Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;

            foreach (T item in collection)
            {
                Add(item);
            }
        }

        /// <summary>
        /// Sets the dispatcher.
        /// </summary>
        /// <param name="dispatcher">
        /// The dispatcher.
        /// </param>
        public void SetDispatcher(Dispatcher dispatcher)
        {
            Contract.Requires<ArgumentNullException>(dispatcher != null, "The dispatcher value is null.");

            _dispatcher = dispatcher;
        }

        /// <summary>
        /// The clear items.
        /// </summary>
        protected override void ClearItems()
        {
            using (new LockWriter(_syncLock))
            {
                ExecuteOrBeginInvoke(ClearItemsBase);
            }
        }

        /// <summary>
        /// The insert item.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <param name="item">
        /// The item.
        /// </param>
        protected override void InsertItem(Int32 index, T item)
        {
            using (new LockWriter(_syncLock))
            {
                ExecuteOrBeginInvoke(() => InsertItemBase(index, item));
            }
        }

        /// <summary>
        /// The move item.
        /// </summary>
        /// <param name="oldIndex">
        /// The old index.
        /// </param>
        /// <param name="newIndex">
        /// The new index.
        /// </param>
        protected override void MoveItem(Int32 oldIndex, Int32 newIndex)
        {
            using (new LockWriter(_syncLock))
            {
                ExecuteOrBeginInvoke(() => MoveItemBase(oldIndex, newIndex));
            }
        }

        /// <summary>
        /// The remove item.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        protected override void RemoveItem(Int32 index)
        {
            using (new LockWriter(_syncLock))
            {
                ExecuteOrBeginInvoke(() => RemoveItemBase(index));
            }
        }

        /// <summary>
        /// The set item.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <param name="item">
        /// The item.
        /// </param>
        protected override void SetItem(Int32 index, T item)
        {
            using (new LockWriter(_syncLock))
            {
                ExecuteOrBeginInvoke(() => SetItemBase(index, item));
            }
        }

        /// <summary>
        /// The clear items base.
        /// </summary>
        private void ClearItemsBase()
        {
            base.ClearItems();
        }

        /// <summary>
        /// The execute or begin invoke.
        /// </summary>
        /// <param name="action">
        /// The action.
        /// </param>
        private void ExecuteOrBeginInvoke(Action action)
        {
            if (_dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                _dispatcher.BeginInvoke(action);
            }
        }

        /// <summary>
        /// The insert item base.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <param name="item">
        /// The item.
        /// </param>
        private void InsertItemBase(Int32 index, T item)
        {
            base.InsertItem(index, item);
        }

        /// <summary>
        /// The move item base.
        /// </summary>
        /// <param name="oldIndex">
        /// The old index.
        /// </param>
        /// <param name="newIndex">
        /// The new index.
        /// </param>
        private void MoveItemBase(Int32 oldIndex, Int32 newIndex)
        {
            base.MoveItem(oldIndex, newIndex);
        }

        /// <summary>
        /// The remove item base.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        private void RemoveItemBase(Int32 index)
        {
            base.RemoveItem(index);
        }

        /// <summary>
        /// The set item base.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <param name="item">
        /// The item.
        /// </param>
        private void SetItemBase(Int32 index, T item)
        {
            base.SetItem(index, item);
        }
    }
}