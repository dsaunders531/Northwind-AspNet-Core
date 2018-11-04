using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using mezzanine;

namespace mezzanine.TagHelpers
{
    /// <summary>
    /// The pagianation tag helper for splitting lists
    /// Mostly lifted from Pro ASP.Net Core MVC Alan Freeman.
    /// Requires ViewModelModels.PagingInfo
    /// </summary>
    [HtmlTargetElement("ul", Attributes = "page-model")]
    public sealed class PagianationTagHelper : TagHelper // : TagHelpersBase
    {
        private IUrlHelperFactory _urlHelperFactory;

        private string _pageAction = string.Empty;

        public PagianationTagHelper(IUrlHelperFactory helperFactory)
        {
            this._urlHelperFactory = helperFactory;
        }

        public IPagination PageModel { get; set; }

        #region "Bootstrap support"
        public string PageClass { get; set; } = "pagination";

        public string PageClassNormal { get; set; }

        public string PageClassSelected { get; set; } = "active";

        public bool PageShowPreviousNext { get; set; } = true;

        public string PagePreviousText { get; set; } = "Previous";

        public string PageNextText { get; set; } = "Next";
        #endregion

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        /// <summary>
        /// Add a link to the specified element
        /// </summary>
        /// <param name="targetTag"></param>
        /// <param name="url"></param>
        /// <param name="cssClass"></param>
        /// <returns></returns>
        private TagBuilder AddLink(ref TagBuilder targetTag, string hrefUrl, string linkText, string cssClass)
        {
            TagBuilder linkTag = new TagBuilder("a");

            linkTag.Attributes["href"] = hrefUrl.URLDecode().HTMLDecode();
            linkTag.InnerHtml.Append(linkText);
            linkTag.AddCssClass(cssClass);

            targetTag.InnerHtml.AppendHtml(linkTag);

            linkTag = null;

            return targetTag;
        }

        // This is what I am aiming for
        //<ul class="pagination">
        //    <li class="previous"><a href = "#" > Previous </a ></li>
        //            <li class="active"><a href = "#" > 1 </a ></li>
        //            <li class=""><a href = "#" > 2 </a></li>
        //      <li class="next"><a href = "#" > Next </a></li>
        //   </ ul >
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
            TagBuilder result = null;
            TagBuilder tmpTag = null;

            if (this.PageModel.PageCount() > 1)
            {
                result = new TagBuilder("ul");
                result.AddCssClass(this.PageClass);

                // Add the previous button
                if (this.PageShowPreviousNext == true)
                {
                    tmpTag = new TagBuilder("li");
                    
                    if (this.PageModel.CurrentPage <= 1)
                    {
                        tmpTag.AddCssClass("disabled");
                        tmpTag.Attributes["disabled"] = "disabled";
                        tmpTag = this.AddLink(ref tmpTag, urlHelper.Action(this.PageModel.PageAction, new { page = 1 }), this.PagePreviousText, string.Empty);
                    }
                    else
                    {
                        tmpTag = this.AddLink(ref tmpTag, urlHelper.Action(this.PageModel.PageAction, new { page = this.PageModel.CurrentPage - 1 }), 
                                                this.PagePreviousText, string.Empty);

                    }

                    tmpTag.AddCssClass("previous"); // Needs to go at the end because new items are added before the previous entry

                    result.InnerHtml.AppendHtml(tmpTag);
                }

                // Add the pages
                for (int i = 1; i <= this.PageModel.PageCount(); i++)
                {
                    tmpTag = new TagBuilder("li");

                    if (this.PageModel.CurrentPage == i)
                    {
                        tmpTag.AddCssClass(this.PageClassSelected);
                    }
                    else
                    {
                        tmpTag.AddCssClass(this.PageClassNormal);
                    }

                    // Create the link
                    tmpTag = this.AddLink(ref tmpTag, urlHelper.Action(this.PageModel.PageAction, new { page = i }),
                                               i.ToString(), string.Empty);

                    result.InnerHtml.AppendHtml(tmpTag);
                }

                // Add the next button
                if (this.PageShowPreviousNext == true)
                {
                    tmpTag = new TagBuilder("li");
                    
                    if (this.PageModel.CurrentPage >= this.PageModel.PageCount())
                    {
                        tmpTag.AddCssClass("disabled");
                        tmpTag.Attributes["disabled"] = "disabled";
                        tmpTag = this.AddLink(ref tmpTag, urlHelper.Action(this.PageModel.PageAction, new { page = this.PageModel.PageCount().ToString()}), this.PageNextText, string.Empty);
                    }
                    else
                    {
                        tmpTag = this.AddLink(ref tmpTag, urlHelper.Action(this.PageModel.PageAction, new { page = this.PageModel.CurrentPage + 1 }),
                                                this.PageNextText, string.Empty);
                    }

                    tmpTag.AddCssClass("next");

                    result.InnerHtml.AppendHtml(tmpTag);
                }

                // finally write the output.
                if (result != null)
                {
                    output.Content.AppendHtml(result.InnerHtml);
                }
            }
            
            // tidy up
            result = null;
            tmpTag = null;
        }
    }
}
