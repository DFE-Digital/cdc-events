SELECT [SiteName]
      ,[SiteId]
      ,[Name]
      ,[BlobSubFolderName]
      ,[SurveyId]
      ,[Description]
      ,[MimeType]
      ,[ID]
      ,[BlobKey]
      ,[ParentObjectType]
      ,[ParentObjectId]
      ,[BlobData]
      ,[URL]
      ,[Load_DateTime]
      ,[Upload_DateTime]
 FROM 
	[dbo].[Raw_Load_Attachments]
 WHERE
	[Load_DateTime] = @RunIdentifier
 ORDER BY
    [BlobKey]