namespace SmartSchool.WebAPI.Helpers
{
    public class PageParams
    {
        public const int MAxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int _pageSize { get; set; } = 10;
        public int PageSize 
        { 
            get { return _pageSize; } 
            set { _pageSize = (value > MAxPageSize) ? MAxPageSize : value; }
        }
    }
}