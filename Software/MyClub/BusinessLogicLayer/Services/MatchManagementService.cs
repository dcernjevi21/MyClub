using DataAccessLayer.EntityRepository;
using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class MatchManagementService
    {
        private List<Match> _cachedMatches = new List<Match>();

        public async Task<List<Match>> GetMatches()
        {
            using (var repo = new MatchManagementRepository())
            {
                var matches = await repo.GetAllMatches().ToListAsync();
                return matches;
            }
        }

        public Match GetMatchById(int matchId)
        {
            using (var repo = new MatchManagementRepository())
            {
                return repo.GetMatchById(matchId).FirstOrDefault();
            }
        }

        public async Task<List<Match>> GetMatchesByTeamId(int teamId)
        {
            using (var repo = new MatchManagementRepository())
            {
                if(_cachedMatches.Count == 0)
                {
                    _cachedMatches = await repo.GetMatchesByTeamId(teamId)?.ToListAsync() ?? new List<Match>();
                }
                return _cachedMatches;
            }
        }

        public List<Match> FilterMatches(DateTime? startDate, DateTime? endDate, string status)
        {
            var filteredMatches = _cachedMatches.AsQueryable(); // Početni set podataka

            Console.WriteLine($"Filtering Matches - Start: {startDate}, End: {endDate}, Status: {status}");

            if (startDate.HasValue && endDate.HasValue)
            {
                filteredMatches = filteredMatches.Where(m => m.MatchDate >= startDate.Value && m.MatchDate <= endDate.Value);
                Console.WriteLine($"After Date Filter: {filteredMatches.Count()} matches found.");
            }

            if (!string.IsNullOrEmpty(status) && status != "- Select a status -")
            {
                filteredMatches = filteredMatches.Where(m => m.Status.Trim().ToLower() == status.Trim().ToLower());
                Console.WriteLine($"After Status Filter: {filteredMatches.Count()} matches found.");
            }

            var result = filteredMatches.OrderBy(m => m.MatchDate).ToList();
            Console.WriteLine($"Final Count: {result.Count} matches");

            return result;
        }


        public List<Match> GetMatchesForMonth(int year, int month)
        {
            return _cachedMatches
                .Where(m => m.MatchDate.Year == year && m.MatchDate.Month == month)
                .OrderBy(m => m.MatchDate)
                .ToList();
        }

        public async Task<List<Match>> GetMatchesByStatus(int? teamId, string status)
        {
            using (var repo = new MatchManagementRepository())
            {
                return await repo.GetMatchesByStatus(teamId, status)?.ToListAsync() ?? new List<Match>();
            }
        }

        public async Task<List<Match>> GetMatchesByDate(int? teamId, DateTime startDate, DateTime endDate)
        {
            using (var repo = new MatchManagementRepository())
            {
                return await repo.GetMatchesByDate(teamId, startDate, endDate)?.ToListAsync() ?? new List<Match>();
            }
        }

        public bool AddMatch(Match match)
        {
            bool isSuccessful = false;
            using (var repo = new MatchManagementRepository())
            {
                int affectedRows = repo.AddNewMatch(match);
                isSuccessful = affectedRows > 0;
            }
            return isSuccessful;
        }

        public bool RemoveMatch(Match match) 
        {
            bool isSuccessful = false;
            using (var repo = new MatchManagementRepository())
            {
                int affectedRows = repo.DeleteMatch(match);
                isSuccessful = affectedRows > 0;
            }
            return isSuccessful;
        }

        public bool UpdateMatch(Match match)
        {
            bool isSuccessful = false;
            using (var repo = new MatchManagementRepository())
            {
                int affectedRows = repo.Update(match);
                isSuccessful = affectedRows > 0;
            }
            return isSuccessful;
        }

        public bool DoesMatchExist(int teamId, DateTime matchDate, TimeSpan startTime)
        {
            using (var repo = new MatchManagementRepository())
            {
                return repo.GetAllMatches().Any(m =>
                    m.TeamID == teamId &&
                    m.MatchDate == matchDate &&
                    m.StartTime == startTime);
            }
        }
    }
}
