// <copyright file="InputTagHelper.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.AspNetCore.Mvc.TagHelpers;
//using Microsoft.AspNetCore.Mvc.ViewFeatures;
//using Microsoft.AspNetCore.Razor.TagHelpers;

//namespace duncans.TagHelpers
//{
//    [HtmlTargetElement("div", Attributes = "input-group-for, [form-group]")]
//    public class InputGroupTagHelper : TagHelper
//    {
//        public InputGroupTagHelper(IHtmlGenerator generator) : base()
//        {
//            this.HtmlGenerator = generator;
//        }

//        ////<div class="form-group">
//        ////    <div class="row">
//        ////        <div class="col-sm-12 col-md-4">
//        ////            <label asp-for="ViewData.Email" class="control-label"></label>
//        ////        </div>
//        ////        <div class="col-sm-12 col-md-8">
//        ////            <input id = "Email" asp-for="ViewData.Email" class="form-control" autocomplete="off" inputmode="email" type="email" />
//        ////        </div>
//        ////    </div>
//        ////    <div class="row">
//        ////        <div class="col-sm-12 col-md-8 offset-md-4">
//        ////            <span class="text-danger" asp-validation-for="ViewData.Email"></span>
//        ////        </div>
//        ////    </div>
//        ////</div>

//        public ModelExpression InputGroupFor { get; set; }

//        [ViewContext]
//        [HtmlAttributeNotBound]
//        public ViewContext ViewContext { get; set; }

//        private IHtmlGenerator HtmlGenerator { get; set; }

//        /// <summary>
//        /// Wrapper for all the extra things needed for a form control.
//        /// </summary>
//        /// <remarks>
//        /// Targeting Bootstrap 4 for the css class names.
//        /// </remarks>
//        /// <param name="context"></param>
//        /// <param name="output"></param>
//        public override void Process(TagHelperContext context, TagHelperOutput output)
//        {
//            // TODO handle model meta data if present
//            // Different types
//            // Select box

//            // Get the standard elements first
//            // label
//            TagBuilder labelTag = this.HtmlGenerator.GenerateLabel(this.ViewContext, this.InputGroupFor.ModelExplorer, this.InputGroupFor.Name, this.InputGroupFor.Metadata.DisplayName ?? this.InputGroupFor.Name, null);
//            labelTag.AddCssClass("col-form-label");

//            // input
//            TagBuilder inputTag = this.HtmlGenerator.GenerateTextBox(this.ViewContext, this.InputGroupFor.ModelExplorer, this.InputGroupFor.Name, this.InputGroupFor.Model, string.Empty, null);
//            inputTag.AddCssClass("form-control");

//            // validation
//            TagBuilder validationTag = this.HtmlGenerator.GenerateValidationMessage(this.ViewContext, this.InputGroupFor.ModelExplorer, this.InputGroupFor.Name, string.Empty, "span", null);
//            validationTag.AddCssClass("text-danger");

//            TagBuilder result = new TagBuilder("div");
//            result.AddCssClass("form-group");

//            TagBuilder rowTag = new TagBuilder("div");
//            rowTag.AddCssClass("row");

//            // Label
//            TagBuilder labelColumnTag = new TagBuilder("div");
//            labelColumnTag.AddCssClass("col-sm-12");
//            labelColumnTag.AddCssClass("col-md-4");
//            labelColumnTag.InnerHtml.AppendHtml(labelTag);

//            // input
//            TagBuilder inputColumnTag = new TagBuilder("div");
//            inputColumnTag.AddCssClass("col-sm-12");
//            inputColumnTag.AddCssClass("col-md-8");

//            // adds the label stuff to the output. This will need moving
//            inputColumnTag.InnerHtml.AppendHtml(inputTag);

//            rowTag.InnerHtml.AppendHtml(labelColumnTag);
//            rowTag.InnerHtml.AppendHtml(inputColumnTag);

//            // validation
//            TagBuilder validationRow = new TagBuilder("div");
//            validationRow.AddCssClass("row");

//            TagBuilder validationColumn = new TagBuilder("div");
//            validationColumn.AddCssClass("col-sm-12");
//            validationColumn.AddCssClass("col-md-8");
//            validationColumn.AddCssClass("offset-md-4");

//            // Todo the asp-validation-for bits
//            validationColumn.InnerHtml.AppendHtml(validationTag);
//            validationRow.InnerHtml.AppendHtml(validationColumn);

//            result.InnerHtml.AppendHtml(rowTag);
//            result.InnerHtml.AppendHtml(validationRow);

//            output.Content.AppendHtml(result.InnerHtml);
//        }
//    }
//}
