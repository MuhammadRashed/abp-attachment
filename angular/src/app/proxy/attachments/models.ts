import type { FullAuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface AttachmentCreateDto {
  filesCount: number;
  filesSize: number;
  name?: string;


}

export interface AttachmentDetailDto extends FullAuditedEntityDto<string> {
  name?: string;
  fileSize: number;
  extension?: string;
  attachmentId?: string;
}

export interface AttachmentDetailWithNavigationPropertiesDto {
  attachmentDetail: AttachmentDetailDto;
  attachment: AttachmentDto;
}

export interface AttachmentDto extends FullAuditedEntityDto<string> {
  filesCount: number;
  filesSize: number;
  name?: string;
}

export interface AttachmentUpdateDto {
  filesCount: number;
  filesSize: number;
  name?: string;
}

export interface FileUploadInputDto {
  attachmentId?: string;
}

export interface GetAttachmentDetailsInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  name?: string;
  fileSizeMin?: number;
  fileSizeMax?: number;
  extension?: string;
  attachmentId?: string;
}

export interface GetAttachmentsInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  filesCountMin?: number;
  filesCountMax?: number;
  filesSizeMin?: number;
  filesSizeMax?: number;
  name?: string;
}
