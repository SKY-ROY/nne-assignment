using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Serializable]
public class CubeData
{
    public int ID;
    public string Name;
    public string Info;
    public float Percentage;
    public string LastEdit;
    public string DetailedDescription;
}

public static class DataDeserializer
{
    public static List<CubeData> DeserializeDataList(string data)
    {
        List<CubeData> cubeDataList = new List<CubeData>();
        string[] lines = data.Split('\n');
        Debug.Log($"lines: {lines.Length}");

        for (int i = 1; i < lines.Length - 1; i++) // Start from 1 to skip the header
        {
            string[] values = lines[i].Split(';');
            Debug.Log($"line[{i}] values: {values.Length} : {lines.Length}");

            CubeData cubeData = DeserializeSingleData(values);
            cubeDataList.Add(cubeData);
        }

        Debug.Log("For loop exit");
        return cubeDataList;
    }

    private static CubeData DeserializeSingleData(string[] values)
    {
        CubeData cubeData = new CubeData();

        cubeData.ID = int.Parse(values[0]);
        cubeData.Name = values[1];
        cubeData.Info = values[2];
        cubeData.Percentage = float.Parse(values[3]);
        cubeData.LastEdit = values[4];//DateTime.ParseExact(values[4], "dd-MM-yyyy HH:mm", null);
        cubeData.DetailedDescription = values[5];

        Debug.Log($"{cubeData.ID}; {cubeData.Name}; {cubeData.Info}; {cubeData.Percentage}; {cubeData.LastEdit}; {cubeData.DetailedDescription};");

        return cubeData;
    }
}
