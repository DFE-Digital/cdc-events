INSERT INTO [etl].[Control] 
    ([Load_DateTime]) 
SELECT
	@runIdentifier as [Load_DateTime]
