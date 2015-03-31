namespace Neovolve.Switch.Controls
{
    using System;
    using System.Diagnostics;
    using System.Windows.Documents;

    /// <summary>
    /// The <see cref="ClickableLink"/>
    ///   class is a WPF control that extends <see cref="Hyperlink"/> to provide the click implementation.
    /// </summary>
    public class ClickableLink : Hyperlink
    {
        /// <summary>
        /// Handles the <see cref="E:System.Windows.Documents.Hyperlink.Click"/> routed event.
        /// </summary>
        protected override void OnClick()
        {
            base.OnClick();

            Uri navigateUri = ResolveAddressValue(NavigateUri);

            if (navigateUri == null)
            {
                return;
            }

            String address = navigateUri.ToString();

            ProcessStartInfo startInfo = new ProcessStartInfo(address);
            
            Process.Start(startInfo);
        }
        
        /// <summary>
        /// Resolves the address value.
        /// </summary>
        /// <param name="navigateUri">
        /// The navigate URI.
        /// </param>
        /// <returns>
        /// A <see cref="Uri"/> instance.
        /// </returns>
        private static Uri ResolveAddressValue(Uri navigateUri)
        {
            if (navigateUri == null)
            {
                return null;
            }

            // Disallow file urls
            if (navigateUri.IsAbsoluteUri)
            {
                if (navigateUri.IsFile)
                {
                    return null;
                }

                if (navigateUri.IsUnc)
                {
                    return null;
                }
            }

            String address = navigateUri.ToString();

            if (String.IsNullOrWhiteSpace(address))
            {
                return null;
            }

            if (address.Contains("@") && address.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase) == false)
            {
                address = "mailto:" + address;
            }
            else if (address.StartsWith("http://", StringComparison.OrdinalIgnoreCase) == false &&
                     address.StartsWith("https://", StringComparison.OrdinalIgnoreCase) == false)
            {
                address = "http://" + address;
            }

            try
            {
                return new Uri(address);
            }
            catch (UriFormatException)
            {
                return null;
            }
        }
    }
}