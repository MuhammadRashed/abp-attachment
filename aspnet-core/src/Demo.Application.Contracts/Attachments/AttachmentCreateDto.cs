using System;
using System.ComponentModel.DataAnnotations;

namespace Demo.Attachments
{
    public class AttachmentCreateDto
    {
        [Required]
        public int FilesCount { get; set; }
        [Required]
        public long FilesSize { get; set; }
        public string Name { get; set; }
    }
}