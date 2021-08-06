namespace GameStore.WEB.DTO.UserModels
{
    public class BasicUserRoleModel : BasicUserModel
    {
        public BasicUserRoleModel(string role, string email)
        {
            Role = role;
            Email = email;
        }

        public string Role { get; set; }
    }
}