﻿using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Memstate.Core
{
    public static class SerializerExtensions
    {

        public static byte[] Serialize(this ISerializer serializer, object graph)
        {
            var ms = new MemoryStream();
            
            serializer.WriteObject(ms, graph);
            ms.Position = 0;
            
            return ms.ToArray();
        }

        public static object Deserialize(this ISerializer serializer, byte[] bytes)
        {
            var ms = new MemoryStream(bytes)
            {
                Position = 0
            };
            
            return serializer.ReadObject(ms);
        }
    }
}