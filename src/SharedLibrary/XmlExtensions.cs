using System;
using System.Xml.Linq;

namespace SharedLibrary
{
    public static class XmlExtensions
    {
        public static string GetAttributeFailOnMissing(this XElement element, XName key)
        {
            return GetAttributeFailOnMissing(element, key, x => x);
        }

        public static TValue GetAttributeFailOnMissing<TValue>(this XElement element, XName key, Func<string,TValue> getter)
        {
            return GetAttributeValueOrDefault(element, key, getter, true);
        }

        public static string GetAttributeValueOrDefault(this XElement element, XName key)
        {
            return GetAttributeValueOrDefault(element, key, x => x);
        }

        public static TValue GetAttributeValueOrDefault<TValue>(this XElement element, XName key, Func<string,TValue> valueGetter)
        {
            return GetAttributeValueOrDefault(element, key, valueGetter, false);
        }

        public static TValue GetAttributeValueOrDefault<TValue>(this XElement element, XName key, Func<string,TValue> valueGetter, bool throwOnAttributeMissing)
        {
            var result = element.Attribute(key);
            
            if (result == null)
            {
                if (throwOnAttributeMissing)
                    throw new InvalidOperationException(string.Format("Element part {0} does not contain attribute {1}", element, key));
                else
                {
                    return default(TValue);
                }
            }
           
            try
            {
                return valueGetter(result.Value);
            }
            catch (Exception)
            {
                return default(TValue);
            }
        }
    }
}