﻿using FleetFlow.DAL.IRepositories;
using FleetFlow.Domain.Entities;
using FleetFlow.Service.DTOs.Attachments;
using FleetFlow.Service.Interfaces;
using FleetFlow.Shared.Helpers;
using Path = System.IO.Path;

namespace FleetFlow.Service.Services;

public class AttachmentService : IAttachmentService
{
    private readonly IRepository<Attachment> attachmentRepository;
    public AttachmentService(IRepository<Attachment> attachmentRepository)
    {
        this.attachmentRepository = attachmentRepository;
    }

    public ValueTask<bool> DeleteAsync(long id)
    {
        throw new NotImplementedException();
    }

    public async ValueTask<Attachment> UploadAsync(AttachmentCreationDto dto)
    {
        string path = EnvironmentHelper.WebRootPath;
        string fullPath = Path.Combine(path, "files", $"{dto.FileName}{dto.FIleExtension}");
        FileStream targetFile = new FileStream(fullPath, FileMode.OpenOrCreate);
        await targetFile.WriteAsync(dto.File);

        Attachment attachment = new Attachment
        {
            FileName = dto.FileName + dto.FIleExtension,
            FilePath = fullPath,
            CreatedAt = DateTime.UtcNow,
        };
        return await this.attachmentRepository.InsertAsync(attachment);
    }
}
