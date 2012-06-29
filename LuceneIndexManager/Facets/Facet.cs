using System;
using System.Collections.Generic;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Util;
using System.Linq;
using LuceneIndexManager.Util;

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

        public OpenBitSetDISI TermToBitSet(string term, IndexReader indexReader)
        {
            var facetQuery = new TermQuery(new Term(this.Field, term));
            var facetQueryFilter = new CachingWrapperFilter(new QueryWrapperFilter(facetQuery));
            var bitSet = new OpenBitSetDISI(facetQueryFilter.GetDocIdSet(indexReader).Iterator(), indexReader.MaxDoc());

            return bitSet;
        }

        public void AddValue(string value, OpenBitSetDISI matchingDocuments)
        {
            this.Values.Add(new Tuple<string, OpenBitSetDISI>(value, matchingDocuments));
        }

        internal void Build(IndexReader indexReader)
        {
            indexReader.ExtractTermsForField(this.Field)
             .Select(x => new { Value = x, MatchingDocuments = this.TermToBitSet(x, indexReader) })
             .ToList()
             .ForEach(x => this.AddValue(x.Value, x.MatchingDocuments));
        }

        public override int GetHashCode()
        {            
            return this.UniqueName.GetHashCode();
        }
    }
}
