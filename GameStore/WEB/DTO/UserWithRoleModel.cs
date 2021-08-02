namespace GameStore.WEB.DTO
{
    public class UserWithRoleModel : UserWithPasswordModel
    {
        public UserWithRoleModel(string role, string email)
        {
            Role = role;
            Email = email;
        }

        public string Role { get; set; }
    }
}