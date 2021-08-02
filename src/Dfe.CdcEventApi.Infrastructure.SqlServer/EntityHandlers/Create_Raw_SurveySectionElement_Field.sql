INSERT INTO [raw].[SurveySectionElement_Field]
    ([bk_SurveySectionElementId], [CustomFieldType], [Value], [Name], [SortOrder], [ExternalReference], [sk_Id], [bk_Id], [bk_TenantId], [target_Date], [source_DateCreated], [source_DateLastModified], [EntityStatus], [Load_DateTime])
SELECT
    Tbl.Col.value('bk_SurveySectionElementId[1]', 'nvarchar(40)') AS [bk_SurveySectionElementId],
    Tbl.Col.value('CustomFieldType[1]', 'nvarchar(max)') AS [CustomFieldType],
    Tbl.Col.value('Value[1]', 'nvarchar(max)') AS [Value],
    Tbl.Col.value('Name[1]', 'nvarchar(max)') AS [Name],
    Tbl.Col.value('SortOrder[1]', 'nvarchar(40)') AS [SortOrder],
    Tbl.Col.value('ExternalReference[1]', 'nvarchar(max)') AS [ExternalReference],
    Tbl.Col.value('sk_Id[1]', 'nvarchar(40)') AS [sk_Id],
    Tbl.Col.value('bk_Id[1]', 'nvarchar(40)') AS [bk_Id],
    Tbl.Col.value('bk_TenantId[1]', 'nvarchar(40)') AS [bk_TenantId],
    Tbl.Col.value('target_Date[1]', 'nvarchar(40)') AS [target_Date],
    Tbl.Col.value('source_DateCreated[1]', 'nvarchar(40)') AS [source_DateCreated],
    Tbl.Col.value('source_DateLastModified[1]', 'nvarchar(40)') AS [source_DateLastModified],
    Tbl.Col.value('EntityStatus[1]', 'nvarchar(max)') AS [EntityStatus],
    @RunIdentifier as [Load_DateTime] 
FROM @Entities.nodes('//Entity') Tbl(Col);