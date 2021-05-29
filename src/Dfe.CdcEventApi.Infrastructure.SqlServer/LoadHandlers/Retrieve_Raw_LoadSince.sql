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
OR
	[Load_DateTime] = 
( 

SELECT 
		TOP 1 [Load_DateTime] 
		 FROM
			[dbo].[Raw_Load] 
		WHERE 
			[Load_DateTime] < DATEADD(ms,-5, @RunIdentifier)
		AND 
			[Count] != 0
		GROUP BY 
			[Load_DateTime], [Status], [Count]
		HAVING
			[Status] = 32
		ORDER BY
			[Load_DateTime] DESC
	) 

ORDER BY
	[Load_DateTime] DESC
