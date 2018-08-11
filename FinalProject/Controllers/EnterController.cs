using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalProject.Data;
using FinalProject.Models;

namespace FinalProject.Controllers
{
    public class EnterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EnterController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Enter
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return View(_context.Enters.ToList());
            }
            else if (User.IsInRole("Regular") || User.IsInRole("Author"))
            {
                return RedirectToAction("PrivilegeError", "News");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        private class Output
        {
            public string articleTitle;
            public int total;
            public Output(string articleTitle, int total)
            {
                this.articleTitle = articleTitle;
                this.total = total;
            }
        }

        public ActionResult Diagnostics()
        {
            if (User.IsInRole("Admin"))
            {
                return View();
            }
            else if (User.IsInRole("NormalUser") || User.IsInRole("Author"))
            {
                return RedirectToAction("PrivilegeError", "News");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        /*

        // GET: Enter/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enter = await _context.Enters
                .Include(e => e.EnterArticle)
                .Include(e => e.EnterUser)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (enter == null)
            {
                return NotFound();
            }

            return View(enter);
        }

        // GET: Enter/Create
        public IActionResult Create()
        {
            ViewData["ArticleId"] = new SelectList(_context.Articles, "ID", "Text");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Enter/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ArticleId,UserId,EnterDate")] Enter enter)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArticleId"] = new SelectList(_context.Articles, "ID", "Text", enter.ArticleId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", enter.UserId);
            return View(enter);
        }

        // GET: Enter/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enter = await _context.Enters.SingleOrDefaultAsync(m => m.Id == id);
            if (enter == null)
            {
                return NotFound();
            }
            ViewData["ArticleId"] = new SelectList(_context.Articles, "ID", "Text", enter.ArticleId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", enter.UserId);
            return View(enter);
        }

        // POST: Enter/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ArticleId,UserId,EnterDate")] Enter enter)
        {
            if (id != enter.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnterExists(enter.Id))
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
            ViewData["ArticleId"] = new SelectList(_context.Articles, "ID", "Text", enter.ArticleId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", enter.UserId);
            return View(enter);
        }

        // GET: Enter/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enter = await _context.Enters
                .Include(e => e.EnterArticle)
                .Include(e => e.EnterUser)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (enter == null)
            {
                return NotFound();
            }

            return View(enter);
        }

        // POST: Enter/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enter = await _context.Enters.SingleOrDefaultAsync(m => m.Id == id);
            _context.Enters.Remove(enter);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }*/

        private bool EnterExists(int id)
        {
            return _context.Enters.Any(e => e.Id == id);
        }
    }
}
