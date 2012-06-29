using System.Collections.Generic;
using Lucene.Net.Documents;
using LuceneIndexManager.Facets;
using LuceneIndexManager.Util;

namespace LuceneIndexManager.Tests.TestIndexes
{
    public class DiskProductIndex : FSIndexDefinition
    {
        public DiskProductIndex()
            : base(@"C:\temp\Harry")
        {
        }

        public override IEnumerable<Document> GetAllDocuments()
        {
            yield return new Document()
                .AddField("Name", "harry potter 1", Field.Store.YES, Field.Index.ANALYZED)
                .AddField("ProductType", "DVD", Field.Store.YES, Field.Index.ANALYZED);

            yield return new Document()
                .AddField("Name", "harry potter 2", Field.Store.YES, Field.Index.ANALYZED)
                .AddField("ProductType", "DVD", Field.Store.YES, Field.Index.ANALYZED);

            yield return new Document()
                .AddField("Name", "harry potter 3", Field.Store.YES, Field.Index.ANALYZED)
                .AddField("ProductType", "DVD", Field.Store.YES, Field.Index.ANALYZED);

            yield return new Document()
                .AddField("Name", "harry potter 1", Field.Store.YES, Field.Index.ANALYZED)
                .AddField("ProductType", "Book", Field.Store.YES, Field.Index.ANALYZED);

            yield return new Document()
                .AddField("Name", "harry potter 1", Field.Store.YES, Field.Index.ANALYZED)
                .AddField("ProductType", "Book", Field.Store.YES, Field.Index.ANALYZED);
        }

        public override List<FacetDefinition> GetFacetsDefinition()
        {
            return new List<FacetDefinition>() { new FacetDefinition("ProductType", "Product Type", "ProductType") };
        }
    }
}
