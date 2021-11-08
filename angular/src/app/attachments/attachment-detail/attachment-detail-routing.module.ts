import { AuthGuard, PermissionGuard } from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AttachmentDetailComponent } from './components/attachment-detail.component';

const routes: Routes = [
  {
    path: '',
    component: AttachmentDetailComponent,
    canActivate: [AuthGuard, PermissionGuard],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AttachmentDetailRoutingModule {}
