INSERT INTO [Raw_Blob]
    ([BlobKey], [ParentObjectType], [ParentObjectId], [BlobData], [MimeType], [Load_DateTime])
SELECT
    @BlobKey ,
    @ParentObjectType ,
    @ParentObjectId ,
    @BlobData,
    @MimeType ,
    @RunIdentifier
