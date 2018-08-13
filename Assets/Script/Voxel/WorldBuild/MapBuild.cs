using MyBox.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyBox.Voxel
{
    public class MapBuild : SingletonMono<MapBuild>
    {

        public static GameObject chunkPrefab;//chunk对象


        //所以，需要储存和加载的就是这个Dictionary容器
        //不是这个容器，是这个容器里的部分内容
        //储存chunks的容器
        public Dictionary<Vector3i,ChunkBuild> chunks = new Dictionary<Vector3i, ChunkBuild>();

        //当前是否正在生成Chunk
        //private bool spawningChunk = false;

        protected override void Awake()
        {
            base.Awake();
            chunkPrefab = Resources.Load("Prefab/ChunkBuild") as GameObject;//加载预制体
        }

        private void Start()
        {
            //StartCoroutine(SpawnChunk(new Vector3i(0, 0, 0)));
            CreateTerrain();
        }

        
        //需更改
        //生成16*16*16的固定大小的地图
        private void CreateTerrain()
        {
            for (float x = transform.position.x - ChunkBuild.width * 3;
            x < transform.position.x + ChunkBuild.width * 3; x += ChunkBuild.width)
            {
                for (float y = transform.position.y;
                    y < transform.position.y + ChunkBuild.height * 6; y += ChunkBuild.height)
                {
                    //Y轴上是允许最大16个Chunk，方块高度最大是256
                    if (y <= ChunkBuild.height * 16 && y >= 0)
                    {
                        for (float z = transform.position.z - ChunkBuild.width * 3;
                            z < transform.position.z + ChunkBuild.width * 3; z += ChunkBuild.width)
                        {
                            int xx = Mathf.FloorToInt(x);
                            int yy = Mathf.FloorToInt(y);
                            int zz = Mathf.FloorToInt(z);
                            if (!ChunkExists(xx, yy, zz))
                            {
                                CreateChunk(new Vector3i(xx, yy, zz));
                            }

                        }
                    }
                }
            }
        }
        
        //生成chunk
        public void CreateChunk(Vector3i pos)
        {
            //if (spawningChunk) return;

            StartCoroutine(SpawnChunk(pos));
        }

        public IEnumerator SpawnChunk(Vector3i pos)
        {
            //spawningChunk = true;
            Instantiate(chunkPrefab, pos, Quaternion.identity);//实例化对象
            yield return 0;
            //spawningChunk = false;
        }

        //通过Chunk的坐标来判断它是否存在
        public bool ChunkExists(Vector3i worldPosition)
        {
            return this.ChunkExists(worldPosition.x, worldPosition.y, worldPosition.z);
        }
        //通过Chunk的坐标来判断它是否存在
        public bool ChunkExists(int x, int y, int z)
        {
            return chunks.ContainsKey(new Vector3i(x, y, z));
        }
        
        public ChunkBuild GetChunk(Vector3i worldPosition)
        {
            ChunkBuild getIt;
            if (chunks.TryGetValue(worldPosition, out getIt) == false)
            {
                throw new Exception("dicOpenUIs TryGetValue Failure! ");
            }
            return getIt;
        }
    }
}
