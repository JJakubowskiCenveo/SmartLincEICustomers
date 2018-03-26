using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using SmartLincInterface;
namespace SmartLincServiceTemplate
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        EntityShipment SimplePullMethod(EntityShipment objRequest);
        [OperationContract]
        string DoSimpleOperation(string strApplicationIdentifier, string strOperationParameter);
        [OperationContract]
        EntityShipment SimplePutbackMethod(EntityShipment objShipment);
        [OperationContract]
        EntityShipment SimpleVoidMethod(EntityShipment objShipment);
        [OperationContract]
        EntityShipment GetRates(EntityShipment request);
        [OperationContract]
        string GetEncryption(string strPassword);
        [OperationContract]
        List<EntityOrderSelector> SimpleOrderSelectorPullMethod(EntityOrderSelector objOrderSelector);
        // TODO: Add your service operations here
    }
}
