
INSERT INTO [Raw_Oncost]
    ([bk_PlanId], [Name], [Description], [Percentage], [CustomImpact1Value], [CustomImpact2Value], [CustomImpact3Value], [CustomImpact4Value], [CustomImpact5Value], [sk_Id], [bk_Id], [bk_TenantId], [target_Date], [source_DateCreated], [source_DateLastModified], [EntityStatus], [Load_DateTime])
SELECT
    Tbl.Col.value('bk_PlanId[1]', 'nvarchar(40)') AS [bk_PlanId],
    Tbl.Col.value('Name[1]', 'nvarchar(max)') AS [Name],
    Tbl.Col.value('Description[1]', 'nvarchar(max)') AS [Description],
    Tbl.Col.value('Percentage[1]', 'nvarchar(40)') AS [Percentage],
    Tbl.Col.value('CustomImpact1Value[1]', 'nvarchar(40)') AS [CustomImpact1Value],
    Tbl.Col.value('CustomImpact2Value[1]', 'nvarchar(40)') AS [CustomImpact2Value],
    Tbl.Col.value('CustomImpact3Value[1]', 'nvarchar(40)') AS [CustomImpact3Value],
    Tbl.Col.value('CustomImpact4Value[1]', 'nvarchar(40)') AS [CustomImpact4Value],
    Tbl.Col.value('CustomImpact5Value[1]', 'nvarchar(40)') AS [CustomImpact5Value],
    Tbl.Col.value('sk_Id[1]', 'nvarchar(40)') AS [sk_Id],
    Tbl.Col.value('bk_Id[1]', 'nvarchar(40)') AS [bk_Id],
    Tbl.Col.value('bk_TenantId[1]', 'nvarchar(40)') AS [bk_TenantId],
    Tbl.Col.value('target_Date[1]', 'nvarchar(40)') AS [target_Date],
    Tbl.Col.value('source_DateCreated[1]', 'nvarchar(40)') AS [source_DateCreated],
    Tbl.Col.value('source_DateLastModified[1]', 'nvarchar(40)') AS [source_DateLastModified],
    Tbl.Col.value('EntityStatus[1]', 'nvarchar(max)') AS [EntityStatus],
    sysdatetime() as [Load_DateTime] 
FROM @Entities.nodes('//Entity') Tbl(Col);
