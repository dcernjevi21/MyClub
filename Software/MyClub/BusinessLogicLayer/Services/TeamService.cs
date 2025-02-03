using DataAccessLayer.EntityRepository;
using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Valec kompletno
namespace BusinessLogicLayer.Services
{
    public class TeamService
    {
        public List<Team> GetTeams()
        {
            using (var repo = new TeamRepository())
            {
                List<Team> teams = repo.GetAll().ToList();
                return teams;
            }
        }
    }
}
