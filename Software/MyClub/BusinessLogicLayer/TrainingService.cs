using DataAccessLayer.EntityRepositories;
using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    class TrainingService
    {
        public List<Training> GetTrainings()
        {
            using (var repo = new TrainingRepository())
            {
                List<Training> trainings = repo.GetAllTrainings().ToList();
            }
        }
    }
}
