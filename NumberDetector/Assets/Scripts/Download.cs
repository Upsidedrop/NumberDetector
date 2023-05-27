using System.Collections;
using System.IO;
using UnityEngine;

public class Download : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetButtonDown("DownloadFile"))
        {
            DownloadDrawing();
        } 
    }
    void DownloadDrawing()
    {
        StartCoroutine(DownloadIt());
    }
    IEnumerator DownloadIt()
    {
        yield return new WaitForEndOfFrame();
        Texture2D texture = new(Screen.width, Screen.height, TextureFormat.RGB24, false);
        texture.ReadPixels(new(0, 0, Screen.width, Screen.height), 0, 0);
        string name = "/Screenshot" + System.DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss") + ".png";
        texture.Apply();
        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + name, bytes);
        Destroy(texture);

    }
}
