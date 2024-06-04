using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectOnStart : MonoBehaviour
{
    // Start is called before the first frame update
    private EventSystem eventSystem;
    [SerializeField] private GameObject firstButton;

    void Start()
    {
        
    }

    private void OnEnable()
    {
        eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(firstButton);
    }


}
