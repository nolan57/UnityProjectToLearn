using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContainerCounter : BaseCounter,IKObjInterActions
{

    public static ContainerCounter instance
    {
        get;private set;
    }
    //private static BaseCounter[] cList;
    private KObjScript kObjScript;
    private IKObjInterActions nextCounter;
    private GameObject kObj;

    new void Awake()
    {
        base.Awake();
        if(instance == null)
        {
            instance = this;
        }
        BaseCounter.counterList.Add(this);
        //Debug.Log("1 Now List has " + BaseCounter.counterList.Count + " counter " + this.name);
    }

    new void Start()
    {
        base.Start();
    }

    public void setKObjScript(KObjScript kObjScript)
    {
        this.kObjScript = kObjScript;
    }
    public KObjScript getKObjScript()
    {
        return this.kObjScript;
    }
    public void setNext(IKObjInterActions nextCounter)
    {
        this.nextCounter = nextCounter;
    }
    public IKObjInterActions getNext()
    {
        return this.nextCounter;
    }
    public KitcherObjectSO GetKitcherObjectSO()
    {
        return this.kitcherObject;
    }
    public GameObject getKObj()
    {
        return this.kObj;
    }
    public void setKObj(GameObject obj)
    {
        this.kObj = obj;
        this.kObj.transform.SetParent(this.GetTransform().Find("CounterTop"));
        this.kObj.transform.localPosition=Vector3.zero;
        this.kObjScript = this.kObj.GetComponent<KObjScript>();
        this.kObjScript.setCurrentParent(this);
        this.kObj.SetActive(true);
    }
    public Transform GetTransform()
    {
        return this.gameObject.transform;
    }

    public string getName()
    {
        return this.name;
    }

    public int getInstanceID()
    {
        return this.InstanceID;
    }

    public void releaseKObj()
    {
        Destroy(this.kObj);
    }
}
