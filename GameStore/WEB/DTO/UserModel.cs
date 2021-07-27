using GameStore.BL.Constants;
using System.ComponentModel.DataAnnotations;

namespace GameStore.WEB.DTO
{
    public class UserModel
    {
        [Required]
        [EmailAddress]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = ExceptionMessageConstants.InvalidEmailString)]
        public string Email { get; set; }

        [Required]
        //At least one upper case english letter
        //At least one lower case english letter
        //At least one digit
        //At least one special character
        //Minimum 8 in length
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = ExceptionMessageConstants.InvalidPasswordString)]
        public string Password { get; set; }

        public string UserName { get; set; }
    }
}