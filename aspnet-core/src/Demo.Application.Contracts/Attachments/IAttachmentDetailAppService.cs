using Demo.Shared;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Demo.Attachments
{
    public interface IAttachmentDetailsAppService : IApplicationService
    {
        Task<PagedResultDto<AttachmentDetailWithNavigationPropertiesDto>> GetListAsync(GetAttachmentDetailsInput input);

        Task<AttachmentDetailWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id);

        Task<AttachmentDetailDto> GetAsync(Guid id);

        //Task<PagedResultDto<LookupDto<Guid>>> GetAttachmentLookupAsync(LookupRequestDto input);

        Task DeleteAsync(Guid id);

        Task<AttachmentDetailDto[]> CreateAsync(AttachmentDetailCreateDto[] input);

        Task<AttachmentDetailDto> UpdateAsync(Guid id, AttachmentDetailUpdateDto input);
        Task<AttachmentDetailDownloadDto> DownloadAsync(Guid id);
    }
}