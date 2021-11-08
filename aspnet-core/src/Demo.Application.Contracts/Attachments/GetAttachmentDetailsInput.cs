using Volo.Abp.Application.Dtos;
using System;

namespace Demo.Attachments
{
    public class GetAttachmentDetailsInput : PagedAndSortedResultRequestDto
    {
        public string FilterText { get; set; }

        public string Name { get; set; }
        public long? FileSizeMin { get; set; }
        public long? FileSizeMax { get; set; }
        public string Extension { get; set; }
        public Guid? AttachmentId { get; set; }

        public GetAttachmentDetailsInput()
        {

        }
    }
}