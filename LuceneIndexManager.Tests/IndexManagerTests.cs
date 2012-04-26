using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Index;
using NUnit.Framework;
using SharpTestsEx;

namespace LuceneIndexManager.Tests
{
    [TestFixture]
    public class IndexManagerTests
    {
        [Test]
        public void can_register_an_IIndexSource()
        {
            var indexService = new IndexManager();
            var indexSource = new TestIndexSource();

            indexService.RegisterIndex(indexSource);
            
            indexService.RegisteredIndexSources.Count.Should().Be.EqualTo(1);
        }

        [Test]
        public void can_create_an_Index()
        {
            var indexService = new IndexManager();
            var indexSource = new TestIndexSource();

            indexService.RegisterIndex(indexSource);

            var count = indexService.CreateIndexes();

            count.Should().Be.EqualTo(1);
        }

        private class TestIndexSource : AbstractIndexDefinition
        {            
        }
    }
}
