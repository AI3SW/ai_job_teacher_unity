using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(GridLayoutGroup))]
public class GridLayoutSizeAutomater : MonoBehaviour
{
    [SerializeField]
    [Range(0.1f,1f)]
    float RelativeWidthPerCell = 0.2f;
    [SerializeField]
    [Range(0.1f, 1f)]
    float RelativeHeightPerCell = 0.5f;


    [SerializeField]
    Scrollbar scrollbarToInitialize;
    enum resolutionRatio
    {
        None = 0,
        HeightByWidth,
        WidthByHeight,
    }
    [SerializeField]
    resolutionRatio cellRatio = resolutionRatio.None;
    [SerializeField]
    /// <summary>
    /// Allows the elements to squeeze in the parent or split evenly with a 1 screen to 1 element ratio;
    /// </summary>
    bool AdjustParentSize = false;
    private GridLayoutGroup grid;
    private RectTransform parentRectT;

    public bool scrollVertical;
    public bool scrollHorizontal;
    // Start is called before the first frame update

    int childNum = 0;
    void Start()
    {
        grid = GetComponent<GridLayoutGroup>();
        parentRectT = GetComponent<RectTransform>();
        childNum = GetComponentsInChildren<ScrollViewElement>().Length;
        //Debug.Log(childNum);
        Invoke("InitGrid", 0.01f);
    }

    void InitGrid()
    {
        float length = 0;
        float newWidth = 0;
        float newHeight = 0;
        switch (cellRatio)
        {
            
            case resolutionRatio.HeightByWidth:
                length = parentRectT.rect.width;
                grid.cellSize = new Vector2(parentRectT.rect.width, length * RelativeWidthPerCell);

                break;
            case resolutionRatio.WidthByHeight:
                float newheight = parentRectT.rect.height;
                if (Screen.width> parentRectT.rect.height)
                    length = parentRectT.rect.height;
                else
                {
                    length = Screen.width*0.9f;
                    newheight = length;
                }
                //Debug.Log(length);
                grid.cellSize = new Vector2(length, newheight);
                break;
            default:
                grid.cellSize = new Vector2(parentRectT.rect.width * RelativeWidthPerCell, parentRectT.rect.height * RelativeHeightPerCell);
                break;
        }

        newWidth = parentRectT.rect.width;
        newHeight = parentRectT.rect.height;
        //Debug.Log(newWidth);
        //Debug.Log(newHeight);

        int relativeSpacingx = 0;
        int relativeSpacingy = 0;

        if (AdjustParentSize)
        {
            relativeSpacingx = Mathf.CeilToInt((Screen.width - grid.cellSize.x));
            relativeSpacingy = Mathf.CeilToInt((Screen.width - grid.cellSize.y));
            grid.spacing = new Vector2(relativeSpacingx, relativeSpacingy);
        }
            
        else
        {
            grid.spacing = new Vector2(0, 0);
        }
            



        if (scrollHorizontal)
        {
            
            grid.padding.left = relativeSpacingx/2;
            grid.padding.right = relativeSpacingx/2;
            newWidth = grid.cellSize.x * childNum + grid.spacing.x * (childNum-1) + grid.padding.left + grid.padding.right;
        }

        else if (scrollVertical)
        {

            grid.padding.top = relativeSpacingy / 2;
            grid.padding.bottom = relativeSpacingy / 2;
            newHeight = grid.cellSize.y * childNum + grid.spacing.y * (childNum - 1) + grid.padding.top + grid.padding.bottom;
        }

        if(AdjustParentSize)
            parentRectT.sizeDelta = new Vector2(newWidth, newHeight);

        Invoke("delaySetScrollbarValue", 0.5f);


    }

    void delaySetScrollbarValue()
    {
        if (scrollbarToInitialize != null)
        {
            //Debug.Log("scrollbar");
            scrollbarToInitialize.value = 0;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
