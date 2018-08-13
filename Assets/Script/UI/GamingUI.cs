using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamingUI : UIBasePanel {

    public override void OnInit()
    {
        //指定id
        this.id = EUiId.ID_GamingPanel;
        //this.showMode = EUIShowMode.hideAll;


    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (!GameState.Instance.isPause)//非暂停状态
            {
                GameState.Instance.Pause();
                UIPanelManager.Instance.hideUI(this.id);
                UIPanelManager.Instance.showUI(EUiId.ID_PausePanel);
            }
        }
    }


}
