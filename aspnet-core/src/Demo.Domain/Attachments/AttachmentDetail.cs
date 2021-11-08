using Demo.Attachments;
using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;
using Volo.Abp;

namespace Demo.Attachments
{
    public class AttachmentDetail : FullAuditedEntity<Guid>
    {
        [NotNull]
        public virtual string Name { get; set; }

        public virtual long FileSize { get; set; }

        [NotNull]
        public virtual string Extension { get; set; }
        public Guid AttachmentId { get; set; }

        public AttachmentDetail()
        {

        }

        public AttachmentDetail(Guid id, string name, long fileSize, string extension)
        {
            Id = id;
            Check.NotNull(name, nameof(name));
            Check.NotNull(extension, nameof(extension));
            Name = name;
            FileSize = fileSize;
            Extension = extension;
        }
    }
}