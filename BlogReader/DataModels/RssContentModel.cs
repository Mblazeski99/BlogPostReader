using BlogReader.Helpers;

namespace BlogReader.DataModels
{
    public class RssContentModel : BaseEntity
    {
        public string ModelName { get; set; }

        public string ItemContainerTag { get; set; }
        public string TitleTag { get; set; }
        public string SummaryTag { get; set; }
        public string ContentTag { get; set; }
        public string DateTag { get; set; }
        public string AuthorTag { get; set; }
        public string ItemLinkTag { get; set; }
        public string ItemImageTag { get; set; }

        public static void Copy(RssContentModel copyFrom, RssContentModel copyTo)
        {
            PropertyCopier<RssContentModel, RssContentModel>.Copy(copyFrom, copyTo);
        }

        public static RssContentModel CreateNewCopy(RssContentModel model)
        {
            var newModel = new RssContentModel();
            Copy(model, newModel);
            return newModel;
        }
    }
}
