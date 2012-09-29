using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Tridion.Extensions.UI.WFN.Logic.Model
{
    [DataContract]
    public class WFNConfiguration
    {
        private string _name; 

        [DataMember]
        public string Name {
            get { return _name; }
            set { _name = value;  }
        }
    }
}
