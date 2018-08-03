
using MyBox.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyBox.Voxel
{
    public class Map : MonoBehaviour
    {
        public static Map instance;//单例map对象

        //public static GameObject chunkPrefab;//chunk对象
        public static GameObject chunkPrefab;//chunk对象

        //储存chunks的容器
        //public Dictionary<Vector3i, GameObject> chunks = new Dictionary<Vector3i, GameObject>();
        public Dictionary<Vector3i, Chunk> chunks = new Dictionary<Vector3i, Chunk>();


        //当前是否正在生成Chunk
        private bool spawningChunk = false;


        void Awake()
        {
            instance = this;
            chunkPrefab = Resources.Load("Prefab/Chunk") as GameObject;//加载预制体
        }

        //void Start()
        //{
        //    StartCoroutine(SpawnChunk(new Vector3i(0, 0, 0)));
        //}

        //生成chunk
        public void CreateChunk(Vector3i pos)
        {
            if (spawningChunk) return;

            StartCoroutine(SpawnChunk(pos));
        }

        //void Update()
        //{

        //}

        public IEnumerator SpawnChunk(Vector3i pos)
        {
            spawningChunk = true;
            Instantiate(chunkPrefab, pos, Quaternion.identity);//实例化对象
            yield return 0;
            spawningChunk = false;
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
        //查找Chunk对象
        //public GameObject GetChunk(Vector3i worldPosition)
        //{
        //    GameObject getIt;
        //    if(chunks.TryGetValue(worldPosition, out getIt) == false)
        //    {
        //        throw new Exception("dicOpenUIs TryGetValue Failure! ");
        //    }
        //    return getIt; 
        //}
        public Chunk GetChunk(Vector3i worldPosition)
        {
            Chunk getIt;
            if (chunks.TryGetValue(worldPosition, out getIt) == false)
            {
                throw new Exception("dicOpenUIs TryGetValue Failure! ");
            }
            return getIt;
        }

    }

}