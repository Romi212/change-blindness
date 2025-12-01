using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger : MonoBehaviour
{
    public restore restorer;
    public GameObject wall;
    public door door1;

    private bool triggered = false;
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
        
        if( !triggered && other.tag == "MainCamera")
        {
            triggered = true;
            print("TRIGGERED");
            wall.SetActive(true);
            
            restorer.StartRestoring();
            lobby.SetActive(false);
            door1.closeDoor();
        }
            
        
    }

    private void OnTriggerExit(Collider other)
    {
        if(triggered && other.tag == "MainCamera")
        {
            triggered = false;
            
        }
    }
}
