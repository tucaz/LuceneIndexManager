using System;
using System.Collections.Generic;
using Lucene.Net.Index;
using ThrowHelper;

namespace LuceneIndexManager.Util
{
    public static class TermExtractor
    {
        public static List<string> ExtractTermsForField(this IndexReader indexReader, string field)
        {
            Throw.IfArgumentNull(indexReader);
            Throw.IfArgumentNullOrEmpty(field);
            
            var allterms = new List<string>();
            var termReader = indexReader.Terms(new Term(field, String.Empty));

            do
            {
                if (termReader.Term().Field() != field)
                    break;

                allterms.Add(termReader.Term().Text());
            }
            while (termReader.Next());

            return allterms;
        }
    }
}
