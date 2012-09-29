using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Reflection;
using System.IO;
using System.Xml;
using System.Collections;
using Tridion.Extensions.SMP.Model;


namespace Tridion.Extensions.SMP.Configuration
{
    public class SMPConfiguration
    {       
        private XmlDocument _doc = null;
        private string _basePath;
        private Dictionary<String, List<Account>> _accounts = new Dictionary<String, List<Account>>();

        public Dictionary<String, List<Account>> Accounts
        {
          get { return _accounts; }
          set { _accounts = value; }
        }

        
        Dictionary<String, XmlNode> _dPubConfig = new Dictionary<String, XmlNode>();

        public Dictionary<String, XmlNode> PubConfig
        {
            get { return _dPubConfig; }
            set { _dPubConfig = value; }
        }


       
        
        
        private bool _initialized = false;
        private bool isLoaded = false;
        
        //public Dictionary<String, String> Settings { get; set; }

        public bool Initialized
        {
            get { return _initialized; }            
        }
        

        public string BasePath
        {
            get { return _basePath; }            
        }




        public SMPConfiguration(XmlDocument doc)
        {            
            _doc = doc;            
            Init();
        }

        public SMPConfiguration()
        {
            Init();                 
        }

        private void Init() {
            if(!isLoaded){
                loadBasePath();
                if(_doc == null){
                    _doc = new XmlDocument();
                    _doc.Load(_basePath + Path.DirectorySeparatorChar + "SMP.config");
                }   
     
                LoadAccounts();
                
                foreach(XmlNode pubNode in _doc.SelectNodes("//Publication")){
                    _dPubConfig.Add(pubNode.Attributes["ID"].Value, pubNode);
                }

                isLoaded = true;
            }
        }

        public string[] GetAccountTypes(){
            Init();
            return _accounts.Keys.ToArray<String>();
        }

        private void LoadAccounts()
        {            
            
            foreach(XmlNode node in _doc.SelectNodes("//Account")){
                List<Account> accList = new List<Account>();
                string key = node.Attributes["Type"].Value;
                if (_accounts.ContainsKey(key)) {
                    accList = _accounts[key];                    
                }
                Account accData = new Account { 
                    Type = node.Attributes["Type"].Value,
                    Name = node.SelectSingleNode("./Name").InnerText,
                    Email = node.Attributes["Email"].Value, 
                    Password = node.SelectSingleNode("./Password").InnerText,
                    Nickname = node.SelectSingleNode("./Nickname").InnerText,
                    AvatarImage = node.SelectSingleNode("./AvatarImage").InnerText

                };
                accList.Add(accData);
                _accounts[key] = accList;
            }
            
        
        }

        private void loadBasePath(){
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            _basePath = Path.GetDirectoryName(path);
        }

    }
}