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

public class CheezeContainerCounter : BaseCounter,IContainerCounterActions,IBaseActions
{
    public static CheezeContainerCounter instance
    {
        get;private set;
    }
    [SerializeField]private KitcherObjectSO kObjSO;
    private int kObjCount = 0;
    private GameObject kObj;
    private KObjScript kObjScript;
    private new UnityEngine.GameObject nextCounter;
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
    private string counterName;
    new void Awake()
    {
        base.Awake();
        if(instance == null)
        {
            instance = this;
        }
        BaseCounter.CounterList.Add(this.gameObject);
        BaseCounter.playerActions.OnClickEvents += toClick;
        onCounterctionsEnableEvent += OnCounterActionsEnable;
        counterName = InstanceID.ToString();
        this.gameObject.transform.Find("CheezeContainerCounterSelected").name = counterName;
    }
    private void OnCounterActionsEnable(bool e)
    {
        Debug.Log(this.gameObject.name + "'s actions's enabled is " + e);
    } 

    private void toClick(object sender, OnClickArgs e)
    {
        Vector3 mousePosition = e.mousePosition;
        Ray ray = m_Camera.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("Hited is " + hit.collider.gameObject.name);
            if(hit.collider.gameObject.name == this.counterName)
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

    public void newKObj()
    {
        this.kObj = Instantiate(this.kObjSO.original);
        this.kObj.name = this.kObjSO.original.name;
        this.kObj.transform.SetParent(this.gameObject.transform.Find("CounterTop"));
        this.kObj.transform.localPosition = Vector3.zero;
        this.kObjScript = this.kObj.GetComponent<KObjScript>();
        this.kObjScript.setCurrentCounter(this.gameObject);
        this.kObj.SetActive(true);
        kObjCount++;
    }

    public int getKObjCount()
    {
        return kObjCount;
    }
    public GameObject getKObj()
    {
        return this.kObj;
    }

    public void setKObj(GameObject kObj)
    {
    }

    public void releaseKObj()
    {
        this.kObj = null;
    }
}
