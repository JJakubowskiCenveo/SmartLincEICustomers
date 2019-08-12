using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using SmartLincInterface;
using CMS;
using Lexis;
using LexisGlobal;
using SAP;

namespace SmartLincServiceTemplate
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class SmartLincContractImplementation : IExpressInterface
    {
        #region IExpressInterface Members

        public string DoSimpleOperation(string strApplicationIdentifier, string strOperationParameter)
        {
            return string.Empty;
        }
        public EntityShipment SimplePullMethod(EntityShipment objRequest)
        {
            switch (objRequest.ToolKit.ID.ToUpper())
            {
                case "LEXIS":
                    LexisInterface objLexis = new LexisInterface();
                    objRequest = objLexis.Pull(objRequest);
                    break;
                case "LEXISGLOBAL":
                    LexisGlobalInterface objLexisGlobal = new LexisGlobalInterface();
                    objRequest = objLexisGlobal.Pull(objRequest);
                    break;
                case "CMS":
                    CMSInterface objCMS = new CMSInterface();
                    objRequest = objCMS.Pull(objRequest);
                    break;
                case "SAP":
                    SAPInterface objSAP = new SAPInterface();
                    objRequest = objSAP.Pull(objRequest);
                    break;
            }
            return objRequest;
        }
        public EntityShipment SimplePutbackMethod(EntityShipment objRequest)
        {
            switch (objRequest.ToolKit.ID.ToUpper())
            {
                case "LEXIS":
                    LexisInterface objLexis = new LexisInterface();
                    objRequest = objLexis.Putback(objRequest);
                    break;
                case "LEXISGLOBAL":
                    LexisGlobalInterface objLexisGlobal = new LexisGlobalInterface();
                    objRequest = objLexisGlobal.Putback(objRequest);
                    break;
                case "CMS":
                    CMSInterface objCMS = new CMSInterface();
                    objRequest = objCMS.Putback(objRequest);
                    break;
                case "SAP":
                    SAPInterface objSAP = new SAPInterface();
                    objRequest = objSAP.Putback(objRequest);
                    break;
            }
            return objRequest;
        }
        public EntityShipment SimpleVoidMethod(EntityShipment objRequest)
        {
            switch (objRequest.ToolKit.ID.ToUpper())
            {
                case "LEXIS":
                    LexisInterface objLexis = new LexisInterface();
                    objRequest = objLexis.Void(objRequest);
                    break;
                case "LEXISGLOBAL":
                    LexisGlobalInterface objLexisGlobal = new LexisGlobalInterface();
                    objRequest = objLexisGlobal.Void(objRequest);
                    break;
                case "CMS":
                    CMSInterface objCMS = new CMSInterface();
                    objRequest = objCMS.Void(objRequest);
                    break;
                case "SAP":
                    SAPInterface objSAP = new SAPInterface();
                    objRequest = objSAP.Void(objRequest);
                    break;
            }
            return objRequest;
        }
        #endregion
        public EntityShipment GetRates(EntityShipment request)
        {
            throw new NotImplementedException(); //Not Used at this time.
        }
        public string GetEncryption(string strPassword)
        {
            throw new NotImplementedException(); //Not used at this time.
        }
        public List<EntityOrderSelector> SimpleOrderSelectorPullMethod(EntityOrderSelector objOS)
        {            
            throw new NotImplementedException(); //Not used at this time.
        }
    }
}
