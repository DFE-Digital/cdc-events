SELECT [Status]
      ,[Subject]
      ,[Body]
      ,[IncludeRowStats]
      ,[IncludeRunInfo]
      ,[IncludeRIChecks]
FROM 
    [dbo].[Raw_LoadNotificationTemplate]
WHERE 
    [Status] & @Status <> 0
