using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour,IPointerDownHandler,IDragHandler
{
    private GameCore Core;
    private NumberSprite[,] spriteActionArray;
    private void Start()
    {
        Core = new GameCore();
        spriteActionArray = new NumberSprite[4, 4];
        Init();
        GenerateNumber();
    }
    private void Update()
    {
        if (Core.IsChange) 
        {
            UpdateMap();
            GenerateNumber();
            if (Core.IsOver())
            { 
                
            }
        }
        Core.IsChange = false;
    }

    private void UpdateMap()
    {
        for (int r = 0; r < 4; r++)
        {
            for (int c = 0; c < 4; c++)
            {
                spriteActionArray[r, c].SetImage(Core.Map[r,c]);
            }
        }
    }

    private void Init()
    {
        for (int r = 0; r < 4; r++)
        {
            for (int c = 0; c < 4; c++)
            {
                spriteActionArray[r,c]= CreateSprite(r,c);
            }
        }
    }

    private NumberSprite CreateSprite(int r,int c)
    {
        GameObject go = new GameObject(r.ToString() + c.ToString());
        go.AddComponent<Image>();
        go.transform.SetParent(this.transform, false);
        NumberSprite sprite = go.AddComponent<NumberSprite>();
        sprite.SetImage(0);
        return sprite;
    }

    //生成新数字
    private void GenerateNumber()
    {
        Location loc;
        int number;
        Core.GenerateNumber(out loc,out number);
        spriteActionArray[loc.RIndex, loc.CIndex].SetImage(number);
        spriteActionArray[loc.RIndex, loc.CIndex].CreateEffect();
    }

    private Vector2 beginPoint;
    private bool isDown = false;
    public void OnPointerDown(PointerEventData eventData)
    {
        beginPoint = eventData.position;
        isDown = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDown) return;
        //偏移的向量
        Vector3 offSet = eventData.position - beginPoint;
        float x = Mathf.Abs(offSet.x);
        float y = Mathf.Abs(offSet.y);

        MoveDirection? dir = null;
        if (x>y &&  x>=50) 
        {
            dir = offSet.x > 0 ? MoveDirection.Right : MoveDirection.Left;
        }
        if (x<y && y>=50)
        {
            dir = offSet.y > 0 ? MoveDirection.Up : MoveDirection.Down;
        }

        if (dir != null)
        {
            Core.Move(dir.Value);
            isDown = false;
        }
    }
}
