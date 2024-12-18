using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WhatHaveIRead.Models;

namespace WhatHaveIRead.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<WhatHaveIRead.Models.Books> Books { get; set; } = default!;
        public DbSet<WhatHaveIRead.Models.MyLibrary> MyLibrary { get; set; } = default!;
        public DbSet<WhatHaveIRead.Models.ToRead> ToRead { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MyLibrary>()
                .HasOne(e => e.Books)          // Relacja 1-do-wielu: Książka -> MyLibrary
                .WithMany()                    // Brak nawigacji w Books (jeśli jest, zastąp `WithMany("MyLibraries")`)
                .HasForeignKey(e => e.BookId)  // Obce klucz w MyLibrary
                .OnDelete(DeleteBehavior.Restrict); // Opcjonalne zachowanie usuwania

            modelBuilder.Entity<ToRead>()
                .HasOne(e => e.Books)          // Relacja 1-do-wielu: Książka -> MyLibrary
                .WithMany()                    // Brak nawigacji w Books (jeśli jest, zastąp `WithMany("MyLibraries")`)
                .HasForeignKey(e => e.BookId)  // Obce klucz w MyLibrary
                .OnDelete(DeleteBehavior.Restrict); // Opcjonalne zachowanie usuwania
        }
    }
}
