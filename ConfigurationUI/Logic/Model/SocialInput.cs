using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Tridion.Extensions.SMP.Model
{
    [DataContract]
    public class SocialInput
    {
        [DataMember]
        public string AccountType { get; set; }
        [DataMember]
        public bool GetLink { get; set; }
        [DataMember]
        public bool GetTextFromItem{ get; set; }
        [DataMember]
        public bool Enabled { get; set; }
        [DataMember]
        public string Text { get; set; }
        [DataMember]
        public string Date { get; set; }
        [DataMember]
        public bool Propagate { get; set; }

        




    }
}