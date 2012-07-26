using System.IO;
using LuceneIndexManager.Tests.TestIndexes;
using LuceneIndexManager.Util;
using NUnit.Framework;
using SharpTestsEx;
using System.Linq;

namespace LuceneIndexManager.Tests
{
    [TestFixture]
    public class TermExtractorTests
    {
        IIndexDefinition _index;

        [SetUp]
        public void create_index()
        {
            _index = new InMemoryProductIndex();

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
        public void can_extract_all_terms_from_IndexReader()
        {
            var values = _index.GetIndexSearcher().GetIndexReader().ExtractTermsForField("ProductType");

            values.Should().Not.Be.Null();
            values.Count.Should().Be.EqualTo(2);
            values.Select(x => x.Item1).Should().Have.SameValuesAs("dvd", "book");
        }
    }
}
