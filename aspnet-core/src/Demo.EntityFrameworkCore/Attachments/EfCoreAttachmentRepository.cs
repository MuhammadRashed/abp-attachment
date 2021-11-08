using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Demo.EntityFrameworkCore;

namespace Demo.Attachments
{
    public class EfCoreAttachmentRepository : EfCoreRepository<DemoDbContext, Attachment, Guid>, IAttachmentRepository
    {
        public EfCoreAttachmentRepository(IDbContextProvider<DemoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public async Task<List<Attachment>> GetListAsync(
            string filterText = null,
            int? filesCountMin = null,
            int? filesCountMax = null,
            long? filesSizeMin = null,
            long? filesSizeMax = null,
            string name = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, filesCountMin, filesCountMax, filesSizeMin, filesSizeMax, name);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? AttachmentConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public async Task<long> GetCountAsync(
            string filterText = null,
            int? filesCountMin = null,
            int? filesCountMax = null,
            long? filesSizeMin = null,
            long? filesSizeMax = null,
            string name = null,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText, filesCountMin, filesCountMax, filesSizeMin, filesSizeMax, name);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<Attachment> ApplyFilter(
            IQueryable<Attachment> query,
            string filterText,
            int? filesCountMin = null,
            int? filesCountMax = null,
            long? filesSizeMin = null,
            long? filesSizeMax = null,
            string name = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Name.Contains(filterText))
                    .WhereIf(filesCountMin.HasValue, e => e.FilesCount >= filesCountMin.Value)
                    .WhereIf(filesCountMax.HasValue, e => e.FilesCount <= filesCountMax.Value)
                    .WhereIf(filesSizeMin.HasValue, e => e.FilesSize >= filesSizeMin.Value)
                    .WhereIf(filesSizeMax.HasValue, e => e.FilesSize <= filesSizeMax.Value)
                    .WhereIf(!string.IsNullOrWhiteSpace(name), e => e.Name.Contains(name));
        }
    }
}