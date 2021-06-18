SELECT 
	[Load_DateTime],
	[Since_DateTime],
	[Finish_DateTime],
	[Count],
	[Status] ,
	[ReportTitle],
	[ReportBody] ,
	[ReportedTo] 
FROM 
	[raw].[Load] 
WHERE 
	[Load_DateTime] = @runIdentifier