using System;
using Demo.Shared;
using Volo.Abp.AutoMapper;
using Demo.Attachments;
using AutoMapper;

namespace Demo
{
    public class DemoApplicationAutoMapperProfile : Profile
    {
        public DemoApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */

            CreateMap<AttachmentCreateDto, Attachment>().IgnoreFullAuditedObjectProperties().Ignore(x => x.ExtraProperties).Ignore(x => x.ConcurrencyStamp).Ignore(x => x.Id);
            CreateMap<AttachmentUpdateDto, Attachment>().IgnoreFullAuditedObjectProperties().Ignore(x => x.ExtraProperties).Ignore(x => x.ConcurrencyStamp).Ignore(x => x.Id);
            CreateMap<Attachment, AttachmentDto>();

            CreateMap<AttachmentDetailCreateDto, AttachmentDetail>().IgnoreFullAuditedObjectProperties().Ignore(x => x.Id);
            CreateMap<AttachmentDetailUpdateDto, AttachmentDetail>().IgnoreFullAuditedObjectProperties().Ignore(x => x.Id);
            CreateMap<AttachmentDetail, AttachmentDetailDto>();
            CreateMap<AttachmentDetailWithNavigationProperties, AttachmentDetailWithNavigationPropertiesDto>();
            CreateMap<Attachment, LookupDto<Guid>>().ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Name));
        }
    }
}