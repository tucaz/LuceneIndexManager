
using System;
using System.IO;
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

        public override string ToString()
        {
            return String.Format("UniqueName: {0}, Field: {1}, Display Name: {2}", this.UniqueName, this.Field, this.DisplayName);
        }

        public override int GetHashCode()
        {
            return this.UniqueName.GetHashCode();
        }
    }
}
