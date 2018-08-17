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

        public ActionResult Json()
        {
            if (User.IsInRole("Admin") || User.IsInRole("Author") || User.IsInRole("Regular"))
            {
                var articleEnters = _context.Enters
                    .Join(
                        _context.Articles,
                        enter => enter.ArticleId,
                        article => article.ID,
                        (enter, article) => new { Article = article, Enter = enter }
                    )
                    .GroupBy(c => new {
                        ArticleTitle = c.Article.Title
                    })
                    .Select(c => new {
                        articleTitle = c.Key.ArticleTitle,
                        total = c.Count()
                    })
                    .OrderBy(a => a.total)
                    .ToList();
                List<Output> jsonOutput = new List<Output>();
                foreach (var a in articleEnters)
                {
                    jsonOutput.Add(new Output(a.articleTitle, a.total));
                }
                return this.Json(jsonOutput);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult Diagnostics()
        {
            if (User.IsInRole("Admin"))
            {
                return View();
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

        private bool EnterExists(int id)
        {
            return _context.Enters.Any(e => e.Id == id);
        }
    }
}
