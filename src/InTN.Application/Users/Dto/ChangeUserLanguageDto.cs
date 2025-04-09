using System.ComponentModel.DataAnnotations;

namespace InTN.Users.Dto;

public class ChangeUserLanguageDto
{
    [Required]
    public string LanguageName { get; set; }
}