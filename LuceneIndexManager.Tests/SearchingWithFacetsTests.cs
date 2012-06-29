using System.IO;
using LuceneIndexManager.Tests.TestIndexes;
using NUnit.Framework;
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
            //Clean any existing facets before running the tests to make sure 
            //we are using new and correct data
            var facetsDirectory = @"C:\temp\facets";

            if (Directory.Exists(facetsDirectory))
            {
                var files = Directory.GetFiles(facetsDirectory, "*.facet");
                foreach (var file in files)
                {
                    File.Delete(file);
                }
            }
            
            _index = new DiskProductIndex();
            _manager = new IndexManager();
            _manager.RegisterIndex(_index);
            _manager.CreateIndexes();            
        }       
        
        [Test]
        public void can_search_and_get_facets()
        {
            var results = _manager.SearchWithFacets<DiskProductIndex>("Name:Harry");            

            results.Should().Not.Be.Null();
            results.Facets.Should().Not.Be.Null();
            results.Facets.Count.Should().Be.EqualTo(2);

            results.Facets[0].Value.Should().Be.EqualTo("book");
            results.Facets[0].Count.Should().Be.EqualTo(2);

            results.Facets[1].Value.Should().Be.EqualTo("dvd");
            results.Facets[1].Count.Should().Be.EqualTo(3);
        }

        [Test]
        public void can_search_and_get_facets2()
        {
            var results = _manager.SearchWithFacets<DiskProductIndex>("ProductType:dvd");

            results.Should().Not.Be.Null();
            results.Facets.Should().Not.Be.Null();
            results.Facets.Count.Should().Be.EqualTo(2);

            results.Facets[0].Value.Should().Be.EqualTo("book");
            results.Facets[0].Count.Should().Be.EqualTo(0);

            results.Facets[1].Value.Should().Be.EqualTo("dvd");
            results.Facets[1].Count.Should().Be.EqualTo(3);
        }
    }
}
