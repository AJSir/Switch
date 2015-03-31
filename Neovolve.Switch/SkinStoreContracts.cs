namespace Neovolve.Switch
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;

    /// <summary>
    /// The <see cref="SkinStoreContracts"/>
    ///   class is used to define code contracts for the <see cref="ISkinStore"/> interface.
    /// </summary>
    [ContractClassFor(typeof(ISkinStore))]
    internal abstract class SkinStoreContracts : ISkinStore
    {
        /// <summary>
        /// Resolves the skins.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerable&lt;T&gt;"/> instance.
        /// </returns>
        public IEnumerable<Skin> ResolveSkins()
        {
            return null;
        }

        /// <summary>
        /// Stores the skin.
        /// </summary>
        /// <param name="skinPath">
        /// The skin path.
        /// </param>
        /// <returns>
        /// The new path of the skin.
        /// </returns>
        public String StoreSkin(String skinPath)
        {
            Contract.Requires<ArgumentNullException>(
                String.IsNullOrWhiteSpace(skinPath) == false, "The skinData value is null, empty or only contains whitespace.");
            Contract.Requires<FileNotFoundException>(File.Exists(skinPath), "The specified skin path does not exist.");

            return null;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is writable.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is writable; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsWritable
        {
            get
            {
                return false;
            }
        }
    }
}