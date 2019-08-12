using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SmartLincInterface;
using IBM.Data.DB2.iSeries;

namespace CMS
{
    public class CMSInterface
    {
        private List<EntityResponseStatus> lstResponse = new List<EntityResponseStatus>();
        private iDB2Connection objSQLCon;
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

        private iDB2Connection GetSqlConnection(EntityConnection objEC)
        {
            try
            {
                objSQLCon = new iDB2Connection();
                objSQLCon.ConnectionString =
                    "DataSource=" + objEC.strDSNName + "; " +
                    "UserID=" + objEC.strUserID + "; " +
                    "Password=" + objEC.strPassword + ";";
            }
            catch (Exception ex)
            {
                lstResponse.Add(SetResponse("Getting SQL Connection String", ex.Message, ResponseStatusType.CRITICAL));
            }
            return objSQLCon;
        }

        private DataTable GetShipmentInformation(string strDeliveryDocNumber)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select * from SHLIB.FRF01050 where LTRIM(RTRIM(FIREF1)) =  '" + strDeliveryDocNumber.Trim() + "'" + "and FISRCE = 'CMS'");
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
                iDB2DataAdapter daSelect = new iDB2DataAdapter(strStatement, objSQLCon);
                daSelect.Fill(dtSelect);
                if (objSQLCon.State == ConnectionState.Open)
                    objSQLCon.Close();
                return dtSelect;
            }
            catch (iDB2Exception ex)
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

        private bool ExecuteQuery(string strQuery)
        {
            try
            {
                lstResponse.Add(SetResponse(strQuery, string.Empty, ResponseStatusType.LOG));
                iDB2Command sqlCMD = new iDB2Command(strQuery, objSQLCon);
                if (objSQLCon.State == ConnectionState.Closed)
                    objSQLCon.Open();
                sqlCMD.ExecuteNonQuery();
                if (objSQLCon.State == ConnectionState.Open)
                    objSQLCon.Close();
                return true;
            }
            catch (iDB2Exception ex)
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
                objES.objBillTo = FillBillTo(dtHeader.Rows[0]);
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
                if (dr["FICOMP"] != DBNull.Value) objEntityAddress.strCompanyName = dr["FICOMP"].ToString().Trim();
                if (dr["FIADD1"] != DBNull.Value) objEntityAddress.strAddressLine1 = dr["FIADD1"].ToString().Trim();
                if (dr["FIADD2"] != DBNull.Value) objEntityAddress.strAddressLine2 = dr["FIADD2"].ToString().Trim();
                if (dr["FIADD3"] != DBNull.Value) objEntityAddress.strAddressLine3 = dr["FIADD3"].ToString().Trim();
                if (dr["FICITY"] != DBNull.Value) objEntityAddress.strCity = dr["FICITY"].ToString().Trim();
                if (dr["FISTAT"] != DBNull.Value) objEntityAddress.strState = dr["FISTAT"].ToString().Trim();
                if (dr["FIZIPC"] != DBNull.Value) objEntityAddress.strPostalCode = dr["FIZIPC"].ToString().Trim();
                if (dr["FIPHON"] != DBNull.Value) objEntityAddress.strPhoneNumber = dr["FIPHON"].ToString().Trim();
                if (dr["FIATTN"] != DBNull.Value) objEntityAddress.strContactName = dr["FIATTN"].ToString().Trim();
                if (dr["FISEML"] != DBNull.Value) objEntityAddress.strEmailAddress = dr["FISEML"].ToString().Trim();
                if (dr["FICNTY"] != DBNull.Value) objEntityAddress.strCountryCode = dr["FICNTY"].ToString().Trim();
            }
            catch (Exception ex)
            {
                lstResponse.Add(SetResponse("Error Filling In Ship To Details!", ex.Message, ResponseStatusType.WARNING));
            }

            return objEntityAddress;
        }

        private EntityAddress FillBillTo(DataRow dr)
        {
            EntityAddress objBT = new EntityAddress();
            try
            {
                if (dr["FIACCT"] != DBNull.Value)
                {
                    if (dr["FIBTCM"] != DBNull.Value) objBT.strCompanyName = dr["FIBTCM"].ToString().Trim();
                    if (dr["FIBTAT"] != DBNull.Value) objBT.strContactName = dr["FIBTAT"].ToString().Trim();
                    if (dr["FIBTA1"] != DBNull.Value) objBT.strAddressLine1 = dr["FIBTA1"].ToString().Trim();
                    if (dr["FIBTA2"] != DBNull.Value) objBT.strAddressLine2 = dr["FIBTA2"].ToString().Trim();
                    if (dr["FIBTCT"] != DBNull.Value) objBT.strCity = dr["FIBTCT"].ToString().Trim();
                    if (dr["FIBTST"] != DBNull.Value) objBT.strState = dr["FIBTST"].ToString().Trim();
                    if (dr["FIBTZP"] != DBNull.Value) objBT.strPostalCode = dr["FIBTZP"].ToString().Trim();
                    if (dr["FICNTY"] != DBNull.Value) objBT.strCountryCode = dr["FICNTY"].ToString().Trim();
                    if (dr["FIACCT"] != DBNull.Value) objBT.strAccountNumber = dr["FIACCT"].ToString().Trim();
                };
            }
            catch (Exception ex)
            {
                lstResponse.Add(SetResponse("Error Filling In Bill To Details!", ex.Message, ResponseStatusType.WARNING));
            }
            return objBT;
        }

        private EntityShipmentDetails FillDetails(DataRow dr, EntityShipmentDetails objSD)
        {
            try
            {
                if (dr["FIREF2"] != DBNull.Value) objSD.strPONumber = dr["FIREF2"].ToString().Trim();
                if (dr["FIREF3"] != DBNull.Value) objSD.strInvoiceNumber = dr["FIREF3"].ToString().Trim();
                if (dr["FIINST"] != DBNull.Value) objSD.strShippingInstructions = dr["FIINST"].ToString().Trim();
                objSD.lstReference = new List<EntityReference>();
                EntityReference objReference = new EntityReference();

                objReference = new EntityReference();
                if (dr["FISRCE"] != DBNull.Value) objReference.strReferenceValue = dr["FISRCE"].ToString().Trim();
                objSD.lstReference.Add(objReference);

                objReference = new EntityReference();
                if (dr["FIBOL"] != DBNull.Value) objReference.strReferenceValue = dr["FIBOL"].ToString().Trim();
                objSD.lstReference.Add(objReference);

                objReference = new EntityReference();
                if (dr["FICARR"] != DBNull.Value) objReference.strReferenceValue = dr["FICARR"].ToString().Trim();
                objSD.lstReference.Add(objReference);

                objReference = new EntityReference();
                if (dr["FIPLNT"] != DBNull.Value) objReference.strReferenceValue = dr["FIPLNT"].ToString().Trim();
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
            string str3PAccount = "";
            if (dr["FIACCT"] != null) str3PAccount = dr["FIACCT"].ToString().Trim();
            if (dr["FISHPV"] != null) objSM.strShipViaCode = dr["FISHPV"].ToString().Trim();
            if (dr["FIPMTT"] != null) strPaymentTerms = dr["FIPMTT"].ToString().Trim();
            if (dr["FISACT"] != null) objSM.strAccountNumber = dr["FISACT"].ToString().Trim();
            if (str3PAccount.Length > 0)
            {
                objSM.strPayorAccountNumber = dr["FIACCT"].ToString().Trim();
                strPaymentTerms = "3RD";
            }
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
                if (dr["FIRACM"] != DBNull.Value) objSF.strCompanyName = dr["FIRACM"].ToString().Trim();
                if (dr["FIRAA1"] != DBNull.Value) objSF.strAddressLine1 = dr["FIRAA1"].ToString().Trim();
                if (dr["FIRAA2"] != DBNull.Value) objSF.strAddressLine2 = dr["FIRAA2"].ToString().Trim();
                if (dr["FIRACT"] != DBNull.Value) objSF.strCity = dr["FIRACT"].ToString().Trim();
                if (dr["FIRAST"] != DBNull.Value) objSF.strState = dr["FIRAST"].ToString().Trim();
                if (dr["FIRAZP"] != DBNull.Value) objSF.strPostalCode = dr["FIRAZP"].ToString().Trim();
                if (dr["FIRAAT"] != DBNull.Value) objSF.strContactName = dr["FIRAAT"].ToString().Trim();
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
            int intBOL = 0;
            int intCarrier = 0;
            int intPlant = 0;
            string strWorkStation = objES.strRequesterID.Substring(0, 10);
            string strSQLSource = objES.objDetails.objMiscellaneous.strMisc1.Trim() + "_PS";
            Int32.TryParse(objES.objDetails.objMiscellaneous.strMisc2, out intBOL);
            Int32.TryParse(objES.objDetails.objMiscellaneous.strMisc3, out intCarrier);
            Int32.TryParse(objES.objDetails.objMiscellaneous.strMisc4, out intPlant);
            dtHeader = GetShipmentInformation(objES.objDetails.strDeliveryDocNumber);
            do
            {
                iDB2Command sqlCMD = objSQLCon.CreateCommand();
                {
                    sqlCMD.Connection = objSQLCon;
                    sqlCMD.CommandType = CommandType.Text;
                    sqlCMD.CommandText = "INSERT INTO SHLIB.FRF01050 (FIREF1, FIREF2, FIREF3, FIREF4, FIREF5, FIVOID, FICOMP, FIATTN, FIADD1, FIADD2, FIADD3, FICITY, FISTAT, FIZIPC, FICNTY, FITRK, FIWGHT, FICTOT, FICTHS, FICOST, FILIST, FISRCE, FISQLSTS, FISHPD, FISQLSTAMP, FIBOL, FICARR, FIPLNT, FIPHON, FISRVT, FISHPV, FISACT, FIADCH, FIPRDT, FIWSID, FIPSTS, FIACCT, FICREF, FIEML1, FIEMN1, FIEML2, FIEMN2, FIEML3, FIEMN3, FIRSTS, FITEXT, FISIGN, FISDLV, FIRACM, FIRAAT, FIRAA1, FIRAA2, FIRACT, FIRAST, FIRAZP, FIINST, FIBTCM, FIBTAT, FIBTA1, FIBTA2, FIBTCT, FIBTST, FIBTZP) VALUES (@FIREF1, @FIREF2, @FIREF3, @FIREF4, @FIREF5, @FIVOID, @FICOMP, @FIATTN, @FIADD1, @FIADD2, @FIADD3, @FICITY, @FISTAT, @FIZIPC, @FICNTY, @FITRK, @FIWGHT, @FICTOT, @FICTHS, @FICOST, @FILIST, @FISRCE, @FISQLSTS, @FISHPD, @FISQLSTAMP, @FIBOL, @FICARR, @FIPLNT, @FIPHON, @FISRVT, @FISHPV, @FISACT, @FIADCH, @FIPRDT, @FIWSID, @FIPSTS, @FIACCT, @FICREF, @FIEML1, @FIEMN1, @FIEML2, @FIEMN2, @FIEML3, @FIEMN3, @FIRSTS, @FITEXT, @FISIGN, @FISDLV, @FIRACM, @FIRAAT, @FIRAA1, @FIRAA2, @FIRACT, @FIRAST, @FIRAZP, @FIINST, @FIBTCM, @FIBTAT, @FIBTA1, @FIBTA2, @FIBTCT, @FIBTST, @FIBTZP)";
                    sqlCMD.Parameters.AddWithValue("@FIREF1", TestIfNull(objES.objDetails.strDeliveryDocNumber));
                    sqlCMD.Parameters.AddWithValue("@FIREF2", TestIfNull(objES.objDetails.strPONumber));
                    sqlCMD.Parameters.AddWithValue("@FIREF3", TestIfNull(objES.objDetails.strInvoiceNumber));
                    sqlCMD.Parameters.AddWithValue("@FIREF4", " ");
                    sqlCMD.Parameters.AddWithValue("@FIREF5", " ");
                    sqlCMD.Parameters.AddWithValue("@FIVOID", " ");
                    sqlCMD.Parameters.AddWithValue("@FICOMP", TestIfNull(objES.objShipTo.strCompanyName));
                    sqlCMD.Parameters.AddWithValue("@FIATTN", TestIfNull(objES.objShipTo.strContactName));
                    sqlCMD.Parameters.AddWithValue("@FIADD1", TestIfNull(objES.objShipTo.strAddressLine1));
                    sqlCMD.Parameters.AddWithValue("@FIADD2", TestIfNull(objES.objShipTo.strAddressLine2));
                    sqlCMD.Parameters.AddWithValue("@FIADD3", TestIfNull(objES.objShipTo.strAddressLine3));
                    sqlCMD.Parameters.AddWithValue("@FICITY", TestIfNull(objES.objShipTo.strCity));
                    sqlCMD.Parameters.AddWithValue("@FISTAT", TestIfNull(objES.objShipTo.strState));
                    sqlCMD.Parameters.AddWithValue("@FIZIPC", TestIfNull(objES.objShipTo.strPostalCode));
                    sqlCMD.Parameters.AddWithValue("@FICNTY", TestIfNull(objES.objShipTo.strCountryCode));
                    sqlCMD.Parameters.AddWithValue("@FITRK", TestIfNull(objES.lstContainer[x].strTrackingNumber));
                    sqlCMD.Parameters.AddWithValue("@FIWGHT", objES.lstContainer[x].dblTotalWeight);
                    sqlCMD.Parameters.AddWithValue("@FICTOT", intTotalContainers);
                    sqlCMD.Parameters.AddWithValue("@FICTHS", (x + 1));
                    sqlCMD.Parameters.AddWithValue("@FICOST", objES.lstContainer[x].objRates.dblTotalDiscountedPrice);
                    sqlCMD.Parameters.AddWithValue("@FILIST", objES.lstContainer[x].objRates.dblTotalPublishedPrice);
                    sqlCMD.Parameters.AddWithValue("@FISRCE", strSQLSource);
                    sqlCMD.Parameters.AddWithValue("@FISQLSTS", "R");
                    sqlCMD.Parameters.AddWithValue("@FISHPD", objES.dtShipDate.ToString("MM/dd/yyyy"));
                    sqlCMD.Parameters.AddWithValue("@FISQLSTAMP", DateTime.Now);
                    sqlCMD.Parameters.AddWithValue("@FIBOL", intBOL);
                    sqlCMD.Parameters.AddWithValue("@FICARR", intCarrier);
                    sqlCMD.Parameters.AddWithValue("@FIPLNT", intPlant);
                    sqlCMD.Parameters.AddWithValue("@FIPHON", TestIfNull(objES.objShipTo.strPhoneNumber));
                    sqlCMD.Parameters.AddWithValue("@FISRVT", TestIfNull(objES.objShipMethod.strShipViaCode));
                    sqlCMD.Parameters.AddWithValue("@FISHPV", TestIfNull(objES.objShipMethod.strServiceLevel));
                    sqlCMD.Parameters.AddWithValue("@FISACT", TestIfNull(objES.objShipMethod.strAccountNumber));
                    sqlCMD.Parameters.AddWithValue("@FIADCH", " ");
                    sqlCMD.Parameters.AddWithValue("@FIPRDT", " ");
                    sqlCMD.Parameters.AddWithValue("@FIWSID", TestIfNull(strWorkStation));
                    sqlCMD.Parameters.AddWithValue("@FIPSTS", " ");
                    sqlCMD.Parameters.AddWithValue("@FICREF", " ");
                    sqlCMD.Parameters.AddWithValue("@FIEML1", " ");
                    sqlCMD.Parameters.AddWithValue("@FIEMN1", " ");
                    sqlCMD.Parameters.AddWithValue("@FIEML2", " ");
                    sqlCMD.Parameters.AddWithValue("@FIEMN2", " ");
                    sqlCMD.Parameters.AddWithValue("@FIEML3", " ");
                    sqlCMD.Parameters.AddWithValue("@FIEMN3", " ");
                    sqlCMD.Parameters.AddWithValue("@FIRSTS", " ");
                    sqlCMD.Parameters.AddWithValue("@FITEXT", " ");
                    sqlCMD.Parameters.AddWithValue("@FISIGN", " ");
                    sqlCMD.Parameters.AddWithValue("@FISDLV", " ");
                    sqlCMD.Parameters.AddWithValue("@FIPMTT", PayTypeToCMS(objES.objShipMethod.strPaymentTerms));
                    if (objES.objBillTo != null)
                    {
                        sqlCMD.Parameters.AddWithValue("@FIACCT", TestIfNull(objES.objBillTo.strAccountNumber));
                        sqlCMD.Parameters.AddWithValue("@FIBTCM", TestIfNull(objES.objBillTo.strCompanyName));
                        sqlCMD.Parameters.AddWithValue("@FIBTAT", TestIfNull(objES.objBillTo.strContactName));
                        sqlCMD.Parameters.AddWithValue("@FIBTA1", TestIfNull(objES.objBillTo.strAddressLine1));
                        sqlCMD.Parameters.AddWithValue("@FIBTA2", TestIfNull(objES.objBillTo.strAddressLine2));
                        sqlCMD.Parameters.AddWithValue("@FIBTCT", TestIfNull(objES.objBillTo.strCity));
                        sqlCMD.Parameters.AddWithValue("@FIBTST", TestIfNull(objES.objBillTo.strState));
                        sqlCMD.Parameters.AddWithValue("@FIBTZP", TestIfNull(objES.objBillTo.strPostalCode));
                    }
                    else
                    {
                        sqlCMD.Parameters.AddWithValue("@FIACCT", " ");
                        sqlCMD.Parameters.AddWithValue("@FIBTCM", " ");
                        sqlCMD.Parameters.AddWithValue("@FIBTAT", " ");
                        sqlCMD.Parameters.AddWithValue("@FIBTA1", " ");
                        sqlCMD.Parameters.AddWithValue("@FIBTA2", " ");
                        sqlCMD.Parameters.AddWithValue("@FIBTCT", " ");
                        sqlCMD.Parameters.AddWithValue("@FIBTST", " ");
                        sqlCMD.Parameters.AddWithValue("@FIBTZP", " ");
                    }
                    sqlCMD.Parameters.AddWithValue("@FIRACM", TestIfNull(objES.objShipFrom.strCompanyName));
                    sqlCMD.Parameters.AddWithValue("@FIRAAT", TestIfNull(objES.objShipFrom.strContactName));
                    sqlCMD.Parameters.AddWithValue("@FIRAA1", TestIfNull(objES.objShipFrom.strAddressLine1));
                    sqlCMD.Parameters.AddWithValue("@FIRAA2", TestIfNull(objES.objShipFrom.strAddressLine2));
                    sqlCMD.Parameters.AddWithValue("@FIRACT", TestIfNull(objES.objShipFrom.strCity));
                    sqlCMD.Parameters.AddWithValue("@FIRAST", TestIfNull(objES.objShipFrom.strState));
                    sqlCMD.Parameters.AddWithValue("@FIRAZP", TestIfNull(objES.objShipFrom.strPostalCode));
                    sqlCMD.Parameters.AddWithValue("@FIINST", " ");


                    string strQuery = sqlCMD.CommandText;
                    foreach (iDB2Parameter p in sqlCMD.Parameters)
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
                    catch (iDB2Exception ex)
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
            DataTable dtHeader;
            bool UpdateStatus;
            int x = 0;
            int intTotalContainers = objES.lstContainer.Count();
            int intBOL = 0;
            int intCarrier = 0;
            int intPlant = 0;
            string strWorkStation = objES.strRequesterID.Substring(0, 10);
            string strSQLSource = objES.objDetails.objMiscellaneous.strMisc1.Trim() + "_PS";
            Int32.TryParse(objES.objDetails.objMiscellaneous.strMisc2, out intBOL);
            Int32.TryParse(objES.objDetails.objMiscellaneous.strMisc3, out intCarrier);
            Int32.TryParse(objES.objDetails.objMiscellaneous.strMisc4, out intPlant);
            dtHeader = GetShipmentInformation(objES.objDetails.strDeliveryDocNumber);
            do
            {
                iDB2Command sqlCMD = objSQLCon.CreateCommand();
                {
                    sqlCMD.Connection = objSQLCon;
                    sqlCMD.CommandType = CommandType.Text;
                    sqlCMD.CommandText = "INSERT INTO SHLIB.FRF01050 (FIREF1, FIREF2, FIREF3, FIREF4, FIREF5, FIVOID, FICOMP, FIATTN, FIADD1, FIADD2, FIADD3, FICITY, FISTAT, FIZIPC, FICNTY, FITRK, FIWGHT, FICTOT, FICTHS, FICOST, FILIST, FISRCE, FISQLSTS, FISHPD, FISQLSTAMP, FIBOL, FICARR, FIPLNT, FIPHON, FISRVT, FISHPV, FISACT, FIADCH, FIPRDT, FIWSID, FIPSTS, FIACCT, FICREF, FIEML1, FIEMN1, FIEML2, FIEMN2, FIEML3, FIEMN3, FIRSTS, FITEXT, FISIGN, FISDLV, FIRACM, FIRAAT, FIRAA1, FIRAA2, FIRACT, FIRAST, FIRAZP, FIINST, FIBTCM, FIBTAT, FIBTA1, FIBTA2, FIBTCT, FIBTST, FIBTZP) VALUES (@FIREF1, @FIREF2, @FIREF3, @FIREF4, @FIREF5, @FIVOID, @FICOMP, @FIATTN, @FIADD1, @FIADD2, @FIADD3, @FICITY, @FISTAT, @FIZIPC, @FICNTY, @FITRK, @FIWGHT, @FICTOT, @FICTHS, @FICOST, @FILIST, @FISRCE, @FISQLSTS, @FISHPD, @FISQLSTAMP, @FIBOL, @FICARR, @FIPLNT, @FIPHON, @FISRVT, @FISHPV, @FISACT, @FIADCH, @FIPRDT, @FIWSID, @FIPSTS, @FIACCT, @FICREF, @FIEML1, @FIEMN1, @FIEML2, @FIEMN2, @FIEML3, @FIEMN3, @FIRSTS, @FITEXT, @FISIGN, @FISDLV, @FIRACM, @FIRAAT, @FIRAA1, @FIRAA2, @FIRACT, @FIRAST, @FIRAZP, @FIINST, @FIBTCM, @FIBTAT, @FIBTA1, @FIBTA2, @FIBTCT, @FIBTST, @FIBTZP)";
                    sqlCMD.Parameters.AddWithValue("@FIREF1", TestIfNull(objES.objDetails.strDeliveryDocNumber));
                    sqlCMD.Parameters.AddWithValue("@FIREF2", TestIfNull(objES.objDetails.strPONumber));
                    sqlCMD.Parameters.AddWithValue("@FIREF3", TestIfNull(objES.objDetails.strInvoiceNumber));
                    sqlCMD.Parameters.AddWithValue("@FIREF4", " ");
                    sqlCMD.Parameters.AddWithValue("@FIREF5", " ");
                    sqlCMD.Parameters.AddWithValue("@FIVOID", "Y");
                    sqlCMD.Parameters.AddWithValue("@FICOMP", TestIfNull(objES.objShipTo.strCompanyName));
                    sqlCMD.Parameters.AddWithValue("@FIATTN", TestIfNull(objES.objShipTo.strContactName));
                    sqlCMD.Parameters.AddWithValue("@FIADD1", TestIfNull(objES.objShipTo.strAddressLine1));
                    sqlCMD.Parameters.AddWithValue("@FIADD2", TestIfNull(objES.objShipTo.strAddressLine2));
                    sqlCMD.Parameters.AddWithValue("@FIADD3", TestIfNull(objES.objShipTo.strAddressLine3));
                    sqlCMD.Parameters.AddWithValue("@FICITY", TestIfNull(objES.objShipTo.strCity));
                    sqlCMD.Parameters.AddWithValue("@FISTAT", TestIfNull(objES.objShipTo.strState));
                    sqlCMD.Parameters.AddWithValue("@FIZIPC", TestIfNull(objES.objShipTo.strPostalCode));
                    sqlCMD.Parameters.AddWithValue("@FICNTY", TestIfNull(objES.objShipTo.strCountryCode));
                    sqlCMD.Parameters.AddWithValue("@FITRK", TestIfNull(objES.lstContainer[x].strTrackingNumber));
                    sqlCMD.Parameters.AddWithValue("@FIWGHT", objES.lstContainer[x].dblTotalWeight);
                    sqlCMD.Parameters.AddWithValue("@FICTOT", intTotalContainers);
                    sqlCMD.Parameters.AddWithValue("@FICTHS", (x + 1));
                    sqlCMD.Parameters.AddWithValue("@FICOST", objES.lstContainer[x].objRates.dblTotalDiscountedPrice);
                    sqlCMD.Parameters.AddWithValue("@FILIST", objES.lstContainer[x].objRates.dblTotalPublishedPrice);
                    sqlCMD.Parameters.AddWithValue("@FISRCE", strSQLSource);
                    sqlCMD.Parameters.AddWithValue("@FISQLSTS", "R");
                    sqlCMD.Parameters.AddWithValue("@FISHPD", objES.dtShipDate.ToString("MM/dd/yyyy"));
                    sqlCMD.Parameters.AddWithValue("@FISQLSTAMP", DateTime.Now);
                    sqlCMD.Parameters.AddWithValue("@FIBOL", intBOL);
                    sqlCMD.Parameters.AddWithValue("@FICARR", intCarrier);
                    sqlCMD.Parameters.AddWithValue("@FIPLNT", intPlant);
                    sqlCMD.Parameters.AddWithValue("@FIPHON", TestIfNull(objES.objShipTo.strPhoneNumber));
                    sqlCMD.Parameters.AddWithValue("@FISRVT", TestIfNull(objES.objShipMethod.strShipViaCode));
                    sqlCMD.Parameters.AddWithValue("@FISHPV", TestIfNull(objES.objShipMethod.strServiceLevel));
                    sqlCMD.Parameters.AddWithValue("@FISACT", TestIfNull(objES.objShipMethod.strAccountNumber));
                    sqlCMD.Parameters.AddWithValue("@FIADCH", " ");
                    sqlCMD.Parameters.AddWithValue("@FIPRDT", " ");
                    sqlCMD.Parameters.AddWithValue("@FIWSID", TestIfNull(strWorkStation));
                    sqlCMD.Parameters.AddWithValue("@FIPSTS", " ");
                    sqlCMD.Parameters.AddWithValue("@FICREF", " ");
                    sqlCMD.Parameters.AddWithValue("@FIEML1", " ");
                    sqlCMD.Parameters.AddWithValue("@FIEMN1", " ");
                    sqlCMD.Parameters.AddWithValue("@FIEML2", " ");
                    sqlCMD.Parameters.AddWithValue("@FIEMN2", " ");
                    sqlCMD.Parameters.AddWithValue("@FIEML3", " ");
                    sqlCMD.Parameters.AddWithValue("@FIEMN3", " ");
                    sqlCMD.Parameters.AddWithValue("@FIRSTS", " ");
                    sqlCMD.Parameters.AddWithValue("@FITEXT", " ");
                    sqlCMD.Parameters.AddWithValue("@FISIGN", " ");
                    sqlCMD.Parameters.AddWithValue("@FISDLV", " ");
                    sqlCMD.Parameters.AddWithValue("@FIPMTT", PayTypeToCMS(objES.objShipMethod.strPaymentTerms));
                    if (objES.objBillTo != null)
                    {
                        sqlCMD.Parameters.AddWithValue("@FIACCT", TestIfNull(objES.objBillTo.strAccountNumber));
                        sqlCMD.Parameters.AddWithValue("@FIBTCM", TestIfNull(objES.objBillTo.strCompanyName));
                        sqlCMD.Parameters.AddWithValue("@FIBTAT", TestIfNull(objES.objBillTo.strContactName));
                        sqlCMD.Parameters.AddWithValue("@FIBTA1", TestIfNull(objES.objBillTo.strAddressLine1));
                        sqlCMD.Parameters.AddWithValue("@FIBTA2", TestIfNull(objES.objBillTo.strAddressLine2));
                        sqlCMD.Parameters.AddWithValue("@FIBTCT", TestIfNull(objES.objBillTo.strCity));
                        sqlCMD.Parameters.AddWithValue("@FIBTST", TestIfNull(objES.objBillTo.strState));
                        sqlCMD.Parameters.AddWithValue("@FIBTZP", TestIfNull(objES.objBillTo.strPostalCode));
                    }
                    else
                    {
                        sqlCMD.Parameters.AddWithValue("@FIACCT", " ");
                        sqlCMD.Parameters.AddWithValue("@FIBTCM", " ");
                        sqlCMD.Parameters.AddWithValue("@FIBTAT", " ");
                        sqlCMD.Parameters.AddWithValue("@FIBTA1", " ");
                        sqlCMD.Parameters.AddWithValue("@FIBTA2", " ");
                        sqlCMD.Parameters.AddWithValue("@FIBTCT", " ");
                        sqlCMD.Parameters.AddWithValue("@FIBTST", " ");
                        sqlCMD.Parameters.AddWithValue("@FIBTZP", " ");
                    }
                    sqlCMD.Parameters.AddWithValue("@FIRACM", TestIfNull(objES.objShipFrom.strCompanyName));
                    sqlCMD.Parameters.AddWithValue("@FIRAAT", TestIfNull(objES.objShipFrom.strContactName));
                    sqlCMD.Parameters.AddWithValue("@FIRAA1", TestIfNull(objES.objShipFrom.strAddressLine1));
                    sqlCMD.Parameters.AddWithValue("@FIRAA2", TestIfNull(objES.objShipFrom.strAddressLine2));
                    sqlCMD.Parameters.AddWithValue("@FIRACT", TestIfNull(objES.objShipFrom.strCity));
                    sqlCMD.Parameters.AddWithValue("@FIRAST", TestIfNull(objES.objShipFrom.strState));
                    sqlCMD.Parameters.AddWithValue("@FIRAZP", TestIfNull(objES.objShipFrom.strPostalCode));
                    sqlCMD.Parameters.AddWithValue("@FIINST", " ");


                    string strQuery = sqlCMD.CommandText;
                    foreach (iDB2Parameter p in sqlCMD.Parameters)
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
                    catch (iDB2Exception ex)
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


        private bool ContinueCheck()
        {
            var objError = (from p in lstResponse where p.StatusType == ResponseStatusType.CRITICAL || p.StatusType == ResponseStatusType.ERROR select p);

            if (objError == null || objError.Count() == 0)
                return true;
            else
                return false;
        }

        private EntityResponseStatus SetResponse(string strMessage, string strError, ResponseStatusType eResponseType)
        {
            EntityResponseStatus objResponse = new EntityResponseStatus();
            objResponse.Message = strMessage;
            objResponse.StatusType = eResponseType;
            objResponse.Error = strError;
            return objResponse;
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
