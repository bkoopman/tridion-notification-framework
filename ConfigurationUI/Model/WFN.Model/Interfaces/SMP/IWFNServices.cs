using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;
using Tridion.Extensions.UI.WFN.Logic.Model;





namespace Tridion.Extensions.UI.Services.Interfaces
{
    /// <summary>
    /// Service Contract definition
    /// </summary>
    /// <remarks></remarks>
    [ServiceContract(Name = "WFNServices", Namespace = "Tridion.Extensions.WFN.Services")]
    public interface IWFNServices
    {

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        WFNConfiguration SaveWFNSettings(WFNConfiguration data, string appKey);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        WFNConfiguration GetWFNSettings(string userId, string appKey);

       
        
    }


}
