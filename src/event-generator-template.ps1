# Read in the Json file
$pathToJson = "..\azure\event-generator-template.json"
$a = Get-Content $pathToJson | ConvertFrom-Json
# Set the default values of the various parameters inside the Logic app to the Build parameter functions that set them.
$a.resources[0].properties.definition.parameters.SourceEndpoint.defaultValue = "[variables('dataPathUri')]"
$a.resources[0].properties.definition.parameters.SourceOAUTHEndpoint.defaultValue = "[variables('loginUri')]"
$a.resources[0].properties.definition.parameters.SourceOAUTHId.defaultValue = "[parameters('kycloudApiEmail')]"
$a.resources[0].properties.definition.parameters.SourceOAUTHKeyUri.defaultValue = "[concat(reference(variables('keyVaultSecretKycloudApiToken'), '2019-09-01').secretUriWithVersion, '?api-version=2016-10-01')]"
$a.resources[0].properties.definition.parameters.SourceOAUTHSecretUri.defaultValue = "[concat(reference(variables('keyVaultSecretKycloudApiPassword'), '2019-09-01').secretUriWithVersion, '?api-version=2016-10-01')]"
$a.resources[0].properties.definition.parameters.TargetOAUTHCredentialsUri.defaultValue = "[concat(reference(variables('keyVaultSecretCdcEventsTokenRequestPayload'), '2019-09-01').secretUriWithVersion, '?api-version=2016-10-01')]"
$a.resources[0].properties.definition.parameters.TargetOAUTHKeyUri.defaultValue = "[concat(reference(variables('keyVaultSecretCdcEventsSubscriptionKey'), '2019-09-01').secretUriWithVersion, '?api-version=2016-10-01')]"
$a.resources[0].properties.definition.parameters.TargetEndpoint.defaultValue = "[concat(parameters('cdcEventsApiBaseUri'), '/')]"
$a.resources[0].properties.definition.parameters.TargetControlEndpoint.defaultValue = "[concat(parameters('cdcEventsApiBaseUri'), '/load')]"
$a.resources[0].properties.definition.parameters.TargetOAUTHEndpoint.defaultValue = "[parameters('internalOAuthTokenEndpoint')]"
$a.resources[0].properties.definition.parameters.CDC2NotifyAPIKeySecretUri.defaultValue  = "[resourceId(variables('sharedResourceGroupName'), 'Microsoft.KeyVault/vaults/secrets', variables('keyVaultName'), parameters('keyVaultSecretNameCDC2NotifyAPIKey'))]"

#
# PLEASE NOTE: the values of the Status* Properties are dependent on the Dfe.CdcEventApi.Domain.Models.ControlState Enum values
#
$a.resources[0].properties.definition.parameters.StatusStart.defaultValue = 1
$a.resources[0].properties.definition.parameters.StatusEntities.defaultValue = 3
$a.resources[0].properties.definition.parameters.StatusExtracting.defaultValue = 4
$a.resources[0].properties.definition.parameters.StatusAttachments.defaultValue = 5
$a.resources[0].properties.definition.parameters.StatusTransforming.defaultValue = 6
$a.resources[0].properties.definition.parameters.StatusReporting.defaultValue = 100
# Finally write it back to the same file
$a | ConvertTo-Json -Compress -Depth 32 | % { [System.Text.RegularExpressions.Regex]::Unescape($_) }  | set-content $pathToJson 