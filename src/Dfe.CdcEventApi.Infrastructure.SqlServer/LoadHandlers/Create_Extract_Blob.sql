MERGE [etl].[Blob] AS target
USING (
	SELECT     
		@BlobKey AS [BlobKey],    
		@BlobObtained as [Obtained]
) as source
ON target.[Blobkey] = source.Blobkey
WHEN MATCHED THEN
	UPDATE SET 
			[Obtained] = source.[Obtained]
WHEN NOT MATCHED THEN
	INSERT	(
			[BlobKey],
			[Obtained]
		)
		VALUES	(
		source.[BlobKey], 
		source.[Obtained]
		);
