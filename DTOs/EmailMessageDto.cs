namespace CalorieBurnMgt.DTOs
{
    public class EmailMessageDto
    {
        public string To { get; set; }           // Recipient
        public string Subject { get; set; }      // Email subject
        public string Body { get; set; }         // Email content
        public bool IsHtml { get; set; } = true; // Whether the content is in HTML format
    }
}
