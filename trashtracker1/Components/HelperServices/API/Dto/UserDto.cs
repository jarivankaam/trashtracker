﻿namespace trashtracker1.Components.HelperServices.API.Dto
{
    public class UserDto
    {
        public string Id { get; set; }
        public string IdentityUserId { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int Role { get; set; }
    }
}
