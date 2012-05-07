using Lucene.Net.Store;

namespace LuceneIndexManager
{
    public abstract class FSIndexDefinition : AbstractIndexDefinition
    {
        private FSDirectory _directory;
        private string _path;

        public FSIndexDefinition(string path)
        {
            this._path = path;
        }
        
        protected override Directory GetDirectory()
        {
            if(this._directory == null)
                this._directory = FSDirectory.Open(new System.IO.DirectoryInfo(this._path));

            return this._directory;
        }
    }
}
