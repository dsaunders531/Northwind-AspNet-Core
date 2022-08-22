using mezzanine.Utility;

namespace mezzanine.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object me)
        {
            using (JSONSerialiser serializer = new JSONSerialiser())
            {
                return serializer.Serialize(me);
            }
        }
    }
}
