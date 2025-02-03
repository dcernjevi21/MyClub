using DataAccessLayer.EntityRepositories;
using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Valec kompletno
namespace DataAccessLayer.EntityRepository
{
    public class TeamRepository : Repository<Team>
    {
        public TeamRepository() : base(new MyClubModel())
        {
        }

        public override IQueryable<Team> GetAll()
        {
            var query = from t in Entities
                        select t;
            return query;
        }

        public override int Update(Team entity, bool saveChanges = true)
        {
            throw new NotImplementedException();
        }
    }
}
