using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;
using Volo.Abp;

namespace Demo.Attachments
{
    public class Attachment : FullAuditedAggregateRoot<Guid>
    {
        public virtual int FilesCount { get; set; }

        public virtual long FilesSize { get; set; }

        [CanBeNull]
        public virtual string Name { get; set; }

        public Attachment()
        {

        }

        public Attachment(Guid id, int filesCount, long filesSize, string name)
        {
            Id = id;
            FilesCount = filesCount;
            FilesSize = filesSize;
            Name = name;
        }
    }
}