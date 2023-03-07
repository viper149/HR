using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.HR
{
    public class FBasHrdDesignationViewModel
    {
        public FBasHrdDesignationViewModel()
        {
            FBasHrdGradeList = new List<F_BAS_HRD_GRADE>();
        }

        public F_BAS_HRD_DESIGNATION FBasHrdDesignation { get; set; }

        public List<F_BAS_HRD_GRADE> FBasHrdGradeList { get; set; }
    }
}