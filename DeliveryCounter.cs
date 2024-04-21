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

public class DeliveryCounter : BaseCounter//,IKObjInterActions
{
    public static DeliveryCounter instance
    {
        get;private set;
    }
    //private static BaseCounter[] cList;
    //[SerializeField] private new GameObject selectedPart;
    private KObjScript kObjScript;
    private new UnityEngine.GameObject nextCounter;
    private KitcherObjectSO kObj;
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
    //private new Camera m_Camera;
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
        this.gameObject.transform.Find("DeliveryCounterSelected").name = counterName;
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
}
