using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Threading;

using System.Collections.Generic;
using System.Collections;
using Tridion.ContentManager;


using System.Reflection;
using System.IO;

using System.Web;
using Tridion.ContentManager.CoreService.Client;
using System.Runtime.Serialization;
using System.Xml;
using Tridion.Web.UI.Core.Extensibility;
using System.Text;
using System.Web.Script.Serialization;
using Tridion.Extensions.Services.Base;
using Tridion.Extensions.UI.Services.Interfaces;
using Tridion.Extensions.UI.WFN.Logic.Model;








namespace Tridion.Extensions.Services.WFN
{

    
    /// <summary>
    /// Implementation of the Services
    /// </summary>
    /// <remarks></remarks>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class WFNService : BaseServices, IWFNServices
    {

        private WFNConfiguration _config;

        /// <summary>
        /// Gets or sets the config.
        /// </summary>
        /// <value>The config.</value>
        /// <remarks></remarks>
        public WFNConfiguration Config
        {
            get { return _config; }
            set { _config = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Services"/> class.
        /// </summary>
        /// <remarks></remarks>
        public WFNService()
        {
            
            if (_config == null)
            {
                _config = new WFNConfiguration();
            }          
           
        }



        public WFNConfiguration SaveWFNSettings(UI.WFN.Logic.Model.WFNConfiguration data, string key)
        {   
            
            return data;
        }


        public WFNConfiguration GetWFNSettings(string userId, string appKey)
        {
            return null;
        }
    }
}