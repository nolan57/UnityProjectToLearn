using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
public class CutKObj : MonoBehaviour
{
    [SerializeField]private KObjOSS[] kObjOSS;
    [SerializeField]private CuttingCounter cuttingCounter;
    public void toCutKObj(UnityEngine.GameObject toCut)
    {
        foreach(KObjOSS kObjOS in kObjOSS){
            Debug.Log("To Check if " + kObjOS.toCut.original.name + " is " + toCut.name);
            if(kObjOS.toCut.original.name == toCut.name)
            {
                GameObject cutted = Instantiate(kObjOS.Cutted.original);
                cutted.name = kObjOS.Cutted.original.name;
                cuttingCounter.setKObj(cutted);
                Destroy(toCut);
            }
        }
    }
}
