SELECT [Blobkey]
      ,[Folder]
      ,[Extension]
  FROM [etl].[Extract-Attachments]
WHERE [URL] IS NULL OR (COALESCE([Date Last Modified], [Date Created]) > [Obtained])