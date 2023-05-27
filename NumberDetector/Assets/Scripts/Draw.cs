using UnityEngine;
using UnityEngine.Tilemaps;

public class Draw : MonoBehaviour
{
    [SerializeField]
    Tilemap tilemap;
    [SerializeField]
    TileBase tile;
    [SerializeField]
    TileBase emptyTile;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            MouseDraw();
        }
        if (Input.GetButtonDown("Erase"))
        {
            Erase();
        }
    }
    void MouseDraw()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3Int tilePosition = tilemap.WorldToCell(worldPosition);
        tilemap.SetTile(tilePosition, tile);
    }
    public void Erase()
    {
        for (int i = 0; i < 28; i++)
        {
            for (int i1 = 0; i1 < 28; i1++)
            {
                tilemap.SetTile(new(i -14, i1 -14), emptyTile);
            }
        }
    }
}
