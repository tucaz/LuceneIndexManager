using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LuceneIndexManager
{
    public class IndexManager
    {
        public IndexManager()
        {
            this.RegisteredIndexSources = new List<IIndexDefinition>();
        }
        
        public List<IIndexDefinition> RegisteredIndexSources { get; private set; }
        
        public void RegisterIndex(IIndexDefinition source)
        {
            this.RegisteredIndexSources.Add(source);
        }

        public int CreateIndexes()
        {
            var indexesCreated = 0;
            
            foreach (var index in this.RegisteredIndexSources)
            {
                CreateIndex(index);
                indexesCreated++;
            }

            return indexesCreated;
        }

        private void CreateIndex(IIndexDefinition index)
        {
            using (var indexWriter = index.GetIndexWriter())
            {
                //If we are creating a new index explicitly make sure we start with an empty index
                indexWriter.DeleteAll();

                foreach (var document in index.GetAllDocuments().AsParallel())
                {
                    indexWriter.AddDocument(document);
                }
                
                indexWriter.Commit();
                indexWriter.Optimize();
                indexWriter.Close();
            }
        }

    }
}
