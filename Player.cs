using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

//using System.Numerics;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
//using Vector3 = UnityEngine.Vector3;

public class OnHitedEventArgs:EventArgs
{
    public IKObjInterActions hitedCounter
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


public class Player : MonoBehaviour,IKObjInterActions//,IPointerDownHandler
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
    private IKObjInterActions counter;
    private OnHitedEventArgs onHitedEventArgs;
    public event EventHandler<OnHitedEventArgs> onHitedEvent;
    private bool Holded;
    private GameObject kObj;
    private Camera m_Camera;
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
    //private event Action<IsEnableArgs> onEnableEvent;
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
        playerActions.OnWSDAEvent += toMove;
        playerActions.OnArrowsEvent += toMove;
        playerActions.OnClickEvents += toClick;
        onPlayerActionsEnableEvent += onEnableAction;
        Physics.queriesHitTriggers = true;
        m_Camera = Camera.main;
    }

    private void onEnableAction(bool e)
    {
        Debug.Log(this.gameObject.name + "'s actions's enabled is " + e);
    }

    private void toClick(object sender, OnClickArgs e)
    {
        Debug.Log("Clicked!");
        Vector3 mousePosition = e.mousePosition;
        Ray ray = m_Camera.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("Hited is " + hit.collider.gameObject.name);
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
                if(this.counter.getKObj() == null)
                {
                    this.counter.setKObj(Instantiate(this.kObj));
                    Holded = false;
                    releaseKObj();
                }else{
                    Debug.Log("The counter is not clear!");
                }
            }

        }else{
            Debug.Log("Nothing to put down!");
        }
    }

    private void toPickUp(object sender, InteractionArgs e)
    {
        if(!isPlayerActionsEnable){
            return;
        }
        if(this.counter != null)
        {
            if(this.counter.getKObj() != null)
            {
                if(this.counter.getKObj().transform.parent.transform.gameObject != this){
                    if(!Holded){
                        setKObj(Instantiate(this.counter.getKObj()));
                        //this.counter.getKObj().transform.SetParent(this.transform.Find("HoldPoint"));
                        //this.counter.getKObj().transform.localPosition = Vector3.zero;
                        Holded = true;
                        this.counter.releaseKObj();
                    }
                }else{
                    // this.counter.getKObj().transform.SetParent(this.counter.GetTransform().Find("CounterTop"));
                    // this.counter.getKObj().transform.localPosition = Vector3.zero;
                    // Holded = false;
                    Debug.Log("You already have the Kitchen Object");
                }
            }
        }else{
            Debug.Log("You should select first!");
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
            Debug.Log("To Clear Counter!");
            if(this.counter.getKObj() != null){
                if(this.counter.getNext() != null)
                {
                    if(this.counter.getNext().getKObj() == null )
                    {
                        IKObjInterActions nextCounter = this.counter.getNext();
                        //GameObject kObj = Instantiate(this.counter.getKObj());
                        nextCounter.setKObj(Instantiate(this.counter.getKObj()));
                        //nextCounter.getKObj().transform.SetParent(nextCounter.GetTransform().Find("CounterTop"));
                        //nextCounter.getKObj().transform.localPosition=Vector3.zero;
                        // this.counter.setKObjScript(nextCounter.getKObj().GetComponent<KObjScript>());
                        // this.counter.getKObjScript().setCurrentParent(nextCounter);
                        // this.counter.getKObj().SetActive(true);
                        this.counter.releaseKObj();
                    }else
                    {
                        Debug.Log("Next Counter isn't Clear!");
                    }
                }else
                {
                    Debug.Log("Not Next Counter!");
                    this.counter.releaseKObj();
                }
            }else
            {
                Debug.Log("Nothing to be Cleared!");
            }
        }else
        {
            Debug.Log("Null to Invoke!");
        }
    }

    private void toSelect(object sender, InteractionArgs e)
    {
        if(!isPlayerActionsEnable){
            return;
        }
        if(this.counter != null)
        {
            /* if(this.counter.GetType() == typeof(ClearCounter))
            {
                Debug.Log("This is Clear Counter,Not a Container Counter");
                return;
            } */
            newKObj();
        }else
        {
            Debug.Log("Null to Put Kitchen Object");
            return;
        }
    }
    private void newKObj()
    {
        Debug.Log("To Select Kitchen Object!");
        Quaternion roatation = Quaternion.identity;
        roatation.Set(0,0,0,0);
        if(this.counter.getKObj() != null || this.counter.GetKitcherObjectSO() == null)
        {
            Debug.Log("No Kitchen Object to NEW!");
            return;
        }else
        {
            GameObject newKObj = (GameObject)Instantiate(
                                    this.counter.
                                    GetKitcherObjectSO().kObj,
                                    SceneManager.GetActiveScene());
            this.counter.setKObj(newKObj);
            //this.counter.getKObj().transform.SetParent(this.counter.GetTransform().Find("CounterTop"));
            //this.counter.getKObj().transform.localPosition = Vector3.zero;
            // this.counter.setKObjScript(this.counter.getKObj().GetComponent<KObjScript>());
            // this.counter.getKObjScript().setCurrentParent(this.counter);
            // this.counter.getKObj().SetActive(true);
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
        //Vector2 moved = gameInput.getVector2();
        //Vector3 moveDir = new Vector3(moved.x,0f,moved.y);
        //handleMove(moved,moveDir);
        //doesHitSomething(moved,moveDir);
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
            bool geted = hitedObject.transform.TryGetComponent<IKObjInterActions>(out IKObjInterActions cObject);

            if(geted)
            {
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
    private void setViusal(IKObjInterActions counter)
    {
        onHitedEventArgs = new();
        this.onHitedEventArgs.hitedCounter = counter;
        onHitedEvent?.Invoke(this,onHitedEventArgs);
    }

    public void setKObjScript(KObjScript kObjScript)
    {
        this.kObjScript = kObjScript;
    }

    public KObjScript getKObjScript()
    {
        return this.kObjScript;
    }

    public void setNext(IKObjInterActions next)
    {
        Debug.Log("No need to set next!");
    }

    public IKObjInterActions getNext()
    {
        return null;
    }

    public KitcherObjectSO GetKitcherObjectSO()
    {
        return null;
    }

    public GameObject getKObj()
    {
        return kObj;
    }

    public void setKObj(GameObject obj)
    {
        this.kObj = obj;
        this.kObj.transform.SetParent(this.transform.Find("HoldPoint"));
        this.kObj.transform.localPosition=Vector3.zero;
        this.kObjScript = this.kObj.GetComponent<KObjScript>();
        this.kObjScript.setCurrentParent(this);
        this.kObj.SetActive(true);
    }

    public Transform GetTransform()
    {
        return this.gameObject.transform;
    }

    public string getName()
    {
        return this.name;
    }

    public int getInstanceID()
    {
        return this.GetInstanceID();
    }

    public void releaseKObj()
    {
        Destroy(this.kObj);;
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

    public Transform getSelectedPart()
    {
        return null;
    }

    public void setSelectedPartVisual(bool viusal)
    {
    }
}