namespace Models.DTOs
{
    public class EmailDto
    {
        public string To { get; set; }
        public string From { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
    }
}
