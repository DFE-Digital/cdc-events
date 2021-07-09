UPDATE [etl].[Control]
   SET 
    [Finish_DateTime] = @Finish_DateTime,
    [Since_DateTime] = @Since_DateTime,
    [Count] = @Count,
    [Status] = @Status
 WHERE 
    [Load_DateTime] = @Load_DateTime
