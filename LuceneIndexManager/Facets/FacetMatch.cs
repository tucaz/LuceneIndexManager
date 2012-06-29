using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuceneIndexManager.Facets
{
    public class FacetMatch
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public long Count { get; set; }
    }
}
