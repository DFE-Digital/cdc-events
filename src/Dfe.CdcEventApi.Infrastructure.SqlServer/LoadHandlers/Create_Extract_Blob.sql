INSERT INTO [etl].[Blob]
           ([Blobkey]
           ,[Path]
           ,[Obtained])
SELECT
    @BlobKey ,
    @Path,
    @RunIdentifier
