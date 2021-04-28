INSERT INTO [Raw_Blob]
    ([Blobkey], [ParentObjectType], [ParentObjectId], [URL], [EntityStatus], [Load_DateTime])
SELECT
    Tbl.Col.value('Blobkey[1]', 'nvarchar(40)') AS [Blobkey],
    Tbl.Col.value('ParentObjectType[1]', 'nvarchar(40)') AS [ParentObjectType],
    Tbl.Col.value('ParentObjectId[1]', 'nvarchar(40)') AS [ParentObjectId],
    Tbl.Col.value('URL[1]', 'nvarchar(max)') AS [URL],
    Tbl.Col.value('EntityStatus[1]', 'nvarchar(max)') AS [EntityStatus],
    @RunIdentifier as [Load_DateTime] 
FROM @Entities.nodes('//Entity') Tbl(Col);