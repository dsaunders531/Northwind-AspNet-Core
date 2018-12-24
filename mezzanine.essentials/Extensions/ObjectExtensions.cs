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

        /// <summary>
        /// Get the next value for a primitive object type.
        /// </summary>
        /// <param name="me"></param>
        /// <param name="T"></param>
        /// <returns></returns>
        public static object Increment(this object me, Type T)
        {
            if (T.IsPrimitive)
            {
                switch (T.Name)
                {
                    case "Byte":
                        me = (Byte)me + 1;
                        break;
                    case "SByte":
                        me = (SByte)me + 1;
                        break;
                    case "Int16":
                        me = (Int16)me + 1;
                        break;
                    case "UInt16":
                        me = (UInt16)me + 1;
                        break;
                    case "Int32":
                        me = (Int32)me + 1;
                        break;
                    case "UInt32":
                        me = (UInt32)me + 1;
                        break;
                    case "Int64":
                        me = (Int64)me + 1;
                        break;
                    case "UInt64":
                        me = (UInt64)me + 1;
                        break;
                    case "Double":
                        me = (Double)me + 1;
                        break;
                    case "Single":
                        me = (Single)me + 1;
                        break;
                    default:
                        throw new ArgumentException("Increment can only be used on Byte, SByte, Int16, UInt16, Int32, UInt32, Int64, UInt64, Double, Single, DateTime and Guid");                        
                }
            }
            else
            {
                switch (T.Name)
                {
                    case "DateTime":
                        me = ((DateTime)me).AddMinutes(1);
                        break;
                    case "Guid":
                        me = Guid.NewGuid();
                        break;
                    default:
                        throw new ArgumentException("Increment can only be used on Byte, SByte, Int16, UInt16, Int32, UInt32, Int64, UInt64, Double, Single, DateTime and Guid");
                }                
            }

            return me;
        }
    }
}
