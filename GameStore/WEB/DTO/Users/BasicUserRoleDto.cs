namespace GameStore.WEB.DTO.Users
{
    public class BasicUserRoleDto : BasicUserDto
    {
        public BasicUserRoleDto(string role, string email)
        {
            Role = role;
            Email = email;
        }

        /// <summary>
        /// Name of user's role
        /// </summary>
        /// <example>admin</example>
        public string Role { get; set; }
    }
}