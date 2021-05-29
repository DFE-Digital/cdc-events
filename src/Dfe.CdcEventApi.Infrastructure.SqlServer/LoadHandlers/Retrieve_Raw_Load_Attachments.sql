SELECT [BlobKey]
      ,[MimeType]
      ,[ParentObjectType]
      ,[ParentObjectId]
 FROM 
	[dbo].[Raw_Load_Attachments]
 WHERE
	[ParentObjectLoadDateTime] = @RunIdentifier