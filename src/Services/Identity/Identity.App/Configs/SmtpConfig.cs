namespace Identity.App.Configs;

public class SmtpConfig
{
    public string SenderEmail { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
    public string User { get; set; }
    public string Password { get; set; }
}
