using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Documents;

namespace LuceneIndexManager.Util
{
    public static class DocumentExtentions
    {
        /// <summary>
        /// Add field to document with Store.Yes and Index.NOT_ANALYZED
        /// </summary>
        /// <param name="document">Document to add Field</param>
        /// <param name="name">Field Name</param>
        /// <param name="value">Field Value</param>
        /// <returns>The same Document with Field Added</returns>
        public static Document AddField(this Document document, string name, object value)
        {
            return document.AddField(name, value.ToEmptyString(), Field.Store.YES, Field.Index.NOT_ANALYZED_NO_NORMS);
        }

        private static string ToEmptyString(this object value)
        {
            return value != null ? value.ToString() : string.Empty;
        }

        /// <summary>
        /// Add default field with Index.NOT_ANALYZED
        /// </summary>
        /// <param name="document">Document to add Field</param>
        /// <param name="name">Field Name</param>
        /// <param name="value">Field Value</param>
        /// <param name="store">Field store type</param>
        /// <returns>The same Document with Field Added</returns>
        public static Document AddField(this Document document, string name, string value, Field.Store store)
        {
            return document.AddField(name, value, store, Field.Index.NOT_ANALYZED_NO_NORMS);
        }

        /// <summary>
        /// Add field if contains string value. Add with Store.Yes and Field.Index.NOT_ANALYZED_NO_NORMS
        /// </summary>
        /// <param name="document"></param>
        /// <param name="name">Field Name</param>
        /// <param name="value">Field Value</param>
        /// <returns>The same Document with Field Added</returns>
        public static Document AddStringField(this Document document, string name, string value)
        {
            if (value != null && !string.Empty.Equals(value.Trim()))
                return document.AddField(name, value);
            return document;
        }

        /// <summary>
        /// Add default field at document
        /// </summary>
        /// <param name="document">Document to add Field</param>
        /// <param name="name">Field Name</param>
        /// <param name="value">Field Value</param>
        /// <param name="store">Field store type</param>
        /// <param name="index">Field index type</param>
        /// <returns>The same Document with Field Added</returns>
        public static Document AddField(this Document document, string name, string value, Field.Store store, Field.Index index)
        {
            if (String.IsNullOrEmpty(value))
                return document;

            document.Add(new Field(name, value, store, index));
            
            return document;
        }

        public static Document AddIntField(this Document document, string name, int value)
        {
            return value > 0 ? AddNumericField(document, BuildNumericField(name).SetIntValue(value)) : document;
        }

        private static NumericField BuildNumericField(string name)
        {
            return new NumericField(name, Field.Store.YES, true);
        }

        private static Document AddNumericField(Document document, NumericField field)
        {
            document.Add(field);
            return document;
        }

        public static Document AddDecimalField(this Document document, string name, decimal? value)
        {
            if (value.HasValue && value > decimal.Zero)
                return AddNumericField(document, BuildNumericField(name).SetDoubleValue((double)value));
            return document;
        }

        public static Document AddLongField(this Document document, string name, long? value)
        {
            if (value.HasValue && value > 0)
                return AddNumericField(document, BuildNumericField(name).SetLongValue(value.Value));
            return document;
        }
    }
}
