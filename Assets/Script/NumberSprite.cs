using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberSprite : MonoBehaviour
{
    private Image img;
    private void Awake()
    {
        img = GetComponent<Image>();
    }
    public void SetImage(int number)
    {
        img.sprite = ResourceManager.GetImage(number);
    }

    public void CreateEffect()
    {
        //小---大(需要导入缓动组件库)
        iTween.ScaleFrom(gameObject,Vector3.zero,0.3f);
    }
}
