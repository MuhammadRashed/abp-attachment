import type { AttachmentCreateDto, AttachmentDto, AttachmentUpdateDto, GetAttachmentsInput } from './models';
import { RestService } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AttachmentService {
  apiName = 'Default';

  create = (input: AttachmentCreateDto) =>
    this.restService.request<any, AttachmentDto>({
      method: 'POST',
      url: '/api/app/attachments',
      body: input,
    },
    { apiName: this.apiName });

  delete = (id: string) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/attachments/${id}`,
    },
    { apiName: this.apiName });

  get = (id: string) =>
    this.restService.request<any, AttachmentDto>({
      method: 'GET',
      url: `/api/app/attachments/${id}`,
    },
    { apiName: this.apiName });

  getList = (input: GetAttachmentsInput) =>
    this.restService.request<any, PagedResultDto<AttachmentDto>>({
      method: 'GET',
      url: '/api/app/attachments',
      params: { filterText: input.filterText, filesCountMin: input.filesCountMin, filesCountMax: input.filesCountMax, filesSizeMin: input.filesSizeMin, filesSizeMax: input.filesSizeMax, name: input.name, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName });

  update = (id: string, input: AttachmentUpdateDto) =>
    this.restService.request<any, AttachmentDto>({
      method: 'PUT',
      url: `/api/app/attachments/${id}`,
      body: input,
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
