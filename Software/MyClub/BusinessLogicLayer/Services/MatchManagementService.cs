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
                return await repo.GetMatchesByTeamId(teamId)?.ToListAsync() ?? new List<Match>();
            }
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
