using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATHub.Web.Areas.Administrator.Models
{
    public class AllUsersViewModel
    {
        public ICollection<ManageUserViewModel> Users { get; set; }
    }
}
