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
    public class ToReadsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ToReadsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ToReads
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ToRead
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

        // GET: ToReads/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toRead = await _context.ToRead
                .Include(e => e.User)
                .Include(e => e.Books)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toRead == null)
            {
                return NotFound();
            }

            return View(toRead);
        }

        // GET: ToReads/Create
        public IActionResult Create()
        {
            ViewBag.BookId = new SelectList(_context.Books, "Id", "Name");
            ViewBag.UserId = new SelectList(_context.Users, "Id", "UserName");
            return View();
        }

        // POST: ToReads/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BookId,UserId")] ToRead toRead)
        {
            if (ModelState.IsValid)
            {
                _context.Add(toRead);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(toRead);
        }

        // GET: ToReads/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toRead = await _context.ToRead.FindAsync(id);
            if (toRead == null)
            {
                return NotFound();
            }
            ViewBag.BookId = new SelectList(_context.Books, "Id", "Name", toRead.BookId);
            ViewBag.UserId = new SelectList(_context.Users, "Id", "Username", toRead.UserId);
            return View(toRead);
        }

        // POST: ToReads/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BookId,UserId")] ToRead toRead)
        {
            if (id != toRead.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(toRead);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToReadExists(toRead.Id))
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
            ViewBag.BookId = new SelectList(_context.Books, "Id", "Name", toRead.BookId);
            ViewBag.UserId = new SelectList(_context.Users, "Id", "Username", toRead.UserId);
            return View(toRead);
        }

        // GET: ToReads/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toRead = await _context.ToRead
                .Include(e => e.User)
                .Include(e => e.Books)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toRead == null)
            {
                return NotFound();
            }

            return View(toRead);
        }

        // POST: ToReads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var toRead = await _context.ToRead.FindAsync(id);
            if (toRead != null)
            {
                _context.ToRead.Remove(toRead);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ToReadExists(int id)
        {
            return _context.ToRead.Any(e => e.Id == id);
        }
    }
}
