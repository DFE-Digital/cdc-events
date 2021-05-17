UPDATE [dbo].[Raw_LoadControl]
   SET 
       [Finish_DateTime] = @Finished 
      ,[Status] = @Status
      ,[ReportBody] = @ReportBody
      ,[ReportedTo] = @ReportTo
 WHERE 
    [Load_DateTime] = @RunIdentifier
