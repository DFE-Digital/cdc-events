UPDATE [etl].[Control]
   SET 
       [Status] = @Status
 WHERE 
    [Load_DateTime] = @RunIdentifier
