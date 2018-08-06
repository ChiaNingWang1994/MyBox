using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class blockUI : MonoBehaviour {

    public static blockUI instance;//单例map对象

    public Image blockImage;
    public List<Sprite> blockIcon;
    private int nowicon;
    public byte blocktype;

    void Awake()
    {
        instance = this;
        for (int i = 1; i < 8; i++)//不包含两个玻璃效果
        {
            string path = "icon/block_" + i;
            Sprite icon = Resources.Load(path, typeof(Sprite)) as Sprite;
            blockIcon.Add(icon);
        }
    }

    // Use this for initialization
    void Start () {
        blockImage = this.transform.Find("block").GetComponent<Image>();
        blockImage.sprite = blockIcon[0];
        nowicon = 0;
	}
	
	// Update is called once per frame
	void Update () {
        float change = Input.GetAxis("Mouse ScrollWheel");
        //print(change);
        if (change > 0)
        {
            nowicon -= (int)(change / 0.1f);
            if (nowicon < 0) nowicon = blockIcon.Count-1;
            blockImage.sprite = blockIcon[nowicon];
        }
        if (change < 0)
        {
            nowicon += (int)(change / -0.1f);
            if (nowicon > blockIcon.Count - 1) nowicon = 0;
            blockImage.sprite = blockIcon[nowicon];
        }
        blocktype = (byte)(nowicon + 1);
    }
}
