using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.EntityRepositories
{
    public class UserRepository : Repository<User>
    {
        public UserRepository() : base(new MyClubModel())
        {
        }

        public int AddUser(User user)
        {
            return 1;
        }

        public override int Update(User entity, bool saveChanges = true)
        {
            var user = Entities.SingleOrDefault(x => x.UserID == entity.UserID);

            if (user != null)
            {
                user.FirstName = entity.FirstName;
                user.LastName = entity.LastName;
                user.Email = entity.Email;
                user.Username = entity.Username;
                user.Password = entity.Password;
                user.BirthDate = entity.BirthDate;
                user.RoleID = entity.RoleID;
                user.StatusID = entity.StatusID;
                user.TeamID = entity.TeamID;

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

        public int DeleteUser(User user)
        {
            return 1;
        }

        public IQueryable<User> GetAllUsers()
        {
            var query = from s in Entities 
                        select s;

            return query;
        }

        public IQueryable<User> GetUserByEmail(string email)
        {
            var query = from s in Entities 
                        where s.Email == email 
                        select s;

            return query;
        }

        public IQueryable<User> GetAllPendingUsers()
        {
            var query = from s in Entities
                        //where status = query ??
                        select s;

            return query;
        }
    }
}
