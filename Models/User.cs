using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Online_Shop___DAW.Models
{
    public enum UserType
    {
        administrator,
        collaborator,
        registered,
        unregistered
    }

    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public UserType Type { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        [MinLength(6)]
        [MaxLength(6)]
        public string ZipCode { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}