namespace Demo.Attachments
{
    public static class AttachmentConsts
    {
        private const string DefaultSorting = "{0}FilesCount asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "Attachment." : string.Empty);
        }

    }
}