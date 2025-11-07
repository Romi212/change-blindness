using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;




public class restore : MonoBehaviour

    
{
    public Camera user;
    
    public GameObject[] walls;

    private Quaternion cameraRot;

    public float intensity = 1;


    private static Vector3 RIGHT = new Vector3(1, 0, 0);
    private static Vector3 LEFT = new Vector3(-1, 0, 0);
    private static Vector3 UP = new Vector3(0, 1, 0);
    private static Vector3 DOWN = new Vector3(0, -1, 0);

    
        

    public Room[] rooms; 
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject w in walls)
        {
            wall ws = w.GetComponent<wall>();
            ws.Initialize();
           
        }
        Room r1 = new Room(walls, user, intensity);
        rooms = new Room[]{r1};

        cameraRot = user.transform.rotation;
        


    }

    // Update is called once per frame
    void Update()
    {
        //rooms[0].checkNewWallsToMove();
        if (user.transform.rotation != cameraRot)
        {
            cameraRot = user.transform.rotation;
            print("CAMERA CHANBGED");
            rooms[0].checkNewWallsToMove();
            ArrayList log = rooms[0].getLog();
            foreach (var msg in log)
            {
                print(msg);
            }
        }
    }

    

    
}
