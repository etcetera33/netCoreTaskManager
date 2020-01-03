namespace NotificationService.Aggregates.HtmlAggregate
{
    public interface IHtmlBuilder
    {
        string GetEmailBodyForNewAssignee(string receiver, int workItemId);
    }
}
