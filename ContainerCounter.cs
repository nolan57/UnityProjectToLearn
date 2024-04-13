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
    private event Action<bool> onCounterctionsEnableEvent;
    private bool _isCounterActionsEnable;
    private bool isCounterActionsEnable
    {
        get{return _isCounterActionsEnable;}
        set
        { 
            _isCounterActionsEnable = value;
            if(_isCounterActionsEnable)
            {
                onCounterctionsEnableEvent?.Invoke(true);
            }
        }
    }
    private Camera m_Camera;

    private string hitedCounterName;

    new void Awake()
    {
        base.Awake();
        if(instance == null)
        {
            instance = this;
        }
        BaseCounter.counterList.Add(this);
        BaseCounter.playerActions.OnClickEvents += toClick;
        onCounterctionsEnableEvent += OnCounterActionsEnable;
        m_Camera = Camera.main;
        hitedCounterName = "ContainerCounterSelected" + InstanceID;
        this.GetTransform().Find("ContainerCounterSelected").name = hitedCounterName;

    }

    private void OnCounterActionsEnable(bool e)
    {
        Debug.Log(this.gameObject.name + "'s actions's enabled is " + e);
    }

    private void toClick(object sender, OnClickArgs e)
    {
        //Debug.Log("Clicked!");
        Vector3 mousePosition = e.mousePosition;
        Ray ray = m_Camera.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("Hited is " + hit.collider.gameObject.name);
            if(hit.collider.gameObject.name == this.hitedCounterName)
            {
                this.isCounterActionsEnable = true;
            }else
            {
                this.isCounterActionsEnable = false;
            }
        }
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
