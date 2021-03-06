import { CoreModule } from '@abp/ng.core';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { NgModule } from '@angular/core';
import { ThemeLeptonModule } from '@volo/abp.ng.theme.lepton';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { AttachmentControlComponent } from './attachment-control/attachment-control.component';

@NgModule({
  declarations: [
    AttachmentControlComponent
  ],
  imports: [
    CoreModule,
    ThemeSharedModule,
    ThemeLeptonModule,
    CommercialUiModule,
    NgbDropdownModule,
    NgxValidateCoreModule
  ],
  exports: [
    AttachmentControlComponent,
    CoreModule,
    ThemeSharedModule,
    ThemeLeptonModule,
    CommercialUiModule,
    NgbDropdownModule,
    NgxValidateCoreModule
  ],
  providers: []
})
export class SharedModule {}
