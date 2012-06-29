using System.Collections.Generic;
using Lucene.Net.Index;
using LuceneIndexManager.Util;

namespace LuceneIndexManager.Facets
{
    internal class FacetBuilder
    {
        private IndexReader IndexReader { get; set; }

        public FacetBuilder(IndexReader indexReader)
        {
            this.IndexReader = indexReader;
        }

        public List<Facet> CreateFacets(List<FacetDefinition> facetsToCreate)
        {
            "Facets to create:".Info();
            facetsToCreate.ForEach(x => x.ToString().Info());

            var createdFacets = new List<Facet>();
            
            foreach (var facet in facetsToCreate)
            {
                var createdFacet = CreateFacet(facet);
                createdFacets.Add(createdFacet);
            }

            "Facets created".Info();

            return createdFacets;
        }

        private Facet CreateFacet(FacetDefinition definition)
        {
            var facet = new Facet(definition.UniqueName, definition.DisplayName, definition.Field);
            facet.Build(this.IndexReader);

            return facet;
        }
    }
}
