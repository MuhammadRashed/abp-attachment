using System;
using System.ComponentModel.DataAnnotations;

namespace Demo.Attachments
{
    public class AttachmentDetailCreateDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public long FileSize { get; set; }
        [Required]
        public string Extension { get; set; }
        public Guid AttachmentId { get; set; }
        public byte[] FileBytes { get; set; }
    }
}