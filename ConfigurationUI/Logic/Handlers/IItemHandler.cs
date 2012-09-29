using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Tridion.Extensions.SMP.Model;

namespace Tridion.Extensions.SMP.Configuration
{
    public interface IItemHandler
    {
        Dictionary<string, string> GetSettings();
        String GetClass();
        String GetAssembly();
        void Load(XmlNode handlerNode);
        bool Execute(Account acc, ItemData data, SocialInput input);
    }
}
