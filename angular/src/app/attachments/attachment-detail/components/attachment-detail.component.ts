import { ABP, ListService, PagedResultDto, TrackByService } from '@abp/ng.core';
import { Confirmation, ConfirmationService } from '@abp/ng.theme.shared';
import { DateAdapter } from '@abp/ng.theme.shared/extensions';
import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap';
import { AttachmentDetailService } from '@proxy/attachments';
import { filter, finalize, switchMap, tap } from 'rxjs/operators';
import type {
  GetAttachmentDetailsInput,
  AttachmentDetailWithNavigationPropertiesDto,
} from '../../../proxy/attachments/models';

@Component({
  selector: 'app-attachment-detail',
  changeDetection: ChangeDetectionStrategy.Default,
  providers: [ListService, { provide: NgbDateAdapter, useClass: DateAdapter }],
  templateUrl: './attachment-detail.component.html',
  styles: [],
})
export class AttachmentDetailComponent implements OnInit {
  data: PagedResultDto<AttachmentDetailWithNavigationPropertiesDto> = {
    items: [],
    totalCount: 0,
  };

  filters = {} as GetAttachmentDetailsInput;

  form: FormGroup;

  isFiltersHidden = true;

  isModalBusy = false;

  isModalOpen = false;

  selected?: AttachmentDetailWithNavigationPropertiesDto;

  constructor(
    public readonly list: ListService,
    public readonly track: TrackByService,
    public readonly service: AttachmentDetailService,
    private confirmation: ConfirmationService,
    private fb: FormBuilder
  ) {}

  ngOnInit() {
    const getData = (query: ABP.PageQueryParams) =>
      this.service.getList({
        ...query,
        ...this.filters,
        filterText: query.filter,
      });

    const setData = (list: PagedResultDto<AttachmentDetailWithNavigationPropertiesDto>) => (this.data = list);

    this.list.hookToQuery(getData).subscribe(setData);
  }

  clearFilters() {
    this.filters = {} as GetAttachmentDetailsInput;
  }

  buildForm() {
    const {
      name,
      fileSize,
      extension,
      attachmentId,
    } = this.selected?.attachmentDetail || {};

    this.form = this.fb.group({
      name: [name ?? null, [Validators.required]],
      fileSize: [fileSize ?? null, [Validators.required]],
      extension: [extension ?? null, [Validators.required]],
      attachmentId: [attachmentId ?? null, [Validators.required]],
    });
  }

  hideForm() {
    this.isModalOpen = false;
    this.form.reset();
  }

  showForm() {
    this.buildForm();
    this.isModalOpen = true;
  }

  submitForm() {
    // if (this.form.invalid) return;

    // const request = this.selected
    //   ? this.service.update(this.selected.attachmentDetail.id, this.form.value)
    //   : this.service.create(this.form.value);

    // this.isModalBusy = true;

    // request
    //   .pipe(
    //     finalize(() => (this.isModalBusy = false)),
    //     tap(() => this.hideForm()),
    //   )
    //   .subscribe(this.list.get);
  }

  create() {
    this.selected = undefined;
    this.showForm();
  }

  update(record: AttachmentDetailWithNavigationPropertiesDto) {
    this.selected = record;
    this.showForm();
  }

  delete(record: AttachmentDetailWithNavigationPropertiesDto) {
    this.confirmation.warn(
      '::DeleteConfirmationMessage',
      '::AreYouSure',
      { messageLocalizationParams: [] }
    ).pipe(
      filter(status => status === Confirmation.Status.confirm),
      switchMap(() => this.service.delete(record.attachmentDetail.id)),
    ).subscribe(this.list.get);
  }
}
