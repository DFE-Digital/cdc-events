INSERT INTO [Raw_SurveyLot_SiteInfo]
    ([bk_SurveyLotId], [bk_SiteId], [sk_Id], [bk_Id], [bk_TenantId], [target_Date], [source_DateCreated], [source_DateLastModified], [EntityStatus], [Load_DateTime])
SELECT
    Tbl.Col.value('bk_SurveyLotId[1]', 'nvarchar(max)') AS [bk_SurveyLotId],
    Tbl.Col.value('bk_SiteId[1]', 'nvarchar(max)') AS [bk_SiteId],
    Tbl.Col.value('sk_Id[1]', 'nvarchar(max)') AS [sk_Id],
    Tbl.Col.value('bk_Id[1]', 'nvarchar(max)') AS [bk_Id],
    Tbl.Col.value('bk_TenantId[1]', 'nvarchar(max)') AS [bk_TenantId],
    Tbl.Col.value('target_Date[1]', 'nvarchar(max)') AS [target_Date],
    Tbl.Col.value('source_DateCreated[1]', 'nvarchar(max)') AS [source_DateCreated],
    Tbl.Col.value('source_DateLastModified[1]', 'nvarchar(max)') AS [source_DateLastModified],
    Tbl.Col.value('EntityStatus[1]', 'nvarchar(max)') AS [EntityStatus],
    @RunIdentifier as [Load_DateTime] 
FROM @Entities.nodes('//Entity') Tbl(Col);