INSERT INTO [dbo].[Raw_LoadControl] 
    ([Load_DateTime], [Status]) 
SELECT
	@RunIdentifier as [Load_DateTime]

SELECT TOP 1
	[Load_DateTime]
FROM 
	[dbo].[Raw_LoadControl] 
WHERE 
	[Load_DateTime] BETWEEN DATEADD(ms ,-5,@RunIdentifier) AND DATEADD(ms, 5,@RunIdentifier )
ORDER BY
	[Load_DateTime] DESC
GO;