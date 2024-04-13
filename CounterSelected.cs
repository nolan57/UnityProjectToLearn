using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterSelected : MonoBehaviour
{
    private void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        Player.instance.onHitedEvent += changeState;
    }

    private void changeState(object sender, OnHitedEventArgs e)
    {

        BaseCounter parent = this.GetComponentInParent<BaseCounter>();

        if(e.hitedCounter != null){

            if(e.hitedCounter.getName() == parent.name){
                show();
            }

        }else{
            hide();
        }
        
    }

    private void show(){
        gameObject.SetActive(true);
    }
    private void hide(){
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
