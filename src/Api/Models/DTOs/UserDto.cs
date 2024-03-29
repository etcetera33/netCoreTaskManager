﻿using Core.Enums;

namespace Models.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public string ExternalId { get; set; }

        public int RoleId { get; set; }
        public string Role
        {
            get
            {
                return ((Roles)RoleId).ToString();
            }
        }
    }
}
