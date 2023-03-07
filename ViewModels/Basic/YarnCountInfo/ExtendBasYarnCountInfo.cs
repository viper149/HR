using System.ComponentModel.DataAnnotations;
using DenimERP.Models;

namespace DenimERP.ViewModels.Basic.YarnCountInfo
{
    public class ExtendBasYarnCountInfo : BAS_YARN_COUNTINFO
    {
        [RegularExpression("^([A-Z0-9,-:()%+]+ )+[A-Z0-9,-:()%+]+$|^[A-Z0-9,-:()%+]+$",
            ErrorMessage = "<ol class=\"list-group mt-2\">" +
                               "<li class=\"list-group-item list-group-item-primary\">Invalid Input Format. Accepts:" +
                                   "<ul class=\"list-group mt-2\">" +
                                       "<li class=\"list-group-item list-group-item-primary\">Capital Letters.</li>" +
                                       "<li class=\"list-group-item list-group-item-primary\">Symbols <strong class=\"text-info\">-:()%+</strong></li>" +
                                       "<li class=\"list-group-item list-group-item-primary\">Space Between Two Words.</li>" +
                                    "</ul>" +
                               "</li>" +
                           "</ol>")]

        public override string COUNTNAME { get; set; }
    }
}
