namespace Neovolve.Switch
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The <see cref="SkinSet"/>
    ///   class is used to hold a collection of skins.
    /// </summary>
    public class SkinSet : SafeObservableCollection<Skin>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkinSet"/> class.
        /// </summary>
        /// <param name="stores">
        /// The stores.
        /// </param>
        public SkinSet(ISkinStore[] stores)
        {
            Contract.Requires<ArgumentNullException>(stores != null, "The stores value is null.");

            Stores = stores;
        }

        /// <summary>
        /// Loads the skins.
        /// </summary>
        public void LoadSkins()
        {
            Clear();

            for (Int32 index = 0; index < Stores.Length; index++)
            {
                ISkinStore store = Stores[index];

                foreach (Skin skin in store.ResolveSkins())
                {
                    Add(skin);
                }
            }
        }

        /// <summary>
        /// Gets or sets the stores.
        /// </summary>
        /// <value>
        /// The stores.
        /// </value>
        public ISkinStore[] Stores
        {
            get;
            set;
        }
    }
}