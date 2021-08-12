using System.ComponentModel.DataAnnotations;
using GameStore.BL.Constants;
using GameStore.WEB.Constants;

namespace GameStore.WEB.DTO.Users
{
    public class UpdateUserModel
    {
        /// <summary>
        /// User's email
        /// </summary>
        /// <example>file</example>
        [Required]
        [EmailAddress]
        [RegularExpression(RegexConstants.EmailRegex, ErrorMessage = ExceptionMessageConstants.InvalidEmailString)]
        public string Email { get; set; }

        /// <summary>
        /// User's username
        /// </summary>
        /// <example>KirillNoobSlayer2008</example>

        public string UserName { get; set; }

        /// <summary>
        /// User's phone number
        /// </summary>
        /// <example>+375123456789</example>

        [RegularExpression(RegexConstants.PhoneRegex)]
        public string PhoneNumber { get; set; }
    }
}