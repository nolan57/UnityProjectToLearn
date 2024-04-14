using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUp : MonoBehaviour
{
    private string[] prefabPaths;
    private Scene currentScene;
    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        //prefabPath = "PreObjects/Shaped";
        prefabPaths = new string[]
        {
            "PreObjects/TomatoContainerCounter",
            "PreObjects/TomatoClearCounter",
            "PreObjects/TomatoClearCounter"
        };
        /* for (int i = 0;i<prefabPaths.Count();i++)
        {
            string prefabPath = prefabPaths[i];
            GameObject prefab = Resources.Load<GameObject>(prefabPath);
            Vector3 position = new Vector3(0, 0,0);
            if (prefab != null)
            {             
                position = position + new Vector3(-1.5f*i,0,0);
                Quaternion rotation = Quaternion.identity;
                GameObject newObject = Instantiate(prefab,position,rotation);
                newObject.SetActive(true);
            }
            else
            {
                Debug.LogError("Failed to load prefab at path: " + prefabPath);
            }
        }
        //Debug.Log("List has " + BaseCounter.counterList.Count + " counters!");
        for(int i = 0;i<BaseCounter.tomatoCounterList.Count();i++)
        {
            //Debug.Log(" List[" + BaseCounter.counterList[i].getInstanceID() + "] is " + BaseCounter.counterList[i].getName() + "\n");
            if(i+1<BaseCounter.tomatoCounterList.Count())
            {
                BaseCounter.tomatoCounterList[i].setNext(BaseCounter.tomatoCounterList[i+1]);
                //Debug.Log(" List[" + BaseCounter.counterList[i].getInstanceID() + "] 's next counter is " + BaseCounter.counterList[i].getNext().getName() + "\n");
            }else{
                BaseCounter.tomatoCounterList[i].setNext(null);
            }
        } */
        setCounterList(prefabPaths,"Tomato",new float2(0,0));
        prefabPaths = new string[]
        {
            "PreObjects/CheezeContainerCounter",
            "PreObjects/CheezeClearCounter",
            "PreObjects/CheezeClearCounter"
        };
        setCounterList(prefabPaths,"Cheeze",new float2(0,-10));
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void setCounterList(string[] prefabPaths,string CounterType,float2 startPoint)
    {
        for (int i = 0;i<prefabPaths.Count();i++)
        {
            string prefabPath = prefabPaths[i];
            GameObject prefab = Resources.Load<GameObject>(prefabPath);
            float row = startPoint.x;
            float col = startPoint.y;
            Quaternion rotation = Quaternion.identity;
            Vector3 position = new Vector3(row,0,col);
            if (prefab != null)
            {             
                Debug.Log("Creating " + prefabPath);
                position = position + new Vector3(-1.5f*i,0,0);
                if(CounterType == "Cheeze")
                {
                    rotation = Quaternion.Euler(0f,180f,0f);
                }
                GameObject newObject = Instantiate(prefab,position,rotation);
                newObject.SetActive(true);
            }
            else
            {
                Debug.LogError("Failed to load prefab at path: " + prefabPath);
            }
        }
        List<IKObjInterActions> counterList = new List<IKObjInterActions>();
        if(CounterType == "Tomato")
        {
            counterList = BaseCounter.tomatoCounterList;
        }
        if(CounterType == "Cheeze")
        {
            counterList = BaseCounter.cheezeCounterList;
        }
        if(counterList.Count() == 0){
            Debug.Log("Counter list is null!");
            return;
        }
        for(int i = 0;i<counterList.Count();i++)
        {
            //Debug.Log(" List[" + BaseCounter.counterList[i].getInstanceID() + "] is " + BaseCounter.counterList[i].getName() + "\n");
            if(i+1<counterList.Count())
            {
                counterList[i].setNext(counterList[i+1]);
                //Debug.Log(" List[" + BaseCounter.counterList[i].getInstanceID() + "] 's next counter is " + BaseCounter.counterList[i].getNext().getName() + "\n");
            }else{
                counterList[i].setNext(null);
            }
        }
    }
}
