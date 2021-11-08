import { ABP, ListService, PagedResultDto, TrackByService } from '@abp/ng.core';
import { Confirmation, ConfirmationService } from '@abp/ng.theme.shared';
import { DateAdapter } from '@abp/ng.theme.shared/extensions';
import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap';
import { AttachmentService } from '@proxy/attachments';
import { filter, finalize, switchMap, tap } from 'rxjs/operators';
import type {
  GetAttachmentsInput,
  AttachmentDto,
} from '../../../proxy/attachments/models';

@Component({
  selector: 'app-attachment',
  changeDetection: ChangeDetectionStrategy.Default,
  providers: [ListService, { provide: NgbDateAdapter, useClass: DateAdapter }],
  templateUrl: './attachment.component.html',
  styles: [],
})
export class AttachmentComponent implements OnInit {
  data: PagedResultDto<AttachmentDto> = {
    items: [],
    totalCount: 0,
  };

  filters = {} as GetAttachmentsInput;

  form: FormGroup;

  isFiltersHidden = true;

  isModalBusy = false;

  isModalOpen = false;

  selected?: AttachmentDto;

  constructor(
    public readonly list: ListService,
    public readonly track: TrackByService,
    public readonly service: AttachmentService,
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

    const setData = (list: PagedResultDto<AttachmentDto>) => (this.data = list);

    this.list.hookToQuery(getData).subscribe(setData);
  }

  clearFilters() {
    this.filters = {} as GetAttachmentsInput;
  }

  buildForm() {
    const {
      filesCount,
      filesSize,
      name,
    } = this.selected || {};

    this.form = this.fb.group({
      filesCount: [filesCount ?? null, [Validators.required]],
      filesSize: [filesSize ?? null, [Validators.required]],
      nameat: [name ?? 'ss', []],
    });
  }

  hideForm() {
    this.isModalOpen = false;
    this.form.reset();
  }

  showForm() {
    this.buildForm();
    setTimeout(() => {
      this.isModalOpen = true;
    }, 100);
  }

  submitForm() {
    if (this.form.invalid) return;

    const request = this.selected
      ? this.service.update(this.selected.id, this.form.value)
      : this.service.create(this.form.value);

    this.isModalBusy = true;

    request
      .pipe(
        finalize(() => (this.isModalBusy = false)),
        tap(() => this.hideForm()),
      )
      .subscribe(this.list.get);
  }

  create() {
    this.selected = undefined;
    this.showForm();
  }

  update(record: AttachmentDto) {
    this.selected = record;
    this.showForm();
  }

  delete(record: AttachmentDto) {
    this.confirmation.warn(
      '::DeleteConfirmationMessage',
      '::AreYouSure',
      { messageLocalizationParams: [] }
    ).pipe(
      filter(status => status === Confirmation.Status.confirm),
      switchMap(() => this.service.delete(record.id)),
    ).subscribe(this.list.get);
  }
}
