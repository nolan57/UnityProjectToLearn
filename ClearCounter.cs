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

public class ClearCounter : BaseCounter//,IKObjInterActions
{
    public static ClearCounter instance
    {
        get;private set;
    }
    //private static BaseCounter[] cList;
    //[SerializeField] private new GameObject selectedPart;
    private new KObjScript kObjScript;
    private new IKObjInterActions nextCounter;
    private new GameObject kObj;
    private event Action<bool> onCounterActionsEnableEvent;
    private new bool _isCounterActionsEnable;
    private bool isCounterActionsEnable
    {
        get{return _isCounterActionsEnable;}
        set
        { 
            _isCounterActionsEnable = value;
            if(_isCounterActionsEnable)
            {
                onCounterActionsEnableEvent?.Invoke(true);
            }
        }
    }
    //private Camera m_Camera;
    private new string hitedCounterName;
    new void Awake()
    {
        base.Awake(); 
        if(instance == null)
        {
            instance = this;
        }
        BaseCounter.CounterList.Add(this);
        Debug.Log("cheezeCounterList now has " + CounterList.Count());
        BaseCounter.playerActions.OnClickEvents += toClick;
        onCounterActionsEnableEvent += OnCounterActionsEnable;
        //m_Camera = Camera.main;
        hitedCounterName = "ClearCounterSelected" + InstanceID;
        this.GetTransform().Find("ClearCounterSelected").name = hitedCounterName;
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
    private void OnCounterActionsEnable(bool e)
    {
        Debug.Log(this.gameObject.name + "'s actions's enabled is " + e);
    }
    new void Start()
    {
        base.Start();
    }
    protected void Update()
    {
    }
    public new void setSelectedPartVisual(bool viusal)
    {
        this.selectedPart.SetActive(viusal);
    }
    public new KitcherObjectSO GetKitcherObjectSO()
    {
        Debug.Log("This is Clear Counter only for be Clear!");
        return null;
    }
    /*
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
        return this.transform;
    }

    public string getName()
    {
        return this.name;
    }

    public int getInstanceID()
    {
        return this.instanceID;
    }

    public void releaseKObj()
    {
        Destroy(this.kObj);
    } */
}
