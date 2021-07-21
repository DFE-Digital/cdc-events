Notes: On Logic App script round trip from the Azure Logic App Code view to Repository.

Select all Json except the starting and ending braces { }

Open the event-generator-template.json in the repository.

Replace the resources:properties definition and parameters properties with those extracted from the Azure code view

Collapse all and then expand the inner resources:properties:parameters element and perform the following json element replacements

Element:	SecretsAudience:defaultValue
No change:

Element:	SecretsEndpoint:defaultValue
Replace: 	https://s153d01-kv-01.vault.azure.net/secrets/
With: 		[variables('keyVaultSecretUri')]

Element:	SourceOAUTHEndpoint:defaultValue
Replace: 	https://uatcdc2.kykloud.com/api/user/login
With: 		[variables('loginUri')]

Replace:	
With:		

Element:	SourceEndpoint:defaultValue
Replace: 	https://uatcdc2.kykloud.com/api/data/
With: 		[variables('dataPathUri')]

Replace:	https://dev-api-customerengagement.platform.education.gov.uk/v1/cdc-events/
With: 		[variables('cdcEventsApiBaseUri')]

Replace: 	https://dev-api-customerengagement.platform.education.gov.uk/v1/cdc-events/load
With: 		[concat(variables('cdcEventsApiBaseUri'), '/load')]

