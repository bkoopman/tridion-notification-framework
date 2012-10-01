using System.IO;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Text;

namespace TridionCommunity.NotificationFramework.ExtensionMethods
{
    public static class Extensions
    {
        public static XElement SerializeToXElement<T>(this T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());
            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return XElement.Parse(textWriter.ToString());
            }            
        }
    }
}
