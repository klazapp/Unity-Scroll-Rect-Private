namespace com.Klazapp.Utility
{
    public class CellComponent : ICellComponent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CellType CellType { get; set; }

        public CellComponent(int id = 0, string name = "", CellType cellType = CellType.Cell)
        {
            Id = id;
            Name = name;
            this.CellType = cellType;
        }
    }
}
