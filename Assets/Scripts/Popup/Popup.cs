using TMPro;
using UnityEngine;

public abstract class Popup : MonoBehaviour
{
    public TMP_Text Content;

    public abstract void FillContent(string content);
}
