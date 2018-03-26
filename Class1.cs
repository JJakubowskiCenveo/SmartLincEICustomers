using System;

public class Class1
{
    public Class1()
    {
        SqlCommand sqlCMD = objSQLCon.CreateCommand();
        {
            sqlCMD.Connection = objSQLCon;
            sqlCMD.CommandType = CommandType.Text;
            sqlCMD.CommandText = "INSERT INTO dbo.Shipments (Reference1, Reference2, Reference3, Reference4, Reference5, VoidIndicator, CompanyName, AttentionTo, Address1, Address2, Address3, City, ShipState, ZipCode, Country, ShippedBy, TrackingNumber, ShipWeight, TotalContainers, ThisContainer, CarrierCharge, ListCharge, SourceSystem, SQLStatus, ShipDate, LastModified, BOL, CarrierNumber, PlantNumber, PhoneNumber, ServiceType, SenderAccount, AdditionalCharge, ProcessDate, TerminalID, ProcessStatus, ThirdPartyAccount, CustomerReference, SenderEmailAddress1, SenderEmailNotify1, SenderEmailAddress2, SenderEmailNotify2, SenderEmailAddress3, SenderEmailNotify3, SenderEmailAddress4, SenderEmailNotify4, ResidentialStatus, EmailText, SignatureOption, SaturdayDelivery, PaymentType, BillToCompany, BillToAttention, BillToAddress1, BillToAddress2, BillToCity, BillToState, BillToZip, ReturnAddressCompany, ReturnAddressAttention, ReturnAddressLine1, ReturnAddressLine2, ReturnAddressCity, ReturnAddressState, ReturnAddressZip, Instructions) VALUES (@Reference1, @Reference2, @Reference3, @Reference4, @Reference5, @VoidIndicator, @CompanyName, @AttentionTo, @Address1, @Address2, @Address3, @City, @ShipState, @ZipCode, @Country, @ShippedBy, @TrackingNumber, @ShipWeight, @TotalContainers, @ThisContainer, @CarrierCharge, @ListCharge, @SourceSystem, @SQLStatus, @ShipDate, @LastModified, @BOL, @CarrierNumber, @PlantNumber, @PhoneNumber, @ServiceType, @SenderAccount, @AdditionalCharge, @ProcessDate, @TerminalID, @ProcessStatus, @ThirdPartyAccount, @CustomerReference, @SenderEmailAddress1, @SenderEmailNotify1, @SenderEmailAddress2, @SenderEmailNotify2, @SenderEmailAddress3, @SenderEmailNotify3, @SenderEmailAddress4, @SenderEmailNotify4, @ResidentialStatus, @EmailText, @SignatureOption, @SaturdayDelivery, @PaymentType, @BillToCompany, @BillToAttention, @BillToAddress1, @BillToAddress2, @BillToCity, @BillToState, @BillToZip, @ReturnAddressCompany, @ReturnAddressAttention, @ReturnAddressLine1, @ReturnAddressLine2, @ReturnAddressCity, @ReturnAddressState, @ReturnAddressZip, @Instructions)";
            sqlCMD.Parameters.AddWithValue("@Reference1", objES.objDetails.strDeliveryDocNumber);
            sqlCMD.Parameters.AddWithValue("@Reference2", objES.objDetails.strPONumber);
            sqlCMD.Parameters.AddWithValue("@Reference3", objES.objDetails.strInvoiceNumber);
            sqlCMD.Parameters.AddWithValue("@Reference4", objES.lstMiscellaneous[x].strMisc4);
            sqlCMD.Parameters.AddWithValue("@Reference5", objES.lstMiscellaneous[x].strMisc5);
            sqlCMD.Parameters.AddWithValue("@VoidIndicator", "Y");
            sqlCMD.Parameters.AddWithValue("@CompanyName", objES.objShipTo.strCompanyName);
            sqlCMD.Parameters.AddWithValue("@AttentionTo", objES.objShipTo.strContactName);
            sqlCMD.Parameters.AddWithValue("@Address1", objES.objShipTo.strAddressLine1);
            sqlCMD.Parameters.AddWithValue("@Address2", objES.objShipTo.strAddressLine2);
            sqlCMD.Parameters.AddWithValue("@Address3", objES.objShipTo.strAddressLine3);
            sqlCMD.Parameters.AddWithValue("@City", objES.objShipTo.strCity);
            sqlCMD.Parameters.AddWithValue("@ShipState", objES.objShipTo.strState);
            sqlCMD.Parameters.AddWithValue("@ZipCode", objES.objShipTo.strPostalCode);
            sqlCMD.Parameters.AddWithValue("@Country", objES.objShipTo.strCountryCode);
            sqlCMD.Parameters.AddWithValue("@ShippedBy", objES.strRequesterID);
            sqlCMD.Parameters.AddWithValue("@TrackingNumber", objES.lstContainer[x].strTrackingNumber);
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
            sqlCMD.Parameters.AddWithValue("@PhoneNumber", objES.objShipTo.strPhoneNumber);
            sqlCMD.Parameters.AddWithValue("@ServiceType", objES.objShipMethod.strShipViaCode);
            sqlCMD.Parameters.AddWithValue("@SenderAccount", objES.objShipMethod.strAccountNumber);
            sqlCMD.Parameters.AddWithValue("@AdditionalCharge", " ");
            sqlCMD.Parameters.AddWithValue("@ProcessDate", " ");
            sqlCMD.Parameters.AddWithValue("@TerminalID", " ");
            sqlCMD.Parameters.AddWithValue("@ProcessStatus", " ");
            sqlCMD.Parameters.AddWithValue("@ThirdPartyAccount", " ");
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
            sqlCMD.Parameters.AddWithValue("@PaymentType", PayTypeToCMS(objES.objShipMethod.strPaymentTerms));
            sqlCMD.Parameters.AddWithValue("@BillToCompany", objES.objBillTo.strCompanyName);
            sqlCMD.Parameters.AddWithValue("@BillToAttention", objES.objBillTo.strContactName);
            sqlCMD.Parameters.AddWithValue("@BillToAddress1", objES.objBillTo.strAddressLine1);
            sqlCMD.Parameters.AddWithValue("@BillToAddress2", objES.objBillTo.strAddressLine2);
            sqlCMD.Parameters.AddWithValue("@BillToCity", objES.objBillTo.strCity);
            sqlCMD.Parameters.AddWithValue("@BillToState", objES.objBillTo.strState);
            sqlCMD.Parameters.AddWithValue("@BillToZip", objES.objBillTo.strPostalCode);
            sqlCMD.Parameters.AddWithValue("@ReturnAddressCompany", objES.objShipFrom.strCompanyName);
            sqlCMD.Parameters.AddWithValue("@ReturnAddressAttention", objES.objShipFrom.strContactName);
            sqlCMD.Parameters.AddWithValue("@ReturnAddressLine1", objES.objShipFrom.strAddressLine1);
            sqlCMD.Parameters.AddWithValue("@ReturnAddressLine2", objES.objShipFrom.strAddressLine2);
            sqlCMD.Parameters.AddWithValue("@ReturnAddressCity", objES.objShipFrom.strCity);
            sqlCMD.Parameters.AddWithValue("@ReturnAddressState", objES.objShipFrom.strState);
            sqlCMD.Parameters.AddWithValue("@ReturnAddressZip", objES.objShipFrom.strPostalCode);
            sqlCMD.Parameters.AddWithValue("@Instructions", " ");

            try
            {
                if (objSQLCon.State == ConnectionState.Closed)
                    objSQLCon.Open();
                int recordsAffected = sqlCMD.ExecuteNonQuery();
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
    }
}

