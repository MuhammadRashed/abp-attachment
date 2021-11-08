using System;
using Volo.Abp.Application.Dtos;

namespace Demo.Attachments
{
    public class AttachmentDetailDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public long FileSize { get; set; }
        public string Extension { get; set; }
        public Guid AttachmentId { get; set; }
    }
}