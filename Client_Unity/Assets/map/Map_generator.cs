using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Tilemaps;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEditor.Tilemaps;


public class Map : MonoBehaviour
{
    int[,] values;
    public string folderPath;
    public Tilemap tilemap;
    public TileBase tile;

    public void LoadMap(string fileContents)
    {
        string data = fileContents.Trim();
        string[] lines = data.Split('\n');
        values = new int[lines.Length, lines[0].Length * 2 - 1];
        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                switch (lines[i][j])
                {
                    case '|':
                        values[i, j] = values[i, values.GetLength(i) - j] = 1;
                        break;
                    case '.':
                        values[i, j] = values[i, values.GetLength(i) - j] = 0;
                        break;
                    case 'H':
                        values[i, j] = values[i, values.GetLength(i) - j] = 2;
                        break;
                    default:
                        values[i, j] = values[i, values.GetLength(i) - j] = 0;
                        break;
                }
            }
        }
    }
    void PopulateMap()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        string[] filePaths = Directory.GetFiles(folderPath, "*.txt");
        string randomFilePath = filePaths[Random.Range(0, filePaths.Length)];
        string fileContents = File.ReadAllText(randomFilePath);
        Debug.Log(fileContents);
        TilePalette palette = tilemap.GetComponent<TilemapEditorUserSettings>().tilePalette;
    }


}
