using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox.Voxel;
using MyBox.Util;

public class BlockControl : MonoBehaviour {

    private GameObject play;
    private int layerMask;
    Chunk nowChunk;
    public float maxDistance;
    private bool godDistance;

    // Use this for initialization
    void Start () {
        play = GameObject.Find("FirstPersonCharacter");
        layerMask = LayerMask.GetMask("Chunk");
        maxDistance = 3.0f;
        godDistance = false;
    }
	
	// Update is called once per frame
	void Update () {

        
        Ray ray = new Ray(play.transform.position, play.transform.forward);
        RaycastHit hitInfo;
        if (Input.GetKeyDown(KeyCode.F1))//上帝无限远距离
        {
            godDistance = !godDistance;
            if (godDistance) maxDistance = Mathf.Infinity;
            else maxDistance = 3.0f;
        }

        if (Physics.Raycast(ray, out hitInfo, maxDistance, layerMask))
        {
            Debug.DrawLine(play.transform.position, hitInfo.point, Color.red);

            if (hitInfo.distance > 0.0f && hitInfo.distance <= maxDistance)//过远则不操作
            {
                //print(hitInfo.point);
                int xc = Chunk.width * Mathf.FloorToInt(hitInfo.point.x / Chunk.width);
                int yc = Chunk.height * Mathf.FloorToInt(hitInfo.point.y / Chunk.height);
                int zc = Chunk.width * Mathf.FloorToInt(hitInfo.point.z / Chunk.width);
                Vector3i chunkpos = new Vector3i(xc, yc, zc);
                //print(chunkpos);
                //GameObject nowChunk= Map.instance.GetChunk(chunkpos);//通过坐标获取到当前chunk
                nowChunk = Map.Instance.GetChunk(chunkpos);//通过坐标获取到当前chunk
                                                            //通过坐标及当前chunk找到当前block
                int xb = Mathf.FloorToInt(hitInfo.point.x);
                int yb = Mathf.FloorToInt(hitInfo.point.y);
                int zb = Mathf.FloorToInt(hitInfo.point.z);
                Vector3i blockpos = new Vector3i(xb, yb, zb);


                //print(blockpos);

                //delete block
                if (Input.GetKeyDown("e"))
                {
                    //print(chunkpos);
                    blockpos = nowChunk.getBlock(blockpos);
                    //print(blockpos);
                    blockpos = modifyBlock(hitInfo, blockpos, ref nowChunk);//整合修正块坐标及处理chunk边界
                                                                            //print(nowChunk.position);
                                                                            //print(blockpos);
                    nowChunk.setDeleteBlock(blockpos);
                }

                //add block
                if (Input.GetKeyDown("q"))
                {
                    //print(chunkpos);
                    blockpos = nowChunk.getBlock(blockpos);
                    blockpos = modifyBlock(hitInfo, blockpos, ref nowChunk);//整合修正块坐标及处理chunk边界
                    blockpos = modifyBlockAdd(hitInfo, blockpos, ref nowChunk);//修正添加块及处理chunk边界
                                                                                //print(nowChunk.position);
                                                                                //print(blockpos);
                    nowChunk.setAddBlock(blockpos);

                }




            }
        }
        

    }

    
    //修正使各个面都返回同一个块 且修正chunk的边界情况
    public Vector3i modifyBlock(RaycastHit hit, Vector3i block, ref Chunk chunk)
    {
        //判断碰撞面来区分要操作的碰撞块
        if (hit.normal.y > 0)//上
        {
            block += new Vector3i(0,-1,0);
            if (block.y == -1)
            {
                block.y = Chunk.height + (-1);
                Vector3i chunkposnew = chunk.position;
                chunkposnew.y += (Chunk.height * -1);//新chunk坐标
                chunk = Map.Instance.GetChunk(chunkposnew);
            }
        }
        else if (hit.normal.y < 0)//下
        {

        }
        else if (hit.normal.x > 0)//前
        {
            block += new Vector3i(-1, 0, 0);
            if (block.x == -1)
            {
                block.x = Chunk.width + (-1);
                Vector3i chunkposnew = chunk.position;
                chunkposnew.x += (Chunk.width * -1);//新chunk坐标
                chunk = Map.Instance.GetChunk(chunkposnew);
            }
        }
        else if (hit.normal.x < 0)//后
        {
            
        }
        else if (hit.normal.z > 0)//右
        {
            block += new Vector3i(0, 0,-1);
            if (block.z == -1)
            {
                block.z = Chunk.width + (-1);
                Vector3i chunkposnew = chunk.position;
                chunkposnew.z += (Chunk.width * -1);//新chunk坐标
                chunk = Map.Instance.GetChunk(chunkposnew);
            }
        }
        else if (hit.normal.z < 0)//左
        {
            
        }

        return block;
    }

    public Vector3i modifyBlockAdd(RaycastHit hit, Vector3i block, ref Chunk chunk)
    {
        if (hit.normal.y > 0)//上
        {
            block.y += 1;
            if (block.y == Chunk.height)
            {
                block.y = block.y - Chunk.height;
                Vector3i chunkposnew = chunk.position;
                chunkposnew.y += (Chunk.height * +1);
                chunk = Map.Instance.GetChunk(chunkposnew);
            }
        }
        else if (hit.normal.y < 0)//下
        {
            block.y -= 1;
            if (block.y == -1)
            {
                block.y = Chunk.height + (-1);
                Vector3i chunkposnew = chunk.position;
                chunkposnew.y += (Chunk.height * -1);
                chunk = Map.Instance.GetChunk(chunkposnew);
            }
        }
        else if (hit.normal.x > 0)//前
        {
            block.x += 1;
            if (block.x == Chunk.width)
            {
                block.x = block.x - Chunk.width;
                Vector3i chunkposnew = chunk.position;
                chunkposnew.x += (Chunk.width * +1);
                chunk = Map.Instance.GetChunk(chunkposnew);
            }
        }
        else if (hit.normal.x < 0)//后
        {
            block.x -= 1;
            if (block.x == -1)
            {
                block.x = Chunk.width + (-1);
                Vector3i chunkposnew = chunk.position;
                chunkposnew.x += (Chunk.width * -1);
                chunk = Map.Instance.GetChunk(chunkposnew);
            }
        }
        else if (hit.normal.z > 0)//右
        {
            block.z += 1;
            if (block.z == Chunk.width)
            {
                block.z = block.z - Chunk.width;
                Vector3i chunkposnew = chunk.position;
                chunkposnew.z += (Chunk.width * +1);
                chunk = Map.Instance.GetChunk(chunkposnew);
            }
        }
        else if (hit.normal.z < 0)//左
        {
            block.z -= 1;
            if (block.z == -1)
            {
                block.z = Chunk.width + (-1);
                Vector3i chunkposnew = chunk.position;
                chunkposnew.z += (Chunk.width * -1);
                chunk = Map.Instance.GetChunk(chunkposnew);
            }
        }
        return block;
    }



}
