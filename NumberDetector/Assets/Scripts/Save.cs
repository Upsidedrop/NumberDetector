using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Save : MonoBehaviour
{
    [SerializeField]
    Tilemap tilemap;
    [SerializeField]
    TileBase emptyTile;
    bool[] imgBool;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SaveCurrentImage();
        }
    }
    void SaveCurrentImage()
    {
        imgBool = new bool[784];
        for (int i = 0; i < 28; i++)
        {
            for (int i1 = 0; i1 < 28; i1++)
            {
                if (tilemap.GetTile(new(i - 14, i1 - 14)) == emptyTile)
                {
                    imgBool[i * 28 + i1] = false;
                }
                else
                {
                    imgBool[i * 28 + i1] = true;
                }
            }
        }
        Image image = new Image();

        List<Image> tempImageList = new List<Image>();

        for (int i = 0; i < imgBool.Length; i++)
        {
            image.imgBool[i] = imgBool[i];

        }

        image.correctOutput = 42;
        // Load existing JSON data into a list
        if (File.Exists(Application.dataPath + "/saveFile.json"))
        {
            string json = File.ReadAllText(Application.dataPath + "/saveFile.json");
            tempImageList = JsonUtility.FromJson<JsonableListWrapper<Image>>(json).list;

        }
        // Add new data to the list
        print(tempImageList == null);        
        tempImageList.Add(image);

        JsonableListWrapper<Image> wrappedList = new(tempImageList);
        // Save the updated list back to the JSON file
        string listAsJson = JsonUtility.ToJson(wrappedList);
        print(listAsJson);
        File.WriteAllText(Application.dataPath + "/saveFile.json", listAsJson);
    }
    [Serializable]
    private class Image
    {
        public bool[] imgBool = new bool[784];
        public int correctOutput;
    }
}
[Serializable]
public class JsonableListWrapper<T>
{
    public List<T> list;
    public JsonableListWrapper(List<T> list)
    {
        this.list = list;
    }
    /*
    //For future reference
    List<string> stringList = new List<string>(){"one","two","three"};
    // To Json
    string stringListAsJson = JsonUtility.ToJson(new JsonableListWrapper<string>(stringList));
    // From Json
    List<string> stringListFromJson = JsonUtility.FromJson<JsonableListWrapper<string>>(stringListAsJson).list;
    */
}
