using System.Collections.Generic;

namespace HRMS.ViewModels
{
    public class DataTableObject<T> where T : class
    {
        public string Draw { get; }
        public int RecordsFiltered { get; }
        public int RecordsTotal { get; }
        public List<T> Data { get; }

        public DataTableObject(string draw, int recordsFiltered, int recordsTotal, List<T> data)
        {
            Draw = draw;
            RecordsFiltered = recordsFiltered;
            RecordsTotal = recordsTotal;
            Data = data;
        }
    }
}
