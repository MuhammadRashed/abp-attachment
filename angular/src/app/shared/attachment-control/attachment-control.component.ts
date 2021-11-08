import { OAuthService } from 'angular-oauth2-oidc';
import { environment } from './../../../environments/environment';
import {
  ABP,
  AuthService,
  EnvironmentService,
  ListService,
  PagedResultDto,
  TrackByService,
} from '@abp/ng.core';
import { ConfirmationService } from '@abp/ng.theme.shared';
import { DateAdapter } from '@abp/ng.theme.shared/extensions';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, EventEmitter, forwardRef, Input, OnInit, Output } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap';
import {
  AttachmentCreateDto,
  AttachmentDetailService,
  AttachmentDetailWithNavigationPropertiesDto,
  AttachmentService,
  FileUploadInputDto,
  GetAttachmentDetailsInput,
} from '@proxy/attachments';

@Component({
  selector: 'app-attachment-control',
  templateUrl: './attachment-control.component.html',
  styleUrls: ['./attachment-control.component.scss'],
  // a) copy paste this providers property (adjust the component name in the forward ref)
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => AttachmentControlComponent),
      multi: true,
    },
    ListService,
    { provide: NgbDateAdapter, useClass: DateAdapter },
  ],
})
// b) Add "implements ControlValueAccessor"
export class AttachmentControlComponent implements ControlValueAccessor {
  data: PagedResultDto<AttachmentDetailWithNavigationPropertiesDto> = {
    items: [],
    totalCount: 0,
  };
  constructor(
    public readonly list: ListService,
    public readonly track: TrackByService,
    public readonly attachmentService: AttachmentService,
    public readonly service: AttachmentDetailService,
    public http: HttpClient,
    private confirmation: ConfirmationService,
    private environment: EnvironmentService,
    private oAuthService: OAuthService,
    private authService: AuthService
  ) {}
  filters = {} as GetAttachmentDetailsInput;

  ngOnInit22(): void {
    // const getData = (query: ABP.PageQueryParams) =>
    //   this.service.getList({
    //     ...query,
    //     ...this.filters,
    //     filterText: query.filter,
    //   });

    // const setData = (list: PagedResultDto<AttachmentDetailWithNavigationPropertiesDto>) => (this.data = list);

    // this.list.hookToQuery(getData).subscribe(setData);
    return null;
  }

  // c) copy paste this code
  onChange: any = () => {};
  onTouch: any = () => {};
  registerOnChange(fn: any): void {
    this.onChange = fn;
  }
  registerOnTouched(fn: any): void {
    this.onTouch = fn;
  }

  // d) copy paste this code
  writeValue(input: string) {
     const isNeedToLoadData = !this.AttachmentId;
     this.AttachmentId = input;
    //  if (isNeedToLoadData) {
    //    this.loadData();
    //  }
    this.loadData();
  }
  isLoaded = false;
  loadData() {
    console.log(this.AttachmentId);
    if (!this.AttachmentId || this.isLoaded) return;
    let query: GetAttachmentDetailsInput = {
      maxResultCount: 1000,
      attachmentId: this.AttachmentId
    };
    this.service.getList(query).subscribe(t => {
      this.data = t;
      // this.data.totalCount = t.totalCount;
      // this.data.items = t.items.map(tt=>tt.attachmentDetail);
      this.isLoaded = true;
    });
  }
  @Input() AttachmentName = '';
  AttachmentId: string;
  @Input() Accept =
    '.csv,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel,.png,.jpeg,.docx,application/vnd,application/pdf';
  // @Output() changed = new EventEmitter<string>();

  onFileChange(evt: any) {
    const filesToUpload: File[] = evt.target.files;
    const formData = new FormData();
    if (filesToUpload == null) {
      return;
    } else {
      Array.from(filesToUpload).map(file => {
        return formData.append('Files', file);
      });
    }
    formData.append('AttachmentId', this.AttachmentId);
    // this.changed.emit(res.id);
    let self = this;
    if (!this.AttachmentId) {
      this.createAttachmentId(att => {
        self.AttachmentId = att;
        self.onChange(this.AttachmentId);
        self.UploadAttachment(formData).subscribe((f : any) => {
          self.addItemToList(f);
        });
      });
    } else {
      self.UploadAttachment(formData).subscribe((f : any) => {
        self.addItemToList(f);
      });
    }
  }

  addItemToList(items: any[]) {
    if (!this.isLoaded) {
      this.loadData();
      return;
    }
    let self = this;
    this.data = this.data || {
      items: [],
      totalCount: 0,
    };
    console.log('item', items);
    items.forEach(element => {
      let newItem : AttachmentDetailWithNavigationPropertiesDto =  {
        attachmentDetail: element,
        attachment: null
      };
      self.data.items.push(newItem);
      self.data.totalCount++;
    });
    console.log(self.data);
  }

  UploadAttachment(data: FormData) {
    const tkn = this.oAuthService.getAccessToken();
    var reqHeader = new HttpHeaders({
      Authorization: `Bearer ${tkn}`,
    });
    return this.http.post(
      `${this.environment.getApiUrl()}/api/app/attachment-details?AttachmentId=${
        this.AttachmentId
      }`,
      data,
      { headers: reqHeader }
    );
  }

  createAttachmentId(cb: Function) {
    let d: AttachmentCreateDto = {
      filesCount: 0,
      filesSize: 0,
      name: this.AttachmentName,
    };
    console.log('create att');

    this.attachmentService.create(d).subscribe(r => {
      console.log(r);
      cb(r.id);
    });
  }

  upload(event , file) {
    this.stopDefault(event);
    file.click();
  }
  update(event , rec: any) {
    console.log('todo update');
    this.stopDefault(event);
  }
  delete(event, rec: any) {
    console.log('todo delete');
    this.stopDefault(event);
  }
  download(event, rec: any) {
    this.stopDefault(event);
    console.log('todo download' , rec);
    let r2 = this.environment.getApiUrl() + '/api/app/attachment-details/download/' + rec.attachmentDetail.id;
    window.location.href =  r2;

  }

  stopDefault(event) {
    event.stopPropagation();event.stopImmediatePropagation();event.preventDefault();
  }
}
