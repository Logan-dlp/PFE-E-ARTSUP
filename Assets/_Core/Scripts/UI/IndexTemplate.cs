using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class IndexTemplate
{
    public Image itemImage;
    public Image frameImage;
    public Image elementImage;
    public TMP_Text title;
    public TMP_Text description;
    public Image actionUI1;
    public Image actionUI2;
    public GameObject actionArrow;
    public Sprite[] frameElementArray;
    public Sprite[] elementArray;
    public Sprite[] actionSpriteArray;
}