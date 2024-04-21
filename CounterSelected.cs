using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterSelected : MonoBehaviour
{
    private UnityEngine.GameObject hitedObject;
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
        if(e.hitedCounter != null)
        {
            Debug.Log("Set the visual!");
            e.hitedCounter.GetComponentInParent<BaseCounter>().setHittedPartVisual(true);
        }else
        {
            hide();
        }
    }
    private void show(UnityEngine.GameObject selectedPart)
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