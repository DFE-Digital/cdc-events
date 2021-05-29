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
      ,[ParentObjectLoadDateTime]
      ,NULL AS [BlobData]
      ,[URL]
      ,[Load_DateTime]
      ,[Upload_DateTime]
 FROM 
	[dbo].[Raw_Load_Attachments]
 WHERE
	[ParentObjectLoadDateTime] = @RunIdentifier
 ORDER BY
    [BlobKey]