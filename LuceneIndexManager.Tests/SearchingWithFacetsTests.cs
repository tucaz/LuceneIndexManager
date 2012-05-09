using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Documents;
using NUnit.Framework;
using LuceneIndexManager.Util;
using Lucene.Net.Search;
using Lucene.Net.Util;
using LuceneIndexManager.Facets;
using SharpTestsEx;

namespace LuceneIndexManager.Tests
{
    [TestFixture]
    public class SearchingWithFacetsTests
    {
        IIndexDefinition _index;
        IndexManager _manager;

        [SetUp]
        public void create_index()
        {
            _index = new ProductIndex();
            _manager = new IndexManager();
            _manager.RegisterIndex(_index);
            _manager.CreateIndexes();
        }       
        
        [Test]
        public void can_search_and_get_facets()
        {
            var searcher = _manager.GetSearcher<ProductIndex>();
            var indexReader = searcher.GetIndexReader();
            var queryParser = _index.GetDefaultQueryParser();
            var query = queryParser.Parse("Name:harry*");
            
            var searchQueryFilter = new QueryWrapperFilter(query);

            var resuls = searcher.Search(query, 100);            

            var facets = IndexManager._facets.First().Value.Select(x =>
            {
                var bits = new OpenBitSetDISI(searchQueryFilter.GetDocIdSet(indexReader).Iterator(), indexReader.MaxDoc());
                bits.And(x.MatchingDocuments);
                var count = bits.Cardinality();

                return new { Value = x.Value, Count = count };
            }).ToList();

            facets.Should().Not.Be.Null();
            facets.Count.Should().Be.EqualTo(2);
            
            facets[0].Value.Should().Be.EqualTo("Book");
            facets[0].Count.Should().Be.EqualTo(2);

            facets[1].Value.Should().Be.EqualTo("DVD");
            facets[1].Count.Should().Be.EqualTo(3);
        }

        //TODO: Keep this here?
        private class ProductIndex : FSIndexDefinition
        {
            public ProductIndex() : base(@"C:\temp\Harry")
            {
            }
            
            public override IEnumerable<Document> GetAllDocuments()
            {
                yield return new Document()
                    .AddStringField("Name", "harry potter 1")
                    .AddStringField("ProductType", "DVD");

                yield return new Document()
                    .AddStringField("Name", "harry potter 2")
                    .AddStringField("ProductType", "DVD");

                yield return new Document()
                    .AddStringField("Name", "harry potter 3")
                    .AddStringField("ProductType", "DVD");

                yield return new Document()
                    .AddStringField("Name", "harry potter 1")
                    .AddStringField("ProductType", "Book");

                yield return new Document()
                    .AddStringField("Name", "harry potter 1")
                    .AddStringField("ProductType", "Book");
            }

            public override List<FacetDefinition> GetFacetsDefinition()
            {
                return new List<FacetDefinition>() { new FacetDefinition("ProductType", "Product Type", "ProductType") };
            }
        }
    }
}
