using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Util;
using LuceneIndexManager.Util;

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
            "Facets to create:".Info();
            facetsToCreate.ForEach(x => x.ToString().Info());

            var createdFacets = new List<Facet>();

            //TODO: Add checking to make sure duplicated facets are not created
            foreach (var facet in facetsToCreate)
            {
                var createdFacet = CreateFacet(facet);
                createdFacets.Add(createdFacet);
            }

            "Facets created".Info();
            "Storing Facets".Info();

            foreach (var facet in createdFacets)
            {
                var definition = facetsToCreate.First(x => x.GetHashCode() == facet.GetHashCode());
                StoreFacet(facet, definition.FacetFile);
            }

            "Facets Stored".Info();

            return createdFacets;
        }

        private void StoreFacet(Facet facet, string path)
        {
            if (File.Exists(path))
            {
                "Facet already exists in disk. {0}. Path: {1}".Error(facet.ToString(), path);
                throw new InvalidOperationException("Facet already exists in disk");
            }
            else
            {
                using (var fs = File.CreateText(path))
                {
                    fs.Write(facet.UniqueName);
                    fs.Write("|");
                    fs.Write(facet.Field);
                    fs.Write("|");
                    fs.Write(facet.DisplayName);
                    fs.Write(Environment.NewLine);

                    foreach (var value in facet.Values)
                    {
                        fs.Write(value.Item1);
                        fs.Write("|");
                        fs.Write(SerializeBitSet(value.Item2));
                        fs.Write(Environment.NewLine);
                    }
                }
            }
        }

        private string SerializeBitSet(OpenBitSetDISI bitSet)
        {
            using (var ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, bitSet);

                var serialized = Convert.ToBase64String(ms.ToArray());

                ms.Close();

                return serialized;
            }
        }

        private Facet CreateFacet(FacetDefinition definition)
        {
            //TODO: Should this class have access to the index or should it receive all terms from somewhere else?
            var indexReader = this.Index.GetIndexSearcher().GetIndexReader();
            var allTerms = this.GetAllTermsForField(definition.Field, indexReader);

            var facet = new Facet(definition.UniqueName, definition.DisplayName, definition.Field);

            allTerms
                .Select(x =>
                {
                    var facetQuery = new TermQuery(new Term(definition.Field, x));
                    var facetQueryFilter = new CachingWrapperFilter(new QueryWrapperFilter(facetQuery));
                    var bitSet = new OpenBitSetDISI(facetQueryFilter.GetDocIdSet(indexReader).Iterator(), indexReader.MaxDoc());

                    return new { 
                        Value = x, 
                        MatchingDocuments = bitSet 
                    };
                })
                .ToList()
                .ForEach(x => facet.AddValue(x.Value, x.MatchingDocuments));

            return facet;
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
