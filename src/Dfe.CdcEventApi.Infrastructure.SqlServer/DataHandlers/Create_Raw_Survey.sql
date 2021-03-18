
INSERT INTO [Raw_Survey]
    ([Name], [bk_SurveyLotId], [SurveyLotName], [bk_SiteId], [SiteName], [IsComplete], [bk_SurveyorId], [SurveyorEmail], [StartDate], [AccessTime], [EndDate], [IsApproved], [bk_ApprovedById], [ApprovedByEmail], [Latitude], [Longitude], [SurveyVersion], [ref_MandateName], [ref_MandateBlobKey], [bk_MandateId], [sk_Id], [bk_Id], [bk_TenantId], [target_Date], [source_DateCreated], [source_DateLastModified], [EntityStatus], [Load_DateTime])
SELECT
    Tbl.Col.value('Name[1]', 'nvarchar(max)') AS [Name],
    Tbl.Col.value('bk_SurveyLotId[1]', 'nvarchar(40)') AS [bk_SurveyLotId],
    Tbl.Col.value('SurveyLotName[1]', 'nvarchar(max)') AS [SurveyLotName],
    Tbl.Col.value('bk_SiteId[1]', 'nvarchar(40)') AS [bk_SiteId],
    Tbl.Col.value('SiteName[1]', 'nvarchar(max)') AS [SiteName],
    Tbl.Col.value('IsComplete[1]', 'nvarchar(40)') AS [IsComplete],
    Tbl.Col.value('bk_SurveyorId[1]', 'nvarchar(40)') AS [bk_SurveyorId],
    Tbl.Col.value('SurveyorEmail[1]', 'nvarchar(max)') AS [SurveyorEmail],
    Tbl.Col.value('StartDate[1]', 'nvarchar(40)') AS [StartDate],
    Tbl.Col.value('AccessTime[1]', 'nvarchar(40)') AS [AccessTime],
    Tbl.Col.value('EndDate[1]', 'nvarchar(40)') AS [EndDate],
    Tbl.Col.value('IsApproved[1]', 'nvarchar(40)') AS [IsApproved],
    Tbl.Col.value('bk_ApprovedById[1]', 'nvarchar(40)') AS [bk_ApprovedById],
    Tbl.Col.value('ApprovedByEmail[1]', 'nvarchar(max)') AS [ApprovedByEmail],
    Tbl.Col.value('Latitude[1]', 'nvarchar(40)') AS [Latitude],
    Tbl.Col.value('Longitude[1]', 'nvarchar(40)') AS [Longitude],
    Tbl.Col.value('SurveyVersion[1]', 'nvarchar(40)') AS [SurveyVersion],
    Tbl.Col.value('ref_MandateName[1]', 'nvarchar(max)') AS [ref_MandateName],
    Tbl.Col.value('ref_MandateBlobKey[1]', 'nvarchar(40)') AS [ref_MandateBlobKey],
    Tbl.Col.value('bk_MandateId[1]', 'nvarchar(40)') AS [bk_MandateId],
    Tbl.Col.value('sk_Id[1]', 'nvarchar(40)') AS [sk_Id],
    Tbl.Col.value('bk_Id[1]', 'nvarchar(40)') AS [bk_Id],
    Tbl.Col.value('bk_TenantId[1]', 'nvarchar(40)') AS [bk_TenantId],
    Tbl.Col.value('target_Date[1]', 'nvarchar(40)') AS [target_Date],
    Tbl.Col.value('source_DateCreated[1]', 'nvarchar(40)') AS [source_DateCreated],
    Tbl.Col.value('source_DateLastModified[1]', 'nvarchar(40)') AS [source_DateLastModified],
    Tbl.Col.value('EntityStatus[1]', 'nvarchar(max)') AS [EntityStatus],
    sysdatetime() as [Load_DateTime] 
FROM @Entities.nodes('//Entity') Tbl(Col);
