using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using ProjectX.BusinessLayer.Services.Files;
using ProjectX.Protobuf.Protos.Models;
using ProjectX.Protobuf.Protos.Services;

namespace ProjectX.BusinessLayer.GrpcServices
{
    [Authorize]
    public class FileUploaderService : FileUploader.FileUploaderBase
    {
        private readonly GridFsFileService _gridFsFileService;

        public FileUploaderService(GridFsFileService gridFsFileService)
        {
            _gridFsFileService = gridFsFileService;
        }
        public override async Task<UploadFileResponse> Upload(IAsyncStreamReader<UploadFileRequest> requestStream, ServerCallContext context)
        {
            var bytes = new List<byte>();
            FileMetadata metadata = null;

            await foreach (var uploadFileRequest in requestStream.ReadAllAsync())
            {
                Debug.WriteLine(uploadFileRequest);
                switch (uploadFileRequest.RequestCase)
                {
                    case UploadFileRequest.RequestOneofCase.Chunk:
                        bytes.AddRange(uploadFileRequest.Chunk.Content);
                        break;
                    case UploadFileRequest.RequestOneofCase.Metadata:
                        metadata = uploadFileRequest.Metadata;
                        break;
                }
            }

            var uploadCodeResult = await _gridFsFileService.UploadFile(metadata, bytes.ToArray());
            
            return new UploadFileResponse
            {
                Code = uploadCodeResult,
                Message =  uploadCodeResult.ToString()
            };
        }
    }
}