using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadUI_2 : UIBasePanel {

    public override void OnInit()
    {
        //指定id
        this.id = EUiId.ID_LoadingPanel2;
        this.showMode = EUIShowMode.hideAll;

    }
}
