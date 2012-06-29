using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ThrowHelper;

namespace LuceneIndexManager.Util
{
    public static class ObjectSerializer
    {
        public static string SerializeToBase64String<T>(this T @object)
        {
            Throw.IfArgumentNull(@object);
            
            using (var ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, @object);

                var serialized = Convert.ToBase64String(ms.ToArray());

                ms.Close();

                return serialized;
            }
        }
    }
}
