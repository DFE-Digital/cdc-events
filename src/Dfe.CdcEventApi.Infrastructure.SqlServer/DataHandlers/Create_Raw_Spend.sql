INSERT INTO [Raw_Spend]
    ([bk_ActualId], [Value], [TransactionDate], [CommittedDate], [Planned], [Description], [Contractor], [QualityStatus], [ref_ActivityTypeReference], [CustomImpact1Value], [CustomImpact2Value], [CustomImpact3Value], [CustomImpact4Value], [CustomImpact5Value], [sk_Id], [bk_Id], [bk_TenantId], [target_Date], [source_DateCreated], [source_DateLastModified], [EntityStatus], [Load_DateTime])
SELECT
    Tbl.Col.value('bk_ActualId[1]', 'nvarchar(40)') AS [bk_ActualId],
    Tbl.Col.value('Value[1]', 'nvarchar(40)') AS [Value],
    Tbl.Col.value('TransactionDate[1]', 'nvarchar(40)') AS [TransactionDate],
    Tbl.Col.value('CommittedDate[1]', 'nvarchar(40)') AS [CommittedDate],
    Tbl.Col.value('Planned[1]', 'nvarchar(40)') AS [Planned],
    Tbl.Col.value('Description[1]', 'nvarchar(max)') AS [Description],
    Tbl.Col.value('Contractor[1]', 'nvarchar(max)') AS [Contractor],
    Tbl.Col.value('QualityStatus[1]', 'nvarchar(max)') AS [QualityStatus],
    Tbl.Col.value('ref_ActivityTypeReference[1]', 'nvarchar(max)') AS [ref_ActivityTypeReference],
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
    @RunIdentifier as [Load_DateTime] 
FROM @Entities.nodes('//Entity') Tbl(Col);