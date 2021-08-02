INSERT INTO [raw].[SurveySectionElement_Evidence]
    ([bk_SurveySectionElementId], [Description], [ref_SurveySectionElementEvidenceItemTypeReference], [MimeType], [Reference], [SortOrder], [CustomReference], [CustomTag], [FileOrigin], [BlobKey], [sk_Id], [bk_Id], [bk_TenantId], [target_Date], [source_DateCreated], [source_DateLastModified], [EntityStatus], [Load_DateTime])
SELECT
    Tbl.Col.value('bk_SurveySectionElementId[1]', 'nvarchar(40)') AS [bk_SurveySectionElementId],
    Tbl.Col.value('Description[1]', 'nvarchar(max)') AS [Description],
    Tbl.Col.value('ref_SurveySectionElementEvidenceItemTypeReference[1]', 'nvarchar(max)') AS [ref_SurveySectionElementEvidenceItemTypeReference],
    Tbl.Col.value('MimeType[1]', 'nvarchar(max)') AS [MimeType],
    Tbl.Col.value('Reference[1]', 'nvarchar(max)') AS [Reference],
    Tbl.Col.value('SortOrder[1]', 'nvarchar(40)') AS [SortOrder],
    Tbl.Col.value('CustomReference[1]', 'nvarchar(max)') AS [CustomReference],
    Tbl.Col.value('CustomTag[1]', 'nvarchar(max)') AS [CustomTag],
    Tbl.Col.value('FileOrigin[1]', 'nvarchar(max)') AS [FileOrigin],
    Tbl.Col.value('BlobKey[1]', 'nvarchar(40)') AS [BlobKey],
    Tbl.Col.value('sk_Id[1]', 'nvarchar(40)') AS [sk_Id],
    Tbl.Col.value('bk_Id[1]', 'nvarchar(40)') AS [bk_Id],
    Tbl.Col.value('bk_TenantId[1]', 'nvarchar(40)') AS [bk_TenantId],
    Tbl.Col.value('target_Date[1]', 'nvarchar(40)') AS [target_Date],
    Tbl.Col.value('source_DateCreated[1]', 'nvarchar(40)') AS [source_DateCreated],
    Tbl.Col.value('source_DateLastModified[1]', 'nvarchar(40)') AS [source_DateLastModified],
    Tbl.Col.value('EntityStatus[1]', 'nvarchar(40)') AS [EntityStatus],
    @RunIdentifier as [Load_DateTime] 
FROM @Entities.nodes('//Entity') Tbl(Col);