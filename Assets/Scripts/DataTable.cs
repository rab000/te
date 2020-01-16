
using System.Collections.Generic;

public class DataTable
{

    /// <summary>
    /// 第一个string 数据id，每行第一个字段，也是主键
    /// 第二个string 数据字段名,第一行每列的名称
    /// 第三个string 具体数据内容
    /// </summary>
    public Dictionary<string,Dictionary<string, string>> TableDataDic;

    /// <summary>
    /// 存字段名和对应的列
    /// </summary>
    public Dictionary<string, int> FieldNameDic;

    /// <summary>
    /// 存id及对应的行号
    /// </summary>
    public Dictionary<string, int> IDDic;

    public string[,] TableDataArray;

    /// <summary>
    /// 表名
    /// </summary>
    public string TableName;

    //行
    public int Row;

    //列
    public int Column;

    public DataTable(string name)
    {
        TableName = name;
        TableDataDic = new Dictionary<string, Dictionary<string, string>>();
        FieldNameDic = new Dictionary<string, int>();
        IDDic = new Dictionary<string, int>();
    }

    /// <summary>
    /// 读取二进制，存到TableDataDic中
    /// </summary>
    /// <param name="bs"></param>
    public void Load(byte[] bs)
    {

    }

    

}
