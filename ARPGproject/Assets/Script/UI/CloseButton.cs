using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseButton : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnClick()
    {
        UIManager.Instance.ActiveItemUI(false);
    }
}
