namespace Neovolve.Switch
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// The <see cref="DefaultSkinStore"/>
    ///   class is used to return the default skin compiled into the application resources.
    /// </summary>
    public class DefaultSkinStore : ISkinStore
    {
        /// <summary>
        /// Resolves the skins.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerable&lt;T&gt;"/> instance.
        /// </returns>
        public IEnumerable<Skin> ResolveSkins()
        {
            Byte[] skinData = Properties.Resources.DefaultSkin;

            using (MemoryStream skinStream = new MemoryStream(skinData))
            {
                Skin defaultSkin = new Skin(skinStream);

                defaultSkin.DisplayName = "Default - " + defaultSkin.DisplayName;

                yield return defaultSkin;
            }
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
            throw new NotSupportedException();
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