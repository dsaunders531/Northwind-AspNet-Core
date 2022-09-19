using tools.Utility;
using System;
using System.Collections.Generic;

namespace tools.Models
{
    /// <summary>
    /// A class to hold information about the header data to accompany the page.
    /// Required by the Metas TagHelper.
    /// Inherit this to create an object with defaults in your application.
    /// </summary>
    public abstract class PageMeta : IPageMeta
    {
        private List<string> KeywordsList { get; set; } = new List<string>();

        public bool AddHtml5Defaults { get; set; } = true;

        public string PageTitle { get; set; }

        public void AddKeyword(string value)
        {
            this.KeywordsList.Add(value);
        }

        public string Keywords
        {
            get
            {
                string allKeywords = string.Empty;

                // ??? Different cultures (eg: dutch) use ; as a list seperator char. 
                // does this need to be used as keywork seperator in the header?
                foreach (string k in KeywordsList)
                {
                    if (k != string.Empty)
                    {
                        allKeywords += k + ",";
                    }
                }

                if (allKeywords.EndsWith(",") == true)
                {
                    allKeywords = allKeywords.Substring(0, allKeywords.Length - 1);
                }

                return allKeywords;
            }
        }

        public string Description { get; set; }

        public bool RobotsIndex { get; set; }

        public bool RobotsFollow { get; set; }

        public Uri ThumbnailIconPath { get; set; }

        public AssemblyInfo AppInfo { get; set; }
    }
}
