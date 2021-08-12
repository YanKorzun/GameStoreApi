namespace GameStore.WEB.DTO.Roles
{
    public class RoleDto
    {
        public RoleDto(string name)
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