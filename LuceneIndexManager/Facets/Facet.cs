using System;
using System.Collections.Generic;
using Lucene.Net.Search;
using Lucene.Net.Util;

namespace LuceneIndexManager.Facets
{
    public class Facet
    {
        public string UniqueName { get; private set; }
        public string DisplayName { get; private set; }
        public string Field { get; private set; }

        public HashSet<Tuple<string, OpenBitSetDISI>> Values { get; private set; }

        public Facet(string uniqueName, string displayName, string field)
        {
            this.UniqueName = uniqueName;
            this.DisplayName = displayName;
            this.Field = field;

            this.Values = new HashSet<Tuple<string, OpenBitSetDISI>>();
        }

        public void AddValue(string value, OpenBitSetDISI matchingDocuments)
        {
            this.Values.Add(new Tuple<string,OpenBitSetDISI>(value, matchingDocuments));
        }

        public override int GetHashCode()
        {
            return this.UniqueName.GetHashCode();
        }
    }
}
