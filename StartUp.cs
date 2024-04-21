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
        prefabPaths = new string[]
        {
            "PreObjects/TomatoContainerCounter",
            "PreObjects/CuttingCounter",
            "PreObjects/ClearCounter",
        };
        
        setCounterList(prefabPaths,new float2(0,0),Quaternion.identity);
        prefabPaths = new string[]
        {
            "PreObjects/CheezeContainerCounter",
            "PreObjects/CuttingCounter",
            "PreObjects/ClearCounter",
        };
        setCounterList(prefabPaths,new float2(0,-10),Quaternion.Euler(0f,180f,0f));
        setOrder();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void setCounterList(string[] prefabPaths,float2 startPoint,Quaternion rotation)
    {
        for (int i = 0;i<prefabPaths.Count();i++)
        {
            string prefabPath = prefabPaths[i];
            UnityEngine.GameObject prefab = Resources.Load<UnityEngine.GameObject>(prefabPath);
            float row = startPoint.x;
            float col = startPoint.y;
            Vector3 position = new Vector3(row,0,col);
            if (prefab != null)
            {             
                Debug.Log("Creating " + prefabPath);
                position = position + new Vector3(-1.5f*i,0,0);
                UnityEngine.GameObject newObject = Instantiate(prefab, position, rotation);
                newObject.SetActive(true);
            }
            else
            {
                Debug.LogError("Failed to load prefab at path: " + prefabPath);
            }
        }
    }
    private void setOrder()
    {
        for(int i = 0;i<BaseCounter.CounterList.Count();i++)
        {
            if(i+1<BaseCounter.CounterList.Count())
            {
                BaseCounter bcScript = BaseCounter.CounterList[i].GetComponentInParent<BaseCounter>();
                bcScript.setNext(BaseCounter.CounterList[i+1]);
            }else{
                BaseCounter bcScript = BaseCounter.CounterList[i].GetComponentInParent<BaseCounter>();
                bcScript.setNext(null);
            }
        }
    }
}
