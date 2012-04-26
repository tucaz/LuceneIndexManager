using System.Collections.Generic;
using Lucene.Net.Documents;
using Lucene.Net.Index;

namespace LuceneIndexManager
{
    public interface IIndexDefinition
    {
        IndexWriter GetIndexWriter();
        IEnumerable<Document> GetAllDocuments();
    }
}
