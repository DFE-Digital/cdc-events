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
	 [Load_DateTime]  = @RunIdentifier
UNION
SELECT 
	load.[Load_DateTime] AS [Load_DateTime],
	load.[Finish_DateTime] AS [Finish_DateTime],
	load.[Status] AS [Status],
	load.[ReportTitle] AS [ReportTitle],
	load.[ReportBody] AS [ReportBody],
	load.[ReportedTo] AS [ReportedTo]
FROM
	[dbo].[Raw_Load] load
INNER JOIN 
	 (
		 SELECT 
			MAX( [Load_DateTime] ) AS [Load_DateTime], [Status]
		 FROM
			[dbo].[Raw_Load] 
		WHERE 
			[Load_DateTime] < @RunIdentifier
		GROUP BY 
			[Status]
		HAVING
			[Status] = 32
	) latest
ON load.[Load_DateTime] = latest.[Load_DateTime]
ORDER BY
	[Load_DateTime] DESC
