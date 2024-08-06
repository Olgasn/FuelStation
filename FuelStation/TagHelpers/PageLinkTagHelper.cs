using FuelStation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;

namespace FuelStation.TagHelpers
{
    public class PageLinkTagHelper(IUrlHelperFactory helperFactory) : TagHelper
    {
        private readonly IUrlHelperFactory urlHelperFactory = helperFactory;

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        public PageViewModel PageModel { get; set; }
        public string PageAction { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; } = [];

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
            output.TagName = "div";

            // набор ссылок будет представлять список ul
            TagBuilder tag = new("ul");
            tag.AddCssClass("pagination");

            // формируем три ссылки - на текущую, предыдущую и следующую
            TagBuilder currentItem = CreateTag(PageModel.PageNumber, urlHelper);

            // создаем ссылку на предыдущую страницу, если она есть
            if (PageModel.HasPreviousPage)
            {
                TagBuilder prevItem = CreateTag(PageModel.PageNumber - 1, urlHelper);
                tag.InnerHtml.AppendHtml(prevItem);
            }

            tag.InnerHtml.AppendHtml(currentItem);
            // создаем ссылку на следующую страницу, если она есть
            if (PageModel.HasNextPage)
            {
                TagBuilder nextItem = CreateTag(PageModel.PageNumber + 1, urlHelper);
                tag.InnerHtml.AppendHtml(nextItem);
            }
            output.Content.AppendHtml(tag);
        }

        TagBuilder CreateTag(int pageNumber, IUrlHelper urlHelper)
        {
            TagBuilder item = new("li");
            TagBuilder link = new("a");
            if (pageNumber == this.PageModel.PageNumber)
            {
                item.AddCssClass("active");
            }
            else
            {
                PageUrlValues["page"] = pageNumber;
                link.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
            }
            link.InnerHtml.Append(pageNumber.ToString());
            item.InnerHtml.AppendHtml(link);
            return item;
        }
    }
}