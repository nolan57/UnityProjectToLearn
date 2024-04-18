using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionArgs:EventArgs{
    /* public string datas{
        set;get;
    } */
    public InteractionArgs(){
        //datas="no datas";
    }
}
public class MoveArgs:EventArgs{
    public Vector2 vector{set;get;}
    public Vector3 moveDir{set;get;}
    public MoveArgs(){
        vector = new Vector2(0,0);
        moveDir = new Vector3(0,0,0);
    }

}
public class OnClickArgs:EventArgs
{
    public Vector3 mousePosition;
    public OnClickArgs(Vector3 mousePosition)
    {
        this.mousePosition = mousePosition;
    }
}

public class PlayerActions : MonoBehaviour
{
    private PlayerInputAction inputActions;
    public event EventHandler<InteractionArgs> onEKeyPressedEvent;
    public event EventHandler<InteractionArgs> onCKeyPressedEvent;
    public event EventHandler<InteractionArgs> onPKeyPressedEvent;
    public event EventHandler<InteractionArgs> onRKeyPressedEvent;
    public event EventHandler<InteractionArgs> onVKeyPressedEvent;
    public event EventHandler<MoveArgs> OnWSDAEvent;
    public event EventHandler<MoveArgs> OnArrowsEvent;
    public event EventHandler<OnClickArgs> OnClickEvents;
    private InteractionArgs interactionArgs;
    private MoveArgs moveArgs;
    private OnClickArgs onClickArgs;
    private void Awake(){
        interactionArgs = new InteractionArgs();
        moveArgs = new MoveArgs();
        inputActions = new PlayerInputAction();
        inputActions.Move.WSDA.performed += OnWSDA_performed;
        inputActions.Move.Arrows.performed += OnArrows_performed;
        inputActions.Interaction.Select.performed += eventByEKeyPressed;
        inputActions.Interaction.Clear.performed += eventByCKeyPressed;
        inputActions.Interaction.PickUp.performed += eventByPKeyPressed;
        inputActions.Interaction.Releasse.performed += eventByRKeyPressed;
        inputActions.ToSelectObject.Click.performed += eventByMouseLeftButtonClicked;
        inputActions.Interaction.Cut.performed += eventByVKeyPressed;
        inputActions.Enable();
    }

    private void eventByVKeyPressed(InputAction.CallbackContext context)
    {
        onVKeyPressedEvent?.Invoke(this, interactionArgs);
    }

    private void eventByMouseLeftButtonClicked(InputAction.CallbackContext context)
    {
        //Vector3 mousePosition = context.ReadValue<Vector3>();
        //Vector3 mousePosition = Input.mousePosition;
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        Debug.Log("The mouse position is " + mousePosition);
        OnClickEvents?.Invoke(this, new OnClickArgs(mousePosition));
    }

    private void OnArrows_performed(InputAction.CallbackContext context)
    {
        moveArgs.vector = inputActions.Move.Arrows.ReadValue<Vector2>();
        moveArgs.moveDir = new Vector3(moveArgs.vector .x,0f,moveArgs.vector .y);
        OnArrowsEvent?.Invoke(this,moveArgs);
    }

    private void OnWSDA_performed(InputAction.CallbackContext context)
    {
        moveArgs.vector = inputActions.Move.WSDA.ReadValue<Vector2>();
        moveArgs.moveDir = new Vector3(moveArgs.vector .x,0f,moveArgs.vector .y);
        OnWSDAEvent?.Invoke(this,moveArgs);
    }

    private void eventByRKeyPressed(InputAction.CallbackContext context)
    {
        onRKeyPressedEvent?.Invoke(this, interactionArgs);
    }

    private void eventByPKeyPressed(InputAction.CallbackContext context)
    {
        onPKeyPressedEvent?.Invoke(this,interactionArgs);
    }

    private void eventByCKeyPressed(InputAction.CallbackContext context)
    {
        onCKeyPressedEvent?.Invoke(this, interactionArgs);
    }

    private void eventByEKeyPressed(InputAction.CallbackContext context)
    {
        //e.datas = "E key pressed";
        onEKeyPressedEvent?.Invoke(this,interactionArgs);
    }

    // Start is called before the first frame update
    public Vector2 getVector2(){
        
        Vector2 vector = inputActions.Move.WSDA.ReadValue<Vector2>();

        /* Vector2 vector = new Vector2(0,0);
        //if(Input.GetKey(KeyCode.W)){
        if(Keyboard.current.wKey.isPressed){
            //Debug.Log(" W key is pressed!");
            vector.y +=1;
        }

        if(Keyboard.current.sKey.isPressed){
            //Debug.Log(" S key is pressed!");
            vector.y -= 1;
        }

        if(Keyboard.current.dKey.isPressed){
            //Debug.Log(" D key is pressed!");
            vector.x +=1;
        }

        if(Keyboard.current.aKey.isPressed){
            //Debug.Log(" A key is pressed!");
            vector.x -= 1;
        }
        vector = vector.normalized; */
        return vector;
    }
    public PlayerInputAction getPlayerInputAction()
    {
        return inputActions;
    }
}
