INSERT INTO [raw].[Load] 
    ([Load_DateTime]) 
SELECT
	@runIdentifier as [Load_DateTime]
