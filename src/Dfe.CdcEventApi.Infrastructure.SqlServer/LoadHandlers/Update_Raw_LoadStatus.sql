UPDATE [etl].[Control]
   SET 
       [Status] = @Status,
       [ReportTitle] = @ReportTitle
 WHERE 
    [Load_DateTime] = @RunIdentifier
