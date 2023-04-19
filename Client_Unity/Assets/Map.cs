using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Map_generator : MonoBehaviour
{
    [SerializeField]
    public int [,] values;
    public string folderPath;
    void LoadMap(string fileContents){
        string data=fileContents.Trim();
        string[] lines = data.Split("\n");
        values= new int[lines.Length,lines[0].Length*2];
        for (int i = 0; i < lines.Length; i++)
        {
            Debug.Log(lines[i]);
            for (int j = 0; j < lines[i].Length; j++){
                switch (lines[i][j])
                {
                    case '|':
                        values[i,j] = 1;
                        break;
                    case '.':
                        values[i,j] = 0;
                        break;
                    case 'H':
                        values[i,j] = 2;
                        break;
                    default:
                        values[i,j] = 0;
                        break;
                }
            }
        }
    }
    
    void Start()
    {
        string[] filePaths = Directory.GetFiles(folderPath, "*.txt");
        string randomFilePath = filePaths[Random.Range(0, filePaths.Length)];
        string fileContents = File.ReadAllText(randomFilePath);
        Debug.Log(fileContents);
        LoadMap(fileContents);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
