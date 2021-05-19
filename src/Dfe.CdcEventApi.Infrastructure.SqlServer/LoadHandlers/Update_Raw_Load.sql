UPDATE [dbo].[Raw_Load]
   SET 
       [Finish_DateTime] = @Finish_DateTime 
      ,[Status] = @Status
      ,[ReportTitle] = @ReportTitle
      ,[ReportBody] = @ReportBody
      ,[ReportedTo] = @ReportTo
 WHERE 
    [Load_DateTime] = @Load_DateTime
