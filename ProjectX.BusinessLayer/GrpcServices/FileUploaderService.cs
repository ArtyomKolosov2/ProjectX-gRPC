using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Bson;
using ProjectX.BusinessLayer.Services;
using ProjectX.BusinessLayer.Services.Files;
using ProjectX.Protobuf.Protos.Models;
using ProjectX.Protobuf.Protos.Services;

namespace ProjectX.BusinessLayer.GrpcServices
{
    [Authorize]
    public class FileUploaderService : FileUploader.FileUploaderBase
    {
        private readonly GridFsFileService _gridFsFileService;
        private readonly BusinessUserService _businessUserService;

        public FileUploaderService(GridFsFileService gridFsFileService, BusinessUserService businessUserService)
        {
            _gridFsFileService = gridFsFileService;
            _businessUserService = businessUserService;
        }
        
        public override async Task<UploadFileResponse> Upload(IAsyncStreamReader<UploadFileRequest> requestStream, ServerCallContext context)
        {
            var bytes = new List<byte>();
            var headers = context.RequestHeaders;
            
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = headers.Single(item => item.Key == "authorization").Value;
            var enumerable = tokenHandler.ReadJwtToken(token.Replace("Bearer ", string.Empty))
                .Payload
                .Claims.Single(claim => ClaimTypes.NameIdentifier == claim.Type);

            var user = _businessUserService.FindById(ObjectId.Parse(enumerable.Value));

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