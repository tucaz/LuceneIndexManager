using Lucene.Net.Search;
using Lucene.Net.Util;

namespace LuceneIndexManager.Facets
{
    public class Facet
    {
        public string UniqueName { get; private set; }
        public string DisplayName { get; private set; }
        public string Field { get; private set; }
        public string Value { get; private set; }
        public OpenBitSetDISI MatchingDocuments { get; private set; }

        public Facet(string uniqueName, string displayName, string field, string value, OpenBitSetDISI matchingDocuments)
        {
            this.UniqueName = uniqueName;
            this.DisplayName = displayName;
            this.Field = field;
            this.Value = value;
            this.MatchingDocuments = matchingDocuments;
        }    
    }
}
