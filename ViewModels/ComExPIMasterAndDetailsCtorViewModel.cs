using System.Collections.Generic;
using System.Linq;

namespace DenimERP.ViewModels
{
    public class ComExPIMasterAndDetailsCtorViewModel
    {
        public List<ComExPIMasterAndDetailsViewModel> Items { get; set; }

        public int NumberOfItems
        {
            get => Items.Count();
        }

        public ComExPIMasterAndDetailsCtorViewModel()
        {
            Items = new List<ComExPIMasterAndDetailsViewModel>();
        }
    }
}
