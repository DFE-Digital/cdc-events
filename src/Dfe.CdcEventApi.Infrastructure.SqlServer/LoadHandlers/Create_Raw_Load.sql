INSERT INTO [dbo].[Raw_Load] 
    ([Load_DateTime]) 
SELECT
	@runIdentifier as [Load_DateTime]
