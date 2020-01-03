namespace NotificationService.Configs
{
    public class MailConfig
    {
        public string From { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public bool EnableSsl { get; set; }
        public int Port { get; set; }
    }
}
