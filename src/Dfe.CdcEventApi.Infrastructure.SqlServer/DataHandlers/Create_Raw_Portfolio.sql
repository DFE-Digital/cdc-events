INSERT INTO [Raw_Portfolio]
    ([Name], [Description], [Customer], [ref_SectorReference], [sk_Id], [bk_Id], [bk_TenantId], [target_Date], [source_DateCreated], [source_DateLastModified], [EntityStatus], [Load_DateTime])
SELECT
    Tbl.Col.value('Name[1]', 'nvarchar(max)') AS [Name],
    Tbl.Col.value('Description[1]', 'nvarchar(max)') AS [Description],
    Tbl.Col.value('Customer[1]', 'nvarchar(max)') AS [Customer],
    Tbl.Col.value('ref_SectorReference[1]', 'nvarchar(max)') AS [ref_SectorReference],
    Tbl.Col.value('sk_Id[1]', 'nvarchar(40)') AS [sk_Id],
    Tbl.Col.value('bk_Id[1]', 'nvarchar(40)') AS [bk_Id],
    Tbl.Col.value('bk_TenantId[1]', 'nvarchar(40)') AS [bk_TenantId],
    Tbl.Col.value('target_Date[1]', 'nvarchar(40)') AS [target_Date],
    Tbl.Col.value('source_DateCreated[1]', 'nvarchar(40)') AS [source_DateCreated],
    Tbl.Col.value('source_DateLastModified[1]', 'nvarchar(40)') AS [source_DateLastModified],
    Tbl.Col.value('EntityStatus[1]', 'nvarchar(max)') AS [EntityStatus],
    @RunIdentifier as [Load_DateTime] 
FROM @Entities.nodes('//Entity') Tbl(Col);