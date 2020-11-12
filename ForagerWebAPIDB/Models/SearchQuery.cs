using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForagerWebAPIDB.Models
{
    public class SearchQuery
    {
        public string Query { get; set; }
        public int Results { get; set; }

        public IList<Listing> Listings { get; set; }


        public SearchQuery()
        {
            Listings = new List<Listing>();
        }
    }
}
