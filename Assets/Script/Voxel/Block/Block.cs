using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


//方块方向枚举
public enum BlockDirection : byte
{
    Front = 0,
    Back = 1,
    Left = 2,
    Right = 3,
    Top = 4,
    Bottom = 5
}

//方块对象，存储方块信息
public class Block
{
    //方块ID
    public byte id;

    //方块名称
    public string name;

    //方块朝向
    public BlockDirection direction = BlockDirection.Front;

    //贴图坐标
    //前
    public byte textureFrontX;
    public byte textureFrontY;
    //后
    public byte textureBackX;
    public byte textureBackY;
    //左
    public byte textureLeftX;
    public byte textureLeftY;
    //右
    public byte textureRightX;
    public byte textureRightY;
    //上
    public byte textureTopX;
    public byte textureTopY;
    //下
    public byte textureBottomX;
    public byte textureBottomY;

    //接下来是不同的方块
    //分几种

    //全相同的砖块
    public Block(byte id, string name, byte textureX, byte textureY)
    {
        InitBlock(id, name, textureX, textureY, textureX, textureY,
         textureX, textureY, textureX, textureY,
         textureX, textureY, textureX, textureY);
    }
    //草地？上面为一种，其他面为另一种
    public Block(byte id, string name, byte textureX, byte textureY, byte textureTopX, byte textureTopY)
    {
        InitBlock(id, name, textureX, textureY, textureX, textureY,
         textureX, textureY, textureX, textureY,
         textureTopX, textureTopY, textureX, textureY);
    }
    //上面为一，下面为二，其他面为三
    public Block(byte id, string name, byte textureX, byte textureY,
        byte textureTopX, byte textureTopY, byte textureBottomX, byte textureBottomY)
    {
        InitBlock(id, name, textureX, textureY, textureX, textureY,
         textureX, textureY, textureX, textureY,
         textureTopX, textureTopY, textureBottomX, textureBottomY);
    }

    //上面为一，下面为二，前面为三，其他面为四
    public Block(byte id, string name, byte textureX, byte textureY,
        byte textureFrontX, byte textureFrontY,
        byte textureTopX, byte textureTopY, byte textureBottomX, byte textureBottomY)
    {
        InitBlock(id, name, textureFrontX, textureFrontY, textureX, textureY,
         textureX, textureY, textureX, textureY,
         textureTopX, textureTopY, textureBottomX, textureBottomY);
    }

    //都不一样的方块
    public Block(byte id, string name, byte textureFrontX, byte textureFrontY, byte textureBackX, byte textureBackY,
        byte textureLeftX, byte textureLeftY, byte textureRightX, byte textureRightY,
        byte textureTopX, byte textureTopY, byte textureBottomX, byte textureBottomY)
    {
        InitBlock(id, name, textureFrontX, textureFrontY, textureBackX, textureBackY,
         textureLeftX, textureLeftY, textureRightX, textureRightY,
         textureTopX, textureTopY, textureBottomX, textureBottomY);
    }

    //工具构造函数
    private void InitBlock(byte id, string name, byte textureFrontX, byte textureFrontY, byte textureBackX, byte textureBackY,
        byte textureLeftX, byte textureLeftY, byte textureRightX, byte textureRightY,
        byte textureTopX, byte textureTopY, byte textureBottomX, byte textureBottomY)
    {
        this.id = id;
        this.name = name;

        this.textureFrontX = textureFrontX;
        this.textureFrontY = textureFrontY;

        this.textureBackX = textureBackX;
        this.textureBackY = textureBackY;

        this.textureLeftX = textureLeftX;
        this.textureLeftY = textureLeftY;

        this.textureRightX = textureRightX;
        this.textureRightY = textureRightY;

        this.textureTopX = textureTopX;
        this.textureTopY = textureTopY;

        this.textureBottomX = textureBottomX;
        this.textureBottomY = textureBottomY;
    }

}



