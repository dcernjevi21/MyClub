using DataAccessLayer.EntityRepositories;
using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
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
            return GetAll();
        }

        public IQueryable<Match> GetMatchById(int matchId)
        {
            return Entities.Where(x => x.MatchID == matchId);
        }

        public int DeleteMatch(Match match)
        {
            Entities.Remove(match);
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
