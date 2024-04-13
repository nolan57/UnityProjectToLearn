using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUp : MonoBehaviour
{
    private string[] prefabPaths;
    // Start is called before the first frame update
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        //prefabPath = "PreObjects/Shaped";
        prefabPaths = new string[]{"PreObjects/ClearCounter","PreObjects/ContainerCounter"};
        for (int i = 0;i<prefabPaths.Count();i++)
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
        for(int i = 0;i<BaseCounter.counterList.Count();i++)
        {
            //Debug.Log(" List[" + BaseCounter.counterList[i].getInstanceID() + "] is " + BaseCounter.counterList[i].getName() + "\n");
            if(i+1<BaseCounter.counterList.Count())
            {
                BaseCounter.counterList[i].setNext(BaseCounter.counterList[i+1]);
                //Debug.Log(" List[" + BaseCounter.counterList[i].getInstanceID() + "] 's next counter is " + BaseCounter.counterList[i].getNext().getName() + "\n");
            }else{
                BaseCounter.counterList[i].setNext(null);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
