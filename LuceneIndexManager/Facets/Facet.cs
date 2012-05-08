using Lucene.Net.Search;

namespace LuceneIndexManager.Facets
{
    public class Facet
    {
        public string UniqueName { get; private set; }
        public string DisplayName { get; private set; }
        public string Field { get; private set; }
        public string Value { get; private set; }
        public DocIdSet MatchingDocuments { get; private set; }

        public Facet(string uniqueName, string displayName, string field, string value, DocIdSet matchingDocuments)
        {
            this.UniqueName = uniqueName;
            this.DisplayName = displayName;
            this.Field = field;
            this.Value = value;
            this.MatchingDocuments = matchingDocuments;
        }    
    }
}
