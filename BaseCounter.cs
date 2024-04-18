using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseCounter : MonoBehaviour,IKObjInterActions
{

    protected static int NextID = 0;
    protected int instanceID;

    public int InstanceID
    {
        get { return instanceID; }
    }
    [SerializeField] protected GameObject ClearCounterTop;
    [SerializeField] protected KitcherObjectSO kitcherObject;
    /* [SerializeField]  */protected static PlayerActions playerActions;
    protected KObjScript kObjScript;
    protected IKObjInterActions nextCounter;
    protected GameObject kObj;
    [SerializeField] protected GameObject selectedPart;
    public static List<IKObjInterActions> CounterList;
    protected static Camera m_Camera;
    protected string hitedCounterName;
    protected bool _isCounterActionsEnable;
    protected void Awake()
    {
        instanceID = NextID;
        NextID++;
        if(CounterList == null)
        {
            //Debug.Log("new List<ICounterAtions>!");
            CounterList = new List<IKObjInterActions>();
        }
        if(CounterList == null)
        {
            //Debug.Log("new List<ICounterAtions>!");
            CounterList = new List<IKObjInterActions>();
        }

        if(playerActions == null)
        {
            playerActions = GameObject.FindFirstObjectByType<PlayerActions>();
        }
        if(m_Camera == null)
        {
            m_Camera = Camera.main;
        }
        hitedCounterName = "Base Counter" + InstanceID;
    }
    protected void Start()
    {
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
    public void setKObj()
    {

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
    }

    public void setSelectedPartVisual(bool viusal)
    {
        this.selectedPart.SetActive(viusal);
    }

    public GameObject getGameObject()
    {
        return this.gameObject;
    }
}
