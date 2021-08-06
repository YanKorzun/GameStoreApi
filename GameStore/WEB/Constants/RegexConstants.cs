namespace GameStore.WEB.Constants
{
    public static class RegexConstants
    {
        /// <summary>
        ///At least one upper case english letter<br/>
        ///At least one lower case english letter<br/>
        ///At least one digit<br/>
        ///At least one special character<br/>
        ///Minimum 8 in length<br/>
        /// </summary>
        public const string PasswordRegex = @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$";

        /// <summary>
        /// Examples of correct input<para/>
        /// +375123456789<br/>
        /// +375-12-345-67-89<br/>
        /// </summary>
        public const string PhoneRegex = @"\+?\(?\d{3}\)?-?\d{2}-?-?\d{3}-?\d{2}-?\d{2}";

        public const string EmailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        public const string FileExtensionRegex = @"^\w*\.(jpg|jpeg|png)+$";
    }
}