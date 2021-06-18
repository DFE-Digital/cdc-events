UPDATE [raw].[Load]
   SET 
       [Status] = @Status,
       [ReportTitle] = @ReportTitle
 WHERE 
    [Load_DateTime] = @RunIdentifier
