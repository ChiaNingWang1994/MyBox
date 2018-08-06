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
        Block dirt = new Block(1, "Dirt", 2, 15);//先添加一个土块对象
        blocks.Add(dirt.id, dirt);

        Block grass = new Block(2, "Grass", 3, 15, 0, 15, 2, 15);//草地对象
        blocks.Add(grass.id, grass);

        Block rock = new Block(3, "Rock", 1, 15);//石块对象
        blocks.Add(rock.id, rock);

        Block sand = new Block(4, "Sand", 0, 12);//砂石对象
        blocks.Add(sand.id, sand);

        Block snow = new Block(5, "Snow", 4, 11, 2, 11, 2, 15);//雪地对象
        blocks.Add(snow.id, snow);

        Block brick = new Block(6, "Brick", 5, 15, 6, 15);//灰砖对象
        blocks.Add(brick.id, brick);

        Block redbrick = new Block(7, "Redbrick", 7, 15);//红砖对象
        blocks.Add(redbrick.id, redbrick);

        //Block glass = new Block(8, "Glass", 3, 11);//玻璃对象//失败，需要更改shader
        //blocks.Add(glass.id, glass);

        //Block glassframe = new Block(9, "Glassframe", 1, 12);//玻璃框对象//失败，需要更改shader
        //blocks.Add(glassframe.id, glassframe);


        Block bedrock = new Block(10, "Bedrock", 0, 14 );//基岩对象
        blocks.Add(bedrock.id, bedrock);
    }

    public static Block GetBlock(byte id)//id取对象函数
    {
        return blocks.ContainsKey(id) ? blocks[id] : null;
    }
}

