using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class containing sentences used for dialog
/// </summary>
[System.Serializable]
public class Dialogue
{
    [TextArea(3,10)]
    public string[] sentences;

}
