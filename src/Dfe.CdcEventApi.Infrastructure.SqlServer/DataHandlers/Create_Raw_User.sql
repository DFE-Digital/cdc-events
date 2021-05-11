INSERT INTO [dbo].[Raw_User] 
    ([Email] ,[FirstName] ,[LastName] ,[OrganisationName] ,[PhoneNumber] ,[AcceptedTerms] ,[IsSurveyor] ,[AnalyticsRole] ,[sk_Id] ,[bk_Id] ,[bk_TenantId] ,[target_Date] ,[source_DateCreated] ,[source_DateLastModified] ,[EntityStatus] ,[Load_DateTime] ) 
SELECT
	Tbl.Col.value('Email[1]', 'nvarchar(max)') as [Email],
    Tbl.Col.value('FirstName[1]', 'nvarchar(max)') as [FirstName],
    Tbl.Col.value('LastName[1]', 'nvarchar(max)') as [LastName],
    Tbl.Col.value('OrganisationName[1]', 'nvarchar(max)') as [OrganisationName],
    Tbl.Col.value('PhoneNumber[1]', 'nvarchar(max)') as [PhoneNumber],
    Tbl.Col.value('AcceptedTerms[1]', 'nvarchar(40)') as [AcceptedTerms],
    Tbl.Col.value('IsSurveyor[1]', 'nvarchar(40)') as [IsSurveyor],
    Tbl.Col.value('AnalyticsRole[1]', 'nvarchar(40)') as [AnalyticsRole],
    Tbl.Col.value('sk_Id[1]', 'nvarchar(40)') as [sk_Id],
    Tbl.Col.value('bk_Id[1]', 'nvarchar(40)') as [bk_Id],
    Tbl.Col.value('bk_TenantId[1]', 'nvarchar(40)') as [bk_TenantId],
    Tbl.Col.value('target_Date[1]', 'nvarchar(40)') as [target_Date],
    Tbl.Col.value('source_DateCreated[1]', 'nvarchar(40)') as [source_DateCreated],
    Tbl.Col.value('source_DateLastModified[1]', 'nvarchar(40)') as [source_DateLastModified],
    Tbl.Col.value('EntityStatus[1]', 'nvarchar(max)') as [EntityStatus],
    @RunIdentifier as [Load_DateTime] 
FROM @Entities.nodes('//Entity') Tbl(Col);