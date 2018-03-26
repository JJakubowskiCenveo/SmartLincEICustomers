using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using SmartLincInterface;

namespace CMS
{
    public class CMSInterface
    {
        private List<EntityResponseStatus> lstResponse = new List<EntityResponseStatus>();
        private SqlConnection objSQLCon;
        public EntityShipment Pull(EntityShipment objES)
        {
            objSQLCon = GetSqlConnection(objES.ToolKit.objConnection);
            if (ContinueCheck() == false)
            {
                objES.lstEntityResponseStatus = lstResponse;
                return objES;
            }
            objES = FillShipment(objES);
            objES.lstEntityResponseStatus = lstResponse;
            return objES;
        }
        public EntityShipment Putback(EntityShipment objES)
        {
            objSQLCon = GetSqlConnection(objES.ToolKit.objConnection);
            if (ContinueCheck() == false)
            {
                objES.lstEntityResponseStatus = lstResponse;
                return objES;
            }
            if (InsertData(objES) == false)
                lstResponse.Add(SetResponse("Error - During Putback!", "Error - During Putback!  Contact System Administrator!", ResponseStatusType.CRITICAL));
            objES.lstEntityResponseStatus = lstResponse;
            return objES;
        }
        public EntityShipment Void(EntityShipment objES)
        {
            objSQLCon = GetSqlConnection(objES.ToolKit.objConnection);
            if (UpdateData(objES) == false)
                lstResponse.Add(SetResponse("Error - During Void!", "Error - During Void!  Contact System Administrator!", ResponseStatusType.CRITICAL));
            objES.lstEntityResponseStatus = lstResponse;
            return objES;
        }
        private bool InsertData(EntityShipment objES)
        {
            DataTable dtHeader;
            dtHeader = GetShipmentInformation(objES.objDetails.strDeliveryDocNumber);
            string str = "update solink set trknum = '" + objES.lstContainer[0].strTrackingNumber.Trim() + "', shipdate = '" + objES.dtShipDate.ToString("MM-dd-yyyy H:mm:ss") + "', shpcharge = '" + TotalFreightCosts(objES.lstContainer, dtHeader) + "' where shipid = '" + objES.objDetails.strDeliveryDocNumber.Trim() + "'";
            return ExecuteQuery(str);
        }
        private bool UpdateData(EntityShipment objES)
        {
            string str = "update solink set trknum = 'VOID' where shipid = '" + objES.objDetails.strDeliveryDocNumber.Trim() + "'";
            return ExecuteQuery(str);
        }
        private double TotalFreightCosts(List<EntityContainer> lstEC, DataTable dtHeader)
        {
            double dblFreight = 0.00;
            if (dtHeader.Rows[0]["frtprepay"] != DBNull.Value && dtHeader.Rows[0]["frtprepay"].ToString().ToUpper() == "TRUE") return dblFreight;
            dblFreight = (from c in lstEC select c.objRates.dblTotalPublishedPrice).Sum();
            return Math.Round(dblFreight, 2);
        }
        private bool ExecuteQuery(string strQuery)
        {
            try
            {
                lstResponse.Add(SetResponse(strQuery, string.Empty, ResponseStatusType.LOG));
                SqlCommand sqlCMD = new SqlCommand(strQuery, objSQLCon);
                if (objSQLCon.State == ConnectionState.Closed)
                    objSQLCon.Open();
                sqlCMD.ExecuteNonQuery();
                if (objSQLCon.State == ConnectionState.Open)
                    objSQLCon.Close();
                return true;
            }
            catch (SqlException ex)
            {
                if (objSQLCon.State == ConnectionState.Open)
                    objSQLCon.Close();
                lstResponse.Add(SetResponse(strQuery, ex.Message, ResponseStatusType.CRITICAL));
                return false;
            }
            catch (Exception ex)
            {
                if (objSQLCon.State == ConnectionState.Open)
                    objSQLCon.Close();
                lstResponse.Add(SetResponse(strQuery, ex.Message, ResponseStatusType.CRITICAL));
                return false;
            }
        }
        private SqlConnection GetSqlConnection(EntityConnection objEC)
        {
            try
            {
                objSQLCon = new SqlConnection();
                objSQLCon.ConnectionString =
                    "Data Source=" + objEC.strServer + ";" +
                    "Initial Catalog=" + objEC.strDatabase + ";" +
                    "User ID=" + objEC.strUserID + ";" +
                    "Password=" + objEC.strPassword + ";";
            }
            catch (Exception ex)
            {
                lstResponse.Add(SetResponse("Getting SQL Connection String", ex.Message, ResponseStatusType.CRITICAL));
            }
            return objSQLCon;
        }
        private EntityResponseStatus SetResponse(string strMessage, string strError, ResponseStatusType eResponseType)
        {
            EntityResponseStatus objResponse = new EntityResponseStatus();
            objResponse.Message = strMessage;
            objResponse.StatusType = eResponseType;
            objResponse.Error = strError;
            return objResponse;
        }
        private bool ContinueCheck()
        {
            var objError = (from p in lstResponse where p.StatusType == ResponseStatusType.CRITICAL || p.StatusType == ResponseStatusType.ERROR select p);

            if (objError == null || objError.Count() == 0)
                return true;
            else
                return false;
        }
        private EntityShipment FillShipment(EntityShipment objES)
        {
            DataTable dtHeader;
            dtHeader = GetShipmentInformation(objES.objDetails.strDeliveryDocNumber);
            if (ContinueCheck() == false) { return objES; }
            if (dtHeader != null && dtHeader.Rows.Count > 0)
            {
                if (dtHeader.Rows[0]["trknum"] != null && dtHeader.Rows[0]["trknum"].ToString().Trim() != "" && dtHeader.Rows[0]["trknum"].ToString().Trim() != "VOID")
                {
                    lstResponse.Add(SetResponse("Record Already Shipped", "Record Already Shipped", ResponseStatusType.WARNING));
                    return objES;
                }
                else
                {
                    objES.objShipTo = FillShipTo(dtHeader.Rows[0]);
                    objES.objDetails = FillDetails(dtHeader.Rows[0], objES.objDetails);
                    objES.objShipMethod = GetShipViaCode(dtHeader.Rows[0]);
                }
            }
            else
                lstResponse.Add(SetResponse("No Shipment Information Found!", "No Shipment Information Found!", ResponseStatusType.CRITICAL));
            return objES;
        }
        private EntityShipMethod GetShipViaCode(DataRow dr)
        {
            EntityShipMethod objSM = new EntityShipMethod();
            String strPaymentTerms = "";
            if (dr["shipvia"] != null) objSM.strShipViaCode = dr["shipvia"].ToString().Trim();
            if (dr["shpbillopt"] != null) strPaymentTerms = dr["shpbillopt"].ToString().Trim();
            switch (strPaymentTerms.ToUpper())
            {
                case "SENDER":
                    objSM.PaymentTermType = ePaymentTerms.Shipper;
                    break;
                case "RECIPIENT":
                    objSM.PaymentTermType = ePaymentTerms.Recipient;
                    break;
                case "THIRD PARTY":
                    objSM.PaymentTermType = ePaymentTerms.ThirdParty;
                    break;
                case "CONSIGNEE":
                    objSM.PaymentTermType = ePaymentTerms.Consignee;
                    break;
                case "FREIGHT COLLECT":
                    objSM.PaymentTermType = ePaymentTerms.FreightCollect;
                    break;
                default:
                    objSM.PaymentTermType = ePaymentTerms.Shipper;
                    break;
            }
            return objSM;
        }
        private DataTable GetShipmentInformation(string strDeliveryDocNumber)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select * from solink where shipid =  '" + strDeliveryDocNumber.Trim() + "'");
            return ExecuteSelectQuery(sb.ToString());
        }
        private DataTable ExecuteSelectQuery(string strStatement)
        {
            try
            {
                lstResponse.Add(SetResponse(strStatement, string.Empty, ResponseStatusType.LOG));
                DataTable dtSelect = new DataTable();
                if (objSQLCon.State == ConnectionState.Closed)
                    objSQLCon.Open();
                SqlDataAdapter daSelect = new SqlDataAdapter(strStatement, objSQLCon);
                daSelect.Fill(dtSelect);
                if (objSQLCon.State == ConnectionState.Open)
                    objSQLCon.Close();
                return dtSelect;
            }
            catch (SqlException ex)
            {
                lstResponse.Add(SetResponse(strStatement, ex.Message, ResponseStatusType.CRITICAL));
            }
            catch (Exception ex)
            {
                lstResponse.Add(SetResponse(strStatement, ex.Message, ResponseStatusType.CRITICAL));
            }
            finally
            {
                if (objSQLCon.State == ConnectionState.Open)
                    objSQLCon.Close();
            }
            return null;
        }
        private EntityAddress FillShipTo(DataRow dr)
        {
            EntityAddress objEntityAddress = new EntityAddress();
            try
            {
                if (dr["CompanyName"] != DBNull.Value) objEntityAddress.strCompanyName = dr["CompanyName"].ToString().Trim();
                if (dr["Address1"] != DBNull.Value) objEntityAddress.strAddressLine1 = dr["Address1"].ToString().Trim();
                if (dr["Address2"] != DBNull.Value) objEntityAddress.strAddressLine2 = dr["Address2"].ToString().Trim();
                if (dr["Address3"] != DBNull.Value) objEntityAddress.strAddressLine3 = dr["Address3"].ToString().Trim();
                if (dr["City"] != DBNull.Value) objEntityAddress.strCity = dr["City"].ToString().Trim();
                if (dr["ShipState"] != DBNull.Value) objEntityAddress.strState = dr["ShipState"].ToString().Trim();
                if (dr["ZipCode"] != DBNull.Value) objEntityAddress.strPostalCode = dr["ZipCode"].ToString().Trim();
                if (dr["PhoneNumber"] != DBNull.Value) objEntityAddress.strPhoneNumber = dr["PhoneNumber"].ToString().Trim();
                if (dr["AttentionTo"] != DBNull.Value) objEntityAddress.strContactName = dr["AttentionTo"].ToString().Trim();
                if (dr["SenderEmailAddress1"] != DBNull.Value) objEntityAddress.strEmailAddress = dr["SenderEmailAddress1"].ToString().Trim();
                if (dr["Country"] != DBNull.Value) objEntityAddress.strCountryCode = dr["Country"].ToString().Trim();
            }
            catch (Exception ex)
            {
                lstResponse.Add(SetResponse("Error Filling In Ship To Details!", ex.Message, ResponseStatusType.WARNING));
            }

            return objEntityAddress;
        }
        private EntityShipmentDetails FillDetails(DataRow dr, EntityShipmentDetails objSD)
        {
            try
            {
                if (dr["sono"] != DBNull.Value) objSD.strPONumber = dr["sono"].ToString().Trim();
                if (dr["invno"] != DBNull.Value) objSD.strInvoiceNumber = dr["invno"].ToString().Trim();
            }
            catch (Exception ex)
            {
                lstResponse.Add(SetResponse("Error Filling In Shipment Details!", ex.Message, ResponseStatusType.WARNING));
            }
            return objSD;
        }
    }
}
