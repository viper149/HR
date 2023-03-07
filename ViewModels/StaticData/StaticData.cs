using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DenimERP.ViewModels.StaticData
{
    public static class StaticData
    {
        public static SelectList GetCurrency()
        {
            var currency = new Dictionary<string, string> { { "US$", "US$" }, { "EUR€", "EUR€" } };

            return new SelectList(currency.Select(r => new SelectListItem
            {
                Value = r.Key.ToString(),
                Text = r.Value
            }), "Value", "Text");
        }

        public static SelectList GetStatus()
        {
            var status = new Dictionary<string, string> { { "YES", "Yes" }, { "NO", "No" } };

            return new SelectList(status.Select(r => new SelectListItem
            {
                Value = r.Key.ToString(),
                Text = r.Value
            }), "Value", "Text");
        }

        public static SelectList GetYarnFor()
        {
            var status = new Dictionary<string, string> { { "Sample", "Sample" }, { "Export", "Bulk" }, { "Projection", "Projection" }, { "Lease Yarn", "Lease Yarn" }, { "LC/Work Order", "LC/Work Order" }, {"Leader", "Leader"} };

            return new SelectList(status.Select(r => new SelectListItem
            {
                Value = r.Key.ToString(),
                Text = r.Value
            }), "Value", "Text");
        }

        public static SelectList GetSexualOrientation()
        {
            var currency = new Dictionary<string, string> { { "Male", "Male" }, { "Female", "Female" }, { "Others", "Others" } };

            return new SelectList(currency.Select(r => new SelectListItem
            {
                Value = r.Key.ToString(),
                Text = r.Value
            }), "Value", "Text");
        }

        public static SelectList GetMaritalStatus()
        {
            var currency = new Dictionary<string, string> { { "Single", "Single" }, { "Married", "Married" }, { "Widowed", "Widowed" }, { "Separated", "Separated" }, { "Divorced", "Divorced" } };

            return new SelectList(currency.Select(r => new SelectListItem
            {
                Value = r.Key.ToString(),
                Text = r.Value
            }), "Value", "Text");
        }

        public static SelectList GetEmployeeType()
        {
            var currency = new Dictionary<string, string> { { "Full Time", "Full Time" }, { "Part Time", "Part Time" }, { "Contractual", "Contractual" } };

            return new SelectList(currency.Select(r => new SelectListItem
            {
                Value = r.Key.ToString(),
                Text = r.Value
            }), "Value", "Text");
        }

        public static SelectList GetMFSType()
        {
            var currency = new Dictionary<string, string> { { "bKash", "bKash" }, { "Rocket", "Rocket" }, { "Nagad", "Nagad" }, { "SureCash", "SureCash" }, { "mCash", "mCash" }, { "MyCash", "MyCash" }, { "Upay", "Upay" } };

            return new SelectList(currency.Select(r => new SelectListItem
            {
                Value = r.Key.ToString(),
                Text = r.Value
            }), "Value", "Text");
        }

        public static SelectList GetExamType()
        {
            var currency = new Dictionary<string, string> { { "SSC", "SSC" }, { "HSC", "HSC" }, { "BSc", "BSc" }, { "MSc", "MSc" }, { "BA", "BA" }, { "MA", "MA" }, { "MCom", "MCom" }, { "BBA", "BBA" }, { "MBA", "MBA" } };

            return new SelectList(currency.Select(r => new SelectListItem
            {
                Value = r.Key.ToString(),
                Text = r.Value
            }), "Value", "Text");
        }

        public static SelectList GetAdditionalRelation()
        {
            var currency = new Dictionary<string, string> { { "Father", "Father" }, { "Mother", "Mother" }, { "Brother", "Brother" }, { "Sister", "Sister" }, { "Wife", "Wife" }, { "Son", "Son" }, { "Daughter", "Daughter" } };

            return new SelectList(currency.Select(r => new SelectListItem
            {
                Value = r.Key.ToString(),
                Text = r.Value
            }), "Value", "Text");
        }

        public static SelectList GetSeasons()
        {
            var seasons = new Dictionary<string, string> { { "Spring", "Spring" }, { "Summer", "Summer" }, { "Autumn/Fall", "Autumn/Fall" }, { "Winter", "Winter" } };

            return new SelectList(seasons.Select(r => new SelectListItem
            {
                Text = r.Key.ToString(),
                Value = r.Value.ToString()
            }), "Value", "Text");
        }
        public static SelectList GetOrderTypes()
        {
            var oderTypes = new Dictionary<string, string> { { "Organic", "Organic" } };

            return new SelectList(oderTypes.Select(r => new SelectListItem
            {
                Text = r.Key.ToString(),
                Value = r.Value.ToString()
            }), "Value", "Text");
        }
    }
}
