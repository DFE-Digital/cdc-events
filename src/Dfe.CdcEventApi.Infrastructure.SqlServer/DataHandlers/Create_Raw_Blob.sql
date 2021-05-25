INSERT INTO [Raw_Blob]
    ([BlobKey], [ParentObjectType], [ParentObjectId], [BlobData], [MimeType], [Load_DateTime])
SELECT
    Tbl.Col.value('BlobKey[1]', 'nvarchar(40)') AS [BlobKey],
    Tbl.Col.value('ParentObjectType[1]', 'nvarchar(40)') AS [ParentObjectType],
    Tbl.Col.value('ParentObjectId[1]', 'nvarchar(40)') AS [ParentObjectId],
    Tbl.Col.value('BlobData[1]', 'nvarchar(max)') AS [BlobData],
    Tbl.Col.value('MimeType[1]', 'nvarchar(100)') AS [MimeType],
    @RunIdentifier as [Load_DateTime] 
FROM @Entities.nodes('//Entity') Tbl(Col);