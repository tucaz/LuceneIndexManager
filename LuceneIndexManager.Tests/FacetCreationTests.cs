using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using LuceneIndexManager.Facets;
using LuceneIndexManager.Tests.TestIndexes;
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
        }        
        
        [Test]
        public void can_create_facet()
        {
            var definition = new FacetDefinition("ProductTypeFace", "Product Type", "ProductType");
            var builder = new FacetBuilder(_index.GetIndexSearcher().GetIndexReader());

            var facets = builder.CreateFacets(new List<FacetDefinition>() { definition });

            facets.Should().Not.Be.Null();
            facets.Count.Should().Be.EqualTo(1);
            facets.First().Values.Count.Should().Be.EqualTo(2);
        }
    }
}
