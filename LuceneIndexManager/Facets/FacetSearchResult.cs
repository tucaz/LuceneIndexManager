using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Search;

namespace LuceneIndexManager.Facets
{
    public class FacetSearchResult
    {
        public List<FacetMatch> Facets { get; set; }
        public Hits Hits { get; set; }
    }
}
