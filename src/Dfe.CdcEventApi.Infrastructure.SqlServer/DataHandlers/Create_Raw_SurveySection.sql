
INSERT INTO [Raw_SurveySection]
    ([bk_SurveyId], [Name], [Description], [IsComplete], [Depth], [Latitude], [Longitude], [ref_SurveySectionTypeReference], [bk_AssetId], [sk_Id], [bk_Id], [bk_TenantId], [target_Date], [source_DateCreated], [source_DateLastModified], [EntityStatus], [Load_DateTime])
SELECT
    Tbl.Col.value('bk_SurveyId[1]', 'nvarchar(40)') AS [bk_SurveyId],
    Tbl.Col.value('Name[1]', 'nvarchar(max)') AS [Name],
    Tbl.Col.value('Description[1]', 'nvarchar(max)') AS [Description],
    Tbl.Col.value('IsComplete[1]', 'nvarchar(40)') AS [IsComplete],
    Tbl.Col.value('Depth[1]', 'nvarchar(max)') AS [Depth],
    Tbl.Col.value('Latitude[1]', 'nvarchar(40)') AS [Latitude],
    Tbl.Col.value('Longitude[1]', 'nvarchar(40)') AS [Longitude],
    Tbl.Col.value('ref_SurveySectionTypeReference[1]', 'nvarchar(max)') AS [ref_SurveySectionTypeReference],
    Tbl.Col.value('bk_AssetId[1]', 'nvarchar(40)') AS [bk_AssetId],
    Tbl.Col.value('sk_Id[1]', 'nvarchar(40)') AS [sk_Id],
    Tbl.Col.value('bk_Id[1]', 'nvarchar(40)') AS [bk_Id],
    Tbl.Col.value('bk_TenantId[1]', 'nvarchar(40)') AS [bk_TenantId],
    Tbl.Col.value('target_Date[1]', 'nvarchar(40)') AS [target_Date],
    Tbl.Col.value('source_DateCreated[1]', 'nvarchar(40)') AS [source_DateCreated],
    Tbl.Col.value('source_DateLastModified[1]', 'nvarchar(40)') AS [source_DateLastModified],
    Tbl.Col.value('EntityStatus[1]', 'nvarchar(max)') AS [EntityStatus],
    sysdatetime() as [Load_DateTime] 
FROM @Entities.nodes('//Entity') Tbl(Col);
