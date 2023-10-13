using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text idText;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text infoText;
    [SerializeField] TMP_Text percentageText;
    [SerializeField] TMP_Text lastEditText;
    [SerializeField] TMP_Text detailedDescriptionText;

    static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<UIManager>();
                return instance;
            }
            return instance;
        }
    }

    void OnEnable()
    {
        DataCubeHandler.OnMetaDataUpdate += OnMetaDataUpdateHandler;
    }

    void OnDisable()
    {
        DataCubeHandler.OnMetaDataUpdate += OnMetaDataUpdateHandler;
    }

    void OnMetaDataUpdateHandler(CubeData data)
    {
        idText.text = data.ID.ToString();
        nameText.text = data.Name.ToString();
        infoText.text = data.Info.ToString();
        percentageText.text = data.Percentage.ToString();
        lastEditText.text = data.LastEdit.ToString();
        detailedDescriptionText.text = data.DetailedDescription.ToString();
    }
}
