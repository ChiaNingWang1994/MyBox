using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManger : SingletonMono<MusicManger> {

    public AudioSource meunBGM;
    public float BGMVolume;

    protected override void Awake()
    {
        base.Awake();
        meunBGM = this.GetComponent<AudioSource>();
        if (!PlayerPrefs.HasKey("volume"))//如果没有声音//第一次运行的初始化
        {
            PlayerPrefs.SetFloat("volume", 0.5f);//则将音量置为0.5
        }
    }
    // Use this for initialization
    void Start () {
        BGMVolume = PlayerPrefs.GetFloat("volume");//获取保存的音量
	}
	
	// Update is called once per frame
	void Update () {
        meunBGM.volume = BGMVolume;//实时调整
	}
}
