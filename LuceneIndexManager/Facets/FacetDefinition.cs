
namespace LuceneIndexManager.Facets
{
    public class FacetDefinition
    {
        public string UniqueName { get; private set; }
        public string DisplayName { get; private set; }
        public string Field { get; private set; }
        
        public FacetDefinition(string uniqueName, string displayName, string field)
        {
            this.UniqueName = uniqueName;
            this.DisplayName = displayName;
            this.Field = field;
        }
    }
}
