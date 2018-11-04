using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Globalization;
using System.Linq;

/// <summary>
/// TagHelper classes to automate some tasks.
/// </summary>
namespace mezzanine.TagHelpers
{
    /// <summary>
    /// Write out all the meta tags for a page.
    /// Requires ViewModelModels.PageMeta.
    /// </summary>
    [HtmlTargetElement("meta")]
    public class MetaTagHelper : TagHelper
    {
        private const string _defaultRobots = @"noindex, nofollow";
        private const string _defaultCharset = @"utf-8";
        private const string _defaultXUA = @"IE=edge,chrome=1";

        private CultureInfo Culture { get; set; } = null;

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
               
        public IPageMeta MetaModel { get; set; }

        public string MetaUserCultureName { get; set; } = string.Empty;

        private TagBuilder CreateSelfClosingMeta()
        {
            TagBuilder tb = new TagBuilder(@"meta") { TagRenderMode = TagRenderMode.SelfClosing};
            return tb;
        }

        /// <summary>
        /// Append more tags to the target.
        /// </summary>
        /// <param name="targetTag"></param>
        private void AppendHtml5Defaults(ref TagHelperOutput output, ref TagBuilder targetTag)
        {
            TagBuilder tmpTag = null;

            // The order of the items is important see: http://getbootstrap.com/docs/3.3/getting-started/

            //< meta charset = "utf-8" />
            output.Attributes.Add("charset", _defaultCharset); // note this is applied to the output

            //< meta http-equiv = "X-UA-Compatible" content = "IE=edge" />
            tmpTag = this.CreateSelfClosingMeta();
            tmpTag.Attributes.Add("http-equiv", "X-UA-Compatible");
            tmpTag.Attributes.Add("content", _defaultXUA);
            targetTag.InnerHtml.AppendHtml(tmpTag);            

            //< meta name = "viewport" content = "width=device-width, initial-scale=1" />
            tmpTag = this.CreateSelfClosingMeta();
            tmpTag.Attributes.Add("name", "viewport");
            tmpTag.Attributes.Add("content", "width=device-width, initial-scale=1, minimum-scale=0.5, maximum-scale=1.5, user-scalable=yes, shrink-to-fit=no");
            targetTag.InnerHtml.AppendHtml(tmpTag);

            //< meta http - equiv = "content-type" content = "text/html; charset=UTF-8" >
            tmpTag = this.CreateSelfClosingMeta();
            tmpTag.Attributes.Add("http-equiv", "content-type");
            tmpTag.Attributes.Add("content", "text/html; charset=" + _defaultCharset.ToUpper());
            targetTag.InnerHtml.AppendHtml(tmpTag);
        }

        private void AppendAppInfo(ref TagBuilder targetTag, mezzanine.Utility.AssemblyInfo assemblyInfo)
        {
            string pageKeywords = this.MetaModel.AppInfo.Title;
            TagBuilder tmpTag = null;

            if (this.MetaModel.Keywords.Count() > 0)
            {
                pageKeywords += this.Culture.TextInfo.ListSeparator + this.MetaModel.Keywords;
            }

            //< meta name = "keywords" content = "@defaultKeywords@this.UserCulture.TextInfo.ListSeparator @Model.Keywords" />
            tmpTag = this.CreateSelfClosingMeta();
            tmpTag.Attributes.Add("name", "keywords");
            tmpTag.Attributes.Add("content", pageKeywords);
            targetTag.InnerHtml.AppendHtml(tmpTag);

            tmpTag = this.CreateSelfClosingMeta();
            tmpTag.Attributes.Add("name", "application-name");
            tmpTag.Attributes.Add("content", this.MetaModel.AppInfo.Title);
            targetTag.InnerHtml.AppendHtml(tmpTag);

            //< meta name = "copyright" content = "Copyright, @this.AppInfo.Title @DateTime.UtcNow.ToString(base.UserCulture.DateTimeFormat.LongDatePattern, base.UserCulture). All Rights Reserved." />
            tmpTag = this.CreateSelfClosingMeta();
            tmpTag.Attributes.Add("name", "copyright");
            tmpTag.Attributes.Add("content", "Copyright" + this.Culture.TextInfo.ListSeparator + " " + this.MetaModel.AppInfo.Title + " - " + this.MetaModel.PageTitle + " " + DateTime.UtcNow.ToString(this.Culture.DateTimeFormat.LongDatePattern, this.Culture) + ". All Rights Reserved.");
            targetTag.InnerHtml.AppendHtml(tmpTag);

            //< meta name = "description" content = "@this.AppInfo.Description - @Model.Description" />
            tmpTag = this.CreateSelfClosingMeta();
            tmpTag.Attributes.Add("name", "description");
            tmpTag.Attributes.Add("content", this.MetaModel.Description);
            targetTag.InnerHtml.AppendHtml(tmpTag);

            // < meta name = "publisher" content = "@this.AppInfo.Company - @this.AppInfo.Trademark" />
            string publisherInfo = this.MetaModel.AppInfo.Company;
            if (this.MetaModel.AppInfo.Trademark != string.Empty)
            {
                publisherInfo += " - " + this.MetaModel.AppInfo.Trademark;
            }

            tmpTag = this.CreateSelfClosingMeta();
            tmpTag.Attributes.Add("name", "publisher");
            tmpTag.Attributes.Add("content", publisherInfo);
            targetTag.InnerHtml.AppendHtml(tmpTag);

            //< meta name = "author" content = "@this.AppInfo.Company" />
            tmpTag = this.CreateSelfClosingMeta();
            tmpTag.Attributes.Add("name", "author");
            tmpTag.Attributes.Add("content", this.MetaModel.AppInfo.Company);
            targetTag.InnerHtml.AppendHtml(tmpTag);

        }

        private void AppendIcons(ref TagBuilder targetTag)
        {
            TagBuilder tmpTag = null;

            //< meta name = "thumbnail" content = "~/images/favicon.ico" />
            if (this.MetaModel.ThumbnailIconPath == null)
            {
                HttpRequest request = this.ViewContext?.HttpContext?.Request;

                if (request != null)
                {                    
                    // Using conventional path (the default is ~/favicon.ico)
                    this.MetaModel.ThumbnailIconPath = new Uri(request.Scheme + "://" + request.Host + "/images/favicon.ico");                    
                }
            }

            if (this.MetaModel.ThumbnailIconPath != null)
            {
                tmpTag = this.CreateSelfClosingMeta();
                tmpTag.Attributes.Add("name", "thumbnail");
                tmpTag.Attributes.Add("content", this.MetaModel.ThumbnailIconPath.AbsoluteUri);
                targetTag.InnerHtml.AppendHtml(tmpTag);

                //< link rel = "icon" href = "~/images/favicon.ico" type = "image/x-icon" />
                tmpTag = this.CreateSelfClosingMeta();
                tmpTag.Attributes.Add("rel", "icon");
                tmpTag.Attributes.Add("href", this.MetaModel.ThumbnailIconPath.LocalPath);
                tmpTag.Attributes.Add("type", "image/x-icon");
                targetTag.InnerHtml.AppendHtml(tmpTag);
            }
        }

        private void AppendRobots(ref TagBuilder targetTag)
        {
            // The robots are appended to the first element. It has to go somewhere!
            string robots = _defaultRobots;
            
            if (this.MetaModel.RobotsIndex == true)
            {
                robots = "index";
                if (this.MetaModel.RobotsFollow == true)
                {
                    robots += ", follow";
                }
                else
                {
                    robots += ", nofollow";
                }
            }
            else
            {
                robots = "noindex";
                if (this.MetaModel.RobotsFollow == true)
                {
                    robots += ", follow";
                }
                else
                {
                    robots += ", nofollow";
                }
            }

            targetTag.Attributes.Add("name", "robots");
            targetTag.Attributes.Add("content", robots);
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            TagBuilder result = new TagBuilder("head"); // fake head, only the inner html will be returned.
            TagBuilder tmpTag = null;
            
            if (this.MetaModel == null)
            {
                throw new ArgumentNullException("The meta data for the page has not been defined.");
            }

            if (this.MetaModel.AddHtml5Defaults == true)
            {
                this.AppendHtml5Defaults(ref output, ref result);
            }

            //< meta name = "language" content = "@this.UserCulture.Name" />       
            if (this.MetaUserCultureName == string.Empty )
            {
                this.MetaUserCultureName = CultureInfo.CurrentUICulture.Name.ToLower();
            }

            this.Culture = new CultureInfo(this.MetaUserCultureName);

            tmpTag = this.CreateSelfClosingMeta();
            tmpTag.Attributes.Add("name", "language");
            tmpTag.Attributes.Add("content", this.Culture.Name.ToLower());
            result.InnerHtml.AppendHtml(tmpTag);

            //< meta name = "application-name" content = "@this.AppInfo.Title" />
            if (this.MetaModel.AppInfo == null)
            {
                this.MetaModel.AppInfo = new mezzanine.Utility.AssemblyInfo(System.Reflection.Assembly.GetEntryAssembly());
            }

            // start app info
            this.AppendAppInfo(ref result, this.MetaModel.AppInfo);

            // < title > @this.AppInfo.Title - @Model.Title </ title >
            tmpTag = new TagBuilder("Title") { TagRenderMode = TagRenderMode.Normal };
            tmpTag.InnerHtml.Append(this.MetaModel.AppInfo.Title + " - " + this.MetaModel.PageTitle);
            result.InnerHtml.AppendHtml(tmpTag);

            this.AppendIcons(ref result);
            this.AppendRobots(ref result);

            if (result != null)
            {
                output.PostElement.AppendHtml(result.InnerHtml);
            }

            tmpTag = null;
        }
    }
}
