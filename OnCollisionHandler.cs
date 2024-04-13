using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionHandler : MonoBehaviour
{
    // Start is called before the first frame update
   void OnCollisionEnter(Collision collision){

        Debug.Log("collision!");
        if(collision.gameObject != null){
            if(collision.gameObject.GetType() == typeof(Player)){
                Debug.Log("hited with "+collision.gameObject.name);
            }
        }

    }

    void OnCollisionStay(Collision collision)
    {
        //Debug.Log("Collision Stay with: " + collision.gameObject.name);

    }

    void OnCollisionExit(Collision collision)
    {
        //Debug.Log("Collision Exit with: " + collision.gameObject.name);

    }

    /* void OnTriggerEnter(Collider collider){

        if(collider.gameObject != null){
            if(collider.gameObject.GetType() == typeof(Player)){
                Debug.Log("hited by trigged with "+collider.gameObject.name);
            }
        }

    } */

}
