SELECT [Status]
      ,[Subject]
      ,[Body]
      ,[IncludeRowStats]
      ,[IncludeRIChecks]
FROM 
    [raw].[LoadNotificationTemplate]
WHERE 
    [Status] & @Status <> 0
