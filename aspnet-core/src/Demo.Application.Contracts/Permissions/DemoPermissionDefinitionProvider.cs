using Demo.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Demo.Permissions
{
    public class DemoPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(DemoPermissions.GroupName);

            myGroup.AddPermission(DemoPermissions.Dashboard.Host, L("Permission:Dashboard"), MultiTenancySides.Host);
            myGroup.AddPermission(DemoPermissions.Dashboard.Tenant, L("Permission:Dashboard"), MultiTenancySides.Tenant);

            //Define your own permissions here. Example:
            //myGroup.AddPermission(DemoPermissions.MyPermission1, L("Permission:MyPermission1"));

            var attachmentPermission = myGroup.AddPermission(DemoPermissions.Attachments.Default, L("Permission:Attachments"));
            attachmentPermission.AddChild(DemoPermissions.Attachments.Create, L("Permission:Create"));
            attachmentPermission.AddChild(DemoPermissions.Attachments.Edit, L("Permission:Edit"));
            attachmentPermission.AddChild(DemoPermissions.Attachments.Delete, L("Permission:Delete"));

            var attachmentDetailPermission = myGroup.AddPermission(DemoPermissions.AttachmentDetails.Default, L("Permission:AttachmentDetails"));
            attachmentDetailPermission.AddChild(DemoPermissions.AttachmentDetails.Create, L("Permission:Create"));
            attachmentDetailPermission.AddChild(DemoPermissions.AttachmentDetails.Edit, L("Permission:Edit"));
            attachmentDetailPermission.AddChild(DemoPermissions.AttachmentDetails.Delete, L("Permission:Delete"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<DemoResource>(name);
        }
    }
}