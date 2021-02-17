namespace Bynder.Sdk.Model
{

    public static class CollectionsOrderBy
    {
        public const string DateCreatedAscending = "dateCreated asc";
        public const string DateCreatedDescending = "dateCreated desc";
        public const string NameAscending = "name asc";
        public const string NameDescending = "name desc";
    }

    public static class OrderByType
    {
        public const string DateCreatedAscending = CollectionsOrderBy.DateCreatedAscending;
        public const string DateCreatedDescending = CollectionsOrderBy.DateCreatedDescending;
        public const string NameAscending = CollectionsOrderBy.NameAscending;
        public const string NameDescending = CollectionsOrderBy.NameDescending;
    }

}
