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

public class CuttingCounter : BaseCounter//,IKObjInterActions
{
    public static CuttingCounter instance
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
    //private new Camera m_Camera;
    private new string hitedCounterName;
    new void Awake()
    {
        base.Awake();
        if(instance == null)
        {
            instance = this;
        }
        BaseCounter.CounterList.Add(this);
        Debug.Log("tomatoCounterList now has " + CounterList.Count());
        BaseCounter.playerActions.OnClickEvents += toClick;
        onCounterActionsEnableEvent += OnCounterActionsEnable;
        //m_Camera = Camera.main;
        hitedCounterName = "TomatoCuttingCounterSelected" + InstanceID;
        this.GetTransform().Find("TomatoCuttingCounterSelected").name = hitedCounterName;
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
    public new void setSelectedPartVisual(bool viusal)
    {
        this.selectedPart.SetActive(viusal);
    }
    public new IKObjInterActions getNext()
    {
        Debug.Log("This is Cutting Counter!");
        return null;
    }
    public new KitcherObjectSO GetKitcherObjectSO()
    {
        Debug.Log("This is Cutting Counter only for Cutting");
        return null;
    }
}
