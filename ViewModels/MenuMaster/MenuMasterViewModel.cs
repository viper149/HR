using System.Collections.Generic;

namespace HRMS.ViewModels.MenuMaster
{
    public class MenuMasterViewModel
    {
        public MenuMasterViewModel()
        {
            UserRolesViewModels = new List<UserRolesViewModel>();
            MenuMasters = new List<Models.MenuMaster>();
        }

        public Models.MenuMaster MenuMaster { get; set; }

        public List<Models.MenuMaster> MenuMasters { get; set; }
        public List<UserRolesViewModel> UserRolesViewModels { get; set; }
    }
}
