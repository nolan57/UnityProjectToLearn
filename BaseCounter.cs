using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseCounter : MonoBehaviour
{

    protected static int NextID = 0;
    protected int instanceID;

    public int InstanceID
    {
        get { return instanceID; }
    }
    [SerializeField] protected GameObject ClearCounterTop;
    [SerializeField] protected KitcherObjectSO kitcherObject;
    public static List<IKObjInterActions> counterList;
    protected void Awake()
    {
        instanceID = NextID;
        NextID++;
        if(counterList == null)
        {
            //Debug.Log("new List<ICounterAtions>!");
            counterList = new List<IKObjInterActions>();
        }
    }
    protected void Start()
    {
    }

}
