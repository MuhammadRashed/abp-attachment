import { Component } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  template: `
    <app-host-dashboard *abpPermission="'Demo.Dashboard.Host'"></app-host-dashboard>
    <app-tenant-dashboard *abpPermission="'Demo.Dashboard.Tenant'"></app-tenant-dashboard>
  `,
})
export class DashboardComponent {}
