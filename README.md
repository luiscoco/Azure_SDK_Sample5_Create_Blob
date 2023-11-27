# Azure SDK Sample6 Create Blob

## 1. We first create an Azure Storage Account



## 2. Create a C# console application and input the source code

```csharp
using System;
using System.Threading.Tasks;
using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Storage;
using Azure.ResourceManager.Storage.Models;

ArmClient armClient = new ArmClient(new DefaultAzureCredential());
SubscriptionResource subscription = await armClient.GetDefaultSubscriptionAsync();

//storage account id: resourceId
string resourceId = "/subscriptions/846901e6-da09-45c8-98ca-7cca2353ff0e/resourceGroups/DefaultResourceGroup-WEU/providers/Microsoft.Storage/storageAccounts/mynewstorageaccount1974";
StorageAccountResource storageAccount = armClient.GetStorageAccountResource(new ResourceIdentifier(resourceId));

//Get keys on a storage account
await foreach (StorageAccountKey key in storageAccount.GetKeysAsync())
{
    Console.WriteLine(key.Value);
}

//Get a storage account
Console.WriteLine(storageAccount.Id.Name);

//Add a tag to the storage account
string resourceGroupName = "myNewRgName";
string StorageAccountName = "mynewstorageaccount1974";

ResourceGroupResource resourceGroup =
    armClient.GetDefaultSubscription().GetResourceGroup(resourceGroupName);
StorageAccountCollection accountCollection3 = resourceGroup.GetStorageAccounts();
StorageAccountResource storageAccount3 = await accountCollection3.GetAsync(StorageAccountName);

//Get a blob instance for accessing the blob services
BlobServiceResource blobService = storageAccount.GetBlobService();

//Create a blob container
BlobContainerCollection blobContainerCollection1 = blobService.GetBlobContainers();
string blobContainerName = "myBlobContainer";
BlobContainerData blobContainerData = new BlobContainerData();
ArmOperation<BlobContainerResource> blobContainerCreateOperation = await blobContainerCollection1.CreateOrUpdateAsync(WaitUntil.Completed, blobContainerName, blobContainerData);
BlobContainerResource blobContainer1 = blobContainerCreateOperation.Value;

//List all blobs containers
BlobContainerCollection blobContainerCollection2 = blobService.GetBlobContainers();
AsyncPageable<BlobContainerResource> response = blobContainerCollection2.GetAllAsync();
await foreach (BlobContainerResource blobContainer2 in response)
{
    Console.WriteLine(blobContainer2.Id.Name);
}
```
