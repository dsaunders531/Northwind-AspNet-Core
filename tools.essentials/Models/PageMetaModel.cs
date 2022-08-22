namespace tools.Models
{
    /// <summary>
    /// The page meta model for defining the page meta tags using the meta tag helper.
    /// </summary>
    public sealed class PageMetaModel : PageMeta
    {
        public PageMetaModel()
        {
            // Set the defaults
            AddHtml5Defaults = true;
            AppInfo = new tools.Utility.AssemblyInfo();
            RobotsFollow = true;
            RobotsIndex = true;
        }
    }
}
