using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalProject.Data;
using FinalProject.Models;
using System.Dynamic;
using Microsoft.AspNetCore.Authorization;

namespace FinalProject.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class ArticleController : Controller
    {
        static Dictionary<int, string> months = new Dictionary<int, string>()
                                                            {
                                                                {1, "January"},
                                                                {2, "February"},
                                                                {3, "March"},
                                                                {4, "April"},
                                                                {5, "May"},
                                                                {6, "June"},
                                                                {7, "July"},
                                                                {8, "August"},
                                                                {9, "September"},
                                                                {10, "October"},
                                                                {11, "November"},
                                                                {12, "December"} };

        private readonly ApplicationDbContext _context;

        public ArticleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Article
        [HttpGet]
        public ActionResult Index(string articleTitle, string autherName, string articleText, DateTime? StartDate, DateTime? EndDate)
        {
            string strCurrentUserId = User.Identity.GetUserId();

            if (User.IsInRole("Admin"))
            {
                var articlesToShow = SearcArticleResult(articleTitle, autherName, articleText, StartDate, EndDate);

                return View(articlesToShow.ToList());
            }
            else if (User.IsInRole("Author"))
            {
                var articlesToShow = _context.Articles.ToList().Where(a => a.AuthorID == strCurrentUserId);
                return View(articlesToShow.ToList());
            }
            else if (User.IsInRole("Regular"))
            {
                return RedirectToAction("PrivilegeError", "News");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: Article/Details/5
        public ActionResult Details(int? id)
        {
            if (User.IsInRole("Admin") || User.IsInRole("Author") || User.IsInRole("Regular"))
            {
                //Not Found
                if (id == null)
                {
                    return NotFound();
                }

                Article article = _context.Articles.Find(id);

                article.Comments = _context.Comments.Where(c => c.ArticleID == article.ID).ToList();


                if (article == null)
                {
                    return NotFound();
                }

                AddEnter(User.Identity.GetUserId(), id.Value);
                return View(article);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: Article/Create
        public IActionResult Create()
        {
            if (User.IsInRole("Admin") || User.IsInRole("Author"))
            {
                return View();
            }
            else if (User.IsInRole("Regular"))
            {
                return RedirectToAction("PrivilegeError", "News");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // POST: Article/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,AuthorID,Author,PublishDate,Text,Image,Video,SearchCount")] Article article)
        {
            if (ModelState.IsValid)
            {
                article.PublishDate = DateTime.Today;
                article.Author = User.Identity.GetUserName();
                article.AuthorID = User.Identity.GetUserId();
                _context.Add(article);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(article);
        }

        // GET: Article/Edit/5
        public ActionResult Edit(int? id)
        {
            if (User.IsInRole("Admin") || User.IsInRole("Author"))
            {
                //Not Found
                if (id == null)
                {
                    return NotFound();
                }

                Article article = _context.Articles.Find(id);

                //Not Found
                if (article == null)
                {
                    return NotFound();
                }

                string strCurrentUserId = User.Identity.GetUserId();

                if ((article.AuthorID == strCurrentUserId) || User.IsInRole("Admin"))
                {
                    return View(article);
                }
                else
                {
                    return RedirectToAction("PrivilegeError", "News");
                }
            }
            else if (User.IsInRole("Regular"))
            {
                return RedirectToAction("PrivilegeError", "News");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // POST: Article/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,AuthorID,Author,PublishDate,Text,Image,Video,SearchCount")] Article article)
        {
            if (id != article.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(article);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.ID))
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
            return View(article);
        }

        // GET: Article/Delete/5
        public ActionResult Delete(int? id)
        {
            if (User.IsInRole("Admin") || User.IsInRole("Author"))
            {
                //Not Found
                if (id == null)
                {
                    return NotFound();
                }

                Article article = _context.Articles.Find(id);

                //Not Found
                if (article == null)
                {
                    return NotFound();
                }

                string strCurrentUserId = User.Identity.GetUserId();

                if ((article.AuthorID == strCurrentUserId) || User.IsInRole("Admin"))
                {
                    return View(article);
                }
                else
                {
                    return RedirectToAction("PrivilegeError", "News");
                }
            }
            else if (User.IsInRole("Regular"))
            {
                return RedirectToAction("PrivilegeError", "News");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // POST: Article/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Article article = _context.Articles.Find(id);
            string strCurrentUserId = User.Identity.GetUserId();

            if (User.IsInRole("Admin") || (User.IsInRole("Author") && (article.AuthorID == strCurrentUserId)))
            {
                if (_context.Comments.Where(c => c.ArticleID == article.ID).ToList() != null)
                {
                    var comm = _context.Comments.Where(c => c.ArticleID.Equals(article.ID));

                    foreach (var c in comm)
                    {
                        _context.Comments.Remove(c);
                    }
                }

                _context.Articles.Remove(article);
                _context.SaveChanges();
                return RedirectToAction("Index");
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

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.ID == id);
        }

        private void AddEnter(string userId, int articleId)
        {
            Enter temp = new Enter(articleId, userId);
            _context.Enters.Add(temp);
            _context.SaveChanges();
        }

        [HttpPost]
        public ActionResult NewComment([Bind("ID,ArticleID,CommentTitle,CommentUser,Text, PublishDate")] Comment comment)
        {
            if (User.IsInRole("Admin") || User.IsInRole("Author") || User.IsInRole("Regular"))
            {
                if (ModelState.IsValid)
                {
                    string strCurrentUserId = User.Identity.GetUserId();
                    ApplicationUser user = _context.Users.Find(strCurrentUserId);

                    comment.ArticleComment = _context.Articles.Find(comment.ArticleID);
                    comment.CommentUser = user.FirstName + " " + user.LastName;

                    comment.PublishDate = System.DateTime.Now;

                    _context.Comments.Add(comment);
                    _context.SaveChanges();

                    return RedirectToAction("Details", new { id = comment.ArticleID });
                }

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }

        private class PerMonth
        {
            public string Month;
            public int Total;
            public PerMonth(int Month, int NumberOfArticles)
            {
                months.TryGetValue(Month, out this.Month);
                this.Total = NumberOfArticles;
            }
        };

        // GET: Articale/ByMonth
        public ActionResult ByMonth()
        {
            List<PerMonth> temp = new List<PerMonth>();
            for (int i = 1; i < months.Count + 1; i++)
            {
                temp.Add(new PerMonth(i, 0));
            }
            var articlesByMonth = _context.Articles
            .GroupBy(c => new {
                Month = c.PublishDate.Month
            })
            .Select(c => new {
                Month = c.Key.Month,
                Total = c.Count()
            })
            .OrderByDescending(a => a.Month)
            .ToList();

            foreach (var month in articlesByMonth)
            {
                temp[month.Month - 1].Total = month.Total;
            }
            return this.Json(temp);
        }

        // GET: Articale/Diagnostics
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

        public List<Article> SearcArticleResult(string articleTitle, string autherName, string articleText, DateTime? fromDate, DateTime? toDate)
        {
            var articles = from a in _context.Articles select a;

            if (!String.IsNullOrEmpty(articleTitle))
            {
                articles = articles.Where(a => a.Title.Contains(articleTitle));
            }

            if (!String.IsNullOrEmpty(autherName))
            {
                articles = articles.Where(a => a.Author.Contains(autherName));
            }

            if (!String.IsNullOrEmpty(articleText))
            {
                articles = articles.Where(a => a.Text.Contains(articleText));
            }

            if (fromDate != null)
            {
                articles = articles.Where(a => (a.PublishDate.CompareTo(fromDate) >= 0));

            }

            if (toDate != null)
            {
                articles = articles.Where(a => (a.PublishDate.CompareTo(toDate) <= 0));
            }

            return articles.ToList();
        }
    }
}
