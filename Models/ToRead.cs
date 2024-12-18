using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WhatHaveIRead.Models
{
    public class ToRead
    {
        public int Id { get; set; }

        [Display(Name = "Book")]

        public int BookId { get; set; }

        [Display(Name = "Book")]
        public virtual Books? Books { get; set; }

        public string? UserId { get; set; }

        [Display(Name = "User")]
        public virtual IdentityUser? User { get; set; }
    }
}
