using System;
using Volo.Abp.Application.Dtos;

namespace Demo.Attachments
{
    public class AttachmentDto : FullAuditedEntityDto<Guid>
    {
        public int FilesCount { get; set; }
        public long FilesSize { get; set; }
        public string Name { get; set; }
    }
}