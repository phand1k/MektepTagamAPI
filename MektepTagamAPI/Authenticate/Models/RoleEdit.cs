﻿using MektepTagamAPI.Authentication;
using Microsoft.AspNetCore.Identity;

namespace MektepTagamAPI.Authenticate.Models
{
    public class RoleEdit
    {
        public IdentityRole Role { get; set; }
        public IEnumerable<AspNetUser> Members { get; set; }
        public IEnumerable<AspNetUser> NonMembers { get; set; }
    }
}
