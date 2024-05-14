using EnhancedUI.EnhancedScroller;
using UnityEngine;

namespace com.NavigationApp.CustomScrollRect
{
    public class ScrollRectController : MonoBehaviour, IEnhancedScrollerDelegate
    {
        [Header("Scroll Rect Id")] 
        [SerializeField]
        protected ScrollRectId scrollRectId;
        
        [Header("Enhanced Scroller")]
        [SerializeField]
        protected EnhancedScroller enhancedScroller;

        #region Public Access
        public ScrollRectId GetScrollRectId()
        {
            return scrollRectId;
        }
        #endregion
        
        #region IEnhancedScrollerDelegate
        public virtual int GetNumberOfCells(EnhancedScroller scroller)
        {
            throw new System.NotImplementedException();
        }

        public virtual float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            throw new System.NotImplementedException();
        }

        public virtual EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}