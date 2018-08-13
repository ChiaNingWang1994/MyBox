using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarnUI : UIBasePanel
{
    private float m_time=0;

    public override void OnInit()
    {
        //指定id
        this.id = EUiId.ID_WarnPanel;

    }

    private void Update()
    {
        m_time += Time.fixedDeltaTime;
        if (m_time > 3.0f)
        {
            m_time = 0;
            UIPanelManager.Instance.hideUI(this.id);
        }
    }


}
