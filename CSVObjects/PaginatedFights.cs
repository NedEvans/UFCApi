using System.Collections.Generic;

namespace UFCApi.CSVObjects
{
    public class PaginatedFights
    {
        public List<FightCsv> Fights { get; set; } = new List<FightCsv>();
        public int TotalCount { get; set; }
    }
}