SELECT [Status]
      ,[Subject]
      ,[Body]
      ,[IncludeRowStats]
      ,[IncludeRIChecks]
FROM 
    [dbo].[Raw_LoadNotificationTemplate]
WHERE 
    [Status] & @Status <> 0
