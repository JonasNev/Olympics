using _08_05_Olympics.Models;
using Olympics.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Olympics.Services
{
    public class AthletesDbService
    {
        private readonly SqlConnection _connection;

        public AthletesDbService(SqlConnection connection)
        {
            _connection = connection;
        }

        public List<AthleteModel> GetAthletes()
        {
            List<AthleteModel> athletes = new();

            _connection.Open();

            using var command = new SqlCommand("SELECT * FROM dbo.AthleteModel", _connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                AthleteModel athlete = new()
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Surname = reader.GetString(2),
                    Country_id = reader.GetInt32(3)
                };

                athletes.Add(athlete);
            }

            _connection.Close();

            return athletes;
        }


        public List<AthleteModel> SortAthletes(SortModel sortModel)
        {
            List<AthleteModel> athletes = new();
            _connection.Open();

            using var command = new SqlCommand(@$"Select distinct Id, Name, Surname, CountryName, Country_id, 
                                                        SportName = STUFF((SELECT DISTINCT ', ' + SportName
                                                        FROM(SELECT Id, Name, Surname, CountryName, Country_id, SportName FROM[olympics].[dbo].[AthleteSportCountries]) a
                                                        WHERE[olympics].[dbo].[AthleteSportCountries].id = a.id
                                                        FOR XML PATH('')), 1, 2, '')
                                                        FROM
                                                        [olympics].[dbo].[AthleteSportCountries]
                                                        ORDER BY {sortModel.Sort}", _connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                AthleteModel athlete = new()
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Surname = reader.GetString(2),
                    Country_id = reader.GetInt32(4),
                };

                athletes.Add(athlete);
            }

            _connection.Close();

            return athletes;
        }
        public void AddAthlete(AthleteModel athlete)
        {
            _connection.Open();

            using var command = new SqlCommand($"INSERT INTO dbo.AthleteModel (Name, Surname, Country_id)" +
                $"VALUES ('{athlete.Name}', '{athlete.Surname}', '{athlete.Country_id}'); SELECT CAST(SCOPE_IDENTITY() AS INT)", _connection);
            command.ExecuteNonQuery();

            _connection.Close();

            int athleteId = GetLastAthleteId();
            if (athleteId == 0) return;

            AddAthleteSportJunctions(athlete, athleteId);
        }


        public void UpdateAthlete(AthleteModel athleteModel)
        {
            string command = $@"UPDATE Dbo.AthleteModel 
                                SET Name = '{athleteModel.Name}', Surname = '{athleteModel.Surname}', Country_id = '{athleteModel.Country_id}' 
                                WHERE Id = {athleteModel.Id};";

            _connection.Open();

            using var sqlCommand = new SqlCommand(command, _connection);
            sqlCommand.ExecuteNonQuery();
            _connection.Close();
            DeleteAthleteSportJunction(athleteModel.Id);
            AddAthleteSportJunctions(athleteModel, athleteModel.Id);

        }

        private void AddAthleteSportJunctions(AthleteModel athlete, int id)
        {
            var sportsWhereAthleteAttends = athlete.Sports.Where(s => s.Value == true).ToDictionary(s => s.Key, s => s.Value);
            if (sportsWhereAthleteAttends.Count == 0)
                return;

            string querystring = "";
            for (var i = 0; i < sportsWhereAthleteAttends.Count; i++)
            {
                querystring += $"({id}, {sportsWhereAthleteAttends.ElementAt(i).Key}), ";
            }

            querystring = querystring.Remove(querystring.Length - 2);

            string query = $"INSERT INTO dbo.AthleteSportsJunction VALUES {querystring};";

            _connection.Open();

            using var command = new SqlCommand(query, _connection);
            command.ExecuteNonQuery();

            _connection.Close();
        }

        private int GetLastAthleteId()
        {
            int id = 0;

            _connection.Open();

            using var command = new SqlCommand($"SELECT TOP 1 Id FROM dbo.AthleteModel ORDER BY Id DESC;", _connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                id = reader.GetInt32(0);
            }

            _connection.Close();

            return id;
        }

        public List<int> GetSportIds(int athleteId)
        {
            List<int> sportIds = new List<int>();
            string command = $"SELECT Sports_Id FROM dbo.AthleteSportsJunction WHERE Athlete_Id = {athleteId}";
            _connection.Open();

            using var sqlCommand = new SqlCommand(command, _connection);
            using var reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                sportIds.Add(reader.GetInt32(0));
            }

            _connection.Close();

            return sportIds;
        }

        public void DeleteAthleteSportJunction(int deleteId)
        {
            string command = $"DELETE FROM dbo.AthleteSportsJunction WHERE Athlete_Id = {deleteId};";

            _connection.Open();

            using var sqlCommand = new SqlCommand(command, _connection);
            sqlCommand.ExecuteNonQuery();

            _connection.Close();
        }
    }
}
