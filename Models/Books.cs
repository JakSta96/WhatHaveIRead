using System.ComponentModel.DataAnnotations;

namespace WhatHaveIRead.Models
{
    public class Books
    {
        public int Id { get; set; }
        [Display(Name = "Book Title")]
        [StringLength(100, ErrorMessage = "Comment cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Display(Name = "Author Name")]
        [StringLength(100, ErrorMessage = "Comment cannot exceed 100 characters.")]
        public string Author { get; set; }

        [Display(Name = "Release Year")]
        [Range(1000, 2100, ErrorMessage = "Year must be between 1000 and 2100")]
        public int ReleaseYear { get; set; }

        [Display(Name = "Short Description")]
        [StringLength(500, ErrorMessage = "Comment cannot exceed 500 characters.")]
        public string ShortDescription { get; set; }
    }
}
