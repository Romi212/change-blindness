using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;




public class restore : MonoBehaviour

    
{
    public Camera user;
    
    public Room[] rooms;


    private Quaternion cameraRot;

    public float intensity;
    public bool hasStarted = false;


    private static Vector3 RIGHT = new Vector3(1, 0, 0);
    private static Vector3 LEFT = new Vector3(-1, 0, 0);
    private static Vector3 UP = new Vector3(0, 1, 0);
    private static Vector3 DOWN = new Vector3(0, -1, 0);

    public GameObject lobby;
    public GameObject wallMedium;
    public door door1;

    private Room currentRoom;
        

    
    // Start is called before the first frame update
    void Start()
    {
        
        foreach (Room r in rooms)
        {
            r.user = user;
            r.d = intensity;
            r.Initialize();
        }

        cameraRot = user.transform.rotation;
        


    }

    // Update is called once per frame
    void Update()
    {
        //rooms[0].checkNewWallsToMove();
        if (hasStarted && user.transform.rotation != cameraRot)
        {
            cameraRot = user.transform.rotation;
            print("CAMERA CHANBGED");
            if (!currentRoom.IsRestored())
            {
                currentRoom.checkNewWallsToMove();
                ArrayList log = currentRoom.getLog();
                foreach (var msg in log)
                {
                    print(msg);
                }
            }
            else
            {
                hasStarted = false;
                print("ROOM RESTORED");
                door1.enableDoor();
                compressRest();
            }
            
        }
    }

    public void StartRestoring()
    {
        hasStarted = true;
    }
    
    public void Entered(Room r)
    {
        if(r.isRestored)
        {
            return;
        }
        hasStarted = true;
        currentRoom = r;
        r.boxOpened();
        foreach(Room room in rooms)
        {
            if(room != r)
            {
                room.Hide();
            }
        }
    }

    private void compressRest()
    {
        foreach(Room r in rooms)
        {
            if(r != currentRoom)
            {
                r.Compress();
            }
        }
    }   

    
}
