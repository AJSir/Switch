namespace Neovolve.Switch
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The <see cref="ISkinStore"/>
    ///   interface is used to manage a set of skins available from a storage location.
    /// </summary>
    [ContractClass(typeof(SkinStoreContracts))]
    public interface ISkinStore
    {
        /// <summary>
        /// Resolves the skins.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerable&lt;T&gt;"/> instance.
        /// </returns>
        IEnumerable<Skin> ResolveSkins();

        /// <summary>
        /// Stores the skin.
        /// </summary>
        /// <param name="skinPath">
        /// The skin path.
        /// </param>
        /// <returns>
        /// The new path of the skin.
        /// </returns>
        String StoreSkin(String skinPath);

        /// <summary>
        /// Gets a value indicating whether this instance is writable.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is writable; otherwise, <c>false</c>.
        /// </value>
        Boolean IsWritable
        {
            get;
        }
    }
}