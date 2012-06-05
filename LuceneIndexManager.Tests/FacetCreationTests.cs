using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Lucene.Net.Documents;
using LuceneIndexManager.Facets;
using LuceneIndexManager.Util;
using NUnit.Framework;
using SharpTestsEx;


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

            var files = Directory.GetFiles(@"C:\temp\", "*.facet");
            foreach (var file in files)
            {
                File.Delete(file);
            }
        }        
        
        [Test]
        public void can_create_facet()
        {
            var definition = new FacetDefinition("ProductTypeFace", "Product Type", "ProductType", @"C:\temp\");
            var builder = new FacetBuilder(_index);

            var facets = builder.CreateFacets(new List<FacetDefinition>() { definition });

            facets.Should().Not.Be.Null();
            facets.Count.Should().Be.EqualTo(1);
            facets.First().Values.Count.Should().Be.EqualTo(2);
        }

        [Test] 
        public void facets_are_stored_locally_for_future_use()
        {
            var facetsLocation = @"C:\temp\";
            var facetUniqueName = "ProductTypeFacet";
            var facetDisplayName = "Product Type";
            var facetField = "ProductType";
            
            var definition = new FacetDefinition(facetUniqueName, facetDisplayName, facetField, facetsLocation);
            var builder = new FacetBuilder(_index);

            var facets = builder.CreateFacets(new List<FacetDefinition>() { definition });

            var facetFile = Path.Combine(facetsLocation, definition.FacetFile);
            Debug.WriteLine(facetFile);
            File.Exists(facetFile).Should().Be.True();
        }

        [Test]
        public void can_get_all_terms_for_facet()
        {
            var builder = new FacetBuilder(_index);
            var values = builder.GetAllTermsForField("ProductType", _index.GetIndexSearcher().GetIndexReader());

            values.Should().Not.Be.Null();
            values.Count.Should().Be.EqualTo(2);
            values.Should().Have.SameValuesAs("dvd", "book");
        }
        
        //TODO: Keep this here?
        private class ProductIndex : AbstractIndexDefinition
        {
            public override IEnumerable<Document> GetAllDocuments()
            {
                yield return new Document()
                    .AddField("Name", "Harry Potter 1", Field.Store.YES, Field.Index.ANALYZED)
                    .AddField("ProductType", "DVD", Field.Store.YES, Field.Index.ANALYZED);

                yield return new Document()
                    .AddField("Name", "Harry Potter 2", Field.Store.YES, Field.Index.ANALYZED)
                    .AddField("ProductType", "DVD", Field.Store.YES, Field.Index.ANALYZED);

                yield return new Document()
                    .AddField("Name", "Harry Potter 3", Field.Store.YES, Field.Index.ANALYZED)
                    .AddField("ProductType", "DVD", Field.Store.YES, Field.Index.ANALYZED);

                yield return new Document()
                    .AddField("Name", "Harry Potter 1", Field.Store.YES, Field.Index.ANALYZED)
                    .AddField("ProductType", "Book", Field.Store.YES, Field.Index.ANALYZED);

                yield return new Document()
                    .AddField("Name", "Harry Potter 1", Field.Store.YES, Field.Index.ANALYZED)
                    .AddField("ProductType", "Book", Field.Store.YES, Field.Index.ANALYZED);
            }
        }
    }
}
