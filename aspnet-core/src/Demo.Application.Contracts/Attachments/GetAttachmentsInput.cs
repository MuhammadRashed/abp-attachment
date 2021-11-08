using Volo.Abp.Application.Dtos;
using System;

namespace Demo.Attachments
{
    public class GetAttachmentsInput : PagedAndSortedResultRequestDto
    {
        public string FilterText { get; set; }

        public int? FilesCountMin { get; set; }
        public int? FilesCountMax { get; set; }
        public long? FilesSizeMin { get; set; }
        public long? FilesSizeMax { get; set; }
        public string Name { get; set; }

        public GetAttachmentsInput()
        {

        }
    }
}