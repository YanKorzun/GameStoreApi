using GameStore.BL.Constants;
using GameStore.WEB.Constants;
using System.ComponentModel.DataAnnotations;

namespace GameStore.WEB.DTO.UserModels
{
    public class BasicUserModel : UpdateUserModel
    {
        [Required]
        [RegularExpression(RegexConstants.PasswordRegex, ErrorMessage = ExceptionMessageConstants.InvalidPasswordString)]
        public string Password { get; set; }
    }
}