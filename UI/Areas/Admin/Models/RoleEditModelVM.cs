namespace UI.Areas.Admin.Models
{
    public class RoleEditModelVM
    {
        public string Id { get; set; }
        public string RoleName { get; set; }
        public string[] IdsToAdd { get; set; }
        public string[] IdsToDelete { get; set; }
    }
}



