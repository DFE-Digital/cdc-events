SELECT 
	[Load_DateTime],
	[Finish_DateTime],
	[Status] ,
	[ReportTitle],
	[ReportBody] ,
	[ReportedTo] 
FROM 
	[dbo].[Raw_Load] 
WHERE 
	[Load_DateTime] = @runIdentifier