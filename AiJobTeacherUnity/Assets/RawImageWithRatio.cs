using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AspectRatioFitter))]
[RequireComponent(typeof(RawImage))]
public class RawImageWithRatio : MonoBehaviour
{
    public RawImage _rawImage;
    private AspectRatioFitter imageRatio;

    private void Awake()
    {
        if (_rawImage == null) _rawImage = GetComponent<RawImage>();
    }
    public void setTextureAndRatio(Texture2D newtexture, AspectRatioFitter.AspectMode ratioMode = AspectRatioFitter.AspectMode.None)
    {
        setRatio(ratioMode);
        setTexture(newtexture);
    }
    public void setRatio(float val)
    {
        if (imageRatio == null) imageRatio = GetComponent<AspectRatioFitter>();
        imageRatio.aspectRatio = val;
    }
    public float getRatio()
    {
        return imageRatio.aspectRatio;
    }
    public void setColor(Color color)
    {
        _rawImage.color = color;
    }
    public void setRatio(AspectRatioFitter.AspectMode mode)
    {
        if (imageRatio == null) imageRatio = GetComponent<AspectRatioFitter>();
        if (_rawImage == null) _rawImage = GetComponent<RawImage>();
        imageRatio.aspectMode = mode;
            switch(imageRatio.aspectMode)
        {
            case AspectRatioFitter.AspectMode.HeightControlsWidth:
                imageRatio.aspectRatio = (float)_rawImage.texture.width / (float)_rawImage.texture.height;
                break;
            case AspectRatioFitter.AspectMode.WidthControlsHeight:
                imageRatio.aspectRatio = (float)_rawImage.texture.height / (float)_rawImage.texture.width;
                break;
            default:
                imageRatio.aspectRatio = 1;
                break;

        }

    }
    public void setTexture(Texture newtexture)
    {
        if (_rawImage == null) _rawImage = GetComponent<RawImage>();
        _rawImage.texture = newtexture;
    }

    public Texture getTexture()
    {
        if (_rawImage == null) _rawImage = GetComponent<RawImage>();
        return _rawImage.texture ;
    }
    public RectTransform getRectTransform()
    {
        if (_rawImage == null) _rawImage = GetComponent<RawImage>();
        return _rawImage.rectTransform;
    }

}
