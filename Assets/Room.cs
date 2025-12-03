using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class Room : MonoBehaviour
{
    public wall[] walls;
    private int wallIndex = 0;
    public bool isRestored = false;
    public GameObject keyText1;
    public GameObject keyText2;
    public HashSet<wall> blindWalls;
    public ArrayList log;
    public wall rightWall;
    public wall leftWall;
    public wall upWall;
    public wall downWall;
    public Camera user;
    Dictionary<wall, wall> oposite ;
    Dictionary<wall, wall[]> adjacent ;
    private Vector3 lastMove;
    float realArea;
    float initialArea;
    float biggestArea;
    public restore restorer;
    public float d;
    public door door1;
    public GameObject flor;
    public GameObject boxPrefab;
    public Slider progressSlider;
    public Slider progressSlider2;

    private bool gotKey = false;

public bool triggered = false;

    public void Initialize()
    {
       
        blindWalls = new HashSet<wall>();
        oposite = new Dictionary<wall, wall>();
        adjacent = new Dictionary<wall, wall[]>();
        log = new ArrayList();
        float xlen = 0;
        float initialXlen = 0;
        float zlen = 0;
        float initialZlen = 0;
        foreach (wall wScript in walls)
        {
            wScript.Initialize();
        }
        foreach (wall wScript in walls)
        {
            wScript.Initialize();
        
        Vector3 orientation = wScript.getOrientation();

        
        if (orientation.x > 0.5)
        {
            
            rightWall = wScript;
            xlen  =  wScript.getOriginalScale();
            initialXlen = wScript.getScale();
            
        }
        else
        {
            if (orientation.x < -0.5)
            {
                
                leftWall = wScript;
                zlen = wScript.getOriginalScale();
                initialZlen = wScript.getScale();
                
            }
            else
            {


                if (orientation.z > 0.5)
                {
                    
                    upWall = wScript;
                    
                }

                else
                {
                    if (orientation.z < -0.5)
                    {
                        
                        downWall = wScript;
                        
                    }
                    
                }
            }
        }
            

        

    }
        realArea = xlen * zlen;
        initialArea = initialXlen * initialZlen;
        biggestArea = realArea*1.5f;
        oposite.Add(rightWall, leftWall);
        oposite.Add(upWall, downWall);
        oposite.Add(downWall, upWall);
        oposite.Add(leftWall, rightWall);

        wall[] adj = { upWall, downWall };
        wall[] adj2 = { rightWall, leftWall };

        adjacent.Add(rightWall, adj );
        adjacent.Add(leftWall, adj);
        adjacent.Add(upWall, adj2);
        adjacent.Add(downWall, adj2);

    }

public Vector3 getCenter()
{
    Vector3 posR = rightWall.getPosition();
    
    Vector3 posL = leftWall.getPosition();
    
    float middleX = Math.Abs(posL.x - posR.x) / 2;
    Vector3 center = new Vector3(posL.x + middleX, 0, posL.z);
    
    return center;
}

public void moveWalls(Vector3 dir)
{
    
    foreach (wall w in blindWalls)

    {
        log.Add("Moving wall: ");
        log.Add(w);
        //Determine orientation
            Vector3 orientation = new Vector3(0, 0, 1);

            if (w == rightWall || w == leftWall)
                orientation = new Vector3(1, 0, 0);
            
        //Additional movement if adjacent walls are scaled    
            float difScale = 0;
            //log.Add(dir);

            if (!adjacent[w][0].isScaled()){
                difScale = adjacent[w][0].getScaleDiference();
               // log.Add("DIFSCALE:");
               // log.Add(difScale);
                
                difScale *= d ;
                if (w == rightWall || w == upWall)
                        difScale *= -1;
                dir = new Vector3((dir.x+difScale),0, (dir.z+difScale));
                
                
            }
            if (dir.magnitude > 0.2f)
            {
                dir = dir.normalized * d;
            }
            
            Vector3 moveDir = new Vector3(orientation.x*dir.x, 0, orientation.z * dir.z);
            
            
            if (CanMove(w, moveDir))
            {
               actuallyMoveWall(w, moveDir, orientation);
            }
        
        }
    
}

private void actuallyMoveWall(wall w, Vector3 moveDir, Vector3 orientation )
{
                Vector3 pastPos = w.getPosition();
                w.moveWall(moveDir);
                
                Vector3 newPos = w.getPosition();
                Vector3 opositePosition = oposite[w].getPosition();

                float changeSize = 0;
                if (Math.Abs(Vector3.Dot(opositePosition - newPos, orientation)) > Math.Abs(Vector3.Dot(opositePosition - pastPos, orientation)))
                    changeSize = 1;
                else
                    changeSize = -1;
                
                //log.Add(changeSize);
                Vector3 newCenter = (newPos + opositePosition) / 2;
                foreach (wall a in adjacent[w])
                {
                    Vector3 diference = a.getPosition() - newCenter;
                    Vector3 perpendicular = new Vector3(orientation.z, 0, orientation.x);
                    Vector3 adjMove = new Vector3(orientation.x * diference.x, 0, orientation.z * diference.z);
                    
                    a.moveWall(adjMove);
                    
                    a.RestoreScale(changeSize * Math.Abs(Vector3.Dot(moveDir, orientation)));
                }
}
private bool CanMove(wall w, Vector3 dir)
{
    
    Vector3 newPos = w.getPosition() - dir;

    Vector3 userPos = user.transform.position;

   float buffer = 0.45f;

    Vector3 dif = oposite[w].getPosition() - newPos;

    
    
   float area = w.getScale() * dif.magnitude ;
   if(area>= (realArea-0.061f) && area <= (realArea+0.061f) && getCenter().magnitude <0.1f)
      {
        log.Add("Room fully restored");
        isRestored = true;
       return false;
      }
   //if( area > biggestArea || area < initialArea)
    //    {
    //     log.Add("Room cannot be restored further");
    //     return false;
    ///    }
   

    //Check user inside walls but only w x and z dont carea baout y
    if (w == rightWall && userPos.x >= newPos.x - buffer)
        {
        return false;
        }
        
    if (w == leftWall && userPos.x < newPos.x+buffer)
        return false;
    if (w == upWall && userPos.z > newPos.z-buffer)
        return false;
    if (w == downWall && userPos.z < newPos.z+buffer)
        return false;

    log.Add("Wall can move");
    return true;
}
void RestoreRoom()
{
    Vector3 dir = getCenter();
    
    moveWalls(dir);
}

public void boxOpened()
    {
        generateBox();
    }
private void generateBox()
    {
        //Choose a random wall
        wallIndex = (wallIndex + 1) % walls.Length;
        wall chosenWall = walls[wallIndex];
        
        Vector3 wallPos = chosenWall.getPosition();
        //Generate box position near wall
        Vector3 boxPos = wallPos - chosenWall.getOrientation() * 0.2f;
        boxPos.y = 0.1f;
        
        GameObject newBox = Instantiate(boxPrefab, boxPos, Quaternion.identity);
        chosenWall.addAttached(newBox);
        box boxScrpipt = newBox.transform.GetChild(0).GetComponent<box>();
        if (isRestored) boxScrpipt.SpawnKey();
        boxScrpipt.roomRef = this;
        boxScrpipt.slide1 = progressSlider;
        boxScrpipt.slide2 = progressSlider2;


    }

public void checkNewWallsToMove()
{

if(!isRestored){
    bool changed = false;
    Plane[] planes = GeometryUtility.CalculateFrustumPlanes(user);
    blindWalls = new HashSet<wall>();
    foreach (wall w in walls)
    {
       // log.Add("Checking wall: ");
        //log.Add(w);
        Bounds bounds = w.getBounds();
        if (!GeometryUtility.TestPlanesAABB(planes, bounds))
        {
            changed = changed || w.ChangeVisible(false);
            blindWalls.Add(w);
        } else {
            changed = changed || w.ChangeVisible(true);}

    }

    if (changed)
    {
        //log.Add("Nuevas paredes ocultas");
        
        RestoreRoom();
    }
}


}

    private void modifyD(Vector3 moveDir)
    {
        //if moveDir moves more than lastMove Decrease d else increase d
        if (Vector3.Dot(moveDir, lastMove) > 0)
        {
            d -= 0.1f;
        }
        else
        {
            d += 0.1f;
        }
        if(d>1){
        d =1;
        }
        if (d < 0.1f)
        {
            d = 0.1f;
        }
        lastMove = moveDir;
    }

    public ArrayList getLog()
    {
        ArrayList toReturn = log;
        log = new ArrayList();
        return toReturn;
    }
    
public void keySpawned()
    {
        keyText1.SetActive(true);
        keyText2.SetActive(true);
        gotKey = true;
    }

    public bool IsRestored()
    {
        return isRestored && gotKey;
        }

private void OnTriggerEnter(Collider other)
    {
        
        if( !triggered && other.tag == "MainCamera")
        {
            triggered = true;
            print("TRIGGERED");
            
            
            restorer.Entered(this.GetComponent<Room>());
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
public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Compress()
    {
        isRestored = false;
        gotKey = false;
        this.gameObject.SetActive(true);
        initialArea = leftWall.getScale() * upWall.getScale();

        Bounds florBounds = flor.GetComponent<Collider>().bounds;
        if(leftWall.getPosition().x < florBounds.min.x)
        {
            Vector3 pos = leftWall.getPosition();
            actuallyMoveWall(leftWall, new Vector3(florBounds.min.x - pos.x, 0, 0), new Vector3(1,0,0));
            
        }
        if(rightWall.getPosition().x > florBounds.max.x)
        {
            Vector3 pos = rightWall.getPosition();
            actuallyMoveWall(rightWall, new Vector3(florBounds.max.x - pos.x, 0, 0), new Vector3(1,0,0));
            
        }
        if (downWall.getPosition().z < florBounds.min.z)
        {
            Vector3 pos = downWall.getPosition();
            actuallyMoveWall(downWall, new Vector3(0, 0, pos.z - florBounds.min.z), new Vector3(0,0,1));
            
        }
        if (upWall.getPosition().z > florBounds.max.z)
        {
            Vector3 pos = upWall.getPosition();
            actuallyMoveWall(upWall, new Vector3(0, 0, florBounds.max.z - pos.z), new Vector3(0,0,1));
            
        }

    }
    }