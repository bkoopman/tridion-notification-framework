using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Tridion.Extensions.SMP.Model
{
    [DataContract]
    public class ItemData
    {
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string PublicationId { get; set; }
        [DataMember]
        public string ComponentId { get; set; }
        [DataMember]
        public string ComponentTemplateId { get; set; }
        [DataMember]
        public string PageId { get; set; }
        [DataMember]
        public string PageTemplateId { get; set; }
    }
}
