using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = System.Random;

public class GameCore
{

    private int[,] map;
    private int[] mergeArray;
    private int[] removeZeroArray;
    private int[,] originalMap;
    public bool IsChange { get; set; }

    public int[,] Map
    {
        get { return map; }
    }
    public GameCore()
    {
        //实例化4*4
        map = new int[4, 4];
        //new 数组  每行有多少个
        mergeArray = new int[map.GetLength(0)];
        //去零数组
        removeZeroArray = new int[4];
        //布局
        emptyLOC = new List<Location>(16);
        //随机数
        random = new Random();
        //原来的二维数组
        originalMap = new int[4, 4];
    }

    public void Move(MoveDirection Direction)
    {
        Array.Copy(map, originalMap, map.Length);
        IsChange = false;
        switch (Direction) 
        {
            case MoveDirection.Up:MoveUp();break;
            case MoveDirection.Down:MoveDown();break;
            case MoveDirection.Left: MoveLeft(); break;
            case MoveDirection.Right: MoveRight(); break;
        }
        CheckMapChange();
    }

    private void CheckMapChange()
    {
        for (int r = 0; r < map.GetLength(0); r++)
        {
            for (int c = 0; c < map.GetLength(1); c++)
            {
                if (map[r,c]!= originalMap[r,c])
                {
                    IsChange = true;
                }
            }
        }
    }

    private void Merge() 
    {
        RemoveZero();
        for (int index = 0; index < mergeArray.Length -1 ; index++)
        {
            if (mergeArray[index] !=0 && mergeArray[index] == mergeArray[index + 1]) 
            {
                mergeArray[index] += mergeArray[index + 1];
                mergeArray[index + 1] = 0;
            }
        }
        RemoveZero();
    }

    private void RemoveZero()
    {
        Array.Clear(removeZeroArray, 0, 4);
        int index = 0;
        for (int i = 0; i < mergeArray.Length; i++)
        {
            if (mergeArray[i]!=0) 
            {
                removeZeroArray[index++] = mergeArray[i];
            }
        }
        removeZeroArray.CopyTo(mergeArray, 0);
    }

    //2 2 0 0 --> 4 0 0 0  正常情况
    //2 2 2 0 --> 4 0 2 0 --> 4 2 0 0  情况1 ：需要先合并后去零
    //2 0 2 0 --> 2 2 0 0 --> 4 0 0 0  情况2 ：需要合并前去零
    //上移
    //从上到下获取列数据，形成一维数组
    //合并数据:(合并逻辑：先去零 再合并 再去零）
    //		去零: 将0元素移到末尾				
    //		相邻相同，则合并（将后一个元素累加到前一个元素上，后一个元素清零）
    //		去零：将0元素移到末尾
    // 将 列数组元素还原至原列
    private void MoveUp() 
    {
        for (int c = 0; c < map.GetLength(1); c++)
        {
            for (int r = 0; r < map.GetLength(0); r++)
            {
                mergeArray[r] = map[r, c];
            }
            Merge();

            for (int r = 0; r < map.GetLength(0); r++)
            {
                map[r, c] = mergeArray[r];
            }
        }    
    }

    //下移1.0
    //从上到下获取列数据，形成一维数组
    //合并数据:(合并逻辑：先去零 再合并 再去零）
    //		去零: 将0元素移到开头				
    //		相邻相同，则合并（将前一个元素累加到后一个元素上，前一个元素清零）
    //		去零：将0元素移到开头
    // 将 列数组元素还原至原列

    //下移2.0  （ 取数据顺序变一下 ，之后就完全复用 上移方法）
    //从下到上获取列数据，形成一维数组
    //合并数据:(合并逻辑：先去零 再合并 再去零）
    //		去零: 将0元素移到末尾				
    //		相邻相同，则合并（将后一个元素累加到前一个元素上，后一个元素清零）
    //		去零：将0元素移到末尾
    // 将 列数组元素还原至原列
    private void MoveDown()
    {
        for (int c = 0; c < map.GetLength(1); c++)
        {
            for (int r = map.GetLength(0)-1; r >= 0; r--)
            {
                mergeArray[3 - r] = map[r, c];
            }
            Merge();

            for (int r = map.GetLength(0)-1; r >= 0; r--)
            {
                map[r, c] = mergeArray[3 - r];
            }
        }
    }

    //2 0 2 0 -->2 2 0 0 --> 4 0 0 0
    private void MoveLeft()
    {
        for (int r = 0; r < map.GetLength(0); r++)
        {
            for (int c = 0; c < map.GetLength(1); c++)
            {
                mergeArray[c] = map[r, c];
            }
            Merge();
            for (int c = 0; c < map.GetLength(1); c++)
            {
                map[r, c] = mergeArray[c];
            }
        }
    }

    private void MoveRight()
    {
        for (int r = 0; r < map.GetLength(0); r++)
        {
            for (int c = map.GetLength(1) -1 ; c >= 0; c--)
            {
                mergeArray[3 - c] = map[r, c];
            }
            Merge();
            for (int c = map.GetLength(1)-1; c >=0 ; c--)
            {
                map[r, c] = mergeArray[3 - c];
            }
        }
    }

    /*
        在空白位置上， 随机生成数字(2 (90%)     4(10%))
    * 1.计算空白位置
    * 2.随机选择位置
    * 3.随机生成数字
    */
    private List<Location> emptyLOC;//布局
       
    private void CalculateEmpty()
    {
        emptyLOC.Clear();
        for (int r = 0; r < map.GetLength(0); r++)
        {
            for (int c = 0; c < map.GetLength(1); c++)
            {
                if (map[r,c] == 0) 
                {
                    emptyLOC.Add(new Location(r,c));
                }
            }
        }
    }

    private Random random;
    /// <summary>
    /// 生成新数字
    /// </summary>
    public void GenerateNumber(out Location loc, out int newNumber)
    {
        CalculateEmpty();
        if (emptyLOC.Count > 0)
        {
            int emptyLocIndex = random.Next(0, emptyLOC.Count);
            loc = emptyLOC[emptyLocIndex];
            newNumber = map[loc.RIndex, loc.CIndex] = random.Next(0, 10) == 1 ? 4 : 2;
            emptyLOC.RemoveAt(emptyLocIndex);
        }
        else
        {
            newNumber = -1;
            loc = new Location(-1,-1);
        }
    }

    public bool IsOver()
    {
        if (emptyLOC.Count > 0) return false;
        for (int r = 0; r < 4; r++)
        {
            for (int c = 0; c < 4; c++)
            {
                if (map[r, c] == map[r, c + 1] || map[c, r] == map[c + 1, r])
                    return false;
            }
        }
        return true;
    }

}
