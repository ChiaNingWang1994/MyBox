using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox.Util;

[System.Serializable]
public struct Chunkpos
{
    public int x;
    public int y;
    public int z;
    public List<byte> blockType;
}


//这个类决定了最终存储的数据类型
[System.Serializable]
public class SaveBuild {

    //存档id号，唯一
    public string mapID;

    //chunk的坐标，也即是唯一标识符key
    //本身为vector3i对象，即x，y，z
    public List<Chunkpos> chunkkey = new List<Chunkpos>();

    //chunk中block的位置坐标也是key
    //public List<Blockpos> blockkey = new List<Blockpos>();
    //chunk中每个block的值
    //public List<byte> blockType = new List<byte>();

    //public byte blockType = 0; 

	
}
