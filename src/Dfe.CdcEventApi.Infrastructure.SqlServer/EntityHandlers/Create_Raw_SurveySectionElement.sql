INSERT INTO [raw].[SurveySectionElement]
    ([bk_SurveySectionId], [ref_ElementTypeReference], [ref_ElementTypeDescription], [ref_Grade], [ref_Priority], [ref_UnitOfMeasureReference], [Quantity], [ref_MethodOfMeasurementReference], [PercentageOfParent], [ref_ActionRequiredReference], [ref_FailureTypeReference], [ref_ContributingFactorReference], [HealthAndSafetyIssue], [BudgetCost], [ResidualLife], [IsComplete], [IsRemoved], [ref_AccessReference], [ref_CompositionGroupName], [ActivityCycle], [FrequencyInYear], [Latitude], [Longitude], [SortOrder], [ExternalUUID], [EstimatedCost], [bk_ParentElementId], [sk_Id], [bk_Id], [bk_TenantId], [target_Date], [source_DateCreated], [source_DateLastModified], [EntityStatus], [Load_DateTime])
SELECT
    Tbl.Col.value('bk_SurveySectionId[1]', 'nvarchar(40)') AS [bk_SurveySectionId],
    Tbl.Col.value('ref_ElementTypeReference[1]', 'nvarchar(max)') AS [ref_ElementTypeReference],
    Tbl.Col.value('ref_ElementTypeDescription[1]', 'nvarchar(max)') AS [ref_ElementTypeDescription],
    Tbl.Col.value('ref_Grade[1]', 'nvarchar(max)') AS [ref_Grade],
    Tbl.Col.value('ref_Priority[1]', 'nvarchar(max)') AS [ref_Priority],
    Tbl.Col.value('ref_UnitOfMeasureReference[1]', 'nvarchar(max)') AS [ref_UnitOfMeasureReference],
    Tbl.Col.value('Quantity[1]', 'nvarchar(40)') AS [Quantity],
    Tbl.Col.value('ref_MethodOfMeasurementReference[1]', 'nvarchar(max)') AS [ref_MethodOfMeasurementReference],
    Tbl.Col.value('PercentageOfParent[1]', 'nvarchar(40)') AS [PercentageOfParent],
    Tbl.Col.value('ref_ActionRequiredReference[1]', 'nvarchar(max)') AS [ref_ActionRequiredReference],
    Tbl.Col.value('ref_FailureTypeReference[1]', 'nvarchar(max)') AS [ref_FailureTypeReference],
    Tbl.Col.value('ref_ContributingFactorReference[1]', 'nvarchar(max)') AS [ref_ContributingFactorReference],
    Tbl.Col.value('HealthAndSafetyIssue[1]', 'nvarchar(40)') AS [HealthAndSafetyIssue],
    Tbl.Col.value('BudgetCost[1]', 'nvarchar(40)') AS [BudgetCost],
    Tbl.Col.value('ResidualLife[1]', 'nvarchar(40)') AS [ResidualLife],
    Tbl.Col.value('IsComplete[1]', 'nvarchar(40)') AS [IsComplete],
    Tbl.Col.value('IsRemoved[1]', 'nvarchar(40)') AS [IsRemoved],
    Tbl.Col.value('ref_AccessReference[1]', 'nvarchar(max)') AS [ref_AccessReference],
    Tbl.Col.value('ref_CompositionGroupName[1]', 'nvarchar(max)') AS [ref_CompositionGroupName],
    Tbl.Col.value('ActivityCycle[1]', 'nvarchar(40)') AS [ActivityCycle],
    Tbl.Col.value('FrequencyInYear[1]', 'nvarchar(40)') AS [FrequencyInYear],
    Tbl.Col.value('Latitude[1]', 'nvarchar(40)') AS [Latitude],
    Tbl.Col.value('Longitude[1]', 'nvarchar(40)') AS [Longitude],
    Tbl.Col.value('SortOrder[1]', 'nvarchar(40)') AS [SortOrder],
    Tbl.Col.value('ExternalUUID[1]', 'nvarchar(40)') AS [ExternalUUID],
    Tbl.Col.value('EstimatedCost[1]', 'nvarchar(40)') AS [EstimatedCost],
    Tbl.Col.value('bk_ParentElementId[1]', 'nvarchar(40)') AS [bk_ParentElementId],
    Tbl.Col.value('sk_Id[1]', 'nvarchar(40)') AS [sk_Id],
    Tbl.Col.value('bk_Id[1]', 'nvarchar(40)') AS [bk_Id],
    Tbl.Col.value('bk_TenantId[1]', 'nvarchar(40)') AS [bk_TenantId],
    Tbl.Col.value('target_Date[1]', 'nvarchar(40)') AS [target_Date],
    Tbl.Col.value('source_DateCreated[1]', 'nvarchar(40)') AS [source_DateCreated],
    Tbl.Col.value('source_DateLastModified[1]', 'nvarchar(40)') AS [source_DateLastModified],
    Tbl.Col.value('EntityStatus[1]', 'nvarchar(max)') AS [EntityStatus],
    @RunIdentifier as [Load_DateTime] 
FROM @Entities.nodes('//Entity') Tbl(Col)
LEFT JOIN (SELECT [bk_id] FROM [raw].[SurveySectionElement] WHERE [Load_DateTime] = @RunIdentifier) AS [existing]
ON Tbl.Col.value('bk_Id[1]', 'nvarchar(40)') = [existing].[bk_id]
WHERE [existing].[bk_id] IS NULL;