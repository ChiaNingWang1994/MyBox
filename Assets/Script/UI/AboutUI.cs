using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AboutUI : UIBasePanel {

    private GameObject buttonClose;

    public override void OnInit()
    {
        //指定id
        this.id = EUiId.ID_InfoPanel;

        buttonClose = UtilUI.findChild(this.gameObject, "Cross").gameObject;
        UGUIEventListener.Get(buttonClose).onClick = OnClose;
    }

    private void OnClose(GameObject obj)
    {
        UIPanelManager.Instance.hideUI(this.id);
    }
}
