using DataAccessLayer;
using DataAccessLayer.EntityRepository;
using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class MembershipService
    {
        private readonly MembershipRepository _membershipRepository;

        public MembershipService()
        {
            _membershipRepository = new MembershipRepository();
        }

        public List<Membership> GetAllMemberships() 
        { 
            return _membershipRepository.GetAllMemberships().ToList();
        }

        public IQueryable<Membership> GetMembershipsByMonth(DateTime month)
        {
            return _membershipRepository.GetMembershipsByMonth(month);
        }

        public bool MarkAsPaid(int membershipId)
        {
            var result = _membershipRepository.MarkAsPaid(membershipId);
            return result > 0;
        }

        public bool UpdateMembership(Membership membership)
        {
            var result = _membershipRepository.Update(membership);
            return result > 0;
        }

        public bool MarkAllAsNotPaid()
        {
            var result = _membershipRepository.MarkAllAsNotPaid();
            return result > 0;
        }

        public bool AssignMembershipsToAllMembers(decimal amount)
        {
            var users = _membershipRepository.GetEligiblePlayers();
            DateTime currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            bool isSuccessful = true;

            foreach (var user in users)
            {
                int result = _membershipRepository.AddMembershipForUser(user.UserID, amount, currentMonth);
                if (result == 0)
                    isSuccessful = false;
            }

            return isSuccessful;
        }
    }
}
