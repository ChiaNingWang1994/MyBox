using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetUI : UIBasePanel {

    private GameObject buttonClose;
    public Slider bgmvolume;

    public override void OnInit()
    {
        //指定id
        this.id = EUiId.ID_SettingPanel;

        buttonClose = UtilUI.findChild(this.gameObject, "Cross").gameObject;
        UGUIEventListener.Get(buttonClose).onClick = OnClose;

        bgmvolume = transform.Find("Set").Find("VolumeSlider").GetComponent<Slider>();
        bgmvolume.value = PlayerPrefs.GetFloat("volume"); //初始化为之前的音量
    }

    private void OnClose(GameObject obj)
    {
        UIPanelManager.Instance.hideUI(this.id);
        PlayerPrefs.SetFloat("volume", bgmvolume.value);//关闭时保存音量大小
    }

    private void Update()
    {
        MusicManger.Instance.BGMVolume = bgmvolume.value;
    }
}
