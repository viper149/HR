using System.ComponentModel.DataAnnotations;

namespace HRMS.ViewModels.MenuMaster
{
    public class ExtendMenuMasterViewModel : MenuMasterViewModel
    {
        [Display(Name = "Menu Id")]
        public string MenuID { get; set; }
        [Display(Name = "Menu Name")]
        public string MenuName { get; set; }

        public new Models.MenuMaster MenuMaster { get; set; }
    }
}
