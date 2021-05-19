SELECT 
	notify.[ID] ,
	notify.[Active] ,
	notify.[Status],
	notify.[Group],
	notify.[Email]
FROM
	[dbo].[Raw_LoadNotification] notify
WHERE 
	 notify.[Active]  = 1
AND
	notify.[Status] & @status <> 0
ORDER BY
	notify.[Group], notify.[Email] DESC
