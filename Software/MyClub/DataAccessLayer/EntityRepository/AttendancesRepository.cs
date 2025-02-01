using DataAccessLayer;
using DataAccessLayer.EntityRepositories;
using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class AttendancesRepository : Repository<Attendance>
    {
        public AttendancesRepository() : base(new MyClubModel())
        {
        }

        public IQueryable<Attendance> GetAllAttendances()
        {
            return GetAll();
        }

        public IQueryable<Attendance> GetTrainingAttendanceById(int teamId)
        {
            return Entities.Where(x => x.Training.TeamID == teamId);
        }
        
        public IQueryable<Attendance> GetMatchAttendanceById(int teamId)
        {
            return Entities.Where(x => x.Match.TeamID == teamId);
        }

        public int AddNewAttendance(Attendance attendance)
        {
            Entities.Add(attendance);
            return SaveChanges();
        }

        public IQueryable<Attendance> GetAttendancesByTrainingId(int trainingId)
        {
            return Entities.Where(x => x.TrainingID == trainingId)
                           .Include(x => x.User)
                           .Include(x => x.Status);
        }

        public IQueryable<Attendance> GetAttendancesByMatchId(int matchId)
        {
            return Entities.Where(x => x.MatchId == matchId)
                           .Include(x => x.User)
                           .Include(x => x.Status);
        }

        public override int Update(Attendance entity, bool saveChanges = true)
        {
            var attendance = Entities.SingleOrDefault(x => x.AttendanceID == entity.AttendanceID);
            if (attendance != null)
            {
                attendance.TrainingID = entity.TrainingID;
                attendance.UserID = entity.UserID;
                attendance.StatusID = entity.StatusID;
                attendance.Notes = entity.Notes;
                attendance.MatchId = entity.MatchId;

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

        public IQueryable<Attendance> GetUserAttendances(int userId)
        {
            return Entities.Where(x => x.UserID == userId)
                           .Include(x => x.Training)
                           .Include(x => x.Match)
                           .Include(x => x.Status)
                           .OrderByDescending(x => x.Training != null ? x.Training.TrainingDate : x.Match.MatchDate);
        }

        public IQueryable<Attendance> GetTeamAttendancesForPeriod(int teamId, DateTime startDate, DateTime endDate)
        {
            return Entities.Where(a =>
                (a.Training != null && a.Training.TeamID == teamId &&
                 a.Training.TrainingDate >= startDate && a.Training.TrainingDate <= endDate) ||
                (a.Match != null && a.Match.TeamID == teamId &&
                 a.Match.MatchDate >= startDate && a.Match.MatchDate <= endDate))
                .Include(a => a.Training)
                .Include(a => a.Match)
                .Include(a => a.User)
                .Include(a => a.Status);
        }
    }
}
