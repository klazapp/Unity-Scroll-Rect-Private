namespace com.Klazapp.Utility
{
    public interface ICellComponent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CellType CellType { get; set; }
    }
}
