namespace WinterSportAcademy.Services;

using System.ComponentModel.DataAnnotations;

public class EmailSettings
{
    [Required]
    public string SmtpServer { get; set; } = string.Empty;

    [Range(1, 65535)]
    public int SmtpPort { get; set; }

    [Required]
    public string SmtpUsername { get; set; } = string.Empty;

    [Required]
    public string SmtpPassword { get; set; } = string.Empty;
}


