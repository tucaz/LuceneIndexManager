using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Documents;
using Lucene.Net.Search;
using Lucene.Net.Store;
using LuceneIndexManager.Facets;
using NUnit.Framework;
using SharpTestsEx;
using LuceneIndexManager.Util;

namespace LuceneIndexManager.Tests
{
    [TestFixture]
    public class FacetCreationTests
    {
        IIndexDefinition _index;
        
        [SetUp]
        public void create_index()
        {
            _index = new ProductIndex();

            var manager = new IndexManager();            
            manager.RegisterIndex(_index);
            manager.CreateIndexes();
        }        
        
        [Test]
        public void can_create_facet()
        {
            var definition = new FacetDefinition("DVD", "DVD", "ProductType");
            var builder = new FacetBuilder(_index);

            var facets = builder.CreateFacets(new List<FacetDefinition>() { definition });

            facets.Should().Not.Be.Null();
            facets.Count.Should().Be.EqualTo(2);
        }

        [Test]
        public void can_get_all_terms_for_facet()
        {
            var builder = new FacetBuilder(_index);
            var values = builder.GetAllTermsForField("ProductType", _index.GetIndexSearcher().GetIndexReader());

            values.Should().Not.Be.Null();
            values.Count.Should().Be.EqualTo(2);
            values.Should().Have.SameValuesAs("DVD", "Book");
        }

        private class ProductIndex : AbstractIndexDefinition
        {
            public override IEnumerable<Document> GetAllDocuments()
            {
                yield return new Document()
                    .AddStringField("Name", "Harry Potter 1")
                    .AddStringField("ProductType", "DVD");

                yield return new Document()
                    .AddStringField("Name", "Harry Potter 2")
                    .AddStringField("ProductType", "DVD");

                yield return new Document()
                    .AddStringField("Name", "Harry Potter 3")
                    .AddStringField("ProductType", "DVD");

                yield return new Document()
                    .AddStringField("Name", "Harry Potter 1")
                    .AddStringField("ProductType", "Book");

                yield return new Document()
                    .AddStringField("Name", "Harry Potter 1")
                    .AddStringField("ProductType", "Book");
            }
        }
    }
}
