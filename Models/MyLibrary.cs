using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WhatHaveIRead.Models
{
    public class MyLibrary
    {
        public int Id { get; set; }

        [Display(Name = "Book")]

        public int BookId { get; set; }

        [Display(Name = "Book")]
        public virtual Books? Books { get; set; }

        [Display(Name = "Comment")]
        [StringLength(500, ErrorMessage = "Comment cannot exceed 500 characters.")]
        public string Comment { get; set; }

        public string? UserId { get; set; }

        [Display(Name = "User")]
        public virtual IdentityUser? User { get; set; }

    }
}
