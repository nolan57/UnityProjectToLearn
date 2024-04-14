using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKObjInterActions
{
    public void setKObjScript(KObjScript kObjScript);
    public KObjScript getKObjScript();
    public void setNext(IKObjInterActions next);
    public IKObjInterActions getNext();
    public KitcherObjectSO GetKitcherObjectSO();
    public GameObject getKObj();
    public void setKObj(GameObject obj);
    public Transform GetTransform();
    public string getName();
    public int getInstanceID();
    public void releaseKObj();
    public void setSelectedPartVisual(bool viusal);
}
