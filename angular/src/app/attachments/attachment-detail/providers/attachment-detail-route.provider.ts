import { eLayoutType, RoutesService } from '@abp/ng.core';
import { APP_INITIALIZER } from '@angular/core';

export const ATTACHMENTS_ATTACHMENT_DETAIL_ROUTE_PROVIDER = [
  { provide: APP_INITIALIZER, useFactory: configureRoutes, deps: [RoutesService], multi: true },
];

function configureRoutes(routes: RoutesService) {
  return () => {
    routes.add([
      {
        path: '/attachment-details',
        iconClass: 'fas fa-file-alt',
        name: '::Menu:AttachmentDetails',
        layout: eLayoutType.application,
        requiredPolicy: 'Demo.AttachmentDetails',
      },
    ]);
  };
}
