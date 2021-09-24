using System;
using System.IO;
using System.Threading.Tasks;
using Grpc.Core;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using ProjectX.DataAccess.Context.Base;
using ProjectX.DataAccess.Models.Files;
using ProjectX.DataAccess.Repositories.Base;
using ProjectX.Protobuf.Protos.Models;

namespace ProjectX.BusinessLayer.Services.Files
{
    public class GridFsFileService
    {
        private readonly IMongoContext _context;
        private readonly IRepository<FileRecord> _fileRecordRepository;
        private readonly IGridFSBucket _gridFsBucket;

        public GridFsFileService(IMongoContext context, IRepository<FileRecord> fileRecordRepository)
        {
            _context = context;
            _fileRecordRepository = fileRecordRepository;
            _gridFsBucket = new GridFSBucket(_context.Database);
        }

        public async Task UploadFileAsStream(FileMetadata metadata, Stream stream)
        {
            var id = await _gridFsBucket.UploadFromStreamAsync($"{metadata.Name}.{metadata.Type}", stream);

            var fileRecord = new FileRecord
            {
                GridFsFileId = id,
                Name = metadata.Name,
                Extension = metadata.Type,
            };
                
            await _fileRecordRepository.Insert(fileRecord);
        }

        public async Task<UploadStatusCode> UploadFile(FileMetadata metadata, byte[] file)
        {
            try
            {
                await using var fs = new MemoryStream(file);

                var id = await _gridFsBucket.UploadFromStreamAsync(metadata.Name, fs);

                var fileRecord = new FileRecord
                {
                    GridFsFileId = id,
                    Name = metadata.Name,
                    Extension = metadata.Type,
                    SizeInKb = file.Length / 1024
                };
                
                await _fileRecordRepository.Insert(fileRecord);
            }
            catch (Exception)
            {
                return UploadStatusCode.Failed;
            }

            return UploadStatusCode.Ok;
        }
    }
}