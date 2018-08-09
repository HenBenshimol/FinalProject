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

namespace FinalProject.Controllers
{
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
        //public async Task<IActionResult> Index()
        public ActionResult Index()
        {
            string strCurrentUserId = User.Identity.GetUserId();

            if (User.IsInRole("Admin"))
            {
                var articlesToShow = _context.Articles.ToList();
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
        //public async Task<IActionResult> Details(int? id)
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
        //public ActionResult Create([Bind("ID,Title,AuthorID,Author,PublishDate,Text,Image,Video,SearchCount")] Article article, HttpPostedFileBase ImageUploud, HttpPostedFileBase VideoUpload)
        {
            if (ModelState.IsValid)
            {
                _context.Add(article);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(article);
        }

        // GET: Article/Edit/5
        //public async Task<IActionResult> Edit(int? id)
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
        //public async Task<IActionResult> Delete(int? id)
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
        //public async Task<IActionResult> DeleteConfirmed(int id)
        public ActionResult DeleteConfirmed(int id)
        {
            Article article = _context.Articles.Find(id);
            string strCurrentUserId = User.Identity.GetUserId();

            if (User.IsInRole("Admin") || (User.IsInRole("Author") && (article.AuthorID == strCurrentUserId)))
            {
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
                    //ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(strCurrentUserId);
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
        /*   public ActionResult ByMonth()
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
               return this.Json(temp, JsonRequestBehavior.AllowGet);
           }*/

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
    }
}
