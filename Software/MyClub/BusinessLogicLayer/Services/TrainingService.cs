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
        private List<Training> _cachedTrainings = new List<Training>();

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
               
                    _cachedTrainings = repo.GetAll()
                                               .Where(t => t.TeamID == teamId)
                                               .OrderBy(t => t.TrainingDate)
                                               .ToList();
                return _cachedTrainings;
            }
        }

        public List<Training> GetTrainingsForMonth(int year, int month)
        {
            return _cachedTrainings
                .Where(t => t.TrainingDate.Year == year && t.TrainingDate.Month == month)
                .OrderBy(t => t.TrainingDate)
                .ToList();
        }

        public List<Training> FilterTrainings(DateTime? startDate, DateTime? endDate)
        {
            var filteredTrainigs = _cachedTrainings.AsQueryable();
            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;

            if (startDate.HasValue && endDate.HasValue)
            {
                filteredTrainigs = filteredTrainigs
                    .Where(m => m.TrainingDate >= startDate.Value && m.TrainingDate <= endDate.Value)
                    .OrderBy(m => m.TrainingDate);
            }

            var result = filteredTrainigs.ToList();

            return result;
        }

        public bool AddTraining(Training training)
        {
            using (var repo = new TrainingRepository())
            {
                int affectedRows = repo.Add(training);
                if (affectedRows > 0)
                {
                    _cachedTrainings = new List<Training>();
                    return true;
                }
                return false;
            }
        }

        public bool UpdateTraining(Training training)
        {
            using (var repo = new TrainingRepository())
            {
                int affectedRows = repo.Update(training);
                if (affectedRows > 0)
                {
                    _cachedTrainings = new List<Training>();
                    return true;
                }
                return false;
            }
        }

        public bool RemoveTraining(Training training)
        {
            using (var repo = new TrainingRepository())
            {
                int affectedRows = repo.Remove(training);
                if (affectedRows > 0)
                {
                    _cachedTrainings = new List<Training>();
                    return true;
                }
                return false;
            }
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
