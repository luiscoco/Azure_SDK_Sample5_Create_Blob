# Azure SDK Sample6 Create Blob

## 1. We first create an Azure Storage Account

![image](https://github.com/luiscoco/Azure_SDK_Sample5_Create_Blob/assets/32194879/734a59f3-6891-4502-a10f-4b0d8baf5b06)

![image](https://github.com/luiscoco/Azure_SDK_Sample5_Create_Blob/assets/32194879/3d66cc4c-784c-487c-810a-e260964f8931)

![image](https://github.com/luiscoco/Azure_SDK_Sample5_Create_Blob/assets/32194879/b99a1e09-23f0-4e54-a158-4c449b3f541b)

![image](https://github.com/luiscoco/Azure_SDK_Sample5_Create_Blob/assets/32194879/45010442-a7ed-4b43-8322-0fd7fbb596a0)

![image](https://github.com/luiscoco/Azure_SDK_Sample5_Create_Blob/assets/32194879/790b9c37-c840-4374-b275-636c6f850393)

![image](https://github.com/luiscoco/Azure_SDK_Sample5_Create_Blob/assets/32194879/f54851fc-2f8c-4a24-8ff9-cc98b184393f)

![image](https://github.com/luiscoco/Azure_SDK_Sample5_Create_Blob/assets/32194879/46e3ebe3-8e0b-4b12-a29a-9be0e4e0ad87)

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
