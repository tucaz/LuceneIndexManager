using System.Collections.Generic;
using Lucene.Net.Search;

namespace LuceneIndexManager.Facets
{
    public class FacetSearchResult
    {
        public List<FacetMatch> Facets { get; set; }
        public Hits Hits { get; set; }
    }
}
