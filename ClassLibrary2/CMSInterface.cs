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

        private SqlConnection GetSqlConnection(EntityConnection objEC)
        {
            try
            {
                objSQLCon = new SqlConnection();
                objSQLCon.ConnectionString =
                    "Provider=IBMDA400; " +
                    "Data Source=" + objEC.strDSNName + ";" +
                    "Default Collection=" + objEC.strDatabase + ";" +
                    "User Id=" + objEC.strUserID + ";" +
                    "Password=" + objEC.strPassword + ";";
            }
            catch (Exception ex)
            {
                lstResponse.Add(SetResponse("Getting SQL Connection String", ex.Message, ResponseStatusType.CRITICAL));
            }
            return objSQLCon;
        }

        private bool InsertData(EntityShipment objES)
        {
            DataTable dtHeader;
            bool InsertStatus;
            int x = 0;
            int intTotalContainers = objES.lstContainer.Count();
            int intBOL = 0;
            int intCarrier = 0;
            int intPlant = 0;
            string strSQLSource = "CMS";
            string strThirdParty = objES.objDetails.objMiscellaneous.strMisc5;
            Int32.TryParse(objES.objDetails.objMiscellaneous.strMisc2, out intBOL);
            Int32.TryParse(objES.objDetails.objMiscellaneous.strMisc3, out intCarrier);
            Int32.TryParse(objES.objDetails.objMiscellaneous.strMisc4, out intPlant);
            string strSQLSystem = "";
            switch (strSQLSource.ToUpper())
            {
                case "SAPUSPS":
                    strSQLSystem = "PSSAPUSPS";
                    break;
                case "SAPUPS":
                    strSQLSystem = "PSSAPUPS";
                    break;
                case "SAPFEDEX":
                    strSQLSystem = "PSSAPFedEx";
                    break;
                default:
                    strSQLSystem = "PSSAP";
                    break;
            }
            dtHeader = GetShipmentInformation(objES.objDetails.strDeliveryDocNumber);
            do
            {
                SqlCommand sqlCMD = objSQLCon.CreateCommand();
                {
                    sqlCMD.Connection = objSQLCon;
                    sqlCMD.CommandType = CommandType.Text;
                    sqlCMD.CommandText = "INSERT INTO dbo.Shipments (Reference1, Reference2, Reference3, Reference4, Reference5, VoidIndicator, CompanyName, AttentionTo, Address1, Address2, Address3, City, ShipState, ZipCode, Country, ShippedBy, TrackingNumber, ShipWeight, TotalContainers, ThisContainer, CarrierCharge, ListCharge, SourceSystem, SQLStatus, ShipDate, LastModified, BOL, CarrierNumber, PlantNumber, PhoneNumber, ServiceType, ServiceName, SenderAccount, AdditionalCharge, ProcessDate, TerminalID, ProcessStatus, ThirdPartyAccount, CustomerReference, SenderEmailAddress1, SenderEmailNotify1, SenderEmailAddress2, SenderEmailNotify2, SenderEmailAddress3, SenderEmailNotify3, SenderEmailAddress4, SenderEmailNotify4, ResidentialStatus, EmailText, SignatureOption, SaturdayDelivery, ReturnAddressCompany, ReturnAddressAttention, ReturnAddressLine1, ReturnAddressLine2, ReturnAddressCity, ReturnAddressState, ReturnAddressZip, Instructions) VALUES (@Reference1, @Reference2, @Reference3, @Reference4, @Reference5, @VoidIndicator, @CompanyName, @AttentionTo, @Address1, @Address2, @Address3, @City, @ShipState, @ZipCode, @Country, @ShippedBy, @TrackingNumber, @ShipWeight, @TotalContainers, @ThisContainer, @CarrierCharge, @ListCharge, @SourceSystem, @SQLStatus, @ShipDate, @LastModified, @BOL, @CarrierNumber, @PlantNumber, @PhoneNumber, @ServiceType, @ServiceName, @SenderAccount, @AdditionalCharge, @ProcessDate, @TerminalID, @ProcessStatus, @ThirdPartyAccount, @CustomerReference, @SenderEmailAddress1, @SenderEmailNotify1, @SenderEmailAddress2, @SenderEmailNotify2, @SenderEmailAddress3, @SenderEmailNotify3, @SenderEmailAddress4, @SenderEmailNotify4, @ResidentialStatus, @EmailText, @SignatureOption, @SaturdayDelivery, @ReturnAddressCompany, @ReturnAddressAttention, @ReturnAddressLine1, @ReturnAddressLine2, @ReturnAddressCity, @ReturnAddressState, @ReturnAddressZip, @Instructions)";
                    sqlCMD.Parameters.AddWithValue("@Reference1", TestIfNull(objES.objDetails.strDeliveryDocNumber));
                    sqlCMD.Parameters.AddWithValue("@Reference2", TestIfNull(objES.objDetails.strPONumber));
                    sqlCMD.Parameters.AddWithValue("@Reference3", " ");
                    sqlCMD.Parameters.AddWithValue("@Reference4", " ");
                    sqlCMD.Parameters.AddWithValue("@Reference5", " ");
                    sqlCMD.Parameters.AddWithValue("@VoidIndicator", " ");
                    sqlCMD.Parameters.AddWithValue("@CompanyName", TestIfNull(objES.objShipTo.strCompanyName));
                    sqlCMD.Parameters.AddWithValue("@AttentionTo", TestIfNull(objES.objShipTo.strContactName));
                    sqlCMD.Parameters.AddWithValue("@Address1", TestIfNull(objES.objShipTo.strAddressLine1));
                    sqlCMD.Parameters.AddWithValue("@Address2", TestIfNull(objES.objShipTo.strAddressLine2));
                    sqlCMD.Parameters.AddWithValue("@Address3", TestIfNull(objES.objShipTo.strAddressLine3));
                    sqlCMD.Parameters.AddWithValue("@City", TestIfNull(objES.objShipTo.strCity));
                    sqlCMD.Parameters.AddWithValue("@ShipState", TestIfNull(objES.objShipTo.strState));
                    sqlCMD.Parameters.AddWithValue("@ZipCode", TestIfNull(objES.objShipTo.strPostalCode));
                    sqlCMD.Parameters.AddWithValue("@Country", TestIfNull(objES.objShipTo.strCountryCode));
                    sqlCMD.Parameters.AddWithValue("@ShippedBy", TestIfNull(objES.strRequesterID));
                    sqlCMD.Parameters.AddWithValue("@TrackingNumber", TestIfNull(objES.lstContainer[x].strTrackingNumber));
                    sqlCMD.Parameters.AddWithValue("@ShipWeight", objES.lstContainer[x].dblTotalWeight);
                    sqlCMD.Parameters.AddWithValue("@TotalContainers", intTotalContainers);
                    sqlCMD.Parameters.AddWithValue("@ThisContainer", (x + 1));
                    sqlCMD.Parameters.AddWithValue("@CarrierCharge", objES.lstContainer[x].objRates.dblTotalDiscountedPrice);
                    sqlCMD.Parameters.AddWithValue("@ListCharge", objES.lstContainer[x].objRates.dblTotalPublishedPrice);
                    sqlCMD.Parameters.AddWithValue("@SourceSystem", strSQLSystem);
                    sqlCMD.Parameters.AddWithValue("@SQLStatus", 2);
                    sqlCMD.Parameters.AddWithValue("@ShipDate", objES.dtShipDate.ToString("MM/dd/yyyy"));
                    sqlCMD.Parameters.AddWithValue("@LastModified", DateTime.Now);
                    sqlCMD.Parameters.AddWithValue("@BOL", intBOL);
                    sqlCMD.Parameters.AddWithValue("@CarrierNumber", intCarrier);
                    sqlCMD.Parameters.AddWithValue("@PlantNumber", intPlant);
                    sqlCMD.Parameters.AddWithValue("@PhoneNumber", TestIfNull(objES.objShipTo.strPhoneNumber));
                    sqlCMD.Parameters.AddWithValue("@ServiceType", TestIfNull(objES.objShipMethod.strShipViaCode));
                    sqlCMD.Parameters.AddWithValue("@ServiceName", TestIfNull(objES.objShipMethod.strServiceLevel));
                    sqlCMD.Parameters.AddWithValue("@SenderAccount", TestIfNull(objES.objShipMethod.strAccountNumber));
                    sqlCMD.Parameters.AddWithValue("@AdditionalCharge", " ");
                    sqlCMD.Parameters.AddWithValue("@ProcessDate", " ");
                    sqlCMD.Parameters.AddWithValue("@TerminalID", " ");
                    sqlCMD.Parameters.AddWithValue("@ProcessStatus", " ");
                    sqlCMD.Parameters.AddWithValue("@ThirdPartyAccount", TestIfNull(strThirdParty));
                    sqlCMD.Parameters.AddWithValue("@CustomerReference", " ");
                    sqlCMD.Parameters.AddWithValue("@SenderEmailAddress1", " ");
                    sqlCMD.Parameters.AddWithValue("@SenderEmailNotify1", " ");
                    sqlCMD.Parameters.AddWithValue("@SenderEmailAddress2", " ");
                    sqlCMD.Parameters.AddWithValue("@SenderEmailNotify2", " ");
                    sqlCMD.Parameters.AddWithValue("@SenderEmailAddress3", " ");
                    sqlCMD.Parameters.AddWithValue("@SenderEmailNotify3", " ");
                    sqlCMD.Parameters.AddWithValue("@SenderEmailAddress4", " ");
                    sqlCMD.Parameters.AddWithValue("@SenderEmailNotify4", " ");
                    sqlCMD.Parameters.AddWithValue("@ResidentialStatus", " ");
                    sqlCMD.Parameters.AddWithValue("@EmailText", " ");
                    sqlCMD.Parameters.AddWithValue("@SignatureOption", " ");
                    sqlCMD.Parameters.AddWithValue("@SaturdayDelivery", " ");
                    //                    sqlCMD.Parameters.AddWithValue("@PaymentType", PayTypeToCMS(objES.objShipMethod.strPaymentTerms));
                    //                    sqlCMD.Parameters.AddWithValue("@BillToCompany", TestIfNull(objES.objBillTo.strCompanyName));
                    //                    sqlCMD.Parameters.AddWithValue("@BillToAttention", TestIfNull(objES.objBillTo.strContactName));
                    //                    sqlCMD.Parameters.AddWithValue("@BillToAddress1", TestIfNull(objES.objBillTo.strAddressLine1));
                    //                    sqlCMD.Parameters.AddWithValue("@BillToAddress2", TestIfNull(objES.objBillTo.strAddressLine2));
                    //                    sqlCMD.Parameters.AddWithValue("@BillToCity", TestIfNull(objES.objBillTo.strCity));
                    //                    sqlCMD.Parameters.AddWithValue("@BillToState", TestIfNull(objES.objBillTo.strState));
                    //                    sqlCMD.Parameters.AddWithValue("@BillToZip", TestIfNull(objES.objBillTo.strPostalCode));
                    sqlCMD.Parameters.AddWithValue("@ReturnAddressCompany", TestIfNull(objES.objShipFrom.strCompanyName));
                    sqlCMD.Parameters.AddWithValue("@ReturnAddressAttention", TestIfNull(objES.objShipFrom.strContactName));
                    sqlCMD.Parameters.AddWithValue("@ReturnAddressLine1", TestIfNull(objES.objShipFrom.strAddressLine1));
                    sqlCMD.Parameters.AddWithValue("@ReturnAddressLine2", TestIfNull(objES.objShipFrom.strAddressLine2));
                    sqlCMD.Parameters.AddWithValue("@ReturnAddressCity", TestIfNull(objES.objShipFrom.strCity));
                    sqlCMD.Parameters.AddWithValue("@ReturnAddressState", TestIfNull(objES.objShipFrom.strState));
                    sqlCMD.Parameters.AddWithValue("@ReturnAddressZip", TestIfNull(objES.objShipFrom.strPostalCode));
                    sqlCMD.Parameters.AddWithValue("@Instructions", " ");


                    string strQuery = sqlCMD.CommandText;
                    foreach (SqlParameter p in sqlCMD.Parameters)
                    {
                        strQuery = strQuery.Replace(p.ParameterName, p.Value.ToString());
                    }
                    try
                    {
                        lstResponse.Add(SetResponse(strQuery, string.Empty, ResponseStatusType.LOG));
                        if (objSQLCon.State == ConnectionState.Closed)
                            objSQLCon.Open();
                        int recordsAffected = sqlCMD.ExecuteNonQuery();
                        if (objSQLCon.State == ConnectionState.Open)
                            objSQLCon.Close();
                        InsertStatus = true;
                    }
                    catch (SqlException ex)
                    {
                        if (objSQLCon.State == ConnectionState.Open)
                            objSQLCon.Close();
                        lstResponse.Add(SetResponse(strQuery, ex.Message, ResponseStatusType.CRITICAL));
                        InsertStatus = false;
                    }
                    catch (Exception ex)
                    {
                        if (objSQLCon.State == ConnectionState.Open)
                            objSQLCon.Close();
                        lstResponse.Add(SetResponse(strQuery, ex.Message, ResponseStatusType.CRITICAL));
                        InsertStatus = false;
                    }
                }
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
            int intBOL = 0;
            int intCarrier = 0;
            int intPlant = 0;
            string strSQLSource = "CMS";
            string strThirdParty = objES.objDetails.objMiscellaneous.strMisc5;
            Int32.TryParse(objES.objDetails.objMiscellaneous.strMisc2, out intBOL);
            Int32.TryParse(objES.objDetails.objMiscellaneous.strMisc3, out intCarrier);
            Int32.TryParse(objES.objDetails.objMiscellaneous.strMisc4, out intPlant);
            string strSQLSystem = "";
            switch (strSQLSource.ToUpper())
            {
                case "SAPUSPS":
                    strSQLSystem = "PSSAPUSPS";
                    break;
                case "SAPUPS":
                    strSQLSystem = "PSSAPUPS";
                    break;
                case "SAPFEDEX":
                    strSQLSystem = "PSSAPFedEx";
                    break;
                default:
                    strSQLSystem = "PSSAP";
                    break;
            }
            dtHeader = GetShipmentInformation(objES.objDetails.strDeliveryDocNumber);
            do
            {
                SqlCommand sqlCMD = objSQLCon.CreateCommand();
                {
                    sqlCMD.Connection = objSQLCon;
                    sqlCMD.CommandType = CommandType.Text;
                    sqlCMD.CommandText = "INSERT INTO dbo.Shipments (Reference1, Reference2, Reference3, Reference4, Reference5, VoidIndicator, CompanyName, AttentionTo, Address1, Address2, Address3, City, ShipState, ZipCode, Country, ShippedBy, TrackingNumber, ShipWeight, TotalContainers, ThisContainer, CarrierCharge, ListCharge, SourceSystem, SQLStatus, ShipDate, LastModified, BOL, CarrierNumber, PlantNumber, PhoneNumber, ServiceType, ServiceName, SenderAccount, AdditionalCharge, ProcessDate, TerminalID, ProcessStatus, ThirdPartyAccount, CustomerReference, SenderEmailAddress1, SenderEmailNotify1, SenderEmailAddress2, SenderEmailNotify2, SenderEmailAddress3, SenderEmailNotify3, SenderEmailAddress4, SenderEmailNotify4, ResidentialStatus, EmailText, SignatureOption, SaturdayDelivery, ReturnAddressCompany, ReturnAddressAttention, ReturnAddressLine1, ReturnAddressLine2, ReturnAddressCity, ReturnAddressState, ReturnAddressZip, Instructions) VALUES (@Reference1, @Reference2, @Reference3, @Reference4, @Reference5, @VoidIndicator, @CompanyName, @AttentionTo, @Address1, @Address2, @Address3, @City, @ShipState, @ZipCode, @Country, @ShippedBy, @TrackingNumber, @ShipWeight, @TotalContainers, @ThisContainer, @CarrierCharge, @ListCharge, @SourceSystem, @SQLStatus, @ShipDate, @LastModified, @BOL, @CarrierNumber, @PlantNumber, @PhoneNumber, @ServiceType, @ServiceName, @SenderAccount, @AdditionalCharge, @ProcessDate, @TerminalID, @ProcessStatus, @ThirdPartyAccount, @CustomerReference, @SenderEmailAddress1, @SenderEmailNotify1, @SenderEmailAddress2, @SenderEmailNotify2, @SenderEmailAddress3, @SenderEmailNotify3, @SenderEmailAddress4, @SenderEmailNotify4, @ResidentialStatus, @EmailText, @SignatureOption, @SaturdayDelivery, @ReturnAddressCompany, @ReturnAddressAttention, @ReturnAddressLine1, @ReturnAddressLine2, @ReturnAddressCity, @ReturnAddressState, @ReturnAddressZip, @Instructions)";
                    sqlCMD.Parameters.AddWithValue("@Reference1", TestIfNull(objES.objDetails.strDeliveryDocNumber));
                    sqlCMD.Parameters.AddWithValue("@Reference2", TestIfNull(objES.objDetails.strPONumber));
                    sqlCMD.Parameters.AddWithValue("@Reference3", " ");
                    sqlCMD.Parameters.AddWithValue("@Reference4", " ");
                    sqlCMD.Parameters.AddWithValue("@Reference5", " ");
                    sqlCMD.Parameters.AddWithValue("@VoidIndicator", "Y");
                    sqlCMD.Parameters.AddWithValue("@CompanyName", TestIfNull(objES.objShipTo.strCompanyName));
                    sqlCMD.Parameters.AddWithValue("@AttentionTo", TestIfNull(objES.objShipTo.strContactName));
                    sqlCMD.Parameters.AddWithValue("@Address1", TestIfNull(objES.objShipTo.strAddressLine1));
                    sqlCMD.Parameters.AddWithValue("@Address2", TestIfNull(objES.objShipTo.strAddressLine2));
                    sqlCMD.Parameters.AddWithValue("@Address3", TestIfNull(objES.objShipTo.strAddressLine3));
                    sqlCMD.Parameters.AddWithValue("@City", TestIfNull(objES.objShipTo.strCity));
                    sqlCMD.Parameters.AddWithValue("@ShipState", TestIfNull(objES.objShipTo.strState));
                    sqlCMD.Parameters.AddWithValue("@ZipCode", TestIfNull(objES.objShipTo.strPostalCode));
                    sqlCMD.Parameters.AddWithValue("@Country", TestIfNull(objES.objShipTo.strCountryCode));
                    sqlCMD.Parameters.AddWithValue("@ShippedBy", TestIfNull(objES.strRequesterID));
                    sqlCMD.Parameters.AddWithValue("@TrackingNumber", TestIfNull(objES.lstContainer[x].strTrackingNumber));
                    sqlCMD.Parameters.AddWithValue("@ShipWeight", objES.lstContainer[x].dblTotalWeight);
                    sqlCMD.Parameters.AddWithValue("@TotalContainers", intTotalContainers);
                    sqlCMD.Parameters.AddWithValue("@ThisContainer", (x + 1));
                    sqlCMD.Parameters.AddWithValue("@CarrierCharge", objES.lstContainer[x].objRates.dblTotalDiscountedPrice);
                    sqlCMD.Parameters.AddWithValue("@ListCharge", objES.lstContainer[x].objRates.dblTotalPublishedPrice);
                    sqlCMD.Parameters.AddWithValue("@SourceSystem", strSQLSystem);
                    sqlCMD.Parameters.AddWithValue("@SQLStatus", 2);
                    sqlCMD.Parameters.AddWithValue("@ShipDate", objES.dtShipDate.ToString("MM/dd/yyyy"));
                    sqlCMD.Parameters.AddWithValue("@LastModified", DateTime.Now);
                    sqlCMD.Parameters.AddWithValue("@BOL", intBOL);
                    sqlCMD.Parameters.AddWithValue("@CarrierNumber", intCarrier);
                    sqlCMD.Parameters.AddWithValue("@PlantNumber", intPlant);
                    sqlCMD.Parameters.AddWithValue("@PhoneNumber", TestIfNull(objES.objShipTo.strPhoneNumber));
                    sqlCMD.Parameters.AddWithValue("@ServiceType", TestIfNull(objES.objShipMethod.strShipViaCode));
                    sqlCMD.Parameters.AddWithValue("@ServiceName", TestIfNull(objES.objShipMethod.strServiceLevel));
                    sqlCMD.Parameters.AddWithValue("@SenderAccount", TestIfNull(objES.objShipMethod.strAccountNumber));
                    sqlCMD.Parameters.AddWithValue("@AdditionalCharge", " ");
                    sqlCMD.Parameters.AddWithValue("@ProcessDate", " ");
                    sqlCMD.Parameters.AddWithValue("@TerminalID", " ");
                    sqlCMD.Parameters.AddWithValue("@ProcessStatus", " ");
                    sqlCMD.Parameters.AddWithValue("@ThirdPartyAccount", TestIfNull(strThirdParty));
                    sqlCMD.Parameters.AddWithValue("@CustomerReference", " ");
                    sqlCMD.Parameters.AddWithValue("@SenderEmailAddress1", " ");
                    sqlCMD.Parameters.AddWithValue("@SenderEmailNotify1", " ");
                    sqlCMD.Parameters.AddWithValue("@SenderEmailAddress2", " ");
                    sqlCMD.Parameters.AddWithValue("@SenderEmailNotify2", " ");
                    sqlCMD.Parameters.AddWithValue("@SenderEmailAddress3", " ");
                    sqlCMD.Parameters.AddWithValue("@SenderEmailNotify3", " ");
                    sqlCMD.Parameters.AddWithValue("@SenderEmailAddress4", " ");
                    sqlCMD.Parameters.AddWithValue("@SenderEmailNotify4", " ");
                    sqlCMD.Parameters.AddWithValue("@ResidentialStatus", " ");
                    sqlCMD.Parameters.AddWithValue("@EmailText", " ");
                    sqlCMD.Parameters.AddWithValue("@SignatureOption", " ");
                    sqlCMD.Parameters.AddWithValue("@SaturdayDelivery", " ");
                    //                    sqlCMD.Parameters.AddWithValue("@PaymentType", PayTypeToCMS(objES.objShipMethod.strPaymentTerms));
                    //                    sqlCMD.Parameters.AddWithValue("@BillToCompany", TestIfNull(objES.objBillTo.strCompanyName));
                    //                    sqlCMD.Parameters.AddWithValue("@BillToAttention", TestIfNull(objES.objBillTo.strContactName));
                    //                    sqlCMD.Parameters.AddWithValue("@BillToAddress1", TestIfNull(objES.objBillTo.strAddressLine1));
                    //                    sqlCMD.Parameters.AddWithValue("@BillToAddress2", TestIfNull(objES.objBillTo.strAddressLine2));
                    //                    sqlCMD.Parameters.AddWithValue("@BillToCity", TestIfNull(objES.objBillTo.strCity));
                    //                    sqlCMD.Parameters.AddWithValue("@BillToState", TestIfNull(objES.objBillTo.strState));
                    //                    sqlCMD.Parameters.AddWithValue("@BillToZip", TestIfNull(objES.objBillTo.strPostalCode));
                    sqlCMD.Parameters.AddWithValue("@ReturnAddressCompany", TestIfNull(objES.objShipFrom.strCompanyName));
                    sqlCMD.Parameters.AddWithValue("@ReturnAddressAttention", TestIfNull(objES.objShipFrom.strContactName));
                    sqlCMD.Parameters.AddWithValue("@ReturnAddressLine1", TestIfNull(objES.objShipFrom.strAddressLine1));
                    sqlCMD.Parameters.AddWithValue("@ReturnAddressLine2", TestIfNull(objES.objShipFrom.strAddressLine2));
                    sqlCMD.Parameters.AddWithValue("@ReturnAddressCity", TestIfNull(objES.objShipFrom.strCity));
                    sqlCMD.Parameters.AddWithValue("@ReturnAddressState", TestIfNull(objES.objShipFrom.strState));
                    sqlCMD.Parameters.AddWithValue("@ReturnAddressZip", TestIfNull(objES.objShipFrom.strPostalCode));
                    sqlCMD.Parameters.AddWithValue("@Instructions", " ");


                    string strQuery = sqlCMD.CommandText;
                    foreach (SqlParameter p in sqlCMD.Parameters)
                    {
                        strQuery = strQuery.Replace(p.ParameterName, p.Value.ToString());
                    }
                    try
                    {
                        lstResponse.Add(SetResponse(strQuery, string.Empty, ResponseStatusType.LOG));
                        if (objSQLCon.State == ConnectionState.Closed)
                            objSQLCon.Open();
                        int recordsAffected = sqlCMD.ExecuteNonQuery();
                        if (objSQLCon.State == ConnectionState.Open)
                            objSQLCon.Close();
                        UpdateStatus = true;
                    }
                    catch (SqlException ex)
                    {
                        if (objSQLCon.State == ConnectionState.Open)
                            objSQLCon.Close();
                        lstResponse.Add(SetResponse(strQuery, ex.Message, ResponseStatusType.CRITICAL));
                        UpdateStatus = false;
                    }
                    catch (Exception ex)
                    {
                        if (objSQLCon.State == ConnectionState.Open)
                            objSQLCon.Close();
                        lstResponse.Add(SetResponse(strQuery, ex.Message, ResponseStatusType.CRITICAL));
                        UpdateStatus = false;
                    }
                }
                x++;
            } while (x < intTotalContainers);
            return UpdateStatus;
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
                objSD.lstReference = new List<EntityReference>();
                EntityReference objReference = new EntityReference();

                objReference = new EntityReference();
                if (dr["SourceSystem"] != DBNull.Value) objReference.strReferenceValue = dr["SourceSystem"].ToString().Trim();
                objSD.lstReference.Add(objReference);

                objReference = new EntityReference();
                if (dr["BOL"] != DBNull.Value) objReference.strReferenceValue = dr["BOL"].ToString().Trim();
                objSD.lstReference.Add(objReference);

                objReference = new EntityReference();
                if (dr["CarrierNumber"] != DBNull.Value) objReference.strReferenceValue = dr["CarrierNumber"].ToString().Trim();
                objSD.lstReference.Add(objReference);

                objReference = new EntityReference();
                if (dr["PlantNumber"] != DBNull.Value) objReference.strReferenceValue = dr["PlantNumber"].ToString().Trim();
                objSD.lstReference.Add(objReference);

                objReference = new EntityReference();
                if (dr["ThirdPartyAccount"] != DBNull.Value) objReference.strReferenceValue = dr["ThirdPartyAccount"].ToString().Trim();
                objSD.lstReference.Add(objReference);
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
            String strPaymentTerms = "";
            if (dr["ServiceType"] != null) objSM.strShipViaCode = dr["ServiceType"].ToString().Trim();
            if (dr["PaymentType"] != null) strPaymentTerms = dr["PaymentType"].ToString().Trim();
            if (dr["SenderAccount"] != null) objSM.strAccountNumber = dr["SenderAccount"].ToString().Trim();
            switch (strPaymentTerms.ToUpper())
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
            EntityAddress objSF = new EntityAddress();
            try
            {
                if (dr["ReturnAddressCompany"] != DBNull.Value) objSF.strCompanyName = dr["ReturnAddressCompany"].ToString().Trim();
                if (dr["ReturnAddressLine1"] != DBNull.Value) objSF.strAddressLine1 = dr["ReturnAddressLine1"].ToString().Trim();
                if (dr["ReturnAddressLine2"] != DBNull.Value) objSF.strAddressLine2 = dr["ReturnAddressLine2"].ToString().Trim();
                if (dr["ReturnAddressCity"] != DBNull.Value) objSF.strCity = dr["ReturnAddressCity"].ToString().Trim();
                if (dr["ReturnAddressState"] != DBNull.Value) objSF.strState = dr["ReturnAddressState"].ToString().Trim();
                if (dr["ReturnAddressZip"] != DBNull.Value) objSF.strPostalCode = dr["ReturnAddressZip"].ToString().Trim();
                if (dr["ReturnAddressAttention"] != DBNull.Value) objSF.strContactName = dr["ReturnAddressAttention"].ToString().Trim();
            }
            catch (Exception ex)
            {
                lstResponse.Add(SetResponse("Error Filling In Ship From Details!", ex.Message, ResponseStatusType.WARNING));
            }
            return objSF;
        }

        private DataTable GetShipmentInformation(string strDeliveryDocNumber)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select * from Shipments where LTRIM(RTRIM(Reference1)) =  '" + strDeliveryDocNumber.Trim() + "'" + "and SourceSystem = 'CMS' and SQLStatus = 1");
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

        public static String TestIfNull(string s)
        {
            if (String.IsNullOrEmpty(s))
                return " ";
            else
                return s;
        }

    }
}
