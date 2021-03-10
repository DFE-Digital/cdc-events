-- DECLARE @Entities XML;

INSERT INTO SitesCustomFields
SELECT  
	Tbl.Col.value('bk_SiteId[1]', 'nvarchar(MAX)') AS bk_SiteId,
	Tbl.Col.value('CustomFieldType[1]', 'nvarchar(MAX)') AS CustomFieldType,
	Tbl.Col.value('Value[1]', 'nvarchar(MAX)') AS [Value],
	Tbl.Col.value('Name[1]', 'nvarchar(MAX)') AS [Name],
	Tbl.Col.value('SortOrder[1]', 'INT') AS SortOrder,
	Tbl.Col.value('sk_Id[1]', 'INT') AS sk_Id,
	Tbl.Col.value('bk_Id[1]', 'nvarchar(MAX)') AS bk_Id,
	Tbl.Col.value('bk_TenantId[1]', 'nvarchar(MAX)') AS bk_TenantId,
	Tbl.Col.value('target_Date[1]', 'nvarchar(MAX)') AS target_Date,
	Tbl.Col.value('source_DateLastModified[1]', 'nvarchar(MAX)') AS source_DateLastModified,
	Tbl.Col.value('EntityStatus[1]', 'nvarchar(MAX)') AS EntityStatus
FROM @Entities.nodes('//Entity') Tbl(Col);