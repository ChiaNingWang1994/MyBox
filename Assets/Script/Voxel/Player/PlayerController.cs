using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox.Util;
using MyBox.Voxel;

public class PlayerController : MonoBehaviour {

    //生成范围
    public int viewRange = 30;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        for (float x = transform.position.x - Chunk.width * 3;
            x < transform.position.x + Chunk.width * 3; x += Chunk.width)
        {
            for (float y = transform.position.y - Chunk.height * 3;
                y < transform.position.y + Chunk.height * 3; y += Chunk.height)
            {
                //Y轴上是允许最大16个Chunk，方块高度最大是256
                if (y <= Chunk.height * 16 && y > 0)
                {
                    for (float z = transform.position.z - Chunk.width * 3;
                        z < transform.position.z + Chunk.width * 3; z += Chunk.width)
                    {
                        int xx = Chunk.width * Mathf.FloorToInt(x / Chunk.width);
                        int yy = Chunk.height * Mathf.FloorToInt(y / Chunk.height);
                        int zz = Chunk.width * Mathf.FloorToInt(z / Chunk.width);
                        if (!Map.instance.ChunkExists(xx, yy, zz))
                        {
                            Map.instance.CreateChunk(new Vector3i(xx, yy, zz));
                        }

                    }
                }
            }
        }
       	
	}
}
