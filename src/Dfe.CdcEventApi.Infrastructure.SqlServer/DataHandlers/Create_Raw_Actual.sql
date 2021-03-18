
INSERT INTO [Raw_Actual]
    ([Name], [bk_SiteId], [bk_PlanId], [Description], [IsApproved], [ApprovedBybk_Id], [DateApproved], [sk_Id], [bk_Id], [bk_TenantId], [target_Date], [source_DateCreated], [source_DateLastModified], [EntityStatus], [Load_DateTime])
SELECT
    Tbl.Col.value('Name[1]', 'nvarchar(max)') AS [Name],
    Tbl.Col.value('bk_SiteId[1]', 'nvarchar(40)') AS [bk_SiteId],
    Tbl.Col.value('bk_PlanId[1]', 'nvarchar(40)') AS [bk_PlanId],
    Tbl.Col.value('Description[1]', 'nvarchar(max)') AS [Description],
    Tbl.Col.value('IsApproved[1]', 'nvarchar(40)') AS [IsApproved],
    Tbl.Col.value('ApprovedBybk_Id[1]', 'nvarchar(40)') AS [ApprovedBybk_Id],
    Tbl.Col.value('DateApproved[1]', 'nvarchar(40)') AS [DateApproved],
    Tbl.Col.value('sk_Id[1]', 'nvarchar(40)') AS [sk_Id],
    Tbl.Col.value('bk_Id[1]', 'nvarchar(40)') AS [bk_Id],
    Tbl.Col.value('bk_TenantId[1]', 'nvarchar(40)') AS [bk_TenantId],
    Tbl.Col.value('target_Date[1]', 'nvarchar(40)') AS [target_Date],
    Tbl.Col.value('source_DateCreated[1]', 'nvarchar(40)') AS [source_DateCreated],
    Tbl.Col.value('source_DateLastModified[1]', 'nvarchar(40)') AS [source_DateLastModified],
    Tbl.Col.value('EntityStatus[1]', 'nvarchar(max)') AS [EntityStatus],
    sysdatetime() as [Load_DateTime] 
FROM @Entities.nodes('//Entity') Tbl(Col);
