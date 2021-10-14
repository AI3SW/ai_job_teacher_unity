using UnityEngine;
using UnityEngine.UI;
public class ShareController
{ 
    public static void SaveImageToGallery(Texture2D texture, string AppName, string photoName)
    {
        NativeGallery.SaveImageToGallery(texture, AppName, photoName);

    }

    public static void SharePhoto(Texture2D texture, string title, string photoName, string text)
    {
        NativeShare PhotoSharing = new NativeShare();
        PhotoSharing.AddFile(texture, photoName);
        //PhotoSharing.SetText(text);
        PhotoSharing.SetTitle(title);
        PhotoSharing.Share();
    }

    public static void ShareAppLink(string title, string url, string text)
    {
        NativeShare AppLinkSharing = new NativeShare();
        AppLinkSharing.SetTitle(title);
        AppLinkSharing.SetText(text);
        AppLinkSharing.SetUrl(url);
        AppLinkSharing.Share();
    }
}
