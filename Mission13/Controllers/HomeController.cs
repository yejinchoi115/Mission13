using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mission13.Models;

namespace Mission13.Controllers
{
    public class HomeController : Controller
    {
        private BowlerDbContext bowler { get; set; }

        public HomeController(BowlerDbContext _bowler)
        {
            bowler = _bowler;
        }

        //Index
        public IActionResult Index(string teamName)
        {

            HttpContext.Session.Remove("id");

            // put teamName into ViewBag.TeamName
            ViewBag.TeamName = teamName ?? "Home";

            //record of the bowlers from specific team
            var record = bowler.Bowlers
                .Include(x => x.Team)
                .Where(x => x.Team.TeamName == teamName || teamName == null)
                .ToList();

            return View(record);
        }

        [HttpGet]
        public IActionResult Form()
        {

            //assign list of teams in ViewBag.Teams
            ViewBag.Teams = bowler.Teams.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Form(Bowler b)
        {
            // get the Max BowlerID
            int max = 0;

            foreach (var r in bowler.Bowlers)
            {
                if (max < r.BowlerID)
                {
                    max = r.BowlerID;
                }
            }

            // assiign BowlerID max = max + 1
            b.BowlerID = max + 1;

            if (ModelState.IsValid)
            {
                bowler.Add(b);
                bowler.SaveChanges();

                //go back to Index
                return RedirectToAction("Index", new { teamName = "" });
            }
            else
            {
                return View();
            }

        }

        [HttpGet]
        public IActionResult Edit(int bowlerId)
        {
            //start editing
            ViewBag.New = false;

            //bring list of teams and assign those to ViewBag.Teams
            ViewBag.Teams = bowler.Teams.ToList();

            HttpContext.Session.SetString("id", bowlerId.ToString());

            var record = bowler.Bowlers.Single(x => x.BowlerID == bowlerId);

            return View("Form", record);
        }

        [HttpPost]
        public IActionResult Edit(Bowler b)
        {
            //get the value pair of "id"
            string id = HttpContext.Session.GetString("id");

            int int_id = int.Parse(id);

            b.BowlerID = int_id;

            if(ModelState.IsValid)
            {
                bowler.Update(b);
                bowler.SaveChanges();
                HttpContext.Session.Remove("id");

                return RedirectToAction("Index", new { teamName = "" });
            }

            // when failed
            ViewBag.New = false;

            ViewBag.Teams = bowler.Teams.ToList();

            return View("Form", b);
        }

        //Delete
        public IActionResult Delete(int bowlerId)
        {
            //find a bowler record from database by its id
            var record = bowler.Bowlers.Single(x => x.BowlerID == bowlerId);

            //remove
            bowler.Bowlers.Remove(record);

            //save changes
            bowler.SaveChanges();

            return RedirectToAction("Index", new { teamName = "" });


        }





    }
}
