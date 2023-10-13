using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CodeColor
{
    Red,
    Green,
    Blue,
    Black,
    Yellow
}


public class QRCodeHandler : MonoBehaviour
{
    [SerializeField] CodeColor codeColor;
    public CodeColor Color => codeColor;
}
