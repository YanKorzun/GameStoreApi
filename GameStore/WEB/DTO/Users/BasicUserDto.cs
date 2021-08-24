using System.ComponentModel.DataAnnotations;
using GameStore.BL.Constants;
using GameStore.WEB.Constants;

namespace GameStore.WEB.DTO.Users
{
    public class BasicUserDto : UpdateUserDto
    {
        /// <summary>
        /// Password of the user
        /// </summary>
        /// <example>myNewPas$w0rd</example>
        [Required]
        [RegularExpression(RegexConstants.PasswordRegex,
            ErrorMessage = ExceptionMessageConstants.InvalidPasswordString)]
        public string Password { get; set; }
    }
}