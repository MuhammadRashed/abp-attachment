using Demo.Shared;
using Demo.Attachments;
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
using Volo.Abp.BlobStoring;

namespace Demo.Attachments
{
    [RemoteService(IsEnabled = false)]
    [Authorize(DemoPermissions.AttachmentDetails.Default)]
    public class AttachmentDetailsAppService : ApplicationService, IAttachmentDetailsAppService
    {
        private readonly IAttachmentDetailRepository _attachmentDetailRepository;
        private readonly IRepository<Attachment, Guid> _attachmentRepository;
        private readonly IBlobContainerFactory blobContainerFactory;

        public AttachmentDetailsAppService(IAttachmentDetailRepository attachmentDetailRepository, IRepository<Attachment, Guid> attachmentRepository , IBlobContainerFactory blobContainerFactory )
        {
            _attachmentDetailRepository = attachmentDetailRepository; _attachmentRepository = attachmentRepository;
            this.blobContainerFactory = blobContainerFactory;
        }

        public virtual async Task<PagedResultDto<AttachmentDetailWithNavigationPropertiesDto>> GetListAsync(GetAttachmentDetailsInput input)
        {
            var totalCount = await _attachmentDetailRepository.GetCountAsync(input.FilterText, input.Name, input.FileSizeMin, input.FileSizeMax, input.Extension, input.AttachmentId);
            var items = await _attachmentDetailRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.Name, input.FileSizeMin, input.FileSizeMax, input.Extension, input.AttachmentId, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<AttachmentDetailWithNavigationPropertiesDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<AttachmentDetailWithNavigationProperties>, List<AttachmentDetailWithNavigationPropertiesDto>>(items)
            };
        }

        public virtual async Task<AttachmentDetailWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
        {
            return ObjectMapper.Map<AttachmentDetailWithNavigationProperties, AttachmentDetailWithNavigationPropertiesDto>
                (await _attachmentDetailRepository.GetWithNavigationPropertiesAsync(id));
        }

        public virtual async Task<AttachmentDetailDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<AttachmentDetail, AttachmentDetailDto>(await _attachmentDetailRepository.GetAsync(id));
        }

        public virtual async Task<AttachmentDetailDownloadDto> DownloadAsync(Guid id)
        {
            var data = await _attachmentDetailRepository.GetWithNavigationPropertiesAsync(id);
            var blobContainer = blobContainerFactory.Create(data.Attachment.Name);
            var dataStream = await blobContainer.GetAsync(data.AttachmentDetail.Id.ToString());
            return new AttachmentDetailDownloadDto()
            {
                Name = data.AttachmentDetail.Name,
                Extension = data.AttachmentDetail.Extension,
                Data = dataStream
            };
        }

        [Authorize(DemoPermissions.AttachmentDetails.Create)]
        public virtual async Task<AttachmentDetailDto[]> CreateAsync(AttachmentDetailCreateDto[] inputs)
        {
            List<AttachmentDetailDto> result = new List<AttachmentDetailDto>();
            IBlobContainer blobContainer = null; 
            foreach (var input in inputs)
            {
                var attachmentDetail = ObjectMapper.Map<AttachmentDetailCreateDto, AttachmentDetail>(input);
                attachmentDetail = await _attachmentDetailRepository.InsertAsync(attachmentDetail, autoSave: true);

                if (blobContainer == null )
                {
                    var attachment = await _attachmentRepository.GetAsync(inputs[0].AttachmentId);
                    blobContainer = blobContainerFactory.Create(attachment.Name);
                }
                await blobContainer.SaveAsync(attachmentDetail.Id.ToString(), input.FileBytes);
                result.Add( ObjectMapper.Map<AttachmentDetail,AttachmentDetailDto>(attachmentDetail));
            }
            return result.ToArray();
        }

        //public virtual async Task<PagedResultDto<LookupDto<Guid>>> GetAttachmentLookupAsync(LookupRequestDto input)
        //{
        //    var query = _attachmentRepository.AsQueryable()
        //        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
        //            x => x.Name != null &&
        //                 x.Name.Contains(input.Filter));

        //    var lookupData = await query.PageBy(input.SkipCount, input.MaxResultCount).ToDynamicListAsync<Attachment>();
        //    var totalCount = query.Count();
        //    return new PagedResultDto<LookupDto<Guid>>
        //    {
        //        TotalCount = totalCount,
        //        Items = ObjectMapper.Map<List<Attachment>, List<LookupDto<Guid>>>(lookupData)
        //    };
        //}

        [Authorize(DemoPermissions.AttachmentDetails.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _attachmentDetailRepository.DeleteAsync(id);
        }

      

        [Authorize(DemoPermissions.AttachmentDetails.Edit)]
        public virtual async Task<AttachmentDetailDto> UpdateAsync(Guid id, AttachmentDetailUpdateDto input)
        {
            if (input.AttachmentId == default)
            {
                throw new UserFriendlyException(L["The {0} field is required.", L["Attachment"]]);
            }

            var attachmentDetail = await _attachmentDetailRepository.GetAsync(id);
            ObjectMapper.Map(input, attachmentDetail);
            attachmentDetail = await _attachmentDetailRepository.UpdateAsync(attachmentDetail, autoSave: true);
            return ObjectMapper.Map<AttachmentDetail, AttachmentDetailDto>(attachmentDetail);
        }

    }
}