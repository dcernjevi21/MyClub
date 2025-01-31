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
            //dodati za match id
            return Entities.Where(x => x.Training.TeamID == teamId);
        }

        public int AddNewAttendance(Attendance attendance)
        {
            Entities.Add(attendance);
            return SaveChanges();
        }

        public override int Update(Attendance entity, bool saveChanges = true)
        {
            var attendance = Entities.SingleOrDefault(x => x.AttendanceID == entity.AttendanceID);
            if (attendance != null)
            {
                attendance.TrainingID = entity.TrainingID;
                attendance.UserID = entity.UserID;
                attendance.StatusID = entity.StatusID;
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
    }
}
