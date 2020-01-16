using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Excel;
using System.Data;
using OfficeOpenXml;

//小问题，xlsx里有空格，行数就会发生变化，所以需要写个方法来检查excel数据格式

public class ExcelEditor 
{

    private static IoBuffer Buffer = new IoBuffer(1024000);

    //存放原始excel配表的路径
    private static string ExcelFolderPath = "Assets/Excels";

    /// <summary>
    /// 到处选中的excel
    /// </summary>
    [MenuItem("te/生成选中excel数据表")]
    public static void ExportExcelSelected()
    {

        //Debug.LogError("开始处理");

        Object[] obs = Selection.GetFiltered<Object>(SelectionMode.Assets);

        for (int i = 0; i < obs.Length; i++)
        {
            string filePath = AssetDatabase.GetAssetPath(obs[i]);

            Debug.Log("ExcelEditor.ExportExcelSelected 开始导出选中excel--->" + filePath);

            string absPath = EditorHelper.ChangeToAbsolutePath(filePath);

            LoadData(absPath);

            //载入一张表
            //DataTable dataTable = LoadData(absPath);

            //转存一张表到bytes
            //SaveDataTable2Bytes(dataTable);

        }

    }
        
    /// <summary>
    /// 到处所有excel
    /// </summary>
    [MenuItem("te/生成所有excel数据表")]
    public static void ExportAllExcel()
    {
        string[] allExcelPaths = EditorHelper.GetSubFilesPaths(EditorHelper.ChangeToAbsolutePath(ExcelFolderPath));

        for (int i = 0; i < allExcelPaths.Length; i++)
        {
            if (allExcelPaths[i].EndsWith(".meta")) continue;

            LoadData(allExcelPaths[i]);

        }
    }

    public static void LoadData(string xlsxPath)
    {
        Debug.Log("ExcelEditor.LoadData xlsxPath--->" + xlsxPath);

        string tableName = EditorHelper.GetFileNameFromPath(xlsxPath, true);

        FileStream fileStream = File.Open(xlsxPath, FileMode.Open, FileAccess.Read);

        IExcelDataReader excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);

        //读取表格
        DataSet excelDataSet = excelDataReader.AsDataSet();

        Buffer.Clear();

        Buffer.PutString(tableName);

        int column = excelDataSet.Tables[0].Columns.Count;

        int row = excelDataSet.Tables[0].Rows.Count;

        Buffer.PutInt(column);

        Buffer.PutInt(row);

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                string content = excelDataSet.Tables[0].Rows[i][j].ToString();

                Debug.Log("ExcelEditor.LoadData i:"+i+" j:"+j+" content:"+content);

                Buffer.PutString(content);
            }
        }

        byte[] bs = Buffer.ToArray();

        string outPutPath = EditorHelper.OUTPUT_ROOT_PATH + "/" + tableName + ".n";

        FileHelper.WriteBytes2File(outPutPath, Buffer.ToArray());

    }


    /// <summary>
    /// list内容格式
    /// 赵一|党员|1年|赵一.png| 
    /// </summary>
    /// <param name="newList"></param>
    //public static  void WriteExcel(List<string> newList)
    //{
    //    //自定义excel的路径

    //    string path = Application.streamingAssetsPath + "/党员信息.xlsx";
    //    // print(Application.dataPath);

    //    FileInfo newFile = new FileInfo(path);

    //    if (newFile.Exists)
    //    {
    //        //创建一个新的excel文件

    //        newFile.Delete();

    //        newFile = new FileInfo(path);
    //    }

    //    //通过ExcelPackage打开文件
    //    using (ExcelPackage package = new ExcelPackage(newFile))
    //    {
    //        //在excel空文件添加新sheet

    //        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("message");
    //        //添加列名

    //        worksheet.Cells[1, 1].Value = "姓名";

    //        worksheet.Cells[1, 2].Value = "职务";

    //        worksheet.Cells[1, 3].Value = "党龄";

    //        worksheet.Cells[1, 4].Value = "图片名";

    //        for (int i = 0; i < newList.Count; i++)
    //        {
    //            string[] messages = newList[i].Split('|'); //赵一|党员|1年|赵一.png| 
    //            string itemName = messages[0];
    //            string itemWork = messages[1];
    //            string itemYear = messages[2];
    //            string imageName = messages[3];
    //            //添加一行数据

    //            int num = i + 2;

    //            worksheet.Cells["A" + num].Value = itemName;

    //            worksheet.Cells["B" + num].Value = itemWork;

    //            worksheet.Cells["C" + num].Value = itemYear;

    //            worksheet.Cells["D" + num].Value = imageName;
    //        }

    //        //保存excel
    //        package.Save();
    //        Debug.LogError("写入完成");
    //    }
    //}

}
