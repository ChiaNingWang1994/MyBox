using MyBox.Voxel;
using MyBox.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameState : SingletonMono<GameState> {

    public bool isPause = false;
    public string path;

    protected override void Awake()
    {
        base.Awake();
        //UnPause();
        isPause = false;
        Time.timeScale = 1;
        path = Application.persistentDataPath + "/gamesave.save.";
    }

    public void Pause()
    {
        isPause = true;
        Time.timeScale = 0;
        Cursor.visible = true;
    }

    public void UnPause()
    {
        isPause = false;
        Time.timeScale = 1;
        Cursor.visible = false;
    }

    	
	// Update is called once per frame
	void Update () {
        //if (Input.GetKeyUp(KeyCode.Escape))
        //{
        //    //if (isPause)
        //    //{
        //    //    UnPause();
        //    //}
        //    //else
        //    //{
        //    //    Pause();
        //    //}
        //    if (!isPause)
        //    {
        //        Pause();
        //    }

        //}
    }

    public SaveBuild createSave()
    {
        SaveBuild save = new SaveBuild();
        Chunkpos chunkTemp = new Chunkpos();
        
        save.mapID = DateTime.Now + "";
        
        foreach(ChunkBuild chunk in MapBuild.Instance.chunks.Values)
        {
            chunkTemp.x = chunk.position.x;
            chunkTemp.y = chunk.position.y;
            chunkTemp.z = chunk.position.z;
            chunkTemp.blockType = new List<byte>();

            for (int x = 0; x < ChunkBuild.width; x++)
            {
                for (int y = 0; y < ChunkBuild.height; y++)
                {
                    for (int z = 0; z < ChunkBuild.width; z++)
                    {
                        chunkTemp.blockType.Add(chunk.blocks[x, y, z]);
                    }
                }
            }
            save.chunkkey.Add(chunkTemp);
        }

        return save;      
    }


    public void savegame(int id)
    {
        SaveBuild save = createSave();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(path+id);
        bf.Serialize(file, save);
        file.Close();
    }

    public bool loadgame(int id)//加载存档函数
    {
        if (File.Exists(path + id))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path + id, FileMode.Open);
            SaveBuild save = (SaveBuild)bf.Deserialize(file);
            file.Close();

            Vector3i posTemp = new Vector3i();

            foreach (Chunkpos pos in save.chunkkey)
            {
                posTemp.x = pos.x;posTemp.y = pos.y;posTemp.z = pos.z;
                ChunkBuild chunkTemp = MapBuild.Instance.GetChunk(posTemp);
                for (int x = 0; x < ChunkBuild.width; x++)
                {
                    for (int y = 0; y < ChunkBuild.height; y++)
                    {
                        for (int z = 0; z < ChunkBuild.width; z++)
                        {
                            chunkTemp.blocks[x, y, z] = pos.blockType[x * 256 + y * 16 + z];
                        }
                    }
                }
                chunkTemp.CreateMesh();
            }
            return true;
        }
        else
        {
            Debug.Log("No game saved!");
            return false;
        }
    }

    public string mapIDSaved(int id)
    {
        if(File.Exists(path + id))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path + id, FileMode.Open);
            SaveBuild save = (SaveBuild)bf.Deserialize(file);
            file.Close();
            return save.mapID;
        }
        else
        {
            return "No Saved!";
        }
    }

    public bool canload(int id)//判断存档是否存在函数
    {
        if (File.Exists(path + id))
        {
            return true;
        }
        else
        {
            return false;
        }
    }



}
