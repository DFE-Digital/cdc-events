INSERT INTO [etl].[Blob]
    ([BlobKey], 
        [ParentObjectType], 
        [ParentObjectId], 
        [BlobData], 
        [MimeType], 
        [Load_DateTime])
SELECT
    @BlobKey ,
    @ParentObjectType ,
    @ParentObjectId ,
    @BlobData,
    @MimeType ,
    @RunIdentifier
