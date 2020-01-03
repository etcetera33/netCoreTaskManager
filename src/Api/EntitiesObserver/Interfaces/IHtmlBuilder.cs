using Microsoft.AspNetCore.Html;

namespace EntitiesObserver.Interfaces
{
    internal interface IHtmlBuilder
    {
        string GetWorkItemChangedEmailString(string receiver, int workItemId, string displayName);
    }
}
