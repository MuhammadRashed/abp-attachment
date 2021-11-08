using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Demo.Attachments
{
    public interface IAttachmentDetailRepository : IRepository<AttachmentDetail, Guid>
    {
        Task<AttachmentDetailWithNavigationProperties> GetWithNavigationPropertiesAsync(
    Guid id,
    CancellationToken cancellationToken = default
);

        Task<List<AttachmentDetailWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string filterText = null,
            string name = null,
            long? fileSizeMin = null,
            long? fileSizeMax = null,
            string extension = null,
            Guid? attachmentId = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<List<AttachmentDetail>> GetListAsync(
                    string filterText = null,
                    string name = null,
                    long? fileSizeMin = null,
                    long? fileSizeMax = null,
                    string extension = null,
                    string sorting = null,
                    int maxResultCount = int.MaxValue,
                    int skipCount = 0,
                    CancellationToken cancellationToken = default
                );

        Task<long> GetCountAsync(
            string filterText = null,
            string name = null,
            long? fileSizeMin = null,
            long? fileSizeMax = null,
            string extension = null,
            Guid? attachmentId = null,
            CancellationToken cancellationToken = default);
    }
}