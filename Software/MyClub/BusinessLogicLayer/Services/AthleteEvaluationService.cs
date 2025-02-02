using DataAccessLayer.EntityRepository;
using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class AthleteEvaluationService
    {
        private readonly AthleteEvaluationRepository _evaluationRepository;

        public AthleteEvaluationService()
        {
            _evaluationRepository = new AthleteEvaluationRepository();
        }

        public List<AthleteEvaluation> GetEvaluationsForAthlete(int userId)
        {
            return _evaluationRepository.GetEvaluationsForAthlete(userId).ToList();
        }

        public bool AddEvaluation(AthleteEvaluation evaluation)
        {
            int result = _evaluationRepository.AddEvaluation(evaluation);
            return result > 0;
        }
    }
}
