using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalProject.Data;
using FinalProject.Models;
using TweetSharp;

namespace FinalProject.Controllers
{
    public class TeamController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TeamController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Team
        //public async Task<IActionResult> Index(string teamName, string teamDescription)
        public ActionResult Index(string teamName, string teamDescription)
        {
            var teams = from a in _context.Teams select a; //LINQ

            if (User.IsInRole("Admin"))
            {
                if (!String.IsNullOrEmpty(teamName))
                {
                    teams = teams.Where(s => s.Name.Contains(teamName));
                }

                if (!String.IsNullOrEmpty(teamDescription))
                {
                    teams = teams.Where(s => s.Description.Contains(teamDescription));
                }

                return View(teams.ToList());
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

        // GET: Team/Details/5
        //public async Task<IActionResult> Details(int? id)
        public ActionResult Details(int? id)
        {
            if (User.IsInRole("Admin"))
            {
                //Bad Request
                if (id == null)
                {
                    return BadRequest();
                }

                Team team = _context.Teams.Find(id);

                // Not Found
                if (team == null)
                {
                    return NotFound();
                }

                return View(team);
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

        // GET: Team/Create
        public IActionResult Create()
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

        // POST: Team/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("TeamID,Name,Description,Zoom,Lattitude,Longtidute")] Team team)
        public ActionResult Create([Bind("TeamID,Name,Description,Zoom,Lattitude,Longtidute")] Team team)
        {
            if (User.IsInRole("Admin"))
            {
                if (ModelState.IsValid)
                {
                    _context.Teams.Add(team);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(team);
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

        // GET: Team/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        public ActionResult Edit(int? id)
        {
            if (User.IsInRole("Admin"))
            {
                //Bad Request
                if (id == null)
                {
                    return BadRequest();
                }

                Team t = _context.Teams.Find(id);

                //Not Found
                if (t == null)
                {
                    return NotFound();
                }

                return View(t);
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

        // POST: Team/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TeamID,Name,Description,Zoom,Lattitude,Longtidute")] Team team)
        {
            if (User.IsInRole("Admin"))
            {
                // Not Found
                if (id != team.TeamID)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(team);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!TeamExists(team.TeamID))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                    return RedirectToAction("Index");
                }

                return View(team);
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

        // GET: Team/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        public ActionResult Delete(int? id)
        {
            //Not Found
            if (User.IsInRole("Admin"))
            {
                if (id == null)
                {
                    return NotFound();
                }

                Team team = _context.Teams.Find(id);

                //Not Found
                if (team == null)
                {
                    return NotFound();
                }

                return View(team);
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

        // POST: Team/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        public ActionResult Delete(int id)
        {
            if (User.IsInRole("Admin"))
            {
                Team team = _context.Teams.Find(id);
                _context.Teams.Remove(team);
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

        // GET: Beach/json
        public ActionResult Json()
        {
            if (User.IsInRole("Admin") || User.IsInRole("Author") || User.IsInRole("Regular"))
            {
                return this.Json(_context.Teams.ToList());
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult Map()
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

        public ActionResult Tweets()
        {
            if(User.IsInRole("Admin") || User.IsInRole("Author") || User.IsInRole("Regular"))
            { 
                string consumerKey = "h2HP4QQT1KtJ2b7yzV2QYlZ8C";
                string consumerSecret = "ZykLEiiDFtohllfVlmR0MunZODAgQpmcUUdJRjgiOGvPNAEHKf";
                string accessToken = "1033276389720563712-rSe7s3DjAKpS8ehtz6YBoRuNgicff7";
                string accessTokenSecret = "sbUi5zOafnq6Mmy5W6rahyJ07ptjDHdoEpvJxITsbD0Ay";

                TwitterService twitterService = new TwitterService(consumerKey, consumerSecret);
                twitterService.AuthenticateWith(accessToken, accessTokenSecret);

                var tweets_search = twitterService.Search(new SearchOptions { Q = "#FIFA", Resulttype = TwitterSearchResultType.Popular, Count = 10 });
                List<TwitterStatus> resultList = new List<TwitterStatus>(tweets_search.Statuses);

                ViewBag.Tweets = resultList;

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        private bool TeamExists(int id)
        {
            return _context.Teams.Any(e => e.TeamID == id);
        }
    }
}
