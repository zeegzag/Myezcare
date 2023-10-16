namespace HomeCareApi.Models.General
{
    public class ListModel<T> : ListModel
    {
        public T SearchParams { get; set; }
    }

    public class ListModel
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string SortExpression { get; set; }
        public string SortType { get; set; }
    }

    public class ListDetail : ListModel
    {
        public long? PrimaryId { get; set; }
        public string Search { get; set; }
        public int AskOrShowSample { get; set; }
    }
}