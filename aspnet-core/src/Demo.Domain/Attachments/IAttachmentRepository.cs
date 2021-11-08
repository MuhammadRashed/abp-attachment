using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Demo.Attachments
{
    public interface IAttachmentRepository : IRepository<Attachment, Guid>
    {
        Task<List<Attachment>> GetListAsync(
            string filterText = null,
            int? filesCountMin = null,
            int? filesCountMax = null,
            long? filesSizeMin = null,
            long? filesSizeMax = null,
            string name = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<long> GetCountAsync(
            string filterText = null,
            int? filesCountMin = null,
            int? filesCountMax = null,
            long? filesSizeMin = null,
            long? filesSizeMax = null,
            string name = null,
            CancellationToken cancellationToken = default);
    }
}