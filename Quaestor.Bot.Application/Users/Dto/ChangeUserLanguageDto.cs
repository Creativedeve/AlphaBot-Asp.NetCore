using System.ComponentModel.DataAnnotations;

namespace Quaestor.Bot.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}