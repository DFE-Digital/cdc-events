SELECT TOP 2
	[Load_DateTime],
	[Finish_DateTime],
	[Status],
	[ReportBody],
	[ReportedTo]
FROM
	[dbo].[Raw_LoadControl]
WHERE 
	 [Load_DateTime]  <= @RunIdentifier
AND
	[Status] IN ( 0, 16)
ORDER BY
	[Load_DateTime] DESC
