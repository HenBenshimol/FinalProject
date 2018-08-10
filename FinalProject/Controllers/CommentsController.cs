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
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Comment
        //public async Task<IActionResult> Index()
        public ActionResult Index(string articleTitle, string commentTitle, string commentUserName)
        {
            if (User.IsInRole("Admin"))
            {
                var comments = from a in _context.Comments select a;

                if (!String.IsNullOrEmpty(articleTitle))
                {
                    comments = comments.Where(s => s.ArticleComment.Title.Contains(articleTitle));
                }

                if (!String.IsNullOrEmpty(commentTitle))
                {
                    comments = comments.Where(s => s.CommentTitle.Contains(commentTitle));
                }

                if (!String.IsNullOrEmpty(commentUserName))
                {
                    comments = comments.Where(s => s.CommentUser.Contains(commentUserName));
                }

                return View(comments.ToList());
            }
            else if (User.Identity.IsAuthenticated)
            {
                string strCurrentUserId = User.Identity.GetUserId();
                ApplicationUser user = _context.Users.Find("strCurrentUserId");

                var userName = user.FirstName + " " + user.LastName;

                var comments = _context.Comments.Include(c => c.ArticleComment).Where(c => c.CommentUser == userName);

                if (!String.IsNullOrEmpty(articleTitle))
                {
                    comments = comments.Where(s => s.ArticleComment.Title.Contains(articleTitle));
                }

                if (!String.IsNullOrEmpty(commentTitle))
                {
                    comments = comments.Where(s => s.CommentTitle.Contains(commentTitle));
                }

                if (!String.IsNullOrEmpty(commentUserName))
                {
                    comments = comments.Where(s => s.CommentUser.Contains(commentUserName));
                }

                return View(comments.ToList());
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: Comment/Details/5
        //public async Task<IActionResult> Details(int? id)
        public ActionResult Details(int? id)
        {
            //Bad Request
            if (id == null)
            {
                return BadRequest();
            }
            Comment comment = _context.Comments.Find(id);

            //Not Found
            if (comment == null)
            {
                return NotFound();
            }

            if (User.IsInRole("Admin"))
            {
                return View(comment);
            }
            else if (User.Identity.IsAuthenticated)
            {
                string strCurrentUserId = User.Identity.GetUserId();
                ApplicationUser user = _context.Users.Find("strCurrentUserId");
                //ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(strCurrentUserId);

                var userName = user.FirstName + " " + user.LastName;

                if (userName == comment.CommentUser)
                {
                    return View(comment);
                }
                else
                {
                    return RedirectToAction("PrivilegeError", "News");
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: Comment/Create
        public ActionResult Create()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.ArticleID = new SelectList(_context.Articles, "ID", "Title");
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // POST: Comment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("ID,ArticleID,CommentTitle,CommentUser,Text,PublishDate")] Comment comment)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    string strCurrentUserId = User.Identity.GetUserId();
                    ApplicationUser user = _context.Users.Find("strCurrentUserId");
                    //ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(strCurrentUserId);

                    comment.CommentUser = user.FirstName + " " + user.LastName;

                    comment.PublishDate = System.DateTime.Now;

                    _context.Comments.Add(comment);
                    _context.SaveChanges();

                    return RedirectToAction("Index");
                }

                ViewBag.ArticleID = new SelectList(_context.Articles, "ID", "Title", comment.ArticleID);
                return View(comment);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: Comment/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        public ActionResult Edit(int? id)
        {
            //Bad Request
            if (id == null)
            {
                return BadRequest();
            }

            Comment comment = _context.Comments.Find(id);

            //Not Found
            if (comment == null)
            {
                return NotFound();
            }

            ViewBag.ArticleID = new SelectList(_context.Articles, "ID", "Title", comment.ArticleID);

            string strCurrentUserId = User.Identity.GetUserId();
            ApplicationUser user = _context.Users.Find("strCurrentUserId");
            //ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(strCurrentUserId);

            var userName = user.FirstName + " " + user.LastName;

            if (User.IsInRole("Admin") || (userName == comment.CommentUser))
            {
                return View(comment);
            }
            else if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("PrivilegeError", "News");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // POST: Comment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ArticleID,CommentTitle,CommentUser,Text,PublishDate")] Comment comment)
        {
            string strCurrentUserId = User.Identity.GetUserId();
            ApplicationUser user = _context.Users.Find("strCurrentUserId");
            //ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(strCurrentUserId);

            var userName = user.FirstName + " " + user.LastName;

            if (User.IsInRole("Admin") || (userName == comment.CommentUser))
            {
                if (ModelState.IsValid)
                {
                    comment.PublishDate = System.DateTime.Now;
                    _context.Entry(comment).State = EntityState.Modified;
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.ArticleID = new SelectList(_context.Articles, "ID", "Title", comment.ArticleID);
                return View(comment);
            }
            else if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("PrivilegeError", "News");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: Comment/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        public ActionResult Delete(int? id)
        {
            //Bad Request
            if (id == null)
            {
                return BadRequest();
            }

            Comment comment = _context.Comments.Find(id);


            //Not Found
            if (comment == null)
            {
                return NotFound();
            }

            string strCurrentUserId = User.Identity.GetUserId();
            ApplicationUser user = _context.Users.Find("strCurrentUserId");
            //ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(strCurrentUserId);

            var userName = user.FirstName + " " + user.LastName;

            if (User.IsInRole("Admin") || (userName == comment.CommentUser))
            {
                return View(comment);
            }
            else if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("PrivilegeError", "News");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // POST: Comment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        public ActionResult DeleteConfirmed(int id)
        {

            string strCurrentUserId = User.Identity.GetUserId();
            ApplicationUser user = _context.Users.Find("strCurrentUserId");
            //ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(strCurrentUserId);

            var userName = user.FirstName + " " + user.LastName;
            Comment comment = _context.Comments.Find(id);

            if (User.IsInRole("Admin") || (userName == comment.CommentUser))
            {
                _context.Comments.Remove(comment);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("PrivilegeError", "News");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.ID == id);
        }

    }
}
