using DataAccessLayer.EntityRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataAccessLayer;

namespace BusinessLogicLayer.Services
{
    public class CoachEvaluationService
    {
        private readonly UserRepository _userRepository;

        public CoachEvaluationService()
        {
            _userRepository = new UserRepository();
        }

        public List<User> GetPlayersForTeam(int teamId)
        {
            return _userRepository.GetUsersByTeamId(teamId).ToList();
        }
    }
}
