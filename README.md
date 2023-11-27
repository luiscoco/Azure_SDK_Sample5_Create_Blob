# Azure SDK Sample6 Create Blob

## 1. We first create an Azure Storage Account

![image](https://github.com/luiscoco/Azure_SDK_Sample5_Create_Blob/assets/32194879/2cde42e2-6ca9-44b2-89ab-a4c7826a1af6)

![image](https://github.com/luiscoco/Azure_SDK_Sample5_Create_Blob/assets/32194879/9e94d4de-9ca4-40e7-be09-d8b462669916)

![image](https://github.com/luiscoco/Azure_SDK_Sample5_Create_Blob/assets/32194879/0e7d341a-6b1a-4486-ad57-0d620707cf9a)

![image](https://github.com/luiscoco/Azure_SDK_Sample5_Create_Blob/assets/32194879/4c413071-8c8a-4c2e-882a-0cbfc00daea5)

![image](https://github.com/luiscoco/Azure_SDK_Sample5_Create_Blob/assets/32194879/617da66e-e4f5-49b0-a053-e607591660d4)

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
