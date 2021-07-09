UPDATE [etl].[Control]
   SET 
    [Finish_DateTime] = @Finish_DateTime,
    [Since_DateTime] = @Since_DateTime,
    [Count] = @Count,
    [Status] = @Status,
    [ReportTitle] = @ReportTitle,
    [ReportBody] = @ReportBody,
    [ReportedTo] = @ReportTo
 WHERE 
    [Load_DateTime] = @Load_DateTime
