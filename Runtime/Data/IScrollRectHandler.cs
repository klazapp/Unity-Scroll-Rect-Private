using System.Collections.Generic;

namespace com.Klazapp.Utility
{
    public interface IScrollRectHandler<T, T2> where T : CellComponent where T2 : CellEntity
    {
        public List<T> CellComponents { get; set; }
        public Queue<T2> CellPool { get; set; }
        public Dictionary<int, T2> VisibleCells { get; set; }
    }
}
