using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Index;
using Lucene.Net.Search;

namespace LuceneIndexManager.Facets
{
    internal class FacetBuilder
    {
        private IIndexDefinition Index { get; set; }

        public FacetBuilder(IIndexDefinition index)
        {
            this.Index = index;
        }

        public List<Facet> CreateFacets(List<FacetDefinition> facetsToCreate)
        {
            var createdFacets = new List<Facet>();

            //TODO: Add checking to make sure duplicated facets are not created
            foreach (var facet in facetsToCreate)
            {
                var createdFacet = CreateFacet(facet);
                createdFacets.AddRange(createdFacet);
            }

            return createdFacets;
        }

        private List<Facet> CreateFacet(FacetDefinition definition)
        {
            var indexReader = this.Index.GetIndexSearcher().GetIndexReader();
            var allTerms = this.GetAllTermsForField(definition.Field, indexReader);

            var facets = allTerms.Select(x =>
            {
                var facetQuery = new TermQuery(new Term(definition.Field, x));
                var facetQueryFilter = new CachingWrapperFilter(new QueryWrapperFilter(facetQuery));
                var genreBitArray = facetQueryFilter.GetDocIdSet(indexReader);

                var facet = new Facet(definition.UniqueName, definition.DisplayName, definition.Field, x, genreBitArray);
                return facet;

            }).ToList();

            return facets;
        }

        //TODO: This is internal just to be available for testing. Need to turn it to private and find another way to test it
        internal List<string> GetAllTermsForField(string field, IndexReader reader)
        {
            var allterms = new List<string>();
            var termReader = reader.Terms(new Term(field, String.Empty));

            do
            {
                if (termReader.Term().Field() != field)
                    break;

                allterms.Add(termReader.Term().Text());
            }
            while (termReader.Next());

            return allterms;
        }
    }
}
