using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger : MonoBehaviour
{
    public restore restorer;
    public GameObject wall;
    public GameObject lobby;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if( other.tag == "MainCamera")
        {
            print("TRIGGERED");
            wall.SetActive(true);
            
            restorer.StartRestoring();
            lobby.SetActive(false);
        }
            
        
    }
}
