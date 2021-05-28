SELECT 
	[Load_DateTime],
	[Finish_DateTime],
	[Count],
	[Status],
	[ReportTitle],
	[ReportBody],
	[ReportedTo]
FROM
	[dbo].[Raw_Load]
WHERE 
	 [Load_DateTime]  BETWEEN DATEADD(ms,-5, @RunIdentifier) AND DATEADD(ms,5, @RunIdentifier)
UNION
SELECT 
	load.[Load_DateTime] AS [Load_DateTime],
	load.[Finish_DateTime] AS [Finish_DateTime],
	load.[Count],
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
			[Load_DateTime] < DATEADD(ms,-5, @RunIdentifier)
		AND 
			[Count] != 0
		GROUP BY 
			[Status], [Count]
		HAVING
			[Status] = 32
	) latest
ON load.[Load_DateTime] = latest.[Load_DateTime]
ORDER BY
	[Load_DateTime] DESC
