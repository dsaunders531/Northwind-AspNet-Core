using System;
using System.Collections.Generic;
using System.Text;
using mezzanine.Serialization;

namespace mezzanine
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object me)
        {
            using (JSONSerializer serializer = new JSONSerializer())
            {
                return serializer.Serialize(me);
            }
        }
    }
}
