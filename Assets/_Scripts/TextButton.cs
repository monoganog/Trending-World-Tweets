using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TextButton : MonoBehaviour
{
    public Canvas canvas;
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        canvas.worldCamera = Camera.main;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        Debug.Log("PLEASXEEEeeee");
    }

    private void OnMouseEnter()
    {
        Debug.Log("enter text port11111111111111111111111111111111111111111111111ion");
        text.DOColor(new Color(255, 0, 255, 100), 0.2f);
    }
    

    private void OnMouseExit()
    {
        text.DOColor(new Color(255, 255, 255, 255), 0.2f);
    }
}
