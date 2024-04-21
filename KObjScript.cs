using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KObjScript : MonoBehaviour
{
    private UnityEngine.GameObject currentCounter{
        get;set;
    }
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
    }
    public UnityEngine.GameObject getCurrentCounter(){
        return currentCounter;
    }
    public void setCurrentCounter(UnityEngine.GameObject currentCounter){
        this.currentCounter=currentCounter;
    }
}
