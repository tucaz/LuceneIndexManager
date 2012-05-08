using System.Collections.Generic;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using LuceneIndexManager.Facets;

namespace LuceneIndexManager
{
    public interface IIndexDefinition
    {
        IndexWriter GetIndexWriter();
        IEnumerable<Document> GetAllDocuments();
        IndexSearcher GetIndexSearcher();
        QueryParser GetDefaultQueryParser();
        List<FacetDefinition> GetFacetsDefinition();
    }
}
