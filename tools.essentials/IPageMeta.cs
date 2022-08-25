using System;
using tools.Utility;

namespace tools
{
    /// <summary>
    /// The IPageMeta interface. 
    /// </summary>
    public interface IPageMeta
    {
        bool AddHtml5Defaults { get; set; }
        string PageTitle { get; set; }
        void AddKeyword(string value);
        string Keywords { get; }
        string Description { get; set; }
        bool RobotsIndex { get; set; }
        bool RobotsFollow { get; set; }
        Uri ThumbnailIconPath { get; set; }
        AssemblyInfo AppInfo { get; set; }
    }
}
