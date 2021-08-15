using _08_05_Olympics.Models;
using _08_05_Olympics.Models.ViewModels;
using Olympics.Models;
using Olympics.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace _08_05_Olympics.Services
{
    public class AthletesJoinedService
    {
        private readonly SqlConnection _connection;
        private readonly AthletesDbService _athletesDbService;
        private readonly CountriesDbService _countriesDbService;
        private readonly SportsDbService _sportsDbService;

        public AthletesJoinedService(SqlConnection connection,
                                         AthletesDbService athletesDbService,
                                         CountriesDbService countriesDbService,
                                         SportsDbService sportsDbService)
        {
            _connection = connection;
            _athletesDbService = athletesDbService;
            _countriesDbService = countriesDbService;
            _sportsDbService = sportsDbService;
        }

        public JoinedViewModel GetModelForIndex(SortModel sortModel)
        {
            JoinedViewModel model = new();
            model.Athletes = _athletesDbService.GetAthletes();
            model.Countries = _countriesDbService.GetCountries();
            model.Sports = _sportsDbService.GetSports();
            model.Sort = new();

            foreach (var athlete in model.Athletes)
            {
                athlete.Country = model.Countries.SingleOrDefault(c => c.Id == athlete.Country_id);

                List<int> sportsIds = _athletesDbService.GetSportIds(athlete.Id);

                foreach (int sportId in sportsIds)
                {
                    athlete.Sports.Add(sportId, true);
                }
            }

            return model;
        }

        public JoinedViewModel GetModelForSort(SortModel sortModel)
        {
            JoinedViewModel model = new();
            model.Athletes = _athletesDbService.SortAthletes(sortModel);
            model.Countries = _countriesDbService.GetCountries();
            model.Sports = _sportsDbService.GetSports();
            model.Sort = new();

            foreach (var athlete in model.Athletes)
            {
                athlete.Country = model.Countries.SingleOrDefault(c => c.Id == athlete.Country_id);

                List<int> sportsIds = _athletesDbService.GetSportIds(athlete.Id);

                foreach (int sportId in sportsIds)
                {
                    athlete.Sports.Add(sportId, true);
                }
            }

            return model;
        }


        public JoinedViewModel GetModelForCreate()
        {
            JoinedViewModel model = new();
            AthleteModel newAthlete = new();

            model.Athletes = new List<AthleteModel> { newAthlete };

            model.Countries = _countriesDbService.GetCountries();
            model.Sports = _sportsDbService.GetSports();

            foreach (var sport in model.Sports)
            {
                newAthlete.Sports.Add(sport.Id, false);
            }

            return model;
        }

        public JoinedViewModel GetModelForEdit(int editId)
        {
            JoinedViewModel model = new JoinedViewModel();
            List<AthleteModel> athletes = _athletesDbService.GetAthletes();

            AthleteModel athleteEdit = athletes.SingleOrDefault(x => x.Id == editId);

            model.Athletes = new List<AthleteModel> {athleteEdit};
            model.Countries = _countriesDbService.GetCountries();
            model.Sports = _sportsDbService.GetSports();

            List<int> sportIds = _athletesDbService.GetSportIds(editId);
            foreach (var sport in model.Sports)
            {
                if (sportIds.Contains(sport.Id))
                    athleteEdit.Sports.Add(sport.Id, true);
                else
                    athleteEdit.Sports.Add(sport.Id, false);
            }

            return model;
        }
    }
}
