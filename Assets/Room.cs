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
    foreach (wall w in blindWalls)

    {

            if (w == rightWall || w == leftWall)
            {
                w.moveWall(new Vector3(dir.x, 0, 0));
                Vector3 positionThis = w.getPosition();
                log.Add(positionThis);
                Vector3 opPosition = oposite[w].getPosition();
                log.Add(opPosition);
                Vector3 newCenter = (positionThis + opPosition) / 2 ;
                log.Add(newCenter);

                foreach (wall a in adjacent[w])
                {

                    Vector3 diferencia = a.getPosition() - newCenter ;
                    log.Add(a.getPosition());
                    log.Add(diferencia);
                    a.moveWall(new Vector3(diferencia.x, 0, 0));
                    log.Add(a.getPosition());
                    a.RestoreScale(-dir.x);
                }

            } else
            {
                w.moveWall(new Vector3(0, 0, dir.z));
                Vector3 positionThis = w.getPosition();
                log.Add(positionThis);
                Vector3 opPosition = oposite[w].getPosition();
                log.Add(opPosition);
                Vector3 newCenter = (positionThis + opPosition) / 2 ;
                log.Add(newCenter);

                foreach (wall a in adjacent[w])
                {

                    Vector3 diferencia = a.getPosition() - newCenter;
                    log.Add(a.getPosition());
                    log.Add(diferencia);
                    a.moveWall(new Vector3(0, 0, diferencia.z));
                    log.Add(a.getPosition());
                    a.RestoreScale(-dir.z);
                }

            }



        }
}
void RestoreRoom()
{
    Vector3 dir = getCenter();
    
    moveWalls(dir * d);
}

public void checkNewWallsToMove()
{

    HashSet<wall> curentHidWalls = getHiddenWalls();



    if (!blindWalls.SetEquals(curentHidWalls))
    {
        
        blindWalls = new HashSet<wall>(curentHidWalls);
        RestoreRoom();
    }



}

    public ArrayList getLog()
    {
        return log;
    }
    }