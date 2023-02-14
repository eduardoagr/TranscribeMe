using Azure.Storage;
using Azure.Storage.Blobs;

namespace TranscribeMe.Services {
    public class AzureStorageService {

        readonly string ConectionString = ConstantsHelpers.AZURE_STORAGE_CONNECTIONSTRING;
        BlobContainerClient? ContainerClient;

        public async Task<Uri> UploadToAzureBlobStorageWithssaToken(string FilePath) {

            ContainerClient = new BlobContainerClient(ConectionString,
                ConstantsHelpers.AZURE_CONTAINER_ORIGINAL_DOCUMENT);

            Azure.Storage.Sas.BlobSasBuilder blobSasBuilder = new() {
                BlobContainerName = ConstantsHelpers.AZURE_CONTAINER_ORIGINAL_DOCUMENT,
                ExpiresOn = DateTime.MaxValue,//Let SAS token expire never.
            };

            blobSasBuilder.SetPermissions
                (Azure.Storage.Sas.BlobSasPermissions.Read | Azure.Storage.Sas.BlobSasPermissions.List);
            var sasToken = blobSasBuilder.ToSasQueryParameters
                (new StorageSharedKeyCredential
                (ConstantsHelpers.AZUTE_STORAGE_ACCOUNT_NAME,
                ConstantsHelpers.AZUTE_STORAGE_ACCOUNT_KEY)).ToString();

            var blob = ContainerClient.GetBlobClient(Path.GetFileName(FilePath));
            await blob.UploadAsync(FilePath, true);

            return new Uri($"{ConstantsHelpers.AZURE_UPLOAD_DOUMMENRTS}?{sasToken}");

        }

        public Task<Uri> GetTargetUri() {

            ContainerClient = new BlobContainerClient(ConectionString,
                ConstantsHelpers.AZURE_CONTAINER_TRANSLATED_DOCUMENT);

            Azure.Storage.Sas.BlobSasBuilder blobSasBuilder = new() {
                BlobContainerName = ConstantsHelpers.AZURE_CONTAINER_TRANSLATED_DOCUMENT,
                ExpiresOn = DateTime.MaxValue,//Let SAS token expire never.
            };

            blobSasBuilder.SetPermissions(Azure.Storage.Sas.BlobSasPermissions.Write | Azure.Storage.Sas.BlobSasPermissions.List);
            var sasToken = blobSasBuilder.ToSasQueryParameters(
                new StorageSharedKeyCredential(ConstantsHelpers.AZUTE_STORAGE_ACCOUNT_NAME,
                ConstantsHelpers.AZUTE_STORAGE_ACCOUNT_KEY)).ToString();

            return Task.FromResult(new Uri($"{ConstantsHelpers.AZURE_DOWNLOAD_DOCUMENTS}?{sasToken}"));
        }

        public async Task DeteleFromBlobAsync(string filePath, string containerName) {

            ContainerClient = new BlobContainerClient(ConectionString, containerName);

            var blob = ContainerClient.GetBlobClient(Path.GetFileName(filePath));
            await blob.DeleteIfExistsAsync();

        }

        public async Task<string> DownloadFileFromBlobAsync(string pathToSave, string FilePath) {

            ContainerClient = new BlobContainerClient(ConectionString,
                ConstantsHelpers.AZURE_CONTAINER_TRANSLATED_DOCUMENT);

            var blob = ContainerClient.GetBlobClient(Path.GetFileName(FilePath));
            await blob.DownloadToAsync(pathToSave);
            return pathToSave;
        }


        public async Task<string> UploadToAzureBlobStorage(string FilePath, string id) {

            ContainerClient = new BlobContainerClient(ConectionString,
                ConstantsHelpers.AZURE_CONTAINER_USERSROOT);

            var blob = ContainerClient.GetBlobClient($"{id}/{Path.GetFileName(FilePath)}");
            await blob.UploadAsync(FilePath, true);

            return blob.Uri.AbsoluteUri;

        }
    }
}

