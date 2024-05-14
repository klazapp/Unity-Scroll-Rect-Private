using System;
using System.Runtime.CompilerServices;

namespace com.NavigationApp.CustomScrollRect
{
    //Multi-dimensional id
    //Allows for multiple scroll rect's integration
    //First index = parent scroll rect entity id
    //Second index = cell entity id
    [Serializable]
    public struct CellData
    {
        public (ScrollRectId scrollRectId, CellId cellId) cellData;

        public CellData((ScrollRectId ScrollRectId, CellId cellId) cellDataParam)
        {
            cellData.Item1 = cellDataParam.ScrollRectId;
            cellData.Item2 = cellDataParam.cellId;
        }

        #region Helpers
        public (ScrollRectId, CellId) GetCellDataId()
        {
            return cellData;
        }

        public ScrollRectId GetScrollRectId()
        {
            return cellData.Item1;
        }
        
        public CellId GetCellId()
        {
            return cellData.Item2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(CellData cellData1, CellData cellData2)
        {
            var (cellData11, cellData12) = cellData1.GetCellDataId();
            var (cellData21, cellData22) = cellData2.GetCellDataId();
            return cellData11 == cellData21 && cellData12 == cellData22;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(CellData cellData1, CellData cellData2)
        {
            var (cellData11, cellData12) = cellData1.GetCellDataId();
            var (cellData21, cellData22) = cellData2.GetCellDataId();
            return cellData11 != cellData21 || cellData12 != cellData22;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(CellData otherCellData)
        {
            return cellData.Equals(otherCellData.cellData);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            return obj is CellData otherCellData && Equals(otherCellData);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return cellData.GetHashCode();
        }
        #endregion
    }
}