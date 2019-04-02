using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArchSetup : MonoBehaviour
{
  

    public enum typeContentOption { TEXT, ICON };
    public typeContentOption typeContentList;


   
    public string text;
    public Sprite image;

    private TextMeshPro _textButton;
    private Image _imageButton;


    private void Awake()
    {
        _textButton = transform.Find("CanvasItems/Text").GetComponent<TextMeshPro>();
        _imageButton = transform.Find("CanvasItems/Image").GetComponent<Image>();
    }

    private void Update()
    {
        _textButton.text = text;
        _imageButton.sprite = image;

        if (typeContentList == typeContentOption.TEXT)
        {
            _imageButton.enabled = false;
            _textButton.enabled = true;
        }
        else
        {
            _imageButton.enabled = true;
            _textButton.enabled = false;
        }
    }

}
