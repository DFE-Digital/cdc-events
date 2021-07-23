$pathToJson = "..\azure\event-generator-template.json"
$a = Get-Content $pathToJson | ConvertFrom-Json
$a.resources[0].properties.definition.parameters.SourceEndpoint.defaultValue = "[variables('dataPathUri')]"
$a.resources[0].properties.definition.parameters.SourceOAUTHEndpoint.defaultValue = "[variables('loginUri')]"
$a.resources[0].properties.definition.parameters.SourceOAUTHId.defaultValue = "[parameters('kycloudApiEmail')]"
$a.resources[0].properties.definition.parameters.SourceOAUTHKeyUri.defaultValue = "[concat(reference(variables('keyVaultSecretKycloudApiToken'), '2019-09-01').secretUriWithVersion, '?api-version=2016-10-01')]"
$a.resources[0].properties.definition.parameters.SourceOAUTHSecretUri.defaultValue = "[concat(reference(variables('keyVaultSecretKycloudApiPassword'), '2019-09-01').secretUriWithVersion, '?api-version=2016-10-01')]"
$a.resources[0].properties.definition.parameters.TargetOAUTHCredentialsUri.defaultValue = "[concat(reference(variables('keyVaultSecretCdcEventsTokenRequestPayload'), '2019-09-01').secretUriWithVersion, '?api-version=2016-10-01')]"
$a.resources[0].properties.definition.parameters.TargetOAUTHKeyUri.defaultValue = "[concat(reference(variables('keyVaultSecretCdcEventsSubscriptionKey'), '2019-09-01').secretUriWithVersion, '?api-version=2016-10-01')]"
$a.resources[0].properties.definition.parameters.TargetEndpoint.defaultValue = "[parameters('cdcEventsApiBaseUri')]"
$a.resources[0].properties.definition.parameters.TargetControlEndpoint.defaultValue = "[concat(parameters('cdcEventsApiBaseUri'), '/load')]"
$a.resources[0].properties.definition.parameters.TargetOAUTHEndpoint.defaultValue = "[parameters('internalOAuthTokenEndpoint')]"
$a.resources[0].properties.definition.parameters.StatusStart.defaultValue = 1
$a.resources[0].properties.definition.parameters.StatusLogin.defaultValue = 2
$a.resources[0].properties.definition.parameters.StatusEntities.defaultValue = 3
$a.resources[0].properties.definition.parameters.StatusExtracting.defaultValue = 4
$a.resources[0].properties.definition.parameters.StatusAttachments.defaultValue = 5
$a.resources[0].properties.definition.parameters.StatusTransforming.defaultValue = 6
$a.resources[0].properties.definition.parameters.StatusReporting.defaultValue = 100
$a | ConvertTo-Json -Compress -Depth 32 | % { [System.Text.RegularExpressions.Regex]::Unescape($_) }  | set-content $pathToJson 