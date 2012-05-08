using System;
using System.Collections.Generic;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using LuceneIndexManager.Facets;

namespace LuceneIndexManager
{
    public abstract class AbstractIndexDefinition : IIndexDefinition
    {
        private IndexWriter _indexWriter;
        private RAMDirectory _directory;
        
        public virtual IndexWriter GetIndexWriter()
        {
            if (this._indexWriter == null)
            {
                this._indexWriter = new IndexWriter(this.GetDirectory(), this.GetAnalyzer(), IndexWriter.MaxFieldLength.UNLIMITED);
            }

            return this._indexWriter;
        }

        protected virtual Directory GetDirectory()
        {
            if(this._directory == null)
                this._directory = new RAMDirectory();

            return this._directory;
        }

        protected virtual Analyzer GetAnalyzer()
        {
            return new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29);
        }

        public virtual IEnumerable<Document> GetAllDocuments()
        {
            return new List<Document>();
        }

        public virtual IndexSearcher GetIndexSearcher()
        {
            var indexSearcher = new IndexSearcher(this.GetDirectory(), true);            
            return indexSearcher;
        }

        public virtual QueryParser GetDefaultQueryParser()
        {
            var queryParser = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, String.Empty, this.GetAnalyzer());
            return queryParser;
        }

        public virtual List<FacetDefinition> GetFacetsDefinition()
        {
            return new List<FacetDefinition>();    
        }
    }
}
