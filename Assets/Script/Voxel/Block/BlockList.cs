using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

//存储所有的Block对象信息
public class BlockList:MonoBehaviour
{
    public static Dictionary<byte, Block> blocks = new Dictionary<byte, Block>();
    void Awake()
    {
        Block dirt = new Block(1, "Dirt", 2, 31);//先添加一个土块对象
        blocks.Add(dirt.id, dirt);

        Block grass = new Block(2, "Grass", 3, 31, 0, 31, 2, 31);//草地对象
        blocks.Add(grass.id, grass);
    }

    public static Block GetBlock(byte id)//id取对象函数
    {
        return blocks.ContainsKey(id) ? blocks[id] : null;
    }
}

