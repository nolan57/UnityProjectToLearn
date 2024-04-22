using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

//using System.Numerics;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;
//using Vector3 = UnityEngine.Vector3;

public class OnHitedEventArgs:EventArgs
{
    public UnityEngine.GameObject hitedCounter
    {
        get;set;
    }
}
public class IsPlayerActionsEnableArgs:EventArgs
{
    public string whatHappen;
    public IsPlayerActionsEnableArgs(string whatHappen)
    {
        this.whatHappen=whatHappen;
    }
}


public class Player : MonoBehaviour,IPlayerInterActions
{
    public static Player instance
    {
        get;private set;
    }
    private KObjScript kObjScript;
    [SerializeField] private PlayerActions playerActions;
    [SerializeField] private float speed = 7f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float playerRadius = 0.7f;
    [SerializeField] private float playerHeight = 2f;
    [SerializeField] private float interactionDistance = 2f;
    [SerializeField] private LayerMask counterLayer;
    //[SerializeField] private Canvas canvas;
    private UnityEngine.GameObject counter;
    private OnHitedEventArgs onHitedEventArgs;
    public event EventHandler<OnHitedEventArgs> onHitedEvent;
    private bool Holded;
    private GameObject kObj;
    private Camera m_Camera;
    [SerializeField]private Speech speech;
    private bool _isPlayerActionsEnable;
    private bool isPlayerActionsEnable
    {
        get{return _isPlayerActionsEnable;}
        set
        { 
            _isPlayerActionsEnable = value;
            if(_isPlayerActionsEnable)
            {
                onPlayerActionsEnableEvent?.Invoke(true);
            }
        }
    }
    private event Action<bool> onPlayerActionsEnableEvent;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        this.transform.Find("PointerOver").gameObject.SetActive(false);
        Holded = false;
        transform.position = new Vector3(0, 0,-5);
        lastInteractionDir = transform.position;
        playerActions.onEKeyPressedEvent += toSelect;
        playerActions.onCKeyPressedEvent += toClear;
        playerActions.onPKeyPressedEvent += toPickUp;
        playerActions.onRKeyPressedEvent += toRelease;
        playerActions.onVKeyPressedEvent += toCut;
        playerActions.OnWSDAEvent += toMove;
        playerActions.OnArrowsEvent += toMove;
        playerActions.OnClickEvents += toClick;
        onPlayerActionsEnableEvent += onEnableAction;
        Physics.queriesHitTriggers = true;
        m_Camera = Camera.main;
        speech = this.GetComponentInChildren<Speech>();
        speech.toSpeech("");
    }

    private void toCut(object sender, InteractionArgs e)
    {
        if(!isPlayerActionsEnable)
        {
            return;
        }
        if(this.counter == null)
        {
            return;
        }
        CuttingCounter cuttingCounter = this.counter.GetComponent<CuttingCounter>();
        if(cuttingCounter == null)
        {
            //Debug.Log(this.counter.name + " Is Not a cutting counter!");
            speech.toSpeech(this.counter.name + " Is Not a cutting counter!");
            return;
        }
        if(cuttingCounter.getKObj() != null)
        {
            CutKObj toCutKObjAction = cuttingCounter.GetComponent<CutKObj>();
            toCutKObjAction?.toCutKObj(cuttingCounter.getKObj());
        }else
        {
            Debug.Log("Nothing to cut!");
            speech.toSpeech("Nothing to cut!");
        }
    }
    private void onEnableAction(bool e)
    {
        //Debug.Log(this.gameObject.name + "'s actions's enabled is " + e); 
        speech.toSpeech(this.gameObject.name + "'s actting is " + e);     
    }

    private void toClick(object sender, OnClickArgs e)
    {
        speech.toSpeech("Clicked!");
        Vector3 mousePosition = e.mousePosition;
        Ray ray = m_Camera.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if(hit.collider.gameObject.name == "Player")
            {
                this.isPlayerActionsEnable = true;
            }else
            {
                this.isPlayerActionsEnable = false;
                if(this.transform.Find("PointerOver").gameObject.activeSelf)
                {
                    this.transform.Find("PointerOver").gameObject.SetActive(false);
                }
            }
        }
    }

    private void toMove(object sender, MoveArgs e)
    {
        if(!isPlayerActionsEnable){
            return;
        }
        handleMove(e.vector,e.moveDir);
        doesHitSomething(/* e.vector, */e.moveDir);
    }

    private void toRelease(object sender, InteractionArgs e)
    {
        if(!isPlayerActionsEnable){
            return;
        }
        if(this.kObj != null && Holded)
        {
            if(this.counter != null)
            {
                if(this.counter.GetComponent<IContainerCounterActions>() != null)
                {
                    return;
                }
                IClearCounterActions clearCounter = this.counter.GetComponent<IClearCounterActions>();
                ICuttingCounterActions cuttingCounter = this.counter.GetComponent<ICuttingCounterActions>();
                if(clearCounter != null)
                {
                    clearCounter.setKObj(this.kObj);
                    Holded = false;
                }
                if(cuttingCounter != null)
                {
                    cuttingCounter.setKObj(this.kObj);
                    Holded = false;
                }
            }

        }else{
            //Debug.Log("Nothing to put down!");
            speech.toSpeech("Nothing to put down!");
        }
    }

    private void toPickUp(object sender, InteractionArgs e)
    {
        if(!isPlayerActionsEnable){
            return;
        }
        if(this.counter != null)
        {
            IBaseActions comCounter = this.counter.GetComponent<IBaseActions>();
            if(comCounter.getKObj() != null)
            {
                if(comCounter.getKObj().transform.parent.transform.gameObject != this){
                    if(!Holded){
                        setKObj(comCounter.getKObj());
                        comCounter.releaseKObj();
                        Holded = true;
                    }
                }else{
                    //Debug.Log("You already have the Kitchen Object");
                    speech.toSpeech("You already have the Kitchen Object");
                }
            }
        }else{
            //Debug.Log("You should select first!");
            speech.toSpeech("You should select a counter first");
        }
    }

    private void toClear(object sender, InteractionArgs e)
    {
        if(!isPlayerActionsEnable){
            return;
        }
        ClearKObj();
    }

    private void ClearKObj()
    {
        if(this.counter != null){
            IBaseActions thisConterBaseActions = this.counter.GetComponent<IBaseActions>();
            if(thisConterBaseActions.getKObj() != null){
                if(thisConterBaseActions.getNext() != null)
                {
                    UnityEngine.GameObject nextCounter = thisConterBaseActions.getNext();
                    IBaseActions nextCounterActions = nextCounter.GetComponent<IBaseActions>();
                    if(nextCounterActions.getKObj() == null )
                    {
                        nextCounterActions.setKObj(thisConterBaseActions.getKObj());
                        thisConterBaseActions.releaseKObj();
                    }else
                    {
                        //Debug.Log("Next Counter isn't Clear!");
                        speech.toSpeech("Next counter isn't Clear!");
                    }
                }else
                {
                    //Debug.Log("No Next Counter!");
                    speech.toSpeech("No Next Counter!");
                    Destroy(thisConterBaseActions.getKObj());
                }
            }else
            {
                //Debug.Log("Nothing to be Cleared!");
                speech.toSpeech("Nothing to be Cleared!");
            }
        }else
        {
            //Debug.Log("Null to Invoke!");
            speech.toSpeech("Null to Invoke!");
        }
    }

    private void toSelect(object sender, InteractionArgs e)
    {
        if(!isPlayerActionsEnable){
            return;
        }
        if(this.counter != null)
        {
            if(this.counter.GetComponent<IContainerCounterActions>() == null)
            {
                return;
            }
            //Debug.Log("Newing...");
            speech.toSpeech("Newing...");
            newKObj();
        }else
        {
            return;
        }
    }
    private void newKObj()
    {
        Quaternion roatation = Quaternion.identity;
        roatation.Set(0,0,0,0);
        IBaseActions containerCounter = this.counter.GetComponent<IBaseActions>();
        IContainerCounterActions containerCounterActions = this.counter.GetComponent<IContainerCounterActions>();
        if(containerCounter.getKObj() != null)
        {
            //Debug.Log("No Kitchen Object to NEW!");
            speech.toSpeech("No kitchen Objec to NEW!");
            return;
        }else
        {
            //KitcherObjectSO newKObj = Instantiate<KitcherObjectSO>(containerCounter.getKObj());
            //containerCounter.setKObj(newKObj);
            containerCounterActions.newKObj();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
    }
    private bool isWalking;
    private Vector3 lastInteractionDir;
    // Update is called once per frame
    void Update()
    {
    }

    public bool IsWalking(){
        return isWalking;
    } 

    private void handleMove(Vector2 moved,Vector3 moveDir)
    {
        float moveDistance = speed;//*eee;//*Time.deltaTime;
        bool canMove = !Physics.CapsuleCast(transform.position,transform.position+Vector3.up*playerHeight,playerRadius,moveDir,moveDistance);
        if(canMove)
        {
            transform.position += moveDir*speed;//*eee;//Time.deltaTime;
        }else
        {
            moveDir = new Vector3(moved.x,0,0);
            if(!Physics.CapsuleCast(transform.position,transform.position+Vector3.up*playerHeight,playerRadius,moveDir,moveDistance))
            {
                transform.position += moveDir*speed;//*eee;//Time.deltaTime;
            }else
            {
                moveDir = new Vector3(0,0,moved.y);
                if(!Physics.CapsuleCast(transform.position,transform.position+Vector3.up*playerHeight,playerRadius,moveDir,moveDistance))
                {
                    transform.position += moveDir*speed;//*eee;//Time.deltaTime;
                }
            }
        }
        isWalking = moveDir != Vector3.zero;
        transform.forward = Vector3.Slerp(transform.forward,moveDir,/* Time.deltaTime *//* eee* */rotateSpeed);
    }
    private void doesHitSomething(/* Vector2 moved, */Vector3 moveDir)
    {
        if(moveDir != Vector3.zero)
        {
            lastInteractionDir = moveDir;
        }
        bool hit = Physics.Raycast(transform.position+Vector3.up*(playerHeight/5),lastInteractionDir,out RaycastHit hitedObject,interactionDistance,counterLayer);
        if(hit)
        {
            UnityEngine.GameObject cObject = hitedObject.collider.gameObject;
            if(cObject != null)
            {
                Debug.Log("hitted is "+cObject.name);
                this.counter = cObject;
                setViusal(cObject);              
            }else
            {
                this.counter = null;
                setViusal(null);
            }
        }else
        {
            this.counter = null;
            setViusal(null);
        }
    }
    private void setViusal(UnityEngine.GameObject counter)
    {
        onHitedEventArgs = new();
        this.onHitedEventArgs.hitedCounter = counter;
        onHitedEvent?.Invoke(this,onHitedEventArgs);
    }

    public void setKObj(GameObject obj)
    {
        this.kObj = obj;
        this.kObj.transform.SetParent(this.transform.Find("HoldPoint"));
        this.kObj.transform.localPosition=Vector3.zero;
        this.kObjScript = this.kObj.GetComponent<KObjScript>();
        this.kObjScript.setCurrentCounter(this.gameObject);
        this.kObj.SetActive(true);
    }
    void OnMouseEnter()
    {
        if(this.isPlayerActionsEnable == true)
        {
            return;
        }
        this.transform.Find("PointerOver").gameObject.SetActive(true);
    }
    void OnMouseExit()
    {
        if(this.isPlayerActionsEnable == false)
        {
            this.transform.Find("PointerOver").gameObject.SetActive(false);
        }
    }
}

internal interface IPlayerInterActions
{
}