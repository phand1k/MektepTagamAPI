﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MektepTagamAPI.Authenticate;
using MektepTagamAPI.Models;

namespace MektepTagamAPI.Authentication
{
    public class AspNetUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? SurName { get; set; }
        public DateTime DateOfCreated { get; set; } = DateTime.Now;
        [ForeignKey("OrganizationId")]
        public int? OrganizationId { get; set; }
        [JsonIgnore]
        public Organization? Organization { get; set; }
        [StringLength(12)]
        public string? IndividualNumber { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName + " " + SurName;
            }
        }
        public AspNetUser()
        {
        }
    }
}
