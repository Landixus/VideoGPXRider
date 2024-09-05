using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class LayerSelector : MonoBehaviour
{
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;
    public int currentLayer;

    public KeyCode key;
    public KeyCode key2;
    // Use this for initialization

    private void Awake()
    {
        SelectLayer(0);
    }

    private void SelectLayer(int _index)
    {
        prevButton.interactable = (_index != 0);
        nextButton.interactable = (_index != transform.childCount - 1);

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == _index);
        }
    }
    public void ChangeLayer(int _change)
    {
        currentLayer += _change;
        SelectLayer(currentLayer);
    }

 
    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            prevButton.GetComponent<Button>().onClick.Invoke();
        }
        if (Input.GetKeyDown(key2))
        {
            nextButton.GetComponent<Button>().onClick.Invoke();
        }
    }
}
