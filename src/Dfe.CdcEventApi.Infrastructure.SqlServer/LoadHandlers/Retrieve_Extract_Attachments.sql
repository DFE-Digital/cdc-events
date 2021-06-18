SELECT [SiteUniqueId]
      ,[Entity]
      ,[EntityUniqueId]
      ,[Blobkey]
      ,[MimeType]
      ,[URL]
      ,[Path]
      ,[Folder]
      ,[Extension]
      ,[Date Created]
      ,[Date Last Modified]
      ,[Obtained]
 FROM 
     [etl].[EXTRACT-Attachments]
 WHERE
    [URL] IS NULL
OR
	[Date Created] > [Obtained]
OR 
   [Date Last Modified] > [Obtained]