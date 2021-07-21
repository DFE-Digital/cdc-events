$pathToJson = "..\azure\event-generator-template.json"
$a = Get-Content $pathToJson | ConvertFrom-Json
$a.resources[0].properties.definition.parameters.SourceEndpoint.defaultValue = "[variables('dataPathUri')]"
$a.resources[0].properties.definition.parameters.SourceOAUTHEndpoint.defaultValue = "[variables('loginUri')]"
$a.resources[0].properties.definition.parameters.SourceOAUTHId.defaultValue = "[parameters('kycloudApiEmail')]"
$a.resources[0].properties.definition.parameters.SourceOAUTHKeyUri.defaultValue = "[variables('keyVaultSecretKycloudApiToken')]"
$a.resources[0].properties.definition.parameters.SourceOAUTHSecretUri.defaultValue = "[variables('keyVaultSecretKycloudApiPassword')]"
$a.resources[0].properties.definition.parameters.TargetControlEndpoint.defaultValue = "[concat(variables('dataPathUri'),'/','load')]"
$a.resources[0].properties.definition.parameters.TargetEndpoint.defaultValue = "[variables('dataPathUri')]"
$a.resources[0].properties.definition.parameters.TargetOAUTHCredentialsUri.defaultValue = "[variables('keyVaultSecretCdcEventsTokenRequestPayload')]"
$a.resources[0].properties.definition.parameters.TargetOAUTHEndpoint.defaultValue = "[parameters('internalOAuthTokenEndpoint')]"
$a.resources[0].properties.definition.parameters.TargetOAUTHKeyUri.defaultValue = "[variables('keyVaultSecretCdcEventsSubscriptionKey')]"
$a | ConvertTo-Json -Compress -Depth 32 | % { [System.Text.RegularExpressions.Regex]::Unescape($_) }  | set-content $pathToJson 
Write-Output $a