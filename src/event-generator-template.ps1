# Read in the Json file
$pathToJson = "..\azure\event-generator-template.json"
$a = Get-Content $pathToJson | ConvertFrom-Json
# Set the default values of the various parameters inside the Logic app to the Build parameter functions that set them.
$a.resources[0].properties.definition.parameters.Environment.defaultValue  = "[concat(parameters('environmentName'), '-', parameters('environmentInstance'))]"
$a.resources[0].properties.definition.parameters.SourceEndpoint.defaultValue = "[variables('dataPathUri')]"
$a.resources[0].properties.definition.parameters.SourceOAUTHEndpoint.defaultValue = "[variables('loginUri')]"
$a.resources[0].properties.definition.parameters.SourceOAUTHId.defaultValue = "[parameters('kycloudApiEmail')]"
$a.resources[0].properties.definition.parameters.SourceOAUTHKeyUri.defaultValue = "[concat(reference(variables('keyVaultSecretKycloudApiToken'), '2019-09-01').secretUriWithVersion, '?api-version=2016-10-01')]"
$a.resources[0].properties.definition.parameters.SourceOAUTHSecretUri.defaultValue = "[concat(reference(variables('keyVaultSecretKycloudApiPassword'), '2019-09-01').secretUriWithVersion, '?api-version=2016-10-01')]"
$a.resources[0].properties.definition.parameters.TargetOAUTHCredentialsUri.defaultValue = "[concat(reference(variables('keyVaultSecretCdcEventsTokenRequestPayload'), '2019-09-01').secretUriWithVersion, '?api-version=2016-10-01')]"
$a.resources[0].properties.definition.parameters.TargetOAUTHKeyUri.defaultValue = "[concat(reference(variables('keyVaultSecretCdcEventsSubscriptionKey'), '2019-09-01').secretUriWithVersion, '?api-version=2016-10-01')]"
$a.resources[0].properties.definition.parameters.TargetEndpoint.defaultValue = "[concat(parameters('cdcEventsApiBaseUri'), '/')]"
$a.resources[0].properties.definition.parameters.TargetControlEndpoint.defaultValue = "[concat(parameters('cdcEventsApiBaseUri'), '/control')]"
$a.resources[0].properties.definition.parameters.TargetOAUTHEndpoint.defaultValue = "[parameters('internalOAuthTokenEndpoint')]"
$a.resources[0].properties.definition.parameters.CDC2NotifyAPIKeySecretUri.defaultValue  = "[concat(reference(variables('keyVaultSecretCDC2NotifyAPIKey'), '2019-09-01').secretUriWithVersion, '?api-version=2016-10-01')]"
#Log Analytics Connection
$a.resources[0].properties.parameters.'$connections'.value.azureloganalyticsdatacollector.connectionId = "[concat('/subscriptions/', parameters('subscriptionId'), '/resourceGroups/', parameters('resourceGroupPrefix'), '-cdcaeg/providers/Microsoft.Web/connections/', parameters('resourceGroupPrefix'), '-logapi-01')]"
$a.resources[0].properties.parameters.'$connections'.value.azureloganalyticsdatacollector.connectionName = "[concat(parameters('resourceGroupPrefix'), '-logapi-01')]"
$a.resources[0].properties.parameters.'$connections'.value.azureloganalyticsdatacollector.id = "[concat('/subscriptions/', parameters('subscriptionId'), '/providers/Microsoft.Web/locations/westeurope/managedApis/azureloganalyticsdatacollector')]"
#File Storage Connection
$a.resources[0].properties.parameters.'$connections'.value.azurefile.connectionId = "[concat('/subscriptions/', parameters('subscriptionId'), '/resourceGroups/', parameters('resourceGroupPrefix'), '-cdcaeg/providers/Microsoft.Web/connections/', parameters('resourceGroupPrefix'), '-fileapi-01')]"
$a.resources[0].properties.parameters.'$connections'.value.azurefile.connectionName = "[concat(parameters('resourceGroupPrefix'), '-fileapi-01')]"
$a.resources[0].properties.parameters.'$connections'.value.azurefile.id = "[concat('/subscriptions/', parameters('subscriptionId'), '/providers/Microsoft.Web/locations/westeurope/managedApis/azurefile')]"

# The body template for sending log analytics has to be escaped here otherwise it all goes wrong when the json is unescaped in the last step
# Needs to be replicated if further log analytics steps are added
$logFailedAttachmentBodyTemplate = $a.resources[0].properties.definition.actions.'Scope-Attachments'.actions.'For-Each-Attachment'.actions.'Validate-Post-Attachment'.else.actions.'Log-Failed-Attachment'.inputs.body
$logFailedAttachmentBodyTemplate = $logFailedAttachmentBodyTemplate.Replace('"', '\"').Replace("`n","").Replace("`r","")
$a.resources[0].properties.definition.actions.'Scope-Attachments'.actions.'For-Each-Attachment'.actions.'Validate-Post-Attachment'.else.actions.'Log-Failed-Attachment'.inputs.body = $logFailedAttachmentBodyTemplate

$logGetEntityPageBodyTemplate = $a.resources[0].properties.definition.actions.'Scope-Entities'.actions.'For-Each-Entity'.actions.'Until-Control-Entity-Completed'.actions.'For-Each-Page'.actions.'Log-Get-Entity-Page'.inputs.body
$logGetEntityPageBodyTemplate = $logGetEntityPageBodyTemplate.Replace('"', '\"').Replace("`n","").Replace("`r","")
$a.resources[0].properties.definition.actions.'Scope-Entities'.actions.'For-Each-Entity'.actions.'Until-Control-Entity-Completed'.actions.'For-Each-Page'.actions.'Log-Get-Entity-Page'.inputs.body = $logGetEntityPageBodyTemplate

$logEntityPageRecordCountBodyTemplate = $a.resources[0].properties.definition.actions.'Scope-Entities'.actions.'For-Each-Entity'.actions.'Until-Control-Entity-Completed'.actions.'For-Each-Page'.actions.'Validate-Get-Entity-Page'.actions.'Log-Entity-Page-Record-Count'.inputs.body
$logEntityPageRecordCountBodyTemplate = $logEntityPageRecordCountBodyTemplate.Replace('"', '\"').Replace("`n","").Replace("`r","")
$a.resources[0].properties.definition.actions.'Scope-Entities'.actions.'For-Each-Entity'.actions.'Until-Control-Entity-Completed'.actions.'For-Each-Page'.actions.'Validate-Get-Entity-Page'.actions.'Log-Entity-Page-Record-Count'.inputs.body = $logEntityPageRecordCountBodyTemplate

#
# PLEASE NOTE: the values of the Status* Properties are dependent on the Dfe.CdcEventApi.Domain.Models.ControlState Enum values
#
$a.resources[0].properties.definition.parameters.StatusStart.defaultValue = 0
$a.resources[0].properties.definition.parameters.StatusEntities.defaultValue = 1
$a.resources[0].properties.definition.parameters.StatusAttachments.defaultValue = 2
$a.resources[0].properties.definition.parameters.StatusReporting.defaultValue = 3
# Finally write it back to the same file
$a | ConvertTo-Json -Compress -Depth 32 | % { [System.Text.RegularExpressions.Regex]::Unescape($_) }  | set-content $pathToJson
#
# The template is now compressed to a single line with all hard coded variables set to ARM template parameter and variable selectors.
#