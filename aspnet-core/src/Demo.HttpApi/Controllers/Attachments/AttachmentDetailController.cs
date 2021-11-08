using Demo.Shared;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Demo.Attachments;
using static Demo.Controllers.Attachments.AttachmentController;
using System.Collections.Generic;
using System.IO;

namespace Demo.Controllers.Attachments
{
    [RemoteService]
    [Area("app")]
    [ControllerName("AttachmentDetail")]
    [Route("api/app/attachment-details")]

    public partial class AttachmentDetailController : AbpController//, IAttachmentDetailsAppService
    {
        private readonly IAttachmentDetailsAppService _attachmentDetailsAppService;

        public AttachmentDetailController(IAttachmentDetailsAppService attachmentDetailsAppService)
        {
            _attachmentDetailsAppService = attachmentDetailsAppService;
        }

        [HttpGet]
        public Task<PagedResultDto<AttachmentDetailWithNavigationPropertiesDto>> GetListAsync(GetAttachmentDetailsInput input)
        {
            return _attachmentDetailsAppService.GetListAsync(input);
        }

        //[HttpGet]
        //[Route("with-navigation-properties/{id}")]
        //public Task<AttachmentDetailWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
        //{
        //    return _attachmentDetailsAppService.GetWithNavigationPropertiesAsync(id);
        //}

       

        //[HttpGet]
        //[Route("attachment-lookup")]
        //public Task<PagedResultDto<LookupDto<Guid>>> GetAttachmentLookupAsync(LookupRequestDto input)
        //{
        //    return _attachmentDetailsAppService.GetAttachmentLookupAsync(input);
        //}

        

        //[HttpPut]
        //[Route("{id}")]
        //public virtual Task<AttachmentDetailDto> UpdateAsync(Guid id, AttachmentDetailUpdateDto input)
        //{
        //    return _attachmentDetailsAppService.UpdateAsync(id, input);
        //}

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _attachmentDetailsAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<AttachmentDetailDto> GetAsync(Guid id)
        {
            return _attachmentDetailsAppService.GetAsync(id);
        }

        [HttpGet]
        [Route("download/{id}")]
        public virtual async Task<FileContentResult> DownloadAsync(Guid id)
        {
            var result = await _attachmentDetailsAppService.DownloadAsync(id);
            using (var memoryStream = new MemoryStream())
            {
                result.Data.CopyTo(memoryStream);
                var res = new FileContentResult(memoryStream.ToArray(), result.Extension);
                return res;
            }
        }

        [HttpPost]
        //public async Task<AttachmentDetailDto[]> CreateFile(FileUploadInputDto fileObj)
        //Microsoft.AspNetCore.Http.IFormFileCollection Files { get; set; }
        //Guid AttachmentId
        public async Task<AttachmentDetailDto[]> CreateFile(Microsoft.AspNetCore.Http.IFormFileCollection Files , [FromQuery] Guid? AttachmentId)
        {
            List< AttachmentDetailCreateDto> input = new List<AttachmentDetailCreateDto>();
            foreach (var file in Files)
            {
                using (var ms = new System.IO.MemoryStream())
                {
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    var newFile = new AttachmentDetailCreateDto();
                    newFile.FileBytes = fileBytes;
                    newFile.FileSize = file.Length;
                    newFile.Name = file.FileName;
                    newFile.Extension = file.ContentType;
                    newFile.AttachmentId = AttachmentId ?? Guid.Empty;
                    input.Add(newFile);
                }
            }
            var result = await _attachmentDetailsAppService.CreateAsync(input.ToArray());
            return result;
        }

    }
}