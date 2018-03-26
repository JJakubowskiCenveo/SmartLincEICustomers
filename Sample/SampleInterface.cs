using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartLincInterface;
namespace Sample
{
    public class SampleInterface
    {
        private List<EntityResponseStatus> lstResponse = new List<EntityResponseStatus>();
        public EntityShipment Pull(EntityShipment objES)
        {
            try
            {
                objES.objShipTo = GetShipTo();
                objES.objDetails.strPONumber = "PONumber";
                objES.objDetails.strInvoiceNumber = "InvNumber";
                objES.objShipMethod = new EntityShipMethod();
                objES.objShipMethod.strShipViaCode = "100";
                objES.lstContainer = new List<EntityContainer>(); //If line items are needed...
                objES.lstContainer.Add(FillLineItems());
            }
            catch (Exception ex)
            {
                lstResponse.Add(SetResponse(ex.Message + " - IE - " + ex.InnerException.Message + " - GE", ResponseStatusType.ERROR));
            }
            objES.lstEntityResponseStatus = lstResponse;
            return objES;
        }
        private EntityAddress GetShipTo()
        {
            EntityAddress objEA = new EntityAddress();

            objEA.strCompanyName = "ShipToCompany";
            objEA.strContactName = "ShipToContact";
            objEA.strAddressLine1 = "ShipToAdd1";
            objEA.strAddressLine2 = "ShipToAdd2";
            objEA.strAddressLine3 = "ShipToAdd3";
            objEA.strCity = "Milwaukee";
            objEA.strState = "WI";
            objEA.strPostalCode = "53203";
            objEA.strCountryCode = "US";

            return objEA;
        }
        private EntityResponseStatus SetResponse(string strMessage, ResponseStatusType eResponseType)
        {
            EntityResponseStatus objResponse = new EntityResponseStatus();
            objResponse.Message = strMessage;
            objResponse.StatusType = eResponseType;
            return objResponse;
        }
        private EntityContainer FillLineItems()
        {
            EntityContainer EC = new EntityContainer();
            EC.lstPackageGroup = new List<EntityPackageGroup>();
            EntityPackageGroup EPG = new EntityPackageGroup();

            List<EntityLineItem> lstELI = new List<EntityLineItem>();
            EntityLineItem objELI = new EntityLineItem();
            objELI.strItemNumber = "ABC123";
            objELI.intOrderedQuantity = 20;
            objELI.strItemDescription = "Sample Line Item";

            lstELI.Add(objELI);

            EPG.lstLineItem = lstELI;
            EC.lstPackageGroup.Add(EPG);

            return EC;
        }
        public EntityShipment Putback(EntityShipment objES)
        {
            try
            {
            }
            catch (Exception ex)
            {
                lstResponse.Add(SetResponse(ex.Message, ResponseStatusType.ERROR));
            }
            objES.lstEntityResponseStatus = lstResponse;
            return objES;
        }
        public EntityShipment Void(EntityShipment objES)
        {
            try
            {
            }
            catch (Exception ex)
            {
                lstResponse.Add(SetResponse(ex.Message, ResponseStatusType.ERROR));
            }
            return objES;
        }
    }
}
