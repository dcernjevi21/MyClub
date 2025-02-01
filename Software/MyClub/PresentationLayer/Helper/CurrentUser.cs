using DataAccessLayer;
using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.Helper
{
    // CurrentUser class holds the information of the logged in user
    public static class CurrentUser
    {
        public static User User { get; set; }
    }

}
