using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Room
{
    public wall[] walls;
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
    
    public float d;


    public Room(GameObject[] w, Camera u, float intens)
    {
        walls = new wall[4];
        user = u;
        d = intens;
        blindWalls = new HashSet<wall>();
        oposite = new Dictionary<wall, wall>();
        adjacent = new Dictionary<wall, wall[]>();
        log = new ArrayList();

        foreach (GameObject wa in w)
        {
        wall wScript = wa.GetComponent<wall>();
        Vector3 orientation = wScript.getOrientation();

        if (orientation.x > 0.5)
        {
            walls[0] = wScript;
            rightWall = wScript;
            
        }
        else
        {
            if (orientation.x < -0.5)
            {
                walls[1] = wScript;
                leftWall = wScript;
                
            }
            else
            {


                if (orientation.z > 0.5)
                {
                    walls[2] = wScript;
                    upWall = wScript;
                    
                }

                else
                {
                    if (orientation.z < -0.5)
                    {
                        walls[3] = wScript;
                        downWall = wScript;
                        
                    }
                    
                }
            }
        }
            

        

    }
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
public HashSet<wall> getHiddenWalls()
{
    HashSet<wall> currentHidWall = new HashSet<wall>();
    Plane[] planes = GeometryUtility.CalculateFrustumPlanes(user);
    foreach (wall w in walls)
    {
        Bounds bounds = w.getBounds();
        if (!GeometryUtility.TestPlanesAABB(planes, bounds))
        {
            currentHidWall.Add(w);

        }

    }



    return currentHidWall;
}
public void moveWalls(Vector3 dir)
{
    dir = dir* d;
    foreach (wall w in blindWalls)

    {
        //Determine orientation
            Vector3 orientation = new Vector3(0, 0, 1);

            if (w == rightWall || w == leftWall)
                orientation = new Vector3(1, 0, 0);
            
        //Additional movement if adjacent walls are scaled    
            float difScale = 0;
            log.Add(dir);

            if (!adjacent[w][0].isScaled()){
                difScale = adjacent[w][0].getScaleDiference();
                log.Add("DIFSCALE:");
                log.Add(difScale);
                
                difScale *= d ;
                if (w == rightWall || w == upWall)
                        difScale *= -1;
                dir = new Vector3((dir.x+difScale),0, (dir.z+difScale));
                
                
            }
            Vector3 moveDir = new Vector3(orientation.x*dir.x, 0, orientation.z * dir.z);
            
            Vector3 pastPos = w.getPosition();
            if (CanMove(w, moveDir))
            {
                log.Add("Se mueve la pared w a...");
                log.Add(moveDir);
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
                    //float adjdir = Vector3.Dot(diference, perpendicular);
                    //adjdir = adjdir / Math.Abs(adjdir);
                    //adjdir *= -1;
                    //adjdir *= expanded / 2;
                    //Vector3 moveExp = new Vector3(perpendicular.x * adjdir, 0, perpendicular.z * adjdir);
                    a.moveWall(adjMove);
                    //log.Add(adjMove);
                    a.RestoreScale(changeSize * Math.Abs(Vector3.Dot(dir, orientation)));
                }
            }
         modifyD(moveDir);
        }
    
}

private bool CanMove(wall w, Vector3 dir)
{
    
    Vector3 newPos = w.getPosition() - dir;

    Vector3 userPos = user.transform.position;

   float buffer = 0.45f;

    //Check user inside walls but only w x and z dont carea baout y
    if (w == rightWall && userPos.x >= newPos.x-buffer)
        return false;
    if (w == leftWall && userPos.x < newPos.x+buffer)
        return false;
    if (w == upWall && userPos.z > newPos.z-buffer)
        return false;
    if (w == downWall && userPos.z < newPos.z+buffer)
        return false;

    return true;
}
void RestoreRoom()
{
    Vector3 dir = getCenter();
    
    moveWalls(dir);
}

public void checkNewWallsToMove()
{

    HashSet<wall> curentHidWalls = getHiddenWalls();



    if (!blindWalls.SetEquals(curentHidWalls))
    {
        log.Add("Nuevas paredes ocultas");
        blindWalls = new HashSet<wall>(curentHidWalls);
        RestoreRoom();
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
        return log;
    }
    }