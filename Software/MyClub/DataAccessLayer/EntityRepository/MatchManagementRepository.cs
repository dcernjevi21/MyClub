using DataAccessLayer.EntityRepositories;
using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.EntityRepository
{
    public class MatchManagementRepository : Repository<Match>
    {
        public MatchManagementRepository() : base(new MyClubModel())
        {
        }

        public int AddNewMatch(Match match)
        {
            Entities.Add(match);
            return SaveChanges();
        }

        public IQueryable<Match> GetAllMatches()
        {
            var query = from t in Entities.Include("Team")
                        orderby
                        t.Status == "Scheduled" && t.MatchDate >= DateTime.Now ? 0 : 1, //buduće utakmice (0), prošle (1)
                        t.MatchDate ascending,  //(1) od najbližeg do najdaljeg
                        t.MatchDate descending  //(0) od najnovijeg do najstarijeg
                        select t;
            return query;
        }

        public IQueryable<Match> GetMatchById(int matchId)
        {
            return Entities.Where(x => x.MatchID == matchId).Include("Team");
        }

        public IQueryable<Match> GetMatchesByTeamId(int teamId)
        {
            return Entities.Where(x => x.TeamID == teamId).Include("Team");
        }

        public IQueryable<Match> GetMatchesByStatus(int? teamId, string status)
        {
            var query = Entities.Where(x => x.Status == status);
            if (teamId.HasValue)
            {
                query = query.Where(x => x.TeamID == teamId.Value);
            }

            return query.Include("Team");
        }

        public IQueryable<Match> GetMatchesByDate(int? teamId, DateTime startDate, DateTime endDate)
        {
            var query = Entities.Where(x => x.MatchDate >= startDate && x.MatchDate <= endDate);

            if (teamId.HasValue)
            {
                query = query.Where(x => x.TeamID == teamId.Value);
            }

            return query.Include("Team");
        }



        public int DeleteMatch(Match match)
        {
            if (Entities.Local.Contains(match))
            {
                Entities.Remove(match);
            }
            else
            {
                Entities.Attach(match);
                Entities.Remove(match);
            }
            return SaveChanges();
        }

        public override int Update(Match entity, bool saveChanges = true)
        {
            var match = Entities.SingleOrDefault(x => x.MatchID == entity.MatchID);
            if (match != null)
            {
                match.TeamID = entity.TeamID;
                match.MatchDate = entity.MatchDate;
                match.OpponentTeam = entity.OpponentTeam;
                match.Location = entity.Location;
                match.Result = entity.Result;
                match.StartTime = entity.StartTime;
                match.Summary = entity.Summary;
                match.Status = entity.Status;

                if (saveChanges)
                {
                    return SaveChanges();
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
    }
}
