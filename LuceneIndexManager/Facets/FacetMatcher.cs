using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Util;
using ThrowHelper;

namespace LuceneIndexManager.Facets
{
    public class FacetMatcher
    {
        private Filter _query;
        private IndexReader _indexReader;
        private Filter _filter;

        public FacetMatcher(Filter query, IndexReader indexReader)
        {
            Throw.IfArgumentNull(query);
            Throw.IfArgumentNull(indexReader);

            this._query = query;
            this._indexReader = indexReader;
        }

        public FacetMatcher(Filter query, Filter filter, IndexReader indexReader)
        {
            Throw.IfArgumentNull(query);
            Throw.IfArgumentNull(indexReader);
            Throw.IfArgumentNull(filter);

            this._query = query;
            this._indexReader = indexReader;
            this._filter = filter;
        }

        public List<FacetMatch> GetAllMatches(List<Facet> facetsToMatch)
        {
            Throw.IfArgumentNull(facetsToMatch);

            var matches = facetsToMatch
                .SelectMany(facet => this.FindMatchesInQuery(facet, this._query, this._filter, this._indexReader))
                .ToList();

            return matches;
        }

        private List<FacetMatch> FindMatchesInQuery(Facet facet, Filter query, Filter filter, IndexReader indexReader)
        {
            var matches = facet.Values.Select(value =>
            {                
                var bitsQuery = new OpenBitSetDISI(query.GetDocIdSet(indexReader).Iterator(), indexReader.MaxDoc());
                bitsQuery.And(value.Item2);

                if (filter != null)
                {
                    //TODO: Remove this hard coded value (1000)
                    var bitsFilter = new OpenBitSetDISI(filter.GetDocIdSet(indexReader).Iterator(), 1000);
                    bitsQuery.And(bitsFilter);
                }

                var count = bitsQuery.Cardinality();

                return new FacetMatch() { Count = count, Value = value.Item1, Id = facet.Id };
            }).ToList();

            return matches;
        }
    }
}
