using DataAccessLayer.EntityRepositories;
using EntitiesLayer.Entities;
using System;
using System.Linq;

namespace DataAccessLayer.EntityRepository
{
    public class MembershipRepository : Repository<Membership>
    {
        public MembershipRepository() : base(new MyClubModel()) { }

        public IQueryable<Membership> GetAllMemberships()
        {
            return Entities;
        }

        public IQueryable<Membership> GetMembershipsByMonth(DateTime month)
        {
            var query = from m in Entities
                        where m.Month.Month == month.Month && m.Month.Year == month.Year
                        select m;

            return query;
        }

        public int MarkAsPaid(int membershipId)
        {
            var query = from m in Entities
                        where m.MembershipID == membershipId
                        select m;

            var membership = query.FirstOrDefault();

            if (membership != null)
            {
                membership.Paid = true;
                return SaveChanges();
            }

            return 0;
        }


        public int MarkAllAsNotPaid()
        {
            var currentMonth = DateTime.Now;

            var memberships = from m in Entities
                              where m.Month.Month == currentMonth.Month && m.Month.Year == currentMonth.Year
                              select m;

            foreach (var membership in memberships)
            {
                membership.Paid = false;
            }

            return SaveChanges();
        }

        public override int Update(Membership entity, bool saveChanges = true)
        {
            var query = from m in Entities
                        where m.MembershipID == entity.MembershipID
                        select m;

            var existingMembership = query.FirstOrDefault();

            if (existingMembership == null)
                return 0;

            Context.Entry(existingMembership).CurrentValues.SetValues(entity);

            return saveChanges ? SaveChanges() : 0;
        }
        public IQueryable<User> GetEligiblePlayers()
        {
            return Context.Users.Where(u => u.RoleID == 3);
        }

        public int AddMembershipForUser(int userId, decimal amount, DateTime month)
        {
            var membership = new Membership
            {
                UserID = userId,
                Amount = amount,
                Month = month,
                Paid = false
            };

            Entities.Add(membership);
            return SaveChanges();
        }
    }
}
