
using System;
using System.IO;
namespace LuceneIndexManager.Facets
{
    public class FacetDefinition
    {
        public string UniqueName { get; private set; }
        public string DisplayName { get; private set; }
        public string Field { get; private set; }
        private string Location { get; set; }

        public string FacetFile
        {
            get
            {
                return Path.Combine(this.Location, this.UniqueName + ".facet");
            }
        }
        
        public FacetDefinition(string uniqueName, string displayName, string field, string location)
        {
            this.UniqueName = uniqueName;
            this.DisplayName = displayName;
            this.Field = field;
            this.Location = location;
        }

        public override string ToString()
        {
            return String.Format("UniqueName: {0}, Field: {1}, Display Name: {2}, Location: {3}", this.UniqueName, this.Field, this.DisplayName, this.Location);
        }

        public override int GetHashCode()
        {
            return this.UniqueName.GetHashCode();
        }
    }
}
