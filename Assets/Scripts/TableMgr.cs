using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 用于存储所有导入的配表
/// </summary>
public class TableMgr
{
    static IoBuffer buffer = new IoBuffer(1024000);

    public static Dictionary<string, DataTable> TableDic = new Dictionary<string, DataTable>();

    public static void CreateTable(string tableName)
    {       
        if (!TableDic.ContainsKey(tableName))
        {
            DataTable table = new DataTable(tableName);
        }
    }

    public static void LoadDataTable(string tablePath)
    {
        byte[] bs = FileHelper.ReadBytesFromFile(tablePath);

        buffer.Clear();

        buffer.PutBytes(bs);

        string tableName = buffer.GetString();

        DataTable dataTable = new DataTable(tableName);

        if (TableDic.ContainsKey(tableName))
        {
            Debug.LogError("TableMgr.LoadDataTable table：" + tableName + "已存在，导入失败");
            return;
        }

        dataTable.Column = buffer.GetInt();

        dataTable.Row = buffer.GetInt();

        Debug.LogError("TableMgr.LoadDataTable tableName:" + tableName + " row:" + dataTable.Row + " colomn:" + dataTable.Column);

        dataTable.TableDataArray = new string[dataTable.Row, dataTable.Column];

        for (int i = 0; i < dataTable.Row; i++)
        {
            for (int j = 0; j < dataTable.Column; j++)
            {
                string fieldContent = buffer.GetString();

                dataTable.TableDataArray[i, j] = fieldContent;

                Debug.Log("TableMgr.LoadDataTable i:" + i + " j:" + j + " fieldContent:" + fieldContent);
            }
        }

        for (int i = 0; i < dataTable.Column; i++)
        {
            //Debug.LogError("nani-->i:"+i+" "+ dataTable.TableDataArray[0, i]);
            dataTable.FieldNameDic.Add(dataTable.TableDataArray[0,i], i);
        }

        for (int i = 0; i < dataTable.Row; i++)
        {
            dataTable.IDDic.Add(dataTable.TableDataArray[i, 0], i);
        }

        TableDic.Add(tableName, dataTable);

    }

    /// <summary>
    /// 载入一个数据表
    /// </summary>
    /// <param name="tablePath"></param>
    public static void LoadDataTable1(string tablePath)
    {
        byte[] bs = FileHelper.ReadBytesFromFile(tablePath);

        IoBuffer buffer = new IoBuffer(1024000);

        buffer.PutBytes(bs);

        string tableName = buffer.GetString();

        DataTable dataTable = new DataTable(tableName);

        if (TableDic.ContainsKey(tableName))
        {
            Debug.LogError("TableMgr.LoadDataTable table："+ tableName+"已存在，导入失败");
            return;
        }

        dataTable.Row = buffer.GetInt();

        dataTable.Column = buffer.GetInt();

        Debug.LogError("TableMgr.LoadDataTable tableName:"+tableName+" row:"+dataTable.Row+" colomn:"+dataTable.Column);

        for (int i = 0; i < dataTable.Row; i++)
        {
            //字段名
            string fieldName = buffer.GetString();

            if (!dataTable.TableDataDic.ContainsKey(fieldName))
            {
                dataTable.TableDataDic.Add(fieldName, new Dictionary<string, string>());
            }
            
            for (int j = 0; j < dataTable.Column; j++)
            {
                //id
                string id = buffer.GetString();
                //字段内容
                string content = buffer.GetString();

                Debug.LogError("TableMgr.LoadDataTable 添加 tableName:"+tableName+" fieldName:"+fieldName+" id:"+id+" content:"+content);

                dataTable.TableDataDic[fieldName].Add(id,content);
            }

        }

    }

    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"> 数据id，每行第一个字段，也是主键</param>
    /// <param name="field"> 数据字段名,第一行每列的名称</param>
    public static string Get(string tableName, string id, string fieldName)
    {
        DataTable table = TableDic[tableName];

        int column = table.FieldNameDic[fieldName];

        int row = table.IDDic[id];

        return TableDic[tableName].TableDataArray[row, column];
                       
    }

}
