using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterSelected : MonoBehaviour
{
    private GameObject hitedObject;
    private void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        Player.instance.onHitedEvent += changeState;
        //Player.instance.onHitedEvent += toChangeState;
    }
    private void changeState(object sender, OnHitedEventArgs e)
    {
        //BaseCounter parent = this.GetComponentInParent<BaseCounter>();
        if(e.hitedCounter != null)
        {
            //if(e.hitedCounter.getName() == parent.name)
            //{
                //Debug.Log("The object hited is " + e.hitedCounter.getName());
                Debug.Log("Set the visual!");
                e.hitedCounter.setSelectedPartVisual(true);
                //show();
            //}
        }else
        {
            hide();
        }
    }
    private void show(GameObject selectedPart)
    {
        selectedPart.SetActive(true);
    }
    private void hide()
    {
        gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
    }
}