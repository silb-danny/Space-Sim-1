using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniverseController : MonoBehaviour
{
    [HideInInspector]
    public CelestialBody[] bodies;
    public CelestialBody mainB;
    public bool st = false;
    bool lastSt;
    void Start()
    {
        bodies = GameObject.FindObjectsOfType<CelestialBody>();
        Time.fixedDeltaTime = Universe.physicsTimeStep;
        lastSt = st;
        // foreach(var body in bodies)
        // {
        //     body.position = body.sPosition;
        //     body.currentVel = body.initialVel;
        //     body.sPosition = body.position;
        // }
    }
    // Update is called once per framebodies,
    void FixedUpdate()
    {
        Universe.simStart = st;
        bodies = GameObject.FindObjectsOfType<CelestialBody>();
        if(Input.GetKey(KeyCode.Space))
            mainB = null;
        if(Universe.simStart)
        {
           for(int i = 0; i < bodies.Length; i ++)
            {
                bodies[i].UpdateVelocity(Universe.physicsTimeStep*Universe.relativeDistance/10,Universe.G,bodies);
            }
            
        }
        if(this.GetComponent<OrbitsDisplayDebug>().changeRB)
        {
            for(int i = 0; i < bodies.Length; i ++){
                
                bodies[i].updateRelativePosition(mainB);
            }
        }
        for(int i = 0; i < bodies.Length; i ++)
        {     
            if(Universe.simStart)
                bodies[i].UpdatePosition(Universe.physicsTimeStep*Universe.relativeDistance/10);
            else
            {
                if(lastSt != st)
                {
                    bodies[i].position = bodies[i].sPosition;
                    //body.pastPos.Clear();
                }
                bodies[i].currentVel = bodies[i].initialVel;
                bodies[i].sPosition = bodies[i].position;
            }
        }
        lastSt = st;
    }
}
