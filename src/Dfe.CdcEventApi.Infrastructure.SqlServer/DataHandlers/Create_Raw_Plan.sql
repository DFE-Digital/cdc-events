INSERT INTO [Raw_Plan]
    ([Name], [Description], [LifecycleActivityPlanType], [bk_SiteId], [OperationalStartDate], [IsApproved], [bk_ApprovedById], [DateApproved], [target_LifecycleDate], [bk_SurveySiteId], [sk_Id], [bk_Id], [bk_TenantId], [target_Date], [source_DateCreated], [source_DateLastModified], [EntityStatus], [Load_DateTime])
SELECT
    Tbl.Col.value('Name[1]', 'nvarchar(40)') AS [Name],
    Tbl.Col.value('Description[1]', 'nvarchar(max)') AS [Description],
    Tbl.Col.value('LifecycleActivityPlanType[1]', 'nvarchar(max)') AS [LifecycleActivityPlanType],
    Tbl.Col.value('bk_SiteId[1]', 'nvarchar(40)') AS [bk_SiteId],
    Tbl.Col.value('OperationalStartDate[1]', 'nvarchar(40)') AS [OperationalStartDate],
    Tbl.Col.value('IsApproved[1]', 'nvarchar(40)') AS [IsApproved],
    Tbl.Col.value('bk_ApprovedById[1]', 'nvarchar(40)') AS [bk_ApprovedById],
    Tbl.Col.value('DateApproved[1]', 'nvarchar(40)') AS [DateApproved],
    Tbl.Col.value('target_LifecycleDate[1]', 'nvarchar(40)') AS [target_LifecycleDate],
    Tbl.Col.value('bk_SurveySiteId[1]', 'nvarchar(40)') AS [bk_SurveySiteId],
    Tbl.Col.value('sk_Id[1]', 'nvarchar(40)') AS [sk_Id],
    Tbl.Col.value('bk_Id[1]', 'nvarchar(40)') AS [bk_Id],
    Tbl.Col.value('bk_TenantId[1]', 'nvarchar(40)') AS [bk_TenantId],
    Tbl.Col.value('target_Date[1]', 'nvarchar(40)') AS [target_Date],
    Tbl.Col.value('source_DateCreated[1]', 'nvarchar(40)') AS [source_DateCreated],
    Tbl.Col.value('source_DateLastModified[1]', 'nvarchar(40)') AS [source_DateLastModified],
    Tbl.Col.value('EntityStatus[1]', 'nvarchar(max)') AS [EntityStatus],
    @RunIdentifier as [Load_DateTime] 
FROM @Entities.nodes('//Entity') Tbl(Col);