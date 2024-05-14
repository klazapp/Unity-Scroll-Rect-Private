using System;
using UnityEngine;
using EnhancedUI;
using EnhancedUI.EnhancedScroller;

namespace com.NavigationApp.CustomScrollRect
{
    [Serializable]
    public class ScrollRectComponent 
    {
        [Header("Cell Entity Prefab")]
        [SerializeField]
        public EnhancedScrollerCellView cellEntityPrefab;

        [Header("Cell Entity Height")] 
        [SerializeField]
        public float cellHeight;
        
        //[ReadOnly]
        [Header("Cell Component, Contains info about cell such as id, name, selected?")]
        public SmallList<CellComponent> cellComponent = new();
    }
}