using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Tridion.Extensions.Base
{
    [DataContract]
    public class BaseSettings
    {
        private string userId;

        [DataMember]
        public string UserId
        {
            get
            {
                return userId;
            }
            set
            {
                userId = value;
            }
        }

        private string savedDate;
        
        [DataMember]
        public string SavedDate
        {
            get
            {
                return savedDate;
            }
            set
            {
                savedDate = value;
            }
        }
    }
}
