using MyBox.Voxel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : UIBasePanel {

    private GameObject buttonContinue;
    private GameObject buttonSave;
    private GameObject buttonLoad;
    private GameObject buttonExit;
    private GameObject buttonMenu;

    private string sceneName;


    public override void OnInit()
    {
        //指定id
        this.id = EUiId.ID_PausePanel;

        buttonContinue = UtilUI.findChild(this.gameObject, "ContinueButton").gameObject;
        buttonSave = UtilUI.findChild(this.gameObject, "SaveButton").gameObject;
        buttonLoad = UtilUI.findChild(this.gameObject, "LoadButton").gameObject;
        buttonExit = UtilUI.findChild(this.gameObject, "ExitButton").gameObject;
        buttonMenu = UtilUI.findChild(this.gameObject, "MenuButton").gameObject;
        UGUIEventListener.Get(buttonContinue).onClick = OnClose;
        UGUIEventListener.Get(buttonSave).onClick = OnSave;
        UGUIEventListener.Get(buttonLoad).onClick = OnLoad;
        UGUIEventListener.Get(buttonExit).onClick = OnBtnExit;
        UGUIEventListener.Get(buttonMenu).onClick = OnBtnMenu;

        sceneName = SceneManager.GetActiveScene().name;

    }

    private void OnClose(GameObject obj)
    {
        GameState.Instance.UnPause();
        UIPanelManager.Instance.hideUI(this.id);
        UIPanelManager.Instance.showUI(EUiId.ID_GamingPanel);
    }

    private void OnSave(GameObject obj)
    {
        //GameState.Instance.savegame();
        //Debug.Log("Game Saved");
        //UIPanelManager.Instance.hideUI(this.id);
        if (sceneName == "MyBoxInfinity")
        {
            UIPanelManager.Instance.showUI(EUiId.ID_WarnPanel);
        }
        else
        {
            UIPanelManager.Instance.showUI(EUiId.ID_SaveGamePanel);
        }
        
    }

    private void OnLoad(GameObject obj)
    {
        //GameState.Instance.loadgame();
        //Debug.Log("Game loaded");
        //UIPanelManager.Instance.hideUI(this.id);
        if (sceneName == "MyBoxInfinity")
        {
            UIPanelManager.Instance.showUI(EUiId.ID_WarnPanel);
        }
        else
        {
            UIPanelManager.Instance.showUI(EUiId.ID_LoadGamePanel);
        }
    }

    private void OnBtnExit(GameObject obj)
    {
        UIPanelManager.Instance.showUI(EUiId.ID_ExitPanel);
    }

    private void OnBtnMenu(GameObject obj)
    {
        //UIPanelManager.Instance.showUI(EUiId.ID_LoadingPanel1);
        //LoadSceneHelper.Instance.loadScene("MyBoxStart", delegate
        //{
        //    UIPanelManager.Instance.hideUI(EUiId.ID_LoadingPanel1);
        //    UIPanelManager.Instance.showUI(EUiId.ID_MenuPanel);
        //});
        UIPanelManager.Instance.hideUI(this.id);
        UtilUI.clearMemory();
        GameObject uiRoot = GameObject.FindGameObjectWithTag("UIRoot");
        Destroy(uiRoot);
        SceneManager.LoadScene("MyBoxStart");
    }


}
