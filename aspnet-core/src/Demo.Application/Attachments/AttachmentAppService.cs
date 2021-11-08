using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Demo.Permissions;
using Demo.Attachments;

namespace Demo.Attachments
{
    [RemoteService(IsEnabled = false)]
    [Authorize(DemoPermissions.Attachments.Default)]
    public class AttachmentsAppService : ApplicationService, IAttachmentsAppService
    {
        private readonly IAttachmentRepository _attachmentRepository;

        public AttachmentsAppService(IAttachmentRepository attachmentRepository)
        {
            _attachmentRepository = attachmentRepository;
        }

        public virtual async Task<PagedResultDto<AttachmentDto>> GetListAsync(GetAttachmentsInput input)
        {
            var totalCount = await _attachmentRepository.GetCountAsync(input.FilterText, input.FilesCountMin, input.FilesCountMax, input.FilesSizeMin, input.FilesSizeMax, input.Name);
            var items = await _attachmentRepository.GetListAsync(input.FilterText, input.FilesCountMin, input.FilesCountMax, input.FilesSizeMin, input.FilesSizeMax, input.Name, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<AttachmentDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Attachment>, List<AttachmentDto>>(items)
            };
        }

        public virtual async Task<AttachmentDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<Attachment, AttachmentDto>(await _attachmentRepository.GetAsync(id));
        }

        [Authorize(DemoPermissions.Attachments.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _attachmentRepository.DeleteAsync(id);
        }

        [Authorize(DemoPermissions.Attachments.Create)]
        public virtual async Task<AttachmentDto> CreateAsync(AttachmentCreateDto input)
        {

            var attachment = ObjectMapper.Map<AttachmentCreateDto, Attachment>(input);

            attachment = await _attachmentRepository.InsertAsync(attachment, autoSave: true);
            return ObjectMapper.Map<Attachment, AttachmentDto>(attachment);
        }

        [Authorize(DemoPermissions.Attachments.Edit)]
        public virtual async Task<AttachmentDto> UpdateAsync(Guid id, AttachmentUpdateDto input)
        {

            var attachment = await _attachmentRepository.GetAsync(id);
            ObjectMapper.Map(input, attachment);
            attachment = await _attachmentRepository.UpdateAsync(attachment, autoSave: true);
            return ObjectMapper.Map<Attachment, AttachmentDto>(attachment);
        }
    }
}