using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class AttendanceService
    {
        public List<Attendance> GetAttendances()
        {
            using (var repo = new AttendancesRepository())
            {
                return repo.GetAllAttendances().ToList();
            }
        }

        public List<Attendance> GetTrainingAttendanceById(int teamId)
        {
            using (var repo = new AttendancesRepository())
            {
                return repo.GetTrainingAttendanceById(teamId).ToList();
            }
        }

        public bool AddAttendance(Attendance attendance)
        {
            bool isSuccessful = false;
            using (var repo = new AttendancesRepository())
            {
                int affectedRows = repo.AddNewAttendance(attendance);
                isSuccessful = affectedRows > 0;
            }
            return isSuccessful;
        }

        public bool UpdateAttendance(Attendance attendance)
        {
            bool isSuccessful = false;
            using (var repo = new AttendancesRepository())
            {
                int affectedRows = repo.Update(attendance);
                isSuccessful = affectedRows > 0;
            }
            return isSuccessful;
        }
        public List<Attendance> GetAttendancesByTrainingId(int trainingId)
        {
            using (var repo = new AttendancesRepository())
            {
                return repo.GetAttendancesByTrainingId(trainingId).ToList();
            }
        }
    }
}
