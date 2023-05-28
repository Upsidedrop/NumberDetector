using UnityEngine;
using UnityEngine.Tilemaps;

public class Save : MonoBehaviour
{
    [SerializeField]
    Tilemap tilemap;
    [SerializeField]
    TileBase emptyTile;
    bool[] image;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SaveCurrentImage();
        }
    }
    void SaveCurrentImage()
    {
        image = new bool[784];
        for (int i = 0; i < 28; i++)
        {
            for (int i1 = 0; i1 < 28; i1++)
            {
                if (tilemap.GetTile(new(i - 14, i1 - 14)) == emptyTile)
                {
                    image[i * 28 + i1] = false;
                }
                else
                {
                    image[i * 28 + i1] = true;
                }
            }
        }
        PlayerPrefs.SetInt("ImageNumber", PlayerPrefs.GetInt("ImageNumber") + 1);
        PlayerPrefs.SetString(
            "EncodedImage" + PlayerPrefs.GetInt("ImageNumber"),
            "" + ConvertBoolArrToBinary(image));
        print(ConvertBoolArrToBinary(image));
    }
    //probably copied from stackoverflow
    string ConvertBoolArrToBinary(bool[] boolArr)
    {
        string result = "";
        for (int i = 0; i < boolArr.Length; i++)
        {
            if (boolArr[i])
            {
                result += "1";
            }
            else
            {
                result += "0";
            }
        }
        return result;
    }

}