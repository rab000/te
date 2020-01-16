using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class Demo : MonoBehaviour
{
   
    void Start()
    {



        //载入所有配表
        string dataTableFolderPath = Path.GetDirectoryName(Application.dataPath) + "/data";
        
        string[] allFilePaths = Directory.GetFiles(dataTableFolderPath);

        for (int i = 0; i < allFilePaths.Length; i++)
        {
            TableMgr.LoadDataTable(allFilePaths[i]);
        }

        string s = TableMgr.Get("test1", "A2","D1");
        Debug.Log("demo result---->" + s);


        s = TableMgr.Get("test1", "A2", "C1");
        Debug.Log("demo result---->" + s);
    }

    

}
