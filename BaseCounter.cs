using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseCounter : MonoBehaviour//,IKObjInterActions
{

    protected static int NextID = 0;
    protected int instanceID;

    public int InstanceID
    {
        get { return instanceID; }
    }
    [SerializeField] protected UnityEngine.GameObject CounterTop;
    protected static PlayerActions playerActions;
    protected UnityEngine.GameObject nextCounter;
    [SerializeField] protected UnityEngine.GameObject hittedPart;
    public static List<UnityEngine.GameObject> CounterList;
    protected static Camera m_Camera;
    protected void Awake()
    {
        instanceID = NextID;
        NextID++;
        if(CounterList == null)
        {
            CounterList = new List<UnityEngine.GameObject>();
        }
        if(playerActions == null)
        {
            playerActions = UnityEngine.GameObject.FindFirstObjectByType<PlayerActions>();
        }
        if(m_Camera == null)
        {
            m_Camera = Camera.main;
        }
    }
    protected void Start()
    {
    }
    public void setNext(UnityEngine.GameObject nextCounter)
    {
        this.nextCounter = nextCounter;
    }
    public UnityEngine.GameObject getNext()
    {
        return this.nextCounter;
    }
    public int getInstanceID()
    {
        return this.instanceID;
    }
    public void setHittedPartVisual(bool viusal)
    {
        this.hittedPart.SetActive(viusal);
    }
}
public interface IBaseActions
{
    public GameObject getKObj();
    public UnityEngine.GameObject getNext();
    public void setKObj(GameObject kObj);
    public void releaseKObj();
}