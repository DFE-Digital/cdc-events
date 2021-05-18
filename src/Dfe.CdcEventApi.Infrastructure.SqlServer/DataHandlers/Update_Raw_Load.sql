UPDATE [dbo].[Raw_Load]
   SET 
       [Finish_DateTime] = @Finished 
      ,[Status] = @Status
      ,[ReportTitle] = @ReportTitle
      ,[ReportBody] = @ReportBody
      ,[ReportedTo] = @ReportTo
 WHERE 
    [Load_DateTime] = @RunIdentifier
