using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace com.Klazapp.Utility
{
    public abstract class ScrollRectHandler : MonoBehaviour
    {
        #region Variables
        [Header("Scroll Rect")]
        [SerializeField]
        protected RectTransform scrollView;
        [SerializeField]
        protected RectTransform content;
        [SerializeField]
        protected RectTransform viewPort;

        [Header("Cell Info")]
        [SerializeField]
        protected CellInfo cellInfo;
        
        [Header("Cell Group Info")]
        [SerializeField]
        protected CellInfo cellGroupInfo;

        protected List<ICellComponent> cellData = new List<ICellComponent>();
        protected readonly List<ICellEntity> cellPool = new List<ICellEntity>();
        protected readonly Dictionary<int, ICellEntity> visibleCells = new Dictionary<int, ICellEntity>();
        protected float itemHeight;
        protected int lastFirstVisibleIndex = -1;
        protected int lastLastVisibleIndex = -1;

        protected readonly List<int> indicesToRemove = new List<int>();

        protected ScrollEventManager scrollEventManager;
        #endregion

        #region Lifecycle Flow
        protected void OnSubscribe()
        {
            scrollEventManager ??= new ScrollEventManager();
            scrollEventManager.OnTriggerCellClicked += CellClickedCallback;
        }

        protected void OnUnsubscribe()
        {
            scrollEventManager.OnTriggerCellClicked -= CellClickedCallback;
        }

        protected virtual void Update()
        {
            UpdateVisibleItems();
        }
        #endregion

        #region Public Access
        protected void SetData(List<ICellComponent> cellData)
        {
            this.cellData = cellData;
            UpdateContentHeight();
            UpdateVisibleItems();
        }
        
        //TODO: Complete this implementation
        // public void SetData(List<(ICellComponent cellData, ICellComponent cellHeaderData)> cellGroupData)
        // {
        //     var organizedList = new List<ICellComponent>();
        //
        //     for (var i = 0; i < cellGroupData.Count; i++)
        //     {
        //         var (cellData, cellHeaderData) = cellGroupData[i];
        //
        //         if (i == 0)
        //         {
        //             //Always add first index
        //             organizedList.Add(cellHeaderData);
        //             organizedList.Add(cellData);
        //         }
        //         else
        //         {
        //             foreach (var iCellComponent in organizedList)
        //             {
        //             }
        //         }
        //     }
        //     
        //     this.cellData = organizedList;
        //     UpdateContentHeight();
        //     UpdateVisibleItems();
        // }

        protected void RefreshVisibleCells()
        {
            foreach (var kvp in visibleCells)
            {
                kvp.Value.SetData(cellData[kvp.Key]);
            }
        }

        protected void JumpToCellIndex(int index, bool animate = true, float animationDuration = 0.4f, EaseType easeType = EaseType.OutQuint)
        {
            var itemHeightWithSpacing = itemHeight + cellInfo.spacing;

            // Calculate the contentTopYPos
            var contentTopYPos = -content.rect.height / 2f;

            // Calculate the content's anchored Y position based on the first visible index
            var contentAnchoredPositionY = ((index) * itemHeightWithSpacing) + contentTopYPos;

            var anchoredPosition = content.anchoredPosition;

            if (animate)
            {
                var startYPos = anchoredPosition.y;
                var endYPos = contentAnchoredPositionY;
                KlazTweenManager.Instance.DoTween(startYPos, endYPos, animationDuration, AnimateAnchoredPosY, 0f, easeType);
            }
            else
            {
                anchoredPosition.y = contentAnchoredPositionY;
                content.anchoredPosition = anchoredPosition;
            }
            
            return;

            void AnimateAnchoredPosY(float anchoredPosY)
            {
                anchoredPosition.y = anchoredPosY;
                content.anchoredPosition = anchoredPosition;
            }
        }
        
        protected void JumpToCellHeaderIndex(int index, bool animate = true, float animationDuration = 0.4f, EaseType easeType = EaseType.OutQuint)
        {
            var headerCount = 0;
            //O -> which numberth header
            //1 -> which celldata index is it at
            var headerIndexInfo = new List<(int headerIndex, int cellDataIndex)>();
            
            for (var i = 0; i < cellData.Count; i++)
            {
                if (cellData[i] is not CellComponent { CellType: CellType.CellHeader })
                    continue;

                headerCount++;
                headerIndexInfo.Add((headerCount, i));
            }

            if (index > headerCount)
                index = headerCount;

            if (index < 0)
                index = 0;
            
            foreach (var (headerIndex, cellDataIndex) in headerIndexInfo)
            {
                if (headerIndex != index) 
                    continue;

                index = cellDataIndex;
                break;
            }
            
            JumpToCellIndex(index, animate, animationDuration, easeType);
        }
        #endregion

        #region Modules
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual void InitScrollRect()
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual void UpdateVisibleItems()
        {
            var viewportHeight = viewPort.rect.height;
            var contentAnchoredPositionY = content.anchoredPosition.y;
            var itemHeightWithSpacing = itemHeight + cellInfo.spacing;
            var halfContentSizeDeltaY = content.sizeDelta.y / 2;

            // Calculate the first visible index using the formula
            var contentTopYPos = -content.rect.height / 2f;
            var firstVisibleIndex = (int)math.ceil((contentAnchoredPositionY - contentTopYPos) / (itemHeightWithSpacing)) - 1;
			
            var permissibleCellCount = viewportHeight / itemHeightWithSpacing;
            var lastVisibleIndex = (int)math.ceil(permissibleCellCount + firstVisibleIndex - 1) + 2;
			
            if (lastVisibleIndex > cellData.Count)
            {
                lastVisibleIndex = cellData.Count;
            }

            if (firstVisibleIndex < 0)
            {
                firstVisibleIndex = 0;
            }

            if (firstVisibleIndex == lastFirstVisibleIndex && lastVisibleIndex == lastLastVisibleIndex)
                return;

            lastFirstVisibleIndex = firstVisibleIndex;
            lastLastVisibleIndex = lastVisibleIndex;

            // Clear the list before reuse
            indicesToRemove.Clear();

            // Deactivate items that are out of the new visible range
            foreach (var (index, iCellEntity) in visibleCells)
            {
                if (index >= firstVisibleIndex && index < lastVisibleIndex)
                    continue;

                iCellEntity.Activate(false);
                cellPool.Add(iCellEntity);
                indicesToRemove.Add(index);
            }

            foreach (var index in indicesToRemove)
            {
                visibleCells.Remove(index);
            }

            // Activate and update items that are now visible
            for (var i = firstVisibleIndex; i < lastVisibleIndex; i++)
            {
                if (visibleCells.ContainsKey(i))
                    continue;

                ICellEntity item = null;

                //Check if cellData is cell or cell group
                if (cellPool.Count > 0)
                {
                    for (var j = 0; j < cellPool.Count; j++)
                    {
                        if (cellPool[j].CellComponent.CellType != cellData[i].CellType)
                            continue;

                        item = cellPool[j];
                        cellPool.RemoveAt(j);
                        break;
                    }

                    if (item == null)
                    {
                        GenerateNewPool();
                    }
                }
                else
                {
                    GenerateNewPool();
                }
              
                item.Activate(true);
                var rectTransform = item.RectTransform;

                // Adjusting position to start from the top
                var yPos = -cellInfo.spacing - (i * itemHeightWithSpacing) + halfContentSizeDeltaY - itemHeight / 2;
             
                rectTransform.anchoredPosition = new Vector2(0, yPos);
                visibleCells[i] = item;

                // Update item data here
                item.SetData(cellData[i], scrollEventManager);

                void GenerateNewPool()
                {
                    var obj = Instantiate(cellData[i].CellType == CellType.Cell ? cellInfo.entity : cellGroupInfo.entity, content);
                    item = obj.GetComponent<ICellEntity>();
                    if (item == null)
                    {
                        throw new MissingComponentException($"The prefab {cellInfo.entity.name} is missing the required component ICellEntity");
                    }
                    
                    item.CellComponent = new CellComponent(0, "", cellData[i].CellType);
                }
            }
        }
        #endregion

        protected virtual void OnCreated()
        {
            itemHeight = cellInfo.entity.GetComponent<RectTransform>().rect.height;
            InitializePool();
            InitializePivots();
            UpdateContentHeight();
            UpdateVisibleItems();
        }

        protected virtual void InitializePool()
        {
            if (cellInfo.entity == null)
            {
                throw new MissingComponentException($"The prefab cell entity is missing");
            }
            
            for (var i = 0; i < cellInfo.poolSize; i++)
            {
                var obj = Instantiate(cellInfo.entity, content);
                var item = obj.GetComponent<ICellEntity>();
                if (item == null)
                {
                    throw new MissingComponentException($"The prefab {cellInfo.entity.name} is missing the required component ICellEntity for Cells.");
                }

                item.CellComponent = new CellComponent();
                item.Activate(false);
                cellPool.Add(item);
            }

            if (cellGroupInfo.entity != null)
            {
                for (var i = 0; i < cellGroupInfo.poolSize; i++)
                {
                    var obj = Instantiate(cellGroupInfo.entity, content);
                    var item = obj.GetComponent<ICellEntity>();
                    if (item == null)
                    {
                        throw new MissingComponentException($"The prefab {cellInfo.entity.name} is missing the required component ICellEntity for Cell Groups.");
                    }
                  
                    item.CellComponent = new CellComponent(0, "", CellType.CellHeader);
                    item.Activate(false);
                    cellPool.Add(item);
                }
            }
        }

        //Ensures pivots are adjusted to fit current package's update cell visibility calculations
        protected virtual void InitializePivots()
        {
            if (scrollView == null)
            {
                throw new MissingComponentException($"The scroll view component is missing");
            }

            //Calculate the difference in pivot
            var pivot = scrollView.pivot;
            
            var pivotDelta = new Vector2(0.5f - pivot.x, 0.5f - pivot.y);

            //Calculate the offset needed to maintain position
            var deltaY = pivotDelta.y * scrollView.rect.height;

            //Set new pivot
            scrollView.pivot = new Vector2(0.5f, 0.5f);

            //Adjust the position - this time adding deltaY
            var anchoredPosition = scrollView.anchoredPosition;
            anchoredPosition = new Vector2(anchoredPosition.x, anchoredPosition.y + deltaY);
            scrollView.anchoredPosition = anchoredPosition;

            if (viewPort == null)
            {
                throw new MissingComponentException($"The viewport component is missing");
            }

            viewPort.pivot = new Vector2(0.5f, 0.5f);

            if (content == null)
            {
                throw new MissingComponentException($"The content component is missing");
            }

            content.pivot = new Vector2(0.5f, 0.5f);
        }
        
        protected virtual void UpdateContentHeight()
        {
            //Add spacing at the top and bottom
            var totalContentHeight = cellData.Count * (itemHeight + cellInfo.spacing); // Correctly add spacing at the bottom
            content.sizeDelta = new Vector2(content.sizeDelta.x, totalContentHeight);

            //Temporarily added this to ensure content will always be scrolled to the top
            var anchoredPosition = content.anchoredPosition;
            anchoredPosition.y = -totalContentHeight / 2f;

            content.anchoredPosition = anchoredPosition;
        }

        #region Callbacks
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual void CellClickedCallback(CellEntity cellEntity)
        {
            if (cellEntity is ICellEntity typedCellEntity)
            {
                Debug.Log("cell was clicked with id: " + typedCellEntity.CellComponent.Id);
            }
        }
        #endregion
    }
}
