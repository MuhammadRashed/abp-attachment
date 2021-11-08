using Demo.Attachments;

using System;
using Volo.Abp.Application.Dtos;

namespace Demo.Attachments
{
    public class AttachmentDetailWithNavigationPropertiesDto
    {
        public AttachmentDetailDto AttachmentDetail { get; set; }

        public AttachmentDto Attachment { get; set; }

    }
}