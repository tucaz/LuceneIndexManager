using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using LuceneIndexManager.Facets;
using LuceneIndexManager.Tests.TestIndexes;
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
            var builder = new FacetBuilder(_index.GetIndexSearcher().GetIndexReader());

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
            var builder = new FacetBuilder(_index.GetIndexSearcher().GetIndexReader());

            var facets = builder.CreateFacets(new List<FacetDefinition>() { definition });

            var facetFile = Path.Combine(facetsLocation, definition.FacetFile);
            Debug.WriteLine(facetFile);
            File.Exists(facetFile).Should().Be.True();
        }        
    }
}
