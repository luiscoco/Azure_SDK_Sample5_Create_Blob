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
string resourceId = "/subscriptions/846901e6-da09-45c8-98ca-7cca2353ff0e/resourcegroups/myNewRgName/providers/Microsoft.Storage/storageAccounts/mynewstorageaccount1974";
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
string StorageAccountName = "mynewstorageaccount1974"; //IMPORNTANT: in the storage account name we can only include lowercase letters and numbers

ResourceGroupResource resourceGroup =
    armClient.GetDefaultSubscription().GetResourceGroup(resourceGroupName);
StorageAccountCollection accountCollection3 = resourceGroup.GetStorageAccounts();
StorageAccountResource storageAccount3 = await accountCollection3.GetAsync(StorageAccountName);

//Get a blob instance for accessing the blob services
BlobServiceResource blobService = storageAccount.GetBlobService();

//Create a blob container
BlobContainerCollection blobContainerCollection1 = blobService.GetBlobContainers();
string blobContainerName = "myblobcontainer"; //IMPORNTANT: in the blobcontainer name we can only include lowercase letters and numbers
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

//Get a blob container
BlobContainerCollection blobContainerCollection3 = blobService.GetBlobContainers();
BlobContainerResource blobContainer3 = await blobContainerCollection3.GetAsync("myblobcontainer");
Console.WriteLine(blobContainer3.Id.Name);

//Delete a blob container
BlobContainerCollection blobContainerCollection4 = blobService.GetBlobContainers();
BlobContainerResource blobContainer4 = await blobContainerCollection4.GetAsync("myblobcontainer");
await blobContainer4.DeleteAsync(WaitUntil.Completed);

