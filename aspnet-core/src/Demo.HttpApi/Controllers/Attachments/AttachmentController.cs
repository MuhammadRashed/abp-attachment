using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Demo.Attachments;
using System.Collections.Generic;

namespace Demo.Controllers.Attachments
{
    [RemoteService]
    [Area("app")]
    [ControllerName("Attachment")]
    [Route("api/app/attachments")]

    public class AttachmentController : AbpController, IAttachmentsAppService
    {
        private readonly IAttachmentsAppService _attachmentsAppService;

        public AttachmentController(IAttachmentsAppService attachmentsAppService)
        {
            _attachmentsAppService = attachmentsAppService;
        }

        [HttpGet]
        public virtual Task<PagedResultDto<AttachmentDto>> GetListAsync(GetAttachmentsInput input)
        {
            return _attachmentsAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<AttachmentDto> GetAsync(Guid id)
        {
            return _attachmentsAppService.GetAsync(id);
        }

        [HttpPost]
        public virtual Task<AttachmentDto> CreateAsync(AttachmentCreateDto input)
        {
            return _attachmentsAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<AttachmentDto> UpdateAsync(Guid id, AttachmentUpdateDto input)
        {
            return _attachmentsAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _attachmentsAppService.DeleteAsync(id);
        }

    

       
    }
}