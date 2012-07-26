using System;
using System.Collections.Generic;
using Lucene.Net.Index;
using Lucene.Net.Search;
using ThrowHelper;

namespace LuceneIndexManager.Util
{
    public static class TermExtractor
    {
        public static List<Tuple<string, Filter>> ExtractTermsForField(this IndexReader indexReader, string field)
        {
            Throw.IfArgumentNull(indexReader);
            Throw.IfArgumentNullOrEmpty(field);
            
            var allterms = new List<Tuple<string, Filter>>();
            var termReader = indexReader.Terms(new Term(field, String.Empty));

            do
            {
                if (termReader.Term().Field() != field)
                    break;

                var facetQuery = new TermQuery(termReader.Term().CreateTerm(termReader.Term().Text()));
                var facetQueryFilter = new CachingWrapperFilter(new QueryWrapperFilter(facetQuery));
                allterms.Add(new Tuple<string, Filter>(termReader.Term().Text(), facetQueryFilter));
            }
            while (termReader.Next());

            return allterms;
        }
    }
}
