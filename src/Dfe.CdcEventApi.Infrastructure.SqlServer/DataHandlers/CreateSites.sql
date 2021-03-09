-- DECLARE @Entities XML;

INSERT INTO [Sites]
SELECT  
	Tbl.Col.value('Name[1]', 'nvarchar(MAX)') AS [Name],
	Tbl.Col.value('Description[1]', 'nvarchar(MAX)') AS [Description],
	Tbl.Col.value('GrossFloorArea[1]', 'INT') AS [GrossFloorArea],
	Tbl.Col.value('SiteFootprintArea[1]', 'INT') AS [SiteFootprintArea],
	Tbl.Col.value('AreaUnit[1]', 'nvarchar(MAX)') AS AreaUnit,
	Tbl.Col.value('MainUsage[1]', 'nvarchar(MAX)') AS MainUsage,
	Tbl.Col.value('PeriodOfInterest[1]', 'INT') AS [PeriodOfInterest],
	Tbl.Col.value('RebuildValue[1]', 'INT') AS [RebuildValue],
	Tbl.Col.value('Latitude[1]', 'INT') AS Latitude,
	Tbl.Col.value('Longitude[1]', 'INT') AS Longitude,
	Tbl.Col.value('PostCode[1]', 'nvarchar(MAX)') AS [PostCode],
	Tbl.Col.value('Country[1]', 'nvarchar(MAX)') AS Country,
	Tbl.Col.value('Contract[1]', 'nvarchar(MAX)') AS [Contract],
	Tbl.Col.value('ContractEndCondition[1]', 'nvarchar(MAX)') AS [ContractEndCondition],
	Tbl.Col.value('ContractEndResidualLife[1]', 'nvarchar(MAX)') AS [ContractEndResidualLife],
	Tbl.Col.value('Customer[1]', 'nvarchar(MAX)') AS Customer,
	Tbl.Col.value('HoursOfOperation[1]', 'nvarchar(MAX)') AS HoursOfOperation,
	Tbl.Col.value('IdPath[1]', 'nvarchar(MAX)') AS [IdPath],
	Tbl.Col.value('sk_Id[1]', 'INT') AS sk_Id,
	Tbl.Col.value('bk_Id[1]', 'nvarchar(MAX)') AS bk_Id,
	Tbl.Col.value('bk_TenantId[1]', 'nvarchar(MAX)') AS [bk_TenantId],
	Tbl.Col.value('target_Date[1]', 'nvarchar(MAX)') AS [target_Date],
	Tbl.Col.value('source_DateCreated[1]', 'nvarchar(MAX)') AS source_DateCreated,
	Tbl.Col.value('source_DateLastModifed[1]', 'nvarchar(MAX)') AS source_DateLastModified,
	Tbl.Col.value('EntityStatus[1]', 'nvarchar(MAX)') AS EntityStatus
FROM @Entities.nodes('//Entity') Tbl(Col);