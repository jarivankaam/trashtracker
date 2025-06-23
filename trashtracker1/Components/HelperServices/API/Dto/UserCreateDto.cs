namespace trashtracker1.Components.HelperServices.API.Dto
{
    public class UserCreateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string AdminRole { get; set; }
    }
}
