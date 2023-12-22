using System.Collections;

namespace SitePustok.ViewModels.CommonVM
{
    public class PaginationVM<T> where T : IEnumerable
    {
        public int TotalCount { get; }
        public int LastPage { get; }

        public int CurrentPage { get; }
        public bool HasPrev { get; }
        public bool HasNext { get; }
        public T Items { get; }

        public PaginationVM(int totalCount, int currentPage, T items, int lastPage)
        {
            if (currentPage <= 0)
            {
                throw new ArgumentException();
            }
            TotalCount = totalCount;
            LastPage = lastPage;
            CurrentPage = currentPage;
            Items = items;

            if (CurrentPage <= LastPage)
            {
                if (currentPage == 1)
                {
                    HasPrev = false;
                }
                else
                {
                    HasPrev = true;
                }
                if (currentPage == lastPage)
                {
                    HasNext = false;
                }
                else
                {
                    HasNext= true;
                }
            }
        }


    }
}
