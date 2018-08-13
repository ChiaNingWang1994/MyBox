using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveGameUI : UIBasePanel {

    private GameObject buttonClose;
    private GameObject buttonLoad_1;
    private GameObject buttonLoad_2;
    private GameObject buttonLoad_3;
    private GameObject buttonLoad_4;
    private GameObject buttonLoad_5;
    private GameObject buttonLoad_6;
    private Text textLog;

    public override void OnInit()
    {
        //指定id
        this.id = EUiId.ID_SaveGamePanel;

        buttonClose = UtilUI.findChild(this.gameObject, "Cross").gameObject;
        buttonLoad_1 = UtilUI.findChild(this.gameObject, "Load_1").gameObject;
        buttonLoad_2 = UtilUI.findChild(this.gameObject, "Load_2").gameObject;
        buttonLoad_3 = UtilUI.findChild(this.gameObject, "Load_3").gameObject;
        buttonLoad_4 = UtilUI.findChild(this.gameObject, "Load_4").gameObject;
        buttonLoad_5 = UtilUI.findChild(this.gameObject, "Load_5").gameObject;
        buttonLoad_6 = UtilUI.findChild(this.gameObject, "Load_6").gameObject;

        UGUIEventListener.Get(buttonClose).onClick = OnClose;
        UGUIEventListener.Get(buttonLoad_1).onClick = OnSave_1;
        UGUIEventListener.Get(buttonLoad_2).onClick = OnSave_2;
        UGUIEventListener.Get(buttonLoad_3).onClick = OnSave_3;
        UGUIEventListener.Get(buttonLoad_4).onClick = OnSave_4;
        UGUIEventListener.Get(buttonLoad_5).onClick = OnSave_5;
        UGUIEventListener.Get(buttonLoad_6).onClick = OnSave_6;

        textLog = transform.Find("SaveGame").Find("LogInfo").GetComponent<Text>();
        textLog.color = new Color(textLog.color.r, textLog.color.g, textLog.color.b, 0.0f);


        buttonLoad_1.GetComponentInChildren<Text>().text = GameState.Instance.mapIDSaved(01);
        buttonLoad_2.GetComponentInChildren<Text>().text = GameState.Instance.mapIDSaved(02);
        buttonLoad_3.GetComponentInChildren<Text>().text = GameState.Instance.mapIDSaved(03);
        buttonLoad_4.GetComponentInChildren<Text>().text = GameState.Instance.mapIDSaved(04);
        buttonLoad_5.GetComponentInChildren<Text>().text = GameState.Instance.mapIDSaved(05);
        buttonLoad_6.GetComponentInChildren<Text>().text = GameState.Instance.mapIDSaved(06);

    }

    private void Update()
    {
        if (textLog.color.a > 0)
        {
            textLog.color = new Color(textLog.color.r, textLog.color.g, textLog.color.b, textLog.color.a - 0.01f);
        }
    }


    private void OnClose(GameObject obj)
    {
        UIPanelManager.Instance.hideUI(this.id);
    }

    private void OnSave_1(GameObject obj)
    {
        OnSave(01);
        buttonLoad_1.GetComponentInChildren<Text>().text = GameState.Instance.mapIDSaved(01);
    }
    private void OnSave_2(GameObject obj)
    {
        OnSave(02);
        buttonLoad_2.GetComponentInChildren<Text>().text = GameState.Instance.mapIDSaved(02);
    }
    private void OnSave_3(GameObject obj)
    {
        OnSave(03);
        buttonLoad_3.GetComponentInChildren<Text>().text = GameState.Instance.mapIDSaved(03);
    }
    private void OnSave_4(GameObject obj)
    {
        OnSave(04);
        buttonLoad_4.GetComponentInChildren<Text>().text = GameState.Instance.mapIDSaved(04);
    }
    private void OnSave_5(GameObject obj)
    {
        OnSave(05);
        buttonLoad_5.GetComponentInChildren<Text>().text = GameState.Instance.mapIDSaved(05);
    }
    private void OnSave_6(GameObject obj)
    {
        OnSave(06);
        buttonLoad_6.GetComponentInChildren<Text>().text = GameState.Instance.mapIDSaved(06);
    }

    private void OnSave(int id)
    {
        GameState.Instance.savegame(id);
        textLog.color = new Color(textLog.color.r, textLog.color.g, textLog.color.b, 1.0f);
        textLog.text = "Saved finished!";
    }
}
