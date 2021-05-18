INSERT INTO [dbo].[Raw_Load] 
    ([Load_DateTime]) 
SELECT
	@RunIdentifier as [Load_DateTime]
