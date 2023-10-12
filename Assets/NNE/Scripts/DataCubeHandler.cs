using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public class ColorOrientationBinding
{
    [SerializeField] CodeColor colorID;
    public CodeColor ColorID => colorID;
    // [SerializeField] Vector3 eulerRotation;
    // public Vector3 EulerRotation => eulerRotation;
    [SerializeField] GameObject objectToAlign;
    public GameObject ObjectToAlign => objectToAlign;
}

public class DataCubeHandler : MonoBehaviour
{
    [SerializeField] List<ColorOrientationBinding> bindings;
    Dictionary<CodeColor, ColorOrientationBinding> bindingMap;
    public Dictionary<CodeColor, ColorOrientationBinding> BindingMap => bindingMap;

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

        // Debug.Log("----");
    }

    public void SimulateOrientation(Vector3 pos, Vector3 rot)
    {
        transform.rotation = Quaternion.Euler(rot);
        transform.position = pos;
    }
}
