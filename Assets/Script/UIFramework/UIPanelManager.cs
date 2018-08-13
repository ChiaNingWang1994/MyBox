using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1：当前UI引用
/// 2：缓存所有显示的UI
/// 3：缓存所有UI（包括显示和不显示）
/// 4：单例模式
/// 5：显示指定UI
/// 6：隐藏指定UI
/// 7：获得UI
/// </summary>
public class UIPanelManager : SingletonMono<UIPanelManager> {

    public UIBasePanel curUI;
    public Dictionary<EUiId, UIBasePanel> dicAllUI = new Dictionary<EUiId, UIBasePanel>();//所有打开过的UI,包括显示和不显示
    public Dictionary<EUiId, UIBasePanel> dicShowUI = new Dictionary<EUiId, UIBasePanel>();//所有正在显示的UI
    public Transform transUIRoot;//UI根节点

    public void delUI(EUiId id)
    {
        if (dicShowUI.ContainsKey(id))
        {
            dicShowUI.Remove(id);
        }
    }
    protected override void Awake()
    {
        base.Awake();//一定不能删掉
        init();
    }
    public void init()
    {
        DontDestroyOnLoad(transUIRoot);//不销毁脚本所在的对象
        if (dicAllUI != null) dicAllUI.Clear();
        if (dicShowUI != null) dicShowUI.Clear();

        showUI(EUiId.ID_MenuPanel);//显示主界面UI
    }

    /// <summary>
    /// 从容器中获取需要显示的UI
    /// </summary>
    /// <param name="id">传入进来的ID</param>
    /// <returns></returns>
    public UIBasePanel getUI(EUiId id)
    {
        if (dicAllUI.ContainsKey(id))
        {
            return dicAllUI[id];
        }
        else
        {
            return null;
        }

    }

    public void showUI(EUiId id)
    {
        //1：加载当前UI
        if (dicShowUI.ContainsKey(id))//当前UI已经在显示列表中了，就直接返回
        {
            return;
        }
        UIBasePanel ui = getUI(id);//通过ID获取需要显示的UI，从dicAllUI容器中获取，得到的是隐藏的UIPanel
        if (ui == null)//如果在dicAllUI容器中没有此UI，就从资源中读取ui预制体
        {
            string path = UIPath.getUiIdPath(id);//通过ID，获取对应的路径
            if (!string.IsNullOrEmpty(path))
            {
                GameObject prefab = Resources.Load<GameObject>(path);//加载资源
                //获取UI原始数据
                Vector2 offMin = prefab.GetComponent<RectTransform>().offsetMin;
                Vector2 offMax = prefab.GetComponent<RectTransform>().offsetMax;
                Vector2 anchorMin = prefab.GetComponent<RectTransform>().anchorMin;
                Vector2 anchorMax = prefab.GetComponent<RectTransform>().anchorMax;
                if (prefab != null)//资源加载成功
                {
                    GameObject goWillShowUI = GameObject.Instantiate(prefab);//克隆游戏对象到层次面板上
                    //goWillShowUI.SetActive(true);
                    ui = goWillShowUI.GetComponent<UIBasePanel>();//获取此对象上的UI
                    if (ui != null)
                    {
                        //Transform root = getUIRoot(ui);//获取UI所对应的根节点
                        //放入根节点下面
                        UtilUI.addChildToParent(transUIRoot, goWillShowUI.transform);//放入根节点下面
                        //数据的恢复
                        goWillShowUI.GetComponent<RectTransform>().offsetMin = offMin;
                        goWillShowUI.GetComponent<RectTransform>().offsetMax = offMax;
                        goWillShowUI.GetComponent<RectTransform>().anchorMin = anchorMin;
                        goWillShowUI.GetComponent<RectTransform>().anchorMax = anchorMax;
                        prefab = null;//清空预制体对象
                    }
                }
                else
                {
                    Debug.LogError("资源" + path + "不存在");
                }
            }
        }
        //2:更新显示其它的UI
        UpdateOtherUIState(ui);

        //3:显示当前UI
        dicAllUI[id] = ui;
        dicShowUI[id] = ui;
        ui.show();

    }

    IEnumerator showUILater(EUiId id, float time)
    {
        yield return new WaitForSeconds(time);
        showUI(id);
    }

    public void showLater(EUiId id,float time)
    {
        StartCoroutine(showUILater(id,time));
    }

    public void hideUI(EUiId id, Action a = null)//隐藏UI，传入ID和需要做的事情
    {
        if (!dicShowUI.ContainsKey(id))//正在显示的容器中没有此ID
        {
            return;
        }
        if (a == null)//隐藏UI的时候不需要做别的事情
        {
            dicShowUI[id].hide();//直接隐藏
            dicShowUI.Remove(id);//从显示列表中删除
        }
        else//隐藏窗体之后需要做的事情
        {
            a += delegate { dicShowUI.Remove(id); };
            dicShowUI[id].hide(a);
        }
    }

    IEnumerator hideUIMain(EUiId id, float time,Action a = null)
    {
        yield return new WaitForSeconds(time);
        hideUI(id, a);
    }

    public void hideUILater(EUiId id, float later,Action a = null)//隐藏UI，传入ID和需要做的事情
    {
        StartCoroutine(hideUIMain(id, later, a));
    }


    public void UpdateOtherUIState(UIBasePanel ui)//更新其它UI的状态（显示或者隐藏）
    {
        if (ui.showMode == EUIShowMode.hideAll)
        {
            hideAllUI(true);//隐藏所有的UI
        }
        else if (ui.showMode == EUIShowMode.hideAllOutAbove)//剔除最前面UI
        {
            hideAllUI(false);//隐藏所有的UI
        }
    }
    public void hideAllUI(bool isHideAbove)//隐藏所有的UI，参数表示是否隐藏最前面主UI
    {
        if (isHideAbove)//隐藏最上面的UI
        {
            foreach (KeyValuePair<EUiId, UIBasePanel> item in dicShowUI)//遍历所有正在显示的UI
            {
                item.Value.hide();
            }
            dicShowUI.Clear();
        }
        else
        {//不隐藏最上面的主UI
            List<EUiId> listRemove = new List<EUiId>();
            foreach (KeyValuePair<EUiId, UIBasePanel> item in dicShowUI)
            {
                if (item.Value.IsKeepAbove)
                {
                    continue;
                }
                item.Value.hide();
                listRemove.Add(item.Key);
            }
            for (int i = 0; i < listRemove.Count; i++)
            {
                dicShowUI.Remove(listRemove[i]);
            }
            listRemove.Clear();
        }

    }
}
