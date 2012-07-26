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
        public int Id { get { return this.GetHashCode(); } }

        public HashSet<Tuple<string, OpenBitSetDISI, Filter>> Values { get; private set; }

        public Facet(string uniqueName, string displayName, string field)
        {
            this.UniqueName = uniqueName;
            this.DisplayName = displayName;
            this.Field = field;

            this.Values = new HashSet<Tuple<string, OpenBitSetDISI, Filter>>();
        }

        public OpenBitSetDISI GetBitSetFromFilter(Filter filter, IndexReader indexReader)
        {
            var bitSet = new OpenBitSetDISI(filter.GetDocIdSet(indexReader).Iterator(), indexReader.MaxDoc());

            return bitSet;
        }

        public OpenBitSetDISI TermToBitSet(string term, IndexReader indexReader)
        {
            var facetQuery = new TermQuery(new Term(this.Field, term));
            var facetQueryFilter = new CachingWrapperFilter(new QueryWrapperFilter(facetQuery));
            var bitSet = new OpenBitSetDISI(facetQueryFilter.GetDocIdSet(indexReader).Iterator(), indexReader.MaxDoc());

            return bitSet;
        }

        public void AddValue(string value, OpenBitSetDISI matchingDocuments, Filter facetFilter)
        {
            this.Values.Add(new Tuple<string, OpenBitSetDISI, Filter>(value, matchingDocuments, facetFilter));
        }

        internal void Build(IndexReader indexReader)
        {
            indexReader.ExtractTermsForField(this.Field)
             .Select(x => new { Value = x.Item1, MatchingDocuments = this.GetBitSetFromFilter(x.Item2, indexReader), Filter = x.Item2 })
             .ToList()
             .ForEach(x => this.AddValue(x.Value, x.MatchingDocuments, x.Filter));
        }        

        public override int GetHashCode()
        {            
            return this.UniqueName.GetHashCode();
        }
        
        public override string ToString()
        {
            return this.UniqueName;
        }       
    }
}
