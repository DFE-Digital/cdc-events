SELECT TOP 2
	[Load_DateTime],
	[Finish_DateTime],
	[Status],
	[ReportTitle],
	[ReportBody],
	[ReportedTo]
FROM
	[dbo].[Raw_Load]
WHERE 
	 [Load_DateTime]  <= @RunIdentifier
AND
	[Status] IN (1, 32)
ORDER BY
	[Load_DateTime] DESC