namespace GameStore.WEB.DTO
{
    public class RoleModel
    {
        public RoleModel(string name)
        {
            Name = name;
        }

        /// <summary>
        /// User's role name
        /// </summary>
        /// <example>admin</example>
        public string Name { get; set; }
    }
}