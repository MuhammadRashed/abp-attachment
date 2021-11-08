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
    public class EfCoreAttachmentDetailRepository : EfCoreRepository<DemoDbContext, AttachmentDetail, Guid>, IAttachmentDetailRepository
    {
        public EfCoreAttachmentDetailRepository(IDbContextProvider<DemoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public async Task<AttachmentDetailWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await (await GetQueryForNavigationPropertiesAsync())
                .FirstOrDefaultAsync(e => e.AttachmentDetail.Id == id, GetCancellationToken(cancellationToken));
        }

        public async Task<List<AttachmentDetailWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string filterText = null,
            string name = null,
            long? fileSizeMin = null,
            long? fileSizeMax = null,
            string extension = null,
            Guid? attachmentId = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, name, fileSizeMin, fileSizeMax, extension, attachmentId);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? AttachmentDetailConsts.GetDefaultSorting(true) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        protected virtual async Task<IQueryable<AttachmentDetailWithNavigationProperties>> GetQueryForNavigationPropertiesAsync()
        {
            return from attachmentDetail in (await GetDbSetAsync())
                   join attachment in (await GetDbContextAsync()).Attachments on attachmentDetail.AttachmentId equals attachment.Id into attachments
                   from attachment in attachments.DefaultIfEmpty()

                   select new AttachmentDetailWithNavigationProperties
                   {
                       AttachmentDetail = attachmentDetail,
                       Attachment = attachment
                   };
        }

        protected virtual IQueryable<AttachmentDetailWithNavigationProperties> ApplyFilter(
            IQueryable<AttachmentDetailWithNavigationProperties> query,
            string filterText,
            string name = null,
            long? fileSizeMin = null,
            long? fileSizeMax = null,
            string extension = null,
            Guid? attachmentId = null)
        {
            return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.AttachmentDetail.Name.Contains(filterText) || e.AttachmentDetail.Extension.Contains(filterText))
                    .WhereIf(!string.IsNullOrWhiteSpace(name), e => e.AttachmentDetail.Name.Contains(name))
                    .WhereIf(fileSizeMin.HasValue, e => e.AttachmentDetail.FileSize >= fileSizeMin.Value)
                    .WhereIf(fileSizeMax.HasValue, e => e.AttachmentDetail.FileSize <= fileSizeMax.Value)
                    .WhereIf(!string.IsNullOrWhiteSpace(extension), e => e.AttachmentDetail.Extension.Contains(extension))
                    .WhereIf(attachmentId != null && attachmentId != Guid.Empty, e => e.Attachment != null && e.Attachment.Id == attachmentId);
        }

        public async Task<List<AttachmentDetail>> GetListAsync(
            string filterText = null,
            string name = null,
            long? fileSizeMin = null,
            long? fileSizeMax = null,
            string extension = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, name, fileSizeMin, fileSizeMax, extension);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? AttachmentDetailConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public async Task<long> GetCountAsync(
            string filterText = null,
            string name = null,
            long? fileSizeMin = null,
            long? fileSizeMax = null,
            string extension = null,
            Guid? attachmentId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, name, fileSizeMin, fileSizeMax, extension, attachmentId);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<AttachmentDetail> ApplyFilter(
            IQueryable<AttachmentDetail> query,
            string filterText,
            string name = null,
            long? fileSizeMin = null,
            long? fileSizeMax = null,
            string extension = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Name.Contains(filterText) || e.Extension.Contains(filterText))
                    .WhereIf(!string.IsNullOrWhiteSpace(name), e => e.Name.Contains(name))
                    .WhereIf(fileSizeMin.HasValue, e => e.FileSize >= fileSizeMin.Value)
                    .WhereIf(fileSizeMax.HasValue, e => e.FileSize <= fileSizeMax.Value)
                    .WhereIf(!string.IsNullOrWhiteSpace(extension), e => e.Extension.Contains(extension));
        }
    }
}