using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;
using MyBox.Util;

namespace MyBox.Voxel
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    public class Chunk:MonoBehaviour
    {
        //public byte addBlockType = 1;
        

        public static int width = 16;//尺寸
        public static int height = 16;

        public byte[,,] blocks;//块信息//三维数组//id
        public Vector3i position;//位置信息

        private Mesh mesh;

        //面所需的点
        private List<Vector3> vertices = new List<Vector3>();
        //三边面所需的索引
        private List<int> triangles = new List<int>();
        //所有uv信息的容器
        private List<Vector2> uv = new List<Vector2>();
        //uv的宽度为0~1，则根据图的不同，每块信息不同，此块为32格//更改贴图
        public static float textureOffset = 1 / 16.0f;
        //让UV稍微缩小一点，避免出现它旁边的贴图//避免出现白边及杂色
        public static float shrinkSize = 0.001f;

        //当前Chunk是否处于工作状态中
        //private bool isWorking = false;
        //生成状态
        public static bool isWorking = false;
        private bool isFinished = false;

        void Start()
        {
            position = new Vector3i(this.transform.position);//初始化位置信息
            if (Map.instance.ChunkExists(position))//位置是否已经存在
            {
                Debug.Log("此方块已经存在" + position);
                Destroy(this);//移除
            }
            else
            {
                Map.instance.chunks.Add(position, this);//存储在map里
                this.name = "(" + position.x + "," + position.y + "," + position.z + ")";//对象命名
                //StartFunction();//开始操作
            }
        }

        void Update()
        {
            if(isWorking==false&&isFinished==false)
            {
                isFinished = true;
                StartFunction();
            }
            
        }

        void StartFunction()
        {
            isWorking = true;
            mesh = new Mesh();//网格对象
            mesh.name = "Chunk";

            StartCoroutine(CreateMap());//开启协程
        }

        IEnumerator CreateMap()//创建地图协程
        {
            //while (isWorking)//当此chunk已经处于工作状态时返回空
            //{
            //    yield return null;
            //}
            //isWorking = true;
            blocks = new byte[width, height, width];//块信息为三维数组的byte值
            for(int x = 0; x < Chunk.width; x++)
            {
                for(int y = 0; y < Chunk.height; y++)
                {
                    for(int z = 0;z < Chunk.width; z++)
                    {
                        ////blocks[x, y, z] = 1;//遍历三维数组使所有格子有值
                        //if(y==Chunk.height-1)
                        //{
                        //    if(UnityEngine.Random.Range(1,5)==1)
                        //    {
                        //        blocks[x, y, z] = 2;//最上层随机出现草
                        //    }
                        //}
                        //else
                        //{
                        //    blocks[x, y, z] = 1;//其他层出现土
                        //}
                        //通过噪音为block赋值
                        byte blockid = Terrain.GetTerrainBlock(new Vector3i(x, y, z) + position);
                        //如果本身为1且上面没有了为0，则为草皮，赋予2
                        if (blockid == 1 && Terrain.GetTerrainBlock(new Vector3i(x, y + 1, z) + position) == 0)
                        {
                            blocks[x, y, z] = 2;
                        }
                        else//否则赋予本身0或1
                        {
                            blocks[x, y, z] = Terrain.GetTerrainBlock(new Vector3i(x, y, z) + position);
                        }
                        if (this.position.y == 0)//最下层的chunk
                        {
                            if (y == 0) { blocks[x, y, z] = 10; }//最下层的block的为基岩
                        }
                        //blocks[x, y, z] = Terrain.GetTerrainBlock(new Vector3i(x, y, z) + position);

                    }
                }
            }
            yield return null;

            CreateMesh();//开启协程，创建网格
        }

        public void CreateMesh()
        {

            vertices.Clear();
            triangles.Clear();
            uv.Clear();
            mesh.Clear();

            //把所有面的点和面的索引添加进去
            for (int x = 0; x < Chunk.width; x++)
            {
                for (int y = 0; y < Chunk.height; y++)
                {
                    for (int z = 0; z < Chunk.width; z++)
                    {
                        //获取当前坐标的Block对象
                        Block block = BlockList.GetBlock(this.blocks[x, y, z]);
                        if (block == null) continue;
                        
                        //通过查看相邻块是否透明来检查此面是否需要开启
                        if (IsBlockTransparent(x + 1, y, z))
                        {
                            AddFrontFace(x, y, z, block);
                        }
                        if (IsBlockTransparent(x - 1, y, z))
                        {
                            AddBackFace(x, y, z, block);
                        }
                        if (IsBlockTransparent(x, y, z + 1))
                        {
                            AddRightFace(x, y, z, block);
                        }
                        if (IsBlockTransparent(x, y, z - 1))
                        {
                            AddLeftFace(x, y, z, block);
                        }
                        if (IsBlockTransparent(x, y + 1, z))
                        {
                            AddTopFace(x, y, z, block);
                        }
                        if (IsBlockTransparent(x, y - 1, z))
                        {
                            AddBottomFace(x, y, z, block);
                        }
                    }
                }
            }

                      
            //为点和index赋值
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.uv = uv.ToArray();

            //重新计算顶点和法线
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            //将生成好的面赋值给组件
            this.GetComponent<MeshFilter>().mesh = mesh;
            this.GetComponent<MeshCollider>().sharedMesh = mesh;

            isWorking = false;
            return;

        }


        //此坐标方块是否透明，Chunk中的局部坐标
        public bool IsBlockTransparent(int x, int y, int z)
        {
            if (x >= width || y >= height || z >= width || x < 0 || y < 0 || z < 0)
            {
                return true;//边界绘制即可
            }
            else
            {
                //如果当前方块id为0，那为透明的
                return this.blocks[x, y, z] == 0;
            }
            
        }

        //前面
        void AddFrontFace(int x, int y, int z, Block block)
        {
            //第一个三角面
            triangles.Add(0 + vertices.Count);
            triangles.Add(3 + vertices.Count);
            triangles.Add(2 + vertices.Count);

            //第二个三角面
            triangles.Add(2 + vertices.Count);
            triangles.Add(1 + vertices.Count);
            triangles.Add(0 + vertices.Count);


            //添加4个点
            vertices.Add(new Vector3(1 + x, 0 + y, 0 + z));//在单个基础上加上坐标即可
            vertices.Add(new Vector3(1 + x, 0 + y, 1 + z));
            vertices.Add(new Vector3(1 + x, 1 + y, 1 + z));
            vertices.Add(new Vector3(1 + x, 1 + y, 0 + z));


            //添加UV坐标点，跟上面4个点循环的顺序一致
            uv.Add(new Vector2(block.textureFrontX * textureOffset, block.textureFrontY * textureOffset)
            + new Vector2(shrinkSize, shrinkSize));//防杂色偏差
            uv.Add(new Vector2(block.textureFrontX * textureOffset + textureOffset, block.textureFrontY * textureOffset)
            + new Vector2(-shrinkSize, shrinkSize));
            uv.Add(new Vector2(block.textureFrontX * textureOffset + textureOffset, block.textureFrontY * textureOffset + textureOffset)
            + new Vector2(-shrinkSize, -shrinkSize));
            uv.Add(new Vector2(block.textureFrontX * textureOffset, block.textureFrontY * textureOffset + textureOffset)
            + new Vector2(shrinkSize, -shrinkSize));
        }

        //背面
        void AddBackFace(int x, int y, int z, Block block)
        {
            //第一个三角面
            triangles.Add(0 + vertices.Count);
            triangles.Add(3 + vertices.Count);
            triangles.Add(2 + vertices.Count);

            //第二个三角面
            triangles.Add(2 + vertices.Count);
            triangles.Add(1 + vertices.Count);
            triangles.Add(0 + vertices.Count);


            //添加4个点
            vertices.Add(new Vector3(0 + x, 0 + y, 1 + z));
            vertices.Add(new Vector3(0 + x, 0 + y, 0 + z));
            vertices.Add(new Vector3(0 + x, 1 + y, 0 + z));
            vertices.Add(new Vector3(0 + x, 1 + y, 1 + z));

            //添加UV坐标点，跟上面4个点循环的顺序一致
            uv.Add(new Vector2(block.textureBackX * textureOffset, block.textureBackY * textureOffset)
            + new Vector2(shrinkSize, shrinkSize));//防杂色偏差
            uv.Add(new Vector2(block.textureBackX * textureOffset + textureOffset, block.textureBackY * textureOffset)
            + new Vector2(-shrinkSize, shrinkSize));
            uv.Add(new Vector2(block.textureBackX * textureOffset + textureOffset, block.textureBackY * textureOffset + textureOffset)
            + new Vector2(-shrinkSize, -shrinkSize));
            uv.Add(new Vector2(block.textureBackX * textureOffset, block.textureBackY * textureOffset + textureOffset)
            + new Vector2(shrinkSize, -shrinkSize));
        }

        //右面
        void AddRightFace(int x, int y, int z, Block block)
        {
            //第一个三角面
            triangles.Add(0 + vertices.Count);
            triangles.Add(3 + vertices.Count);
            triangles.Add(2 + vertices.Count);

            //第二个三角面
            triangles.Add(2 + vertices.Count);
            triangles.Add(1 + vertices.Count);
            triangles.Add(0 + vertices.Count);


            //添加4个点
            vertices.Add(new Vector3(1 + x, 0 + y, 1 + z));
            vertices.Add(new Vector3(0 + x, 0 + y, 1 + z));
            vertices.Add(new Vector3(0 + x, 1 + y, 1 + z));
            vertices.Add(new Vector3(1 + x, 1 + y, 1 + z));

            //添加UV坐标点，跟上面4个点循环的顺序一致
            uv.Add(new Vector2(block.textureRightX * textureOffset, block.textureRightY * textureOffset)
            + new Vector2(shrinkSize, shrinkSize));//防杂色偏差
            uv.Add(new Vector2(block.textureRightX * textureOffset + textureOffset, block.textureRightY * textureOffset)
            + new Vector2(-shrinkSize, shrinkSize));
            uv.Add(new Vector2(block.textureRightX * textureOffset + textureOffset, block.textureRightY * textureOffset + textureOffset)
            + new Vector2(-shrinkSize, -shrinkSize));
            uv.Add(new Vector2(block.textureRightX * textureOffset, block.textureRightY * textureOffset + textureOffset)
            + new Vector2(shrinkSize, -shrinkSize));
        }

        //左面
        void AddLeftFace(int x, int y, int z, Block block)
        {
            //第一个三角面
            triangles.Add(0 + vertices.Count);
            triangles.Add(3 + vertices.Count);
            triangles.Add(2 + vertices.Count);

            //第二个三角面
            triangles.Add(2 + vertices.Count);
            triangles.Add(1 + vertices.Count);
            triangles.Add(0 + vertices.Count);


            //添加4个点
            vertices.Add(new Vector3(0 + x, 0 + y, 0 + z));
            vertices.Add(new Vector3(1 + x, 0 + y, 0 + z));
            vertices.Add(new Vector3(1 + x, 1 + y, 0 + z));
            vertices.Add(new Vector3(0 + x, 1 + y, 0 + z));

            //添加UV坐标点，跟上面4个点循环的顺序一致
            uv.Add(new Vector2(block.textureLeftX * textureOffset, block.textureLeftY * textureOffset)
            + new Vector2(shrinkSize, shrinkSize));//防杂色偏差
            uv.Add(new Vector2(block.textureLeftX * textureOffset + textureOffset, block.textureLeftY * textureOffset)
            + new Vector2(-shrinkSize, shrinkSize));
            uv.Add(new Vector2(block.textureLeftX * textureOffset + textureOffset, block.textureLeftY * textureOffset + textureOffset)
            + new Vector2(-shrinkSize, -shrinkSize));
            uv.Add(new Vector2(block.textureLeftX * textureOffset, block.textureLeftY * textureOffset + textureOffset)
            + new Vector2(shrinkSize, -shrinkSize));
        }

        //上面
        void AddTopFace(int x, int y, int z, Block block)
        {
            //第一个三角面
            triangles.Add(1 + vertices.Count);
            triangles.Add(0 + vertices.Count);
            triangles.Add(3 + vertices.Count);

            //第二个三角面
            triangles.Add(3 + vertices.Count);
            triangles.Add(2 + vertices.Count);
            triangles.Add(1 + vertices.Count);


            //添加4个点
            vertices.Add(new Vector3(1 + x, 1 + y, 0 + z));
            vertices.Add(new Vector3(1 + x, 1 + y, 1 + z));
            vertices.Add(new Vector3(0 + x, 1 + y, 1 + z));
            vertices.Add(new Vector3(0 + x, 1 + y, 0 + z));

            //添加UV坐标点，跟上面4个点循环的顺序一致
            uv.Add(new Vector2(block.textureTopX * textureOffset, block.textureTopY * textureOffset)
            + new Vector2(shrinkSize, shrinkSize));//防杂色偏差
            uv.Add(new Vector2(block.textureTopX * textureOffset + textureOffset, block.textureTopY * textureOffset)
            + new Vector2(-shrinkSize, shrinkSize));
            uv.Add(new Vector2(block.textureTopX * textureOffset + textureOffset, block.textureTopY * textureOffset + textureOffset)
            + new Vector2(-shrinkSize, -shrinkSize));
            uv.Add(new Vector2(block.textureTopX * textureOffset, block.textureTopY * textureOffset + textureOffset)
            + new Vector2(shrinkSize, -shrinkSize));
        }

        //下面
        void AddBottomFace(int x, int y, int z, Block block)
        {
            //第一个三角面
            triangles.Add(1 + vertices.Count);
            triangles.Add(0 + vertices.Count);
            triangles.Add(3 + vertices.Count);

            //第二个三角面
            triangles.Add(3 + vertices.Count);
            triangles.Add(2 + vertices.Count);
            triangles.Add(1 + vertices.Count);


            //添加4个点
            vertices.Add(new Vector3(0 + x, 0 + y, 0 + z));
            vertices.Add(new Vector3(0 + x, 0 + y, 1 + z));
            vertices.Add(new Vector3(1 + x, 0 + y, 1 + z));
            vertices.Add(new Vector3(1 + x, 0 + y, 0 + z));

            //添加UV坐标点，跟上面4个点循环的顺序一致
            uv.Add(new Vector2(block.textureBottomX * textureOffset, block.textureBottomY * textureOffset)
            + new Vector2(shrinkSize, shrinkSize));//防杂色偏差
            uv.Add(new Vector2(block.textureBottomX * textureOffset + textureOffset, block.textureBottomY * textureOffset)
            + new Vector2(-shrinkSize, shrinkSize));
            uv.Add(new Vector2(block.textureBottomX * textureOffset + textureOffset, block.textureBottomY * textureOffset + textureOffset)
            + new Vector2(-shrinkSize, -shrinkSize));
            uv.Add(new Vector2(block.textureBottomX * textureOffset, block.textureBottomY * textureOffset + textureOffset)
            + new Vector2(shrinkSize, -shrinkSize));
        }

        //操作block相关函数

        public Vector3i getBlock(Vector3i pos)//获取块的位置，传入为射线点
        {
            pos.x -= this.position.x;
            //if (pos.x == 16) pos.x = 0;
            pos.z -= this.position.z;
            pos.y -= this.position.y;
            return pos;
            //return pos -= this.position;
        }

        public void setDeleteBlock(Vector3i pos)//传入为上个函数的结果，block的坐标，也是id
        {
            //print(blocks[pos.x, pos.y, pos.z]);
            //不为空且不为基岩
            if (blocks[pos.x, pos.y, pos.z] != 0 && blocks[pos.x, pos.y, pos.z] != 10)
            {
                blocks[pos.x, pos.y, pos.z] = 0;
                //重绘网格
                CreateMesh();//开启协程，创建网格
            }

            //print(blocks[pos.x, pos.y, pos.z]);

        }

        public void setAddBlock(Vector3i pos)
        {
            //print(blocks[pos.x, pos.y, pos.z]);
            if (blocks[pos.x, pos.y, pos.z] == 0)
            {
                
                blocks[pos.x, pos.y, pos.z] = blockUI.instance.blocktype;
                //重绘网格
                CreateMesh();//开启协程，创建网格
            }
        }


    }

    
}
