# Enhanced Scroller

## Introduction
The Enhanced Scroller package provides a set of utilities to create smooth and customizable scrollable UI elements in Unity. It includes various easing functions for tweening animations and a powerful framework for handling large data sets efficiently.

## Features
- **Advanced Tweening:** Includes a variety of easing functions for smooth animations.
- **Efficient Data Handling:** Optimized for handling large data sets with minimal performance overhead.
- **Easy Integration:** Simple and modular components that can be easily integrated into existing projects.
- **Customizable Scroll Rects:** Supports customization of scroll behaviors and animations.

## Dependencies
- Unity 2020.3 LTS or newer

## Compatibility

| Compatibility | URP | BRP | HDRP |
|---------------|-----|-----|------|
| Compatible    | ✔️   | ✔️   | ✔️    |

## Installation
1. Download or clone the repository.
2. Add the `EnhancedScroller` folder to your Unity project.
3. Attach the relevant components (`EnhancedScroller`, `EnhancedScrollerCellView`, `Tween`, etc.) to your UI elements.

## Usage
### Setting Up the Scroller
1. Create a `ScrollRect` in your scene.
2. Attach the `EnhancedScroller` component to the `ScrollRect`.
3. Create a cell prefab and attach the `EnhancedScrollerCellView` component.
4. Implement the `EnhancedScrollerDelegate` interface in a script to provide data to the scroller.

### Example Code
```csharp
using UnityEngine;
using EnhancedUI.EnhancedScroller;

public class MyScrollerController : MonoBehaviour, IEnhancedScrollerDelegate
{
    public EnhancedScroller myScroller;
    public EnhancedScrollerCellView myCellViewPrefab;

    private List<string> myData;

    void Start()
    {
        myData = new List<string> { "Item 1", "Item 2", "Item 3" };
        myScroller.Delegate = this;
        myScroller.ReloadData();
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return myData.Count;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return 100f; // Set your cell size
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        var cellView = scroller.GetCellView(myCellViewPrefab) as MyCellView;
        cellView.SetData(myData[dataIndex]);
        return cellView;
    }
}

public class MyCellView : EnhancedScrollerCellView
{
    public Text myText;

    public void SetData(string data)
    {
        myText.text = data;
    }
}
```

## To-Do List
- Add more easing functions.
- Improve documentation and add more usage examples.
- Optimize performance for larger data sets.
- Add support for dynamic content updates.

## License
This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
