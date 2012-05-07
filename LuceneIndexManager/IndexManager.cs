using System;
using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Search;

namespace LuceneIndexManager
{
    public class IndexManager
    {
        public IndexManager()
        {
            this.RegisteredIndexSources = new Dictionary<int, IIndexDefinition>();
        }
        
        public Dictionary<int, IIndexDefinition> RegisteredIndexSources { get; private set; }

        public void RegisterIndex<T>() where T : IIndexDefinition
        {
            var source = Activator.CreateInstance<T>();
            this.RegisterIndex(source);
        }
        
        public void RegisterIndex(IIndexDefinition source)
        {
            this.RegisteredIndexSources.Add(source.GetType().GetHashCode(),  source);
        }

        public int CreateIndexes()
        {
            var indexesCreated = 0;
            
            foreach (var index in this.RegisteredIndexSources)
            {
                CreateIndex(index.Value);
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

        public IndexSearcher GetSearcher<T>() where T : IIndexDefinition
        {
            var index = this.FindRegisteredIndex(typeof(T));
            var searcher = index.GetIndexSearcher();

            

            return searcher;
        }

        //TODO: This is internal just to be available for testing. Need to turn it to private and find another way to test it
        internal IIndexDefinition FindRegisteredIndex(Type t)
        {
            var hash = t.GetHashCode();
            var index = this.RegisteredIndexSources[hash];

            return index;
        }
    }
}
