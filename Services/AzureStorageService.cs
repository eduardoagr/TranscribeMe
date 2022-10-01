﻿using Azure.Storage;
using Azure.Storage.Blobs;

using Config;

namespace TranscribeMe.Services {
    public class AzureStorageService {
        readonly string ConectionString = ConstantsHelpers.AZURE_STORAGE_CONNECTIONSTRING;
        BlobContainerClient? ContainerClient;

        public async Task<Uri> UploadToAzureBlobStorage(string FilePath) {

            ContainerClient = new BlobContainerClient(ConectionString, ConstantsHelpers.AZURE_CONTAINER_ORIGINAL_DOCUMENT);

            Azure.Storage.Sas.BlobSasBuilder blobSasBuilder = new() {
                BlobContainerName = ConstantsHelpers.AZURE_CONTAINER_ORIGINAL_DOCUMENT,
                ExpiresOn = DateTime.MaxValue,//Let SAS token expire never.
            };

            blobSasBuilder.SetPermissions(Azure.Storage.Sas.BlobSasPermissions.Read | Azure.Storage.Sas.BlobSasPermissions.List);
            var sasToken = blobSasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(ConstantsHelpers.AZUTE_STORAGE_ACCOUNT_NAME, ConstantsHelpers.AZUTE_STORAGE_ACCOUNT_KEY)).ToString();

            var blob = ContainerClient.GetBlobClient(Path.GetFileName(FilePath));
            await blob.UploadAsync(FilePath, true);

            return new Uri($"{ConstantsHelpers.AZURE_UPLOAD_DOUMMENRTS}?{sasToken}");

        }

        public async Task<Uri> SaveFromdAzureBlobStorage(string FilePath, string DownloadPath) {

            ContainerClient = new BlobContainerClient(ConectionString, ConstantsHelpers.AZURE_CONTAINER_TRANSLATED_DOCUMENT);

            Azure.Storage.Sas.BlobSasBuilder blobSasBuilder = new() {
                BlobContainerName = ConstantsHelpers.AZURE_CONTAINER_TRANSLATED_DOCUMENT,
                ExpiresOn = DateTime.MaxValue,//Let SAS token expire never.
            };

            blobSasBuilder.SetPermissions(Azure.Storage.Sas.BlobSasPermissions.Read | Azure.Storage.Sas.BlobSasPermissions.List);
            var sasToken = blobSasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(ConstantsHelpers.AZUTE_STORAGE_ACCOUNT_NAME, ConstantsHelpers.AZUTE_STORAGE_ACCOUNT_KEY)).ToString();

            var blob = ContainerClient.GetBlobClient(Path.GetFileName(FilePath));
            await blob.DownloadToAsync(DownloadPath);

            return new Uri($"{ConstantsHelpers.AZURE_DOWNLOAD_DOCUMENTS}?{sasToken}");
        }
    }
}
