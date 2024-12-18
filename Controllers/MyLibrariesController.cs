using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WhatHaveIRead.Data;
using WhatHaveIRead.Models;

namespace WhatHaveIRead.Controllers
{
    [Authorize]
    public class MyLibrariesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MyLibrariesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MyLibraries
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.MyLibrary
                .Where(e => e.User.UserName == User.Identity.Name)
                .Include(e => e.User)
                .Include(e => e.Books);

            var data = await applicationDbContext.ToListAsync();

            // Debugowanie danych
            foreach (var item in data)
            {
                Console.WriteLine($"BookId: {item.BookId}, BookName: {item.Books?.Name}");
            }

            return View(data);
        }

        // GET: MyLibraries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var myLibrary = await _context.MyLibrary
                .Include(e => e.User)
                .Include(e => e.Books)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (myLibrary == null)
            {
                return NotFound();
            }

            return View(myLibrary);
        }

        // GET: MyLibraries/Create
        public IActionResult Create()
        {
            ViewBag.BookId = new SelectList(_context.Books, "Id", "Name");
            ViewBag.UserId = new SelectList(_context.Users, "Id", "UserName");
            return View();
        }

        // POST: MyLibraries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BookId,Comment,UserId")] MyLibrary myLibrary)
        {
            if (ModelState.IsValid)
            {
                _context.Add(myLibrary);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
          
            return View(myLibrary);
        }

        // GET: MyLibraries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var myLibrary = await _context.MyLibrary
                .Include(e => e.Books) // Ładowanie relacji Books
                .Include(e => e.User)  // Ładowanie relacji User
                .FirstOrDefaultAsync(e => e.Id == id);
            if (myLibrary == null)
            {
                return NotFound();
            }
            ViewBag.BookId = new SelectList(_context.Books, "Id", "Name", myLibrary.BookId);
            ViewBag.UserId = new SelectList(_context.Users, "Id", "Username", myLibrary.UserId);
            return View(myLibrary);
        }

        // POST: MyLibraries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BookId,Comment,UserId")] MyLibrary myLibrary)
        {
            if (id != myLibrary.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(myLibrary);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MyLibraryExists(myLibrary.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.BookId = new SelectList(_context.Books, "Id", "Name", myLibrary.BookId);
            ViewBag.UserId = new SelectList(_context.Users, "Id", "Username", myLibrary.UserId);
            return View(myLibrary);
        }

        // GET: MyLibraries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var myLibrary = await _context.MyLibrary
                .Include(e => e.User)
                .Include(e => e.Books)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (myLibrary == null)
            {
                return NotFound();
            }

            return View(myLibrary);
        }

        // POST: MyLibraries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var myLibrary = await _context.MyLibrary.FindAsync(id);
            if (myLibrary != null)
            {
                _context.MyLibrary.Remove(myLibrary);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MyLibraryExists(int id)
        {
            return _context.MyLibrary.Any(e => e.Id == id);
        }
    }
}
