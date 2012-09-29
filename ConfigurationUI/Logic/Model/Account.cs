using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Tridion.Extensions.SMP.Model
{
    [DataContract]
    public class Account
    {
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Nickname { get; set; }
        [DataMember]
        public string AvatarImage { get; set; }

    }
}
