using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Demo.Attachments
{
    public class AttachmentDetailDownloadDto
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        public Stream Data { get; set; }
    }
}
