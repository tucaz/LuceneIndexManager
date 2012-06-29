using System.Collections.Generic;
using Lucene.Net.Documents;
using LuceneIndexManager.Util;

namespace LuceneIndexManager.Tests.TestIndexes
{
    public class InMemoryProductIndex : AbstractIndexDefinition
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
