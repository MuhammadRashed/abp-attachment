using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Demo.Attachments
{
    public interface IAttachmentsAppService : IApplicationService
    {
        Task<PagedResultDto<AttachmentDto>> GetListAsync(GetAttachmentsInput input);

        Task<AttachmentDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<AttachmentDto> CreateAsync(AttachmentCreateDto input);

        Task<AttachmentDto> UpdateAsync(Guid id, AttachmentUpdateDto input);
    }
}