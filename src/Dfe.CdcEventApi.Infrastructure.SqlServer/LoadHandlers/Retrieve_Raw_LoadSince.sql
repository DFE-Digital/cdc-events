SELECT 
	[Load_DateTime],
	[Finish_DateTime],
	[Status],
	[ReportTitle],
	[ReportBody],
	[ReportedTo]
FROM
	[dbo].[Raw_Load]
WHERE 
	 [Load_DateTime]  BETWEEN DATEADD(ms,-5, @RunIdentifier) AND DATEADD(ms,5, @RunIdentifier)
UNION
SELECT TOP 1
	[Load_DateTime],
	[Finish_DateTime],
	[Status],
	[ReportTitle],
	[ReportBody],
	[ReportedTo]
FROM
	[dbo].[Raw_Load]
WHERE 
	 [Load_DateTime]  < DATEADD(ms,-5, @RunIdentifier)
AND
	[Status] = 32
ORDER BY
	[Load_DateTime] DESC