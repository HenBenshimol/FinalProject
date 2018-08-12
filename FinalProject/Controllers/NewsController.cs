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
    public class NewsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private string DEAFULT_RESULT = "###";

        public NewsController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: News (Articales)
        public ActionResult Index()
        {
            if (User.IsInRole("NormalUser") || User.IsInRole("Admin") ||
                User.IsInRole("Author"))
            {
                var orderedArticles = _context.Articles.OrderByDescending(a => a.SearchCount).ThenByDescending(a => a.PublishDate);
                return View(orderedArticles.ToList());
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }



        public ActionResult FindWaves()
        {
            return View();
        }

        //PrivilegeError
        public ActionResult PrivilegeError()
        {
            return View();
        }

        public ActionResult SurfingInfo()
        {
            return View();
        }

        public PartialViewResult SearchArticleResult(string articleTitle, string autherName, string articleText,
          DateTime? startDate, DateTime? endDate)
        {
            string whereQuery = String.Empty;

            if (articleTitle == DEAFULT_RESULT)
            {
                articleTitle = string.Empty;
            }

            if (autherName == DEAFULT_RESULT)
            {
                autherName = string.Empty;
            }

            if (articleText == DEAFULT_RESULT)
            {
                articleText = string.Empty;
            }

            if (startDate == null)
            {
                startDate = DateTime.MinValue;
            }

            if (endDate == null)
            {
                endDate = DateTime.MaxValue;
            }

            var articles = from a in _context.Articles
                           where a.Title.Contains(articleTitle)
                           where a.Author.Contains(autherName)
                           where a.Text.Contains(articleText)
                           where a.PublishDate >= startDate
                           where a.PublishDate <= endDate
                           select a;

            UpdateStatisticResult(articles.ToList());

            return PartialView("../Shared/_ArticlesResult", articles.ToList());
        }

        private void UpdateStatisticResult(List<Article> articles)
        {
            foreach (Article article in articles)
            {
                article.SearchCount++;

                _context.Entry(article).State = EntityState.Modified;
                _context.SaveChanges();
            }

        }

        // GET: News/Status
        public ActionResult Status()
        {
            if (User.IsInRole("Admin") || User.IsInRole("Author") || User.IsInRole("Regular"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }
    }
}
