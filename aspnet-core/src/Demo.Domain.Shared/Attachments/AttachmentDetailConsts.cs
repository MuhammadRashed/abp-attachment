namespace Demo.Attachments
{
    public static class AttachmentDetailConsts
    {
        private const string DefaultSorting = "{0}Name asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "AttachmentDetail." : string.Empty);
        }

    }
}