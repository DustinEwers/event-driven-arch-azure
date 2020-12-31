
# Change this if you want to run this for yourself.
$prefix = "dje-gamebot"

$resourceGroupName = "$prefix-event-driven-arch-demo"
$serviceBusNamespace = "$prefix-servicebus"
$serviceBusAuthRule = "readwrite"
$serviceBusQueue = "in-game-coin-purchases"
$serviceBusTopic = "game-coin-purchase-updates"
$serviceBusTopicSubscription = "$serviceBusTopic-subscription"

Write-Host "Creating Resource Group"
az group create -n $resourceGroupName -l centralus

Write-Host "Creating Service Bus"
az servicebus namespace create `
    --resource-group $resourceGroupName `
    --name $serviceBusNamespace `
    --location centralus `
    --sku Standard

az servicebus queue create `
    --resource-group $resourceGroupName `
     --namespace-name $serviceBusNamespace `
     --name $serviceBusQueue

az servicebus topic create `
    --resource-group $resourceGroupName `
     --namespace-name $serviceBusNamespace `
     --name $serviceBusTopic

az servicebus topic subscription create `
    --resource-group $resourceGroupName `
    --namespace-name $serviceBusNamespace `
    --topic-name $serviceBusTopic `
    --name $serviceBusTopicSubscription

Write-Host "Creating Service Bus Credentials"
az servicebus namespace authorization-rule create `
    --resource-group $resourceGroupName `
    --namespace-name $serviceBusNamespace `
    --name $serviceBusAuthRule `
    --rights Send Listen

$key = az servicebus namespace authorization-rule keys list `
        --resource-group $resourceGroupName `
        --namespace-name $serviceBusNamespace `
        --name $serviceBusAuthRule | ConvertFrom-Json

Write-Host "Service Bus Namespace: $($serviceBusNamespace.servicebus.windows.net)"
Write-Host "Primary Connection String: $($key.primaryConnectionString)"

# Function App Creation 
# $gitUserName = ""
# $storageName="$prefix-storage-account"
# $functionAppName="$prefix-function-app"
# gitrepo=<Replace with your GitHub repo URL e.g. https://github.com/Azure-Samples/functions-quickstart.git>
# token=<Replace with a GitHub access token>

# Write-Host "Creating a Storage Account"
# az storage account create `
#   --name $storageName `
#   --location "centralus" `
#   --resource-group $resourceGroupName `
#   --sku Standard_LRS

# Write-Host "Creating A Function App"
# az functionapp create `
#   --name $functionAppName `
#   --storage-account $storageName `
#   --consumption-plan-location "centralus" `
#   --resource-group $resourceGroupName `
#   --functions-version 2

# az functionapp config appsettings set `
#     --name $functionAppName `
#     --resource-group $resourceGroupName `
#     --settings "ServiceBusConnection=$serviceBusConnectionString"
