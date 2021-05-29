SELECT 
	[Load_DateTime],
	[Finish_DateTime],
	[Count],
	[Status] ,
	[ReportTitle],
	[ReportBody] ,
	[ReportedTo] 
FROM 
	[dbo].[Raw_Load] 
WHERE 
	[Load_DateTime] = @runIdentifier