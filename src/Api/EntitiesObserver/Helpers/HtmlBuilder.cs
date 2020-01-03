using EntitiesObserver.Interfaces;
using Microsoft.AspNetCore.Html;
using System.IO;
using System.Text.Encodings.Web;

namespace EntitiesObserver.Helpers
{
    internal class HtmlBuilder: IHtmlBuilder
    {
        private readonly IHtmlContentBuilder _builder;
        
        public HtmlBuilder(IHtmlContentBuilder builder)
        {
            _builder = builder;
        }

        public string GetWorkItemChangedEmailString(string receiver, int workItemId, string displayName)
        {
            AppendHtml("Dear, <b>" + receiver + "</b>!");
            BreakLine();
            BreakLine();
            AppendHtml("You are now assigned for the work item with Id: <b>" + workItemId + "</b>");
            BreakLine();
            BreakLine();
            AppendHtml("Best regards,");
            BreakLine();
            AppendHtml("<i>" + displayName + "<i>");

            return BuildHtml();
        }

        #region Helpers
        private IHtmlContentBuilder AppendHtml(string html)
        {
            return _builder.AppendHtmlLine(html);
        }

        private IHtmlContentBuilder BreakLine()
        {
            return _builder.AppendHtmlLine("<br>");
        }

        private string BuildHtml()
        {
            using (var writer = new StringWriter())
            {
                _builder.WriteTo(writer, HtmlEncoder.Default);
                return writer.ToString();
            }
        }
        #endregion
    }
}
