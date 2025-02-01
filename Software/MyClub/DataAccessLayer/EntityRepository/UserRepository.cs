﻿using BusinessLogicLayer.Services;
using EntitiesLayer.Entities;
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
            Entities.Add(user);
            return SaveChanges();
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
                user.ProfilePicture = entity.ProfilePicture;

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
            Entities.Attach(user);
            Entities.Remove(user);
            return SaveChanges();
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

        public IQueryable<User> GetUsersByRoleId(int roleId)
        {
            var query = from s in Entities
                        where s.RoleID == roleId
                        select s;

            return query;
        }

        public IQueryable<User> GetAllPendingUsers()
        {
            var query = from u in Entities
                        where u.StatusID == (int)UserStatus.Pending
                        select u;

            return query;
        }

        public IQueryable<User> GetUsersByTeamId(int teamId)
        {
            var query = from u in Entities
                        where u.TeamID == teamId &&
                              u.StatusID == (int)UserStatus.Accepted &&
                              u.RoleID == 3
                        select u;
            return query;
        }

    }
}