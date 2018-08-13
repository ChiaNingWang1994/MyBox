using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitUI : UIBasePanel {

    private GameObject buttonQuit;
    private GameObject buttonClose;
    

    public override void OnInit()
    {
        //指定id
        this.id = EUiId.ID_ExitPanel;

        buttonQuit = UtilUI.findChild(this.gameObject, "Yes").gameObject;
        buttonClose = UtilUI.findChild(this.gameObject, "Maybe").gameObject;
        UGUIEventListener.Get(buttonQuit).onClick = OnQuit;
        UGUIEventListener.Get(buttonClose).onClick = OnClose;

    }

    private void OnQuit(GameObject obj)
    {
        //UIPanelManager.Instance.hideUI(this.id);
        Application.Quit();
    }

    private void OnClose(GameObject obj)
    {
        UIPanelManager.Instance.hideUI(this.id);
    }
}
