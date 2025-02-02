﻿using DataAccessLayer.EntityRepositories;
using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DataAccessLayer.EntityRepository
{
    public class AthleteEvaluationRepository : Repository<AthleteEvaluation>
    {
        public AthleteEvaluationRepository() : base (new MyClubModel()) { }
        
        public IQueryable<AthleteEvaluation> GetEvaluationsForAthlete(int userId)
        {
            var query = from eval in Entities.Include(e => e.User)
                        where eval.UserID == userId
                        select eval;
            return query;
        }

        public int AddEvaluation(AthleteEvaluation evaluation)
        {
            Entities.Add(evaluation);
            return SaveChanges();
        }

        public override int Update(AthleteEvaluation entity, bool saveChanges = true)
        {
            throw new NotImplementedException();
        }
    }
}
