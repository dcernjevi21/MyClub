using DataAccessLayer.EntityRepositories;
using DataAccessLayer.EntityRepository;
using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Valec kompletno
namespace BusinessLogicLayer
{
    public class TrainingService
    {
        public List<Training> GetTrainings()
        {
            using (var repo = new TrainingRepository())
            {
                List<Training> trainings = repo.GetAll().OrderBy(t => t.TrainingDate).ToList();
                return trainings;
            }
        }

        public List<Training> GetTrainingsForTeam(int teamId)
        {
            using (var repo = new TrainingRepository())
            {
                List<Training> trainings = repo.GetAll()
                                               .Where(t => t.TeamID == teamId)
                                               .OrderBy(t => t.TrainingDate)
                                               .ToList();
                return trainings;
            }
        }


        public bool AddTraining(Training training)
        {
            bool isSuccessful = false;

            using (var repo = new TrainingRepository())
            {
                int affectedRows = repo.Add(training);
                isSuccessful = affectedRows > 0;
            }

            return isSuccessful;
        }

        public bool UpdateTraining(Training training)
        {
            bool isSuccessful = false;

            using (var repo = new TrainingRepository())
            {
                int affectedRows = repo.Update(training);
                isSuccessful = affectedRows > 0;
            }

            return isSuccessful;
        }

        public bool RemoveTraining(Training training)
        {
            bool isSuccessful = false;

            using (var repo = new TrainingRepository())
            {
                int affectedRows = repo.Remove(training);
                isSuccessful = affectedRows > 0;
            }

            return isSuccessful;
        }

        public List<Team> GetTeams()
        {
            using (var repo = new TrainingRepository())
            {
                return repo.GetAll()
                    .Select(t => t.Team)
                    .Distinct()
                    .ToList(); 
            }
        }

        //Černjević
        public bool DoesTrainingExist(int teamId, DateTime matchDate, TimeSpan startTime)
        {
            using (var repo = new TrainingRepository())
            {
                return repo.GetAll().Any(t =>
                    t.TeamID == teamId &&
                    t.TrainingDate == matchDate &&
                    t.StartTime == startTime);
            }
        }
    }
}
