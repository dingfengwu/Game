using Game.Web.Models.Common;

namespace Game.Web.Factories
{
    /// <summary>
    /// Represents the interface of the common models factory
    /// </summary>
    public partial interface ICommonModelFactory
    {
        SignUpOrInModel PrepareSignUpOrInModel();
        /// <summary>
        /// Prepare the sitemap model
        /// </summary>
        /// <returns>Sitemap model</returns>
        SitemapModel PrepareSitemapModel();
        /// <summary>
        /// Prepare the logo model
        /// </summary>
        /// <returns>Logo model</returns>
        LogoModel PrepareLogoModel();
        /// <summary>
        /// Prepare the language selector model
        /// </summary>
        /// <returns>Language selector model</returns>
        LanguageSelectorModel PrepareLanguageSelectorModel();
        /// <summary>
        /// Prepare top menu model
        /// </summary>
        /// <returns>Top menu model</returns>
        TopMenuModel PrepareTopMenuModel();
        /// <summary>
        /// Prepare the footer model
        /// </summary>
        /// <returns>Footer model</returns>
        FooterModel PrepareFooterModel();
        /// <summary>
        /// Prepare the admin header links model
        /// </summary>
        /// <returns>Admin header links model</returns>
        AdminHeaderLinksModel PrepareAdminHeaderLinksModel();

        /// <summary>
        /// Get the sitemap in XML format
        /// </summary>
        /// <param name="id">Sitemap identifier; pass null to load the first sitemap or sitemap index file</param>
        /// <returns>Sitemap as string in XML format</returns>
        string PrepareSitemapXml( int? id);
        
        /// <summary>
        /// Prepare the favicon model
        /// </summary>
        /// <returns>Favicon model</returns>
        FaviconModel PrepareFaviconModel();

        /// <summary>
        /// Get robots.txt file
        /// </summary>
        /// <returns>Robots.txt file as string</returns>
        string PrepareRobotsTextFile();
    }
}
