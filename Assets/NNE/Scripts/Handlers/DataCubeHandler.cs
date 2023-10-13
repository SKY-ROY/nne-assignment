using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class ColorOrientationBinding
{
    [SerializeField] CodeColor colorID;
    public CodeColor ColorID => colorID;
    [SerializeField] GameObject objectToAlign;
    public GameObject ObjectToAlign => objectToAlign;
}

public class DataCubeHandler : MonoBehaviour
{
    [SerializeField] List<ColorOrientationBinding> bindings;
    [SerializeField] string url = "";

    Dictionary<CodeColor, ColorOrientationBinding> bindingMap;
    public Dictionary<CodeColor, ColorOrientationBinding> BindingMap => bindingMap;
    private List<CubeData> cubeData = new List<CubeData>();
    private Dictionary<CodeColor, CubeData> cubeDataMap;
    static DataCubeHandler instance;
    public static DataCubeHandler Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<DataCubeHandler>();
                return instance;
            }
            return instance;
        }
    }
    public static Action<CubeData> OnMetaDataUpdate;

    void Start()
    {
        bindingMap = new Dictionary<CodeColor, ColorOrientationBinding>();

        for (int i = 0; i < bindings.Count; i++)
        {
            bindingMap.Add(bindings[i].ColorID, bindings[i]);
        }
    }

    public void UpdateOrientation(CodeColor color, Transform alignmentObjectTransform, ref Vector3 resPos, ref Vector3 resRot)
    {
        // Rotational Alignment
        GameObject ObjectToOverlap = bindingMap[color].ObjectToAlign;

        Quaternion worldRotationDiff = alignmentObjectTransform.rotation * Quaternion.Inverse(bindingMap[color].ObjectToAlign.transform.rotation);
        Quaternion finalRot = worldRotationDiff * transform.rotation;

        transform.rotation = finalRot;

        resRot = finalRot.eulerAngles;

        // Positional Alignment
        Vector3 positionDiff = alignmentObjectTransform.position - bindingMap[color].ObjectToAlign.transform.position;
        Vector3 finalPos = transform.position + positionDiff;

        transform.position = finalPos;

        resPos = finalPos;
    }

    public void SimulateOrientation(Vector3 pos, Vector3 rot)
    {
        transform.rotation = Quaternion.Euler(rot);
        transform.position = pos;
    }

    public async Task ReflectMetaDataAsync(CodeColor color)
    {
        string fileContents = await DownloadFileAsync();
        Debug.Log("File contents:\n" + fileContents);

        cubeData = DataDeserializer.DeserializeDataList(fileContents);

        if (cubeData.Count > 0)
        {
            cubeDataMap = new Dictionary<CodeColor, CubeData>();
            for (int i = 0; i < cubeData.Count; i++)
            {
                cubeDataMap.Add(ConvertStringToColorCode(cubeData[i].Name), cubeData[i]);
            }
        }

        OnMetaDataUpdate?.Invoke(cubeDataMap[color]);
    }

    async Task<string> DownloadFileAsync()
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        UnityWebRequestAsyncOperation asyncOperation = www.SendWebRequest();

        while (!asyncOperation.isDone)
        {
            await Task.Yield();
        }

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error downloading file: " + www.error);
            return null;
        }
        else
        {
            return www.downloadHandler.text;
        }
    }

    CodeColor ConvertStringToColorCode(string color)
    {
        switch (color)
        {
            case "red":
            case "Red":
            case "RED":
                return CodeColor.Red;
            case "green":
            case "Green":
            case "GREEN":
                return CodeColor.Green;
            case "blue":
            case "Blue":
            case "BLUE":
                return CodeColor.Blue;
            case "yellow":
            case "Yellow":
            case "YELLOW":
                return CodeColor.Yellow;
        }
        return CodeColor.Black;
    }
}
/*

ID;Name;Info;Percentage (%);Last Edit;Detailed description
1;Red;A red cube;55.5;06-01-2008 07:47;Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat
2;Blue;A blue cube;34.5;06-01-2008;Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit
3;Green;A green cube;66.8;06-01-2008 00:00;sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur
4;Yellow;A yellow cube;44,3; 06/01/2008 00:00;Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum

*/