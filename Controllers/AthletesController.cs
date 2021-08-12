using _08_05_Olympics.Models;
using _08_05_Olympics.Models.ViewModels;
using _08_05_Olympics.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace _08_05_Olympics.Controllers
{
    public class AthletesController : Controller
    {
        private readonly AthletesJoinedService _joinedService;
        private readonly AthletesDbService _athletesDbService;

        public AthletesController(AthletesJoinedService joinedService, AthletesDbService athletesDbService)
        {
            _joinedService = joinedService;
            _athletesDbService = athletesDbService;
        }

        public IActionResult Index()
        {
            JoinedViewModel model = _joinedService.GetModelForIndex();

            return View(model);
        }

        public IActionResult Create()
        {
            JoinedViewModel model = _joinedService.GetModelForCreate();

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(List<AthleteModel> athletes)
        {
            _athletesDbService.AddAthlete(athletes[0]);

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            JoinedViewModel model = _joinedService.GetModelForEdit(id);

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(List<AthleteModel> athletes)
        {
            _athletesDbService.UpdateAthlete(athletes[0]);

            return RedirectToAction("Index");
        }
    }
}
