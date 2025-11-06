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

    public class Room
    {
        public wall[] walls;
        public HashSet<wall> blindWalls;
        public wall rightWall;
        public wall leftWall;
        public wall upWall;
        public wall downWall;
        public Camera user;

        public float d;

        public Room(GameObject[] w, Camera u, float intens)
        {
            walls = new wall[4];
            user = u;
            d = intens;
            blindWalls = new HashSet<wall>();

            foreach (GameObject wa in w)
            {
                wall wScript = wa.GetComponent<wall>();
                Vector3 orientation = wScript.getOrientation();

                if (orientation.x >0.5)
                {
                    walls[0] = wScript;
                    rightWall = wScript;
                    print("Right");
                }
                else
                {
                    if (orientation.x <-0.5)
                    {
                        walls[1] = wScript;
                        leftWall = wScript;
                        print("Left");
                    }
                    else
                    {
                        

                        if (orientation.z >0.5)
                        {
                            walls[2] = wScript;
                            upWall = wScript;
                            print("Up");
                        }

                        else
                        {
                            if (orientation.z <-0.5)
                            {
                                walls[3] = wScript;
                                downWall = wScript;
                                print("Down");
                            }
                            else print(orientation.z);
                        }
                    }
                }
                print(orientation);

            }
        }

        public Vector3 getCenter()
        {
            Vector3 posR = rightWall.getPosition();
            print(posR);
            Vector3 posL = leftWall.getPosition();
            print(posL);
            float middleX = Math.Abs(posL.x - posR.x)/2;
            Vector3 center = new Vector3(posL.x + middleX, 0, posL.z);
            print(center);
            return center;
        }
        public HashSet<wall> getHiddenWalls()
        {
            HashSet<wall> currentHidWall = new HashSet<wall>();
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(user);
            foreach (wall w in walls)
            {
                Bounds bounds = w.getBounds();
                if(!GeometryUtility.TestPlanesAABB(planes, bounds))
                {
                    currentHidWall.Add(w);
                    print("ADded");
                }
                    
            }
            
            foreach(wall w in currentHidWall) {
                print(w); }
           
            return currentHidWall ;
        }
        public void moveWalls(Vector3 dir)
        {
            foreach (wall w in blindWalls)
            {
                 w.RestorePosition(dir);

            }
        }
        void RestoreRoom()
        {
            Vector3 dir = getCenter();
            print(dir);
            moveWalls(dir * d);
        }

        public void checkNewWallsToMove() {

            HashSet<wall> curentHidWalls = getHiddenWalls();

           
            print(blindWalls.Count);
            if (!blindWalls.SetEquals(curentHidWalls))
            {
                print("ENTROO");
                blindWalls = new HashSet<wall>(curentHidWalls);
                RestoreRoom();
            }


        
        }
    }

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
        }
    }

    

    
}
