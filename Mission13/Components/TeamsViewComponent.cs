using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Mission13.Models;

namespace Mission13.Components
{
    public class TeamsViewComponent: ViewComponent
    {
        private BowlerDbContext bowler { get; set; }

        //constructor
        public TeamsViewComponent(BowlerDbContext temp)
        {
            bowler = temp;
        }

        //invoke
        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedTeam = RouteData?.Values["teamName"] ?? "";

            // team names from context
            var teams = bowler.Bowlers
                .Select(x => x.Team.TeamName)
                .Distinct()
                .OrderBy(x => x);

            return View(teams);
        }
    }
}
