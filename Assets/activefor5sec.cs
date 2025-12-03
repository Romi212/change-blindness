using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activefor5sec : MonoBehaviour
{

    public float activeTime = 5.0f;
    private float activeCounter = 0.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.activeSelf)
        {
            activeCounter += Time.deltaTime;
            if(activeCounter >= activeTime)
            {
                activeCounter = 0.0f;
                this.gameObject.SetActive(false);
                
            }
        }
    }
}
