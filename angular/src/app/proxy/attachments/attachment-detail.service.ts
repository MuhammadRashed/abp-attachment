import type { AttachmentDetailDto, AttachmentDetailWithNavigationPropertiesDto, FileUploadInputDto, GetAttachmentDetailsInput } from './models';
import { RestService } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AttachmentDetailService {
  apiName = 'Default';

  // createFileByFileObj = (fileObj: FileUploadInputDto) =>
  //   this.restService.request<any, AttachmentDetailDto[]>({
  //     method: 'POST',
  //     url: '/api/app/attachment-details',
  //     body: fileObj,
  //   },
  //   { apiName: this.apiName });

  delete = (id: string) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/attachment-details/${id}`,
    },
    { apiName: this.apiName });

  get = (id: string) =>
    this.restService.request<any, AttachmentDetailDto>({
      method: 'GET',
      url: `/api/app/attachment-details/${id}`,
    },
    { apiName: this.apiName });

  getList = (input: GetAttachmentDetailsInput) =>
    this.restService.request<any, PagedResultDto<AttachmentDetailWithNavigationPropertiesDto>>({
      method: 'GET',
      url: '/api/app/attachment-details',
      params: { filterText: input.filterText, name: input.name, fileSizeMin: input.fileSizeMin, fileSizeMax: input.fileSizeMax, extension: input.extension, attachmentId: input.attachmentId, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
