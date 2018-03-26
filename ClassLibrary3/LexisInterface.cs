using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using SmartLincInterface;

namespace Lexis
{
    public class LexisInterface
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
                    "Server=" +   objEC.strDSNName  + ";" +
                    "Database=" + objEC.strDatabase + ";" +
                    "User Id=" +  objEC.strUserID   + ";" +
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

        private EntityShipment FillShipment(EntityShipment objES)
        {
            DataTable dtHeader;
            if (objES.objDetails.strDeliveryDocNumber.Length > 14)
            {
                objES.objDetails.strDeliveryDocNumber = objES.objDetails.strDeliveryDocNumber.Substring(8, 6);
            }
            dtHeader = GetShipmentInformation(objES.objDetails.strDeliveryDocNumber);
            if (ContinueCheck() == false) { return objES; }
            if (dtHeader != null && dtHeader.Rows.Count > 0)
            {
                objES.objShipTo = FillShipTo(dtHeader.Rows[0]);
                objES.objDetails = FillDetails(dtHeader.Rows[0], objES.objDetails);
                objES.objShipMethod = GetShipViaCode(dtHeader.Rows[0]);
                objES.objShipFrom = FillShipFrom(dtHeader.Rows[0]);
            }
            else
                lstResponse.Add(SetResponse("No Shipment Information Found!", "No Shipment Information Found!", ResponseStatusType.CRITICAL));
            return objES;
        }

        private DataTable GetShipmentInformation(string strDeliveryDocNumber)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select * from dbo.Shipments where LTRIM(RTRIM(Reference1)) =  '" + strDeliveryDocNumber.Trim() + "'" + "and SQLStatus = 1");
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
                if (dr["Reference2"] != DBNull.Value) objSD.strPONumber = dr["Reference2"].ToString().Trim();
                if (dr["Reference3"] != DBNull.Value) objSD.strInvoiceNumber = dr["Reference3"].ToString().Trim();
                if (dr["Instructions"] != DBNull.Value) objSD.strShippingInstructions = dr["Instructions"].ToString().Trim();
            }
            catch (Exception ex)
            {
                lstResponse.Add(SetResponse("Error Filling In Shipment Details!", ex.Message, ResponseStatusType.WARNING));
            }
            return objSD;
        }

        private EntityShipMethod GetShipViaCode(DataRow dr)
        {
            EntityShipMethod objSM = new EntityShipMethod();
            String strPaymentType = "";
            if (dr["ServiceType"] != null) objSM.strShipViaCode = dr["ServiceType"].ToString().Trim();
            if (dr["PaymentType"] != null) strPaymentType = dr["PaymentType"].ToString().Trim();
            if (dr["SenderAccount"] != null) objSM.strAccountNumber = dr["SenderAccount"].ToString().Trim();
            switch (strPaymentType.ToUpper())
            {
                case "PPD":
                    objSM.PaymentTermType = ePaymentTerms.Shipper;
                    break;
                case "COL":
                    objSM.PaymentTermType = ePaymentTerms.Recipient;
                    break;
                case "3RD":
                    objSM.PaymentTermType = ePaymentTerms.ThirdParty;
                    break;
                case "CON":
                    objSM.PaymentTermType = ePaymentTerms.Consignee;
                    break;
                case "FRC":
                    objSM.PaymentTermType = ePaymentTerms.FreightCollect;
                    break;
                default:
                    objSM.PaymentTermType = ePaymentTerms.Shipper;
                    break;
            }
            return objSM;
        }

        private EntityAddress FillShipFrom(DataRow dr)
        {
            EntityAddress objSF =  new EntityAddress();
            try
            {
                if (dr["ReturnAddressCompany"] != DBNull.Value) objSF.strCompanyName = dr["ReturnAddressCompany"].ToString().Trim();
                if (dr["ReturnAddressLine1"] != DBNull.Value) objSF.strAddressLine1 = dr["ReturnAddressLine1"].ToString().Trim();
                if (dr["ReturnAddressLine2"] != DBNull.Value) objSF.strAddressLine2 = dr["ReturnAddressLine2"].ToString().Trim();
                if (dr["ReturnAddressCity"] != DBNull.Value) objSF.strCity = dr["ReturnAddressCity"].ToString().Trim();
                if (dr["ReturnAddressState"] != DBNull.Value) objSF.strState = dr["ReturnAddressState"].ToString().Trim();
                if (dr["ReturnAddressZip"] != DBNull.Value) objSF.strPostalCode = dr["ReturnAddressZip"].ToString().Trim();
                if (dr["ReturnAddressAttention"] != DBNull.Value) objSF.strContactName = dr["ReturnAddressAttention"].ToString().Trim();
                if (dr["ReturnAddressPhone"] != DBNull.Value) objSF.strPhoneNumber = dr["ReturnAddressPhone"].ToString().Trim();
                if (objSF.strCompanyName == "LexisNexis Matthew Bender") objSF.strEmailAddress = "Kimberly.Agee@lexisnexis.com";
            }
            catch (Exception ex)
            {
                lstResponse.Add(SetResponse("Error Filling In Ship From Details!", ex.Message, ResponseStatusType.WARNING));
            }
            return objSF;
        }

        private bool InsertData(EntityShipment objES)
        {
            DataTable dtHeader;
            bool InsertStatus;
            int x = 0;
            int intTotalContainers = objES.lstContainer.Count();
            dtHeader = GetShipmentInformation(objES.objDetails.strDeliveryDocNumber);
            do
            {
                string str = "INSERT INTO dbo.Shipments (Reference1, Reference2, Reference3, Reference4, Reference5, VoidIndicator, CompanyName, AttentionTo, Address1, Address2, Address3, City, ShipState, ZipCode, Country, ShippedBy, TrackingNumber, ShipWeight, TotalContainers, ThisContainer, CarrierCharge, ListCharge, SourceSystem, SQLStatus, ShipDate, LastModified, BOL, CarrierNumber, PlantNumber, PhoneNumber, ServiceType, CarrierName, ServiceName, SenderAccount, AdditionalCharge, ProcessDate, TerminalID, ProcessStatus, ThirdPartyAccount, CustomerReference, SenderEmailAddress1, SenderEmailNotify1, SenderEmailAddress2, SenderEmailNotify2, SenderEmailAddress3, SenderEmailNotify3, SenderEmailAddress4, SenderEmailNotify4, ResidentialStatus, EmailText, SignatureOption, SaturdayDelivery, PaymentType, BillToCompany, BillToAttention, BillToAddress1, BillToAddress2, BillToCity, BillToState, BillToZip, ReturnAddressCompany, ReturnAddressAttention, ReturnAddressLine1, ReturnAddressLine2, ReturnAddressCity, ReturnAddressState, ReturnAddressZip, Instructions) VALUES ('" +
                objES.objDetails.strDeliveryDocNumber + "', '" +               // Reference1
                objES.objDetails.strPONumber + "', '" +                        // Reference2 
                objES.objDetails.strInvoiceNumber + "', '" +                   // Reference3
                " " + "', '" +                                                 // Reference4
                " " + "', '" +                                                 // Reference5
                " " + "', '" +                                                 // VoidIndicator  
                objES.objShipTo.strCompanyName + "', '" +                      // CompanyName
                objES.objShipTo.strContactName + "', '" +                      // AttentionTo
                objES.objShipTo.strAddressLine1 + "', '" +                     // Address1
                objES.objShipTo.strAddressLine2 + "', '" +                     // Address2
                objES.objShipTo.strAddressLine3 + "', '" +                     // Address3
                objES.objShipTo.strCity + "', '" +                             // City
                objES.objShipTo.strState + "', '" +                            // ShipState
                objES.objShipTo.strPostalCode + "', '" +                       // ZipCode
                objES.objShipTo.strCountryCode + "', '" +                      // CountryCode 
                objES.strRequesterID + "', '" +                                // ShippedBy
                objES.lstContainer[x].strTrackingNumber + "', " +              // TrackingNumber
                objES.lstContainer[x].dblTotalWeight + ", " +                  // Weight
                intTotalContainers + ", " +                                    // TotalContainers
                (x + 1) + ", " +                                               // ThisContainer
                objES.lstContainer[x].objRates.dblTotalBillablePrice+ ", " +   // CarrierCharge
                objES.lstContainer[x].objRates.dblTotalPublishedPrice + ", " + // ListCharge
                "'ProcLexis '," +                                              // SourceSystem
                "2, '" +                                                       // SQLStatus
                objES.dtShipDate.ToString("MM/dd/yyyy") + "', '" +             // ShipDate
                DateTime.Now + "', " +                                         // LastModified
                0 + ", " +                                                     // BOL 
                0 + ", " +                                                     // CarrierNumber
                0 + ", '" +                                                    // PlantNumber
                objES.objShipTo.strPhoneNumber + "', '" +                      // PhoneNumber
                objES.objShipMethod.strShipViaCode + "', '" +                  // ServiceType
                objES.objShipMethod.strCarrier + "', '" +                      // CarrierName
                objES.objShipMethod.strServiceLevel + "', '" +                 // ServiceName
                objES.objShipMethod.strAccountNumber + "', '" +                // SenderAccount
                " " + "', '" +                                                 // AdditionalCharge
                " " + "', '" +                                                 // ProcessDate
                " " + "', '" +                                                 // TerminalID
                " " + "', '" +                                                 // ProcessStatus
                " " + "', '" +                                                 // ThirdPartyAccount
                " " + "', '" +                                                 // CustomerReference 
                " " + "', '" +                                                 // SenderEmailAddress1
                " " + "', '" +                                                 // SenderEmailNotify1
                " " + "', '" +                                                 // SenderEmailAddress2
                " " + "', '" +                                                 // SenderEmailNotify2
                " " + "', '" +                                                 // SenderEmailAddress3
                " " + "', '" +                                                 // SenderEmailNotify3
                " " + "', '" +                                                 // SenderEmailAddress4
                " " + "', '" +                                                 // SenderEmailNotify4
                " " + "', '" +                                                 // ResidentialStatus
                " " + "', '" +                                                 // EmailText
                " " + "', '" +                                                 // SignatureOption
                " " + "', '" +                                                 // SaturdayDelivery
                PayTypeToCMS(objES.objShipMethod.strPaymentTerms) + "',  '" +  // PaymentType
                " " + "', '" +                                                 // BillToCompany
                " " + "', '" +                                                 // BillToAttention
                " " + "', '" +                                                 // BillToAddress1
                " " + "', '" +                                                 // BillToAddress2
                " " + "', '" +                                                 // BillToCity
                " " + "', '" +                                                 // BillToState
                " " + "', '" +                                                 // BillToZip
                objES.objShipFrom.strCompanyName + "', '" +                    // ReturnAddressCompany
                objES.objShipFrom.strContactName + "', '" +                    // ReturnAddressAttention
                objES.objShipFrom.strAddressLine1 + "', '" +                   // ReturnAddress1
                objES.objShipFrom.strAddressLine2 + "', '" +                   // ReturnAddress2
                objES.objShipFrom.strCity + "', '" +                           // ReturnAddressCity
                objES.objShipFrom.strState + "', '" +                          // ReturnAddressState
                objES.objShipFrom.strPostalCode + "', '" +                     // ReturnAddressZip
                " " + "'" +                                                    // Instructions
                ")";

                InsertStatus = ExecuteQuery(str);
                x++;
            } while (x < intTotalContainers);
            return InsertStatus;
        }

        private bool UpdateData(EntityShipment objES)
        {
            // string str = "INSERT INTO dbo.Shipments (Reference1, Reference2, Reference3, Reference4, Reference5, BOL, CarrierNumber, PlantNumber, PhoneNumber, TrackingNumber, ServiceType, VoidIndicator, CompanyName, AttentionTo, Address1, Address2, Address3, City, ShipState, ZipCode, Country, ShipWeight, CarrierCharge, ListCharge, AdditionalCharge, ProcessDate, TerminalID, ProcessStatus, ThirdPartyAccount, CustomerReference, SenderEmailAddress1, SenderEmailNotify1, SenderEmailAddress2, SenderEmailNotify2, SenderEmailAddress3, SenderEmailNotify3, SenderEmailAddress4, SenderEmailNotify4, ResidentialStatus, EmailText, SignatureOption, SaturdayDelivery, PaymentType, BillToCompany, BillToAttention, BillToAddress1, BillToAddress2, BillToCity, BillToState, BillToZip, ReturnAddressCompany, ReturnAddressAttention, ReturnAddressLine1, ReturnAddressLine2, ReturnAddressCity, ReturnAddressState, ReturnAddressZip, ShipDate, SourceSystem, SQLStatus) VALUES (Reference1, Reference2, Reference3, Reference4, Reference5, BOL, CarrierNumber, PlantNumber, PhoneNumber, TrackingNumber, ServiceType, VoidIndicator, CompanyName, AttentionTo, Address1, Address2, Address3, City, ShipState, ZipCode, Country, ShipWeight, CarrierCharge, ListCharge, AdditionalCharge, ProcessDate, TerminalID, ProcessStatus, ThirdPartyAccount, CustomerReference, SenderEmailAddress1, SenderEmailNotify1, SenderEmailAddress2, SenderEmailNotify2, SenderEmailAddress3, SenderEmailNotify3, SenderEmailAddress4, SenderEmailNotify4, ResidentialStatus, EmailText, SignatureOption, SaturdayDelivery, PaymentType, BillToCompany, BillToAttention, BillToAddress1, BillToAddress2, BillToCity, BillToState, BillToZip, ReturnAddressCompany, ReturnAddressAttention, ReturnAddressLine1, ReturnAddressLine2, ReturnAddressCity, ReturnAddressState, ReturnAddressZip, ShipDate, SourceSystem, SQLStatus)";
            DataTable dtHeader;
            bool UpdateStatus;
            int x = 0;
            int intTotalContainers = objES.lstContainer.Count();
            dtHeader = GetShipmentInformation(objES.objDetails.strDeliveryDocNumber);
            do
            {
                string str = "INSERT INTO dbo.Shipments (Reference1, Reference2, Reference3, Reference4, Reference5, VoidIndicator, CompanyName, AttentionTo, Address1, Address2, Address3, City, ShipState, ZipCode, Country, ShippedBy, TrackingNumber, ShipWeight, TotalContainers, ThisContainer, CarrierCharge, ListCharge, SourceSystem, SQLStatus, ShipDate, LastModified, BOL, CarrierNumber, PlantNumber, PhoneNumber, ServiceType, CarrierName, ServiceName, SenderAccount, AdditionalCharge, ProcessDate, TerminalID, ProcessStatus, ThirdPartyAccount, CustomerReference, SenderEmailAddress1, SenderEmailNotify1, SenderEmailAddress2, SenderEmailNotify2, SenderEmailAddress3, SenderEmailNotify3, SenderEmailAddress4, SenderEmailNotify4, ResidentialStatus, EmailText, SignatureOption, SaturdayDelivery, PaymentType, BillToCompany, BillToAttention, BillToAddress1, BillToAddress2, BillToCity, BillToState, BillToZip, ReturnAddressCompany, ReturnAddressAttention, ReturnAddressLine1, ReturnAddressLine2, ReturnAddressCity, ReturnAddressState, ReturnAddressZip, Instructions) VALUES ('" +
                objES.objDetails.strDeliveryDocNumber + "', '" +               // Reference1
                objES.objDetails.strPONumber + "', '" +                        // Reference2 
                objES.objDetails.strInvoiceNumber + "', '" +                   // Reference3
                " " + "', '" +                                                 // Reference4
                " " + "', '" +                                                 // Reference5
                "Y" + "', '" +                                                 // VoidIndicator  
                objES.objShipTo.strCompanyName + "', '" +                      // CompanyName
                objES.objShipTo.strContactName + "', '" +                      // AttentionTo
                objES.objShipTo.strAddressLine1 + "', '" +                     // Address1
                objES.objShipTo.strAddressLine2 + "', '" +                     // Address2
                objES.objShipTo.strAddressLine3 + "', '" +                     // Address3
                objES.objShipTo.strCity + "', '" +                             // City
                objES.objShipTo.strState + "', '" +                            // ShipState
                objES.objShipTo.strPostalCode + "', '" +                       // ZipCode
                objES.objShipTo.strCountryCode + "', '" +                      // CountryCode 
                objES.strRequesterID + "', '" +                                // ShippedBy
                objES.lstContainer[x].strTrackingNumber + "', " +              // TrackingNumber
                objES.lstContainer[x].dblTotalWeight + ", " +                  // Weight
                intTotalContainers + ", " +                                    // TotalContainers
                (x + 1) + ", " +                                               // ThisContainer
                objES.lstContainer[x].objRates.dblTotalDiscountedPrice + ", " +  // CarrierCharge
                objES.lstContainer[x].objRates.dblTotalPublishedPrice + ", " + // ListCharge
                "'ProcLexis '," +                                              // SourceSystem
                "2, '" +                                                       // SQLStatus
                objES.dtShipDate.ToString("MM/dd/yyyy") + "', '" +             // ShipDate
                DateTime.Now + "', " +                                         // LastModified
                0 + ", " +                                                     // BOL 
                0 + ", " +                                                     // CarrierNumber
                0 + ", '" +                                                    // PlantNumber
                objES.objShipTo.strPhoneNumber + "', '" +                      // PhoneNumber
                objES.objShipMethod.strShipViaCode + "', '" +                  // ServiceType
                objES.objShipMethod.strCarrier + "', '" +                      // CarrierName
                objES.objShipMethod.strServiceLevel + "', '" +                 // ServiceName
                objES.objShipMethod.strAccountNumber + "', '" +                // SenderAccount
                " " + "', '" +                                                 // AdditionalCharge
                " " + "', '" +                                                 // ProcessDate
                " " + "', '" +                                                 // TerminalID
                " " + "', '" +                                                 // ProcessStatus
                " " + "', '" +                                                 // ThirdPartyAccount
                " " + "', '" +                                                 // CustomerReference 
                " " + "', '" +                                                 // SenderEmailAddress1
                " " + "', '" +                                                 // SenderEmailNotify1
                " " + "', '" +                                                 // SenderEmailAddress2
                " " + "', '" +                                                 // SenderEmailNotify2
                " " + "', '" +                                                 // SenderEmailAddress3
                " " + "', '" +                                                 // SenderEmailNotify3
                " " + "', '" +                                                 // SenderEmailAddress4
                " " + "', '" +                                                 // SenderEmailNotify4
                " " + "', '" +                                                 // ResidentialStatus
                " " + "', '" +                                                 // EmailText
                " " + "', '" +                                                 // SignatureOption
                " " + "', '" +                                                 // SaturdayDelivery
                PayTypeToCMS(objES.objShipMethod.strPaymentTerms) + "',  '" +  // PaymentType
                " " + "', '" +                                                 // BillToCompany
                " " + "', '" +                                                 // BillToAttention
                " " + "', '" +                                                 // BillToAddress1
                " " + "', '" +                                                 // BillToAddress2
                " " + "', '" +                                                 // BillToCity
                " " + "', '" +                                                 // BillToState
                " " + "', '" +                                                 // BillToZip
                objES.objShipFrom.strCompanyName + "', '" +                    // ReturnAddressCompany
                objES.objShipFrom.strContactName + "', '" +                    // ReturnAddressAttention
                objES.objShipFrom.strAddressLine1 + "', '" +                   // ReturnAddress1
                objES.objShipFrom.strAddressLine2 + "', '" +                   // ReturnAddress2
                objES.objShipFrom.strCity + "', '" +                           // ReturnAddressCity
                objES.objShipFrom.strState + "', '" +                          // ReturnAddressState
                objES.objShipFrom.strPostalCode + "', '" +                     // ReturnAddressZip
                " " + "'" +                                                    // Instructions
                ")";

                UpdateStatus = ExecuteQuery(str);
                x++;
            } while (x < intTotalContainers);
            return UpdateStatus;
        }

        private bool ContinueCheck()
        {
            var objError = (from p in lstResponse where p.StatusType == ResponseStatusType.CRITICAL || p.StatusType == ResponseStatusType.ERROR select p);

            if (objError == null || objError.Count() == 0)
                return true;
            else
                return false;
        }

        private string PayTypeToCMS(string PayType)
        {
            string CMSPayType = "";
            switch (PayType.ToUpper())
            {
                case "SENDER":
                    CMSPayType = "PPD";
                    break;
                case "RECIPIENT":
                    CMSPayType = "COL";
                    break;
                case "THIRDPARTY":
                    CMSPayType = "3RD";
                    break;
                case "CONSIGNEE":
                    CMSPayType = "CON";
                    break;
                case "FREIGHTCOLLECT":
                    CMSPayType = "FRC";
                    break;
                default:
                    CMSPayType = PayType.Substring(0, 3);
                    break;
            }
            return CMSPayType;
        }
    }
}
