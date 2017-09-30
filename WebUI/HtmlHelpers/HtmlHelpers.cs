using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebUI.Models;

namespace WebUI.HtmlHelpers
{
    public static class HtmlHelpers
    {
        public static MvcHtmlString PageLinks(
            this HtmlHelper html,
            PagingInfo pagingInfo,
            Func<int,string> pageUrl,int size = 2)
        {
            StringBuilder result = new StringBuilder();

            if (pagingInfo.TotalPages > 2 * size + 1)
            {
                TagBuilder firsthref = new TagBuilder("a");
                firsthref.MergeAttribute("href", pageUrl(1));
                firsthref.InnerHtml = "<<";
                firsthref.AddCssClass("btn btn-default");
                result.Append(firsthref.ToString());
            }

            int pagesToEnd = pagingInfo.TotalPages - pagingInfo.CurrentPage;

            for (int i = (pagingInfo.CurrentPage - size) > 1 ? (pagingInfo.CurrentPage - size) : 1;
                i <= (pagesToEnd < size ? pagingInfo.CurrentPage + pagesToEnd : pagingInfo.CurrentPage + size);
                i++)
            {
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml = i.ToString();
                if (i == pagingInfo.CurrentPage)
                {
                    tag.AddCssClass("selected");
                    tag.AddCssClass("btn-primary");
                }
                tag.AddCssClass("btn btn-default");
                result.Append(tag.ToString());
            }

            if (pagingInfo.TotalPages > 2 * size + 1)
            {
                TagBuilder lasthref = new TagBuilder("a"); // Construct an <a> tag
                lasthref.MergeAttribute("href", pageUrl(pagingInfo.TotalPages));
                lasthref.InnerHtml = ">>";
                lasthref.AddCssClass("btn btn-default");
                result.Append(lasthref.ToString());
            }

            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString Button(this HtmlHelper helper,
                                    string innerHtml,
                                    IDictionary<string, object> htmlAttributes)
        {
            var builder = new TagBuilder("button");
            builder.InnerHtml = innerHtml;
            builder.MergeAttributes(htmlAttributes);
            return MvcHtmlString.Create(builder.ToString());
        }
    }
}