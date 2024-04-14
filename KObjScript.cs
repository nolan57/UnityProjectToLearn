using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KObjScript : MonoBehaviour
{
    //[SerializeField] KitcherObjectSO kObjSO;
    private IKObjInterActions currentParent{
        get;set;
    }
    // private BaseCounter PreviousCounter{
    //     get;set;
    // }
    // Start is called before the first frame update
    void Start()
    {
        //BaseCounter.instance.clearEvenByCounter += toClear;
    }
    // Update is called once per frame
    void Update()
    {
    }
    public IKObjInterActions getCurrentParent(){
        return currentParent;
    }
/*     public BaseCounter getPreviousCounter(){
        return PreviousCounter;
    } */
    public void setCurrentParent(IKObjInterActions currentParent){
        this.currentParent=currentParent;
    }
/*     public void setPreviousCounter(BaseCounter previousCounter){
        this.PreviousCounter=previousCounter;
    } */
}
