using GameStore.BL.Constants;
using GameStore.WEB.Constants;
using System.ComponentModel.DataAnnotations;

namespace GameStore.WEB.DTO.UserModels
{
    public class UpdateUserModel
    {
        [Required]
        [EmailAddress]
        [RegularExpression(RegexConstants.EmailRegex, ErrorMessage = ExceptionMessageConstants.InvalidEmailString)]
        public string Email { get; set; }

        public string UserName { get; set; }

        [RegularExpression(RegexConstants.PhoneRegex)]
        public string PhoneNumber { get; set; }
    }
}