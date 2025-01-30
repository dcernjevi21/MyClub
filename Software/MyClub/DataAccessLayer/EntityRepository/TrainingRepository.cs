using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.EntityRepositories
{
    public class TrainingRepository : Repository<Training>
    {
        public TrainingRepository() : base(new MyClubModel())
        {
        }

        public override int Add(Training entity, bool saveChanges = true)
        {
            var training = new Training
            {
                TrainingDate = entity.TrainingDate,
                StartTime = entity.StartTime,
                EndTime = entity.EndTime,
                TeamID = entity.TeamID
            };

            Entities.Add(training);
            if(saveChanges)
            {
                return SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public override int Update(Training entity, bool saveChanges = true)
        {
            var training = Entities.SingleOrDefault(x => x.TrainingID == entity.TrainingID);

            training.TrainingDate = entity.TrainingDate;
            training.StartTime = entity.StartTime;
            training.EndTime = entity.EndTime;
            training.TeamID = entity.TeamID;

            if (saveChanges)
            {
                return SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public override IQueryable<Training> GetAll()
        {
            var query = from t in Entities.Include("Team")
                        select t;
            return query;
        }
    }
}