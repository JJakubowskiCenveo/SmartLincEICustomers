USE [PSIntegration]
GO

INSERT INTO [dbo].[Shipments]
           ([Reference1]
           ,[Reference2]
           ,[Reference3]
           ,[Reference4]
           ,[Reference5]
           ,[BOL]
           ,[CarrierNumber]
           ,[PlantNumber]
           ,[PhoneNumber]
           ,[TrackingNumber]
           ,[ServiceType]
           ,[VoidIndicator]
           ,[CompanyName]
           ,[AttentionTo]
           ,[Address1]
           ,[Address2]
           ,[Address3]
           ,[City]
           ,[ShipState]
           ,[ZipCode]
           ,[Country]
           ,[ShipWeight]
           ,[CarrierCharge]
           ,[ListCharge]
           ,[AdditionalCharge]
           ,[ProcessDate]
           ,[TerminalID]
           ,[ProcessStatus]
           ,[ThirdPartyAccount]
           ,[CustomerReference]
           ,[SenderEmailAddress1]
           ,[SenderEmailNotify1]
           ,[SenderEmailAddress2]
           ,[SenderEmailNotify2]
           ,[SenderEmailAddress3]
           ,[SenderEmailNotify3]
           ,[SenderEmailAddress4]
           ,[SenderEmailNotify4]
           ,[ResidentialStatus]
           ,[EmailText]
           ,[SignatureOption]
           ,[SaturdayDelivery]
           ,[PaymentType]
           ,[BillToCompany]
           ,[BillToAttention]
           ,[BillToAddress1]
           ,[BillToAddress2]
           ,[BillToCity]
           ,[BillToState]
           ,[BillToZip]
           ,[ReturnAddressCompany]
           ,[ReturnAddressAttention]
           ,[ReturnAddressLine1]
           ,[ReturnAddressLine2]
           ,[ReturnAddressCity]
           ,[ReturnAddressState]
           ,[ReturnAddressZip]
           ,[ShipDate]
           ,[SourceSystem]
           ,[SQLStatus])
     VALUES
           (<Reference1, varchar(35),>
           ,<Reference2, varchar(35),>
           ,<Reference3, varchar(35),>
           ,<Reference4, varchar(35),>
           ,<Reference5, varchar(35),>
           ,<BOL, decimal(10,0),>
           ,<CarrierNumber, decimal(10,0),>
           ,<PlantNumber, decimal(10,0),>
           ,<PhoneNumber, varchar(15),>
           ,<TrackingNumber, varchar(30),>
           ,<ServiceType, varchar(21),>
           ,<VoidIndicator, char(1),>
           ,<CompanyName, varchar(35),>
           ,<AttentionTo, varchar(35),>
           ,<Address1, varchar(35),>
           ,<Address2, varchar(35),>
           ,<Address3, varchar(35),>
           ,<City, varchar(30),>
           ,<ShipState, varchar(5),>
           ,<ZipCode, varchar(12),>
           ,<Country, varchar(20),>
           ,<ShipWeight, char(7),>
           ,<CarrierCharge, char(11),>
           ,<ListCharge, char(11),>
           ,<AdditionalCharge, char(9),>
           ,<ProcessDate, char(14),>
           ,<TerminalID, varchar(10),>
           ,<ProcessStatus, char(1),>
           ,<ThirdPartyAccount, varchar(10),>
           ,<CustomerReference, varchar(30),>
           ,<SenderEmailAddress1, varchar(50),>
           ,<SenderEmailNotify1, char(4),>
           ,<SenderEmailAddress2, varchar(50),>
           ,<SenderEmailNotify2, char(4),>
           ,<SenderEmailAddress3, varchar(50),>
           ,<SenderEmailNotify3, char(4),>
           ,<SenderEmailAddress4, varchar(50),>
           ,<SenderEmailNotify4, char(4),>
           ,<ResidentialStatus, char(3),>
           ,<EmailText, varchar(120),>
           ,<SignatureOption, varchar(8),>
           ,<SaturdayDelivery, char(1),>
           ,<PaymentType, char(3),>
           ,<BillToCompany, varchar(35),>
           ,<BillToAttention, varchar(35),>
           ,<BillToAddress1, varchar(35),>
           ,<BillToAddress2, varchar(35),>
           ,<BillToCity, varchar(30),>
           ,<BillToState, char(2),>
           ,<BillToZip, varchar(9),>
           ,<ReturnAddressCompany, varchar(35),>
           ,<ReturnAddressAttention, varchar(35),>
           ,<ReturnAddressLine1, varchar(35),>
           ,<ReturnAddressLine2, varchar(35),>
           ,<ReturnAddressCity, varchar(30),>
           ,<ReturnAddressState, char(2),>
           ,<ReturnAddressZip, varchar(10),>
           ,<ShipDate, char(10),>
           ,<SourceSystem, varchar(10),>
           ,<SQLStatus, char(2),>)
GO

