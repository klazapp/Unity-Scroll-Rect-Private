namespace com.Klazapp.Utility
{
    public interface ICellEntity<T> where T : CellComponent
    {
        public T CellComponent { get; set; }
        public void SetData(T comp, ScrollEventManager scrollEventManager = null);
        public void OnButtonPressed();
    }
}
