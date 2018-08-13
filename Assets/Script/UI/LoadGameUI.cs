using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadGameUI : UIBasePanel {

    private GameObject buttonClose;
    private GameObject buttonLoad_1;
    private GameObject buttonLoad_2;
    private GameObject buttonLoad_3;
    private GameObject buttonLoad_4;
    private GameObject buttonLoad_5;
    private GameObject buttonLoad_6;
    private Text textLog;

    private string sceneName;


    public override void OnInit()
    {
        //指定id
        this.id = EUiId.ID_LoadGamePanel;

        buttonClose = UtilUI.findChild(this.gameObject, "Cross").gameObject;
        buttonLoad_1 = UtilUI.findChild(this.gameObject, "Load_1").gameObject;
        buttonLoad_2 = UtilUI.findChild(this.gameObject, "Load_2").gameObject;
        buttonLoad_3 = UtilUI.findChild(this.gameObject, "Load_3").gameObject;
        buttonLoad_4 = UtilUI.findChild(this.gameObject, "Load_4").gameObject;
        buttonLoad_5 = UtilUI.findChild(this.gameObject, "Load_5").gameObject;
        buttonLoad_6 = UtilUI.findChild(this.gameObject, "Load_6").gameObject;
        
        UGUIEventListener.Get(buttonClose).onClick = OnClose;
        UGUIEventListener.Get(buttonLoad_1).onClick = OnLoad_1;
        UGUIEventListener.Get(buttonLoad_2).onClick = OnLoad_2;
        UGUIEventListener.Get(buttonLoad_3).onClick = OnLoad_3;
        UGUIEventListener.Get(buttonLoad_4).onClick = OnLoad_4;
        UGUIEventListener.Get(buttonLoad_5).onClick = OnLoad_5;
        UGUIEventListener.Get(buttonLoad_6).onClick = OnLoad_6;

        textLog = transform.Find("LoadGame").Find("LogInfo").GetComponent<Text>();
        textLog.color = new Color(textLog.color.r, textLog.color.g, textLog.color.b, 0.0f);


        buttonLoad_1.GetComponentInChildren<Text>().text = GameState.Instance.mapIDSaved(01);
        buttonLoad_2.GetComponentInChildren<Text>().text = GameState.Instance.mapIDSaved(02);
        buttonLoad_3.GetComponentInChildren<Text>().text = GameState.Instance.mapIDSaved(03);
        buttonLoad_4.GetComponentInChildren<Text>().text = GameState.Instance.mapIDSaved(04);
        buttonLoad_5.GetComponentInChildren<Text>().text = GameState.Instance.mapIDSaved(05);
        buttonLoad_6.GetComponentInChildren<Text>().text = GameState.Instance.mapIDSaved(06);

        sceneName = SceneManager.GetActiveScene().name;

    }


    private void Update()
    {
        if (textLog.color.a > 0)
        {
            textLog.color = new Color(textLog.color.r, textLog.color.g, textLog.color.b, textLog.color.a-0.01f);
        }
    }

    private void OnClose(GameObject obj)
    {
        UIPanelManager.Instance.hideUI(this.id);
        Destroy(this.gameObject);
    }

    private void OnLoad_1(GameObject obj)
    {
        howToLoad(01);
    }
    private void OnLoad_2(GameObject obj)
    {
        howToLoad(02);
    }
    private void OnLoad_3(GameObject obj)
    {
        howToLoad(03);
    }
    private void OnLoad_4(GameObject obj)
    {
        howToLoad(04);
    }
    private void OnLoad_5(GameObject obj)
    {
        howToLoad(05);
    }
    private void OnLoad_6(GameObject obj)
    {
        howToLoad(06);
    }

    //对于不同加载方式的逻辑判断
    private void howToLoad(int id)
    {
        if (sceneName == "MyBoxStart")//当场景为开始菜单时
        {
            if (GameState.Instance.canload(id))//如果文件存在
            {   //加载场景
                UIPanelManager.Instance.showUI(EUiId.ID_LoadingPanel1);
                //加载build场景，完成之后显示UI
                LoadSceneHelper.Instance.loadScene("MyBoxBuild", delegate
                {
                    //在场景加载完成之后延时加载界面消失之后，读档
                    UIPanelManager.Instance.hideUILater(EUiId.ID_LoadingPanel1, 8.0f, delegate
                    {
                        OnLoad(id);
                        Destroy(this.gameObject);
                    });
                    UIPanelManager.Instance.showLater(EUiId.ID_GamingPanel, 8.0f);
                });
            }
            else
            {
                textLog.color = new Color(textLog.color.r, textLog.color.g, textLog.color.b, 1.0f);
                textLog.text = "No game saved!";
            }
        }
        else
        {
            OnLoad(id);
        }
    }

    private void OnLoad(int id)
    {
        if (GameState.Instance.loadgame(id))
        {
            textLog.color = new Color(textLog.color.r, textLog.color.g, textLog.color.b, 1.0f);
            textLog.text = "Load finished!";
        }
        else
        {
            textLog.color = new Color(textLog.color.r, textLog.color.g, textLog.color.b, 1.0f);
            textLog.text = "No game saved!";
        }
    }


}
