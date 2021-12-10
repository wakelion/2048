using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 资源管理类
/// </summary>
public class ResourceManager
{
    private static Dictionary<int, Sprite> spriteDIC;

     static  ResourceManager() 
    {
        spriteDIC = new Dictionary<int, Sprite>();
        Sprite[] spriteArray=Resources.LoadAll<Sprite>("2048Atlas");
        foreach (var item in spriteArray)
        {
            int SpriteName = int.Parse(item.name);
            spriteDIC.Add(SpriteName, item);
        }
    }

    public static Sprite GetImage(int number)
    {
        return spriteDIC[number];        
    }
}
