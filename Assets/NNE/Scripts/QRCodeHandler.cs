using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CodeColor
{
    Red,    // 90,0,0
    Green,  // 0,90,0
    Blue,   // -90,0,0
    Black  // 0,-90,0
}


public class QRCodeHandler : MonoBehaviour
{
    [SerializeField] CodeColor codeColor;
    public CodeColor Color => codeColor;
}
