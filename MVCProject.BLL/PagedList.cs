using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MVCProject.BLL {
    public class PagedList<T> : IList<T> {
        #region Fields

        private List<T> items;
        private int currentPage; 

        #endregion

        #region Properties

        public int CurrentPage {
            get => currentPage;
            set {
                if (currentPage == value)
                    return;

                if (value < 1 || (PageCount != 0 && value > PageCount))
                    throw new ArgumentOutOfRangeException(nameof(value));

                currentPage = value;
            }
        }
        public int PageSize { get; set; } = 20;

        public int PageCount {
            get {
                if (PageSize <= 0)
                    return 0;

                var count = items.Count / PageSize;

                return HasPageOverflow
                    ? count + 1  //70 / 20 => 4 pages
                    : count;     //60 / 20 => 3 pages
            }
        }

        public bool HasPageOverflow => items.Count % PageSize != 0;

        #endregion

        public PagedList(IEnumerable<T> collection) {
            SetCollection(collection);
        }

        #region Methods

        public List<T> GetItemsFromCurrentPage() {
            var list = new List<T>();

            //example: 100 items in collection, current page is 3
            //page 1 =>  1 - 20
            //page 2 => 21 - 40
            //page 3 => 41 - 60

            //skip first 2 pages (CurrentPage is 3)
            var itemsToSkip = (CurrentPage - 1) * PageSize;

            //start at item 41 on page 3
            var start = itemsToSkip + 1;

            //end at the end of the 3rd page
            var end = CurrentPage * PageSize; //60

            //fill the list with items
            //(end can be the end of page or the end of collection)
            for(int i = start; i <= end && i <= items.Count; i++)
                list.Add(items[i - 1]); //use indexes (logic is done without indexing)

            return list;
        }

        public void AddRange(IEnumerable<T> collection) => items.AddRange(collection);

        public void SetCollection(IEnumerable<T> collection) => items = collection?.ToList() ?? new List<T>();

        #endregion

        #region IList<T> members

        public T this[int index] { get => items[index]; set => items[index] = value; }

        public int Count => items.Count;

        public bool IsReadOnly => false;

        public void Add(T item) => items.Add(item);

        public void Clear() => items.Clear();

        public bool Contains(T item) => items.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => items.CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator() => items.GetEnumerator();

        public int IndexOf(T item) => items.IndexOf(item);

        public void Insert(int index, T item) => items.Insert(index, item);

        public bool Remove(T item) => items.Remove(item);

        public void RemoveAt(int index) => items.RemoveAt(index);

        IEnumerator IEnumerable.GetEnumerator() => items.GetEnumerator(); 

        #endregion
    }
}
