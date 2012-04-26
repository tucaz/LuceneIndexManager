using System;
using System.Collections.Generic;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;

namespace LuceneIndexManager
{
    public abstract class AbstractIndexDefinition : IIndexDefinition
    {
        private IndexWriter _indexWriter;
        
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
            return new RAMDirectory();
        }

        protected virtual Analyzer GetAnalyzer()
        {
            return new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29);
        }

        public virtual IEnumerable<Document> GetAllDocuments()
        {
            return new List<Document>();
        }
    }
}
