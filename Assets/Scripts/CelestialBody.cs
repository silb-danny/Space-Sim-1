using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CelestialBody : MonoBehaviour
{
    public float surfaceGravity = 1; // -> input
    public float radius; // -> input
    public Vector3 initialVel; // initial inputed velocity // -> input
    public Rigidbody rb;
    public UniverseController uniC;
    [HideInInspector]
    public bool updateTrail = true; // in order for the list to functin correctly
    public Vector3 position; // -> input
    [HideInInspector] 
    public Vector3 sPosition; // -> input
    [HideInInspector]
    public Vector3 currentVel;
    [HideInInspector]
    public float mass;
    void Awake(){
        mass = surfaceGravity*radius*radius/Universe.G; // calculating mass
        transform.localScale = Vector3.one*radius/Universe.radSizeD;
        uniC = GameObject.FindObjectOfType<UniverseController>().GetComponent<UniverseController>();
        currentVel = initialVel;
        rb = this.GetComponent<Rigidbody>();
        position = rb.position * Universe.relativeDistance;
        rb.position = (position - ((uniC.mainB != null)?uniC.mainB.position:Vector3.zero))/Universe.relativeDistance;
        sPosition = position;
    }
    void Update()
    {
        transform.localScale = Vector3.one*radius/Universe.radSizeD;
        mass = surfaceGravity*radius*radius/Universe.G; // calculating mass     
    }
    // Update is called once per frameCelestialBody[] bodies
    public void UpdateVelocity(float timeStep, float g, CelestialBody[] bodies)
    {
        for (int j = 0; j < bodies.Length; j++) {
            if (bodies[j] == this) {
                continue;
            }
                Vector3 difVector = (bodies[j].position-position); // the vector that is the difference between current positions
                float sqrDst = Mathf.Clamp(difVector.sqrMagnitude,25,2500); // distance squared between the two bodies
                Vector3 forceDir = difVector.normalized; // the direction of the new vector
                Vector3 force = forceDir*(g*mass*bodies[j].mass)/sqrDst; // gravitational atraction formula
                Vector3 acceleration = force/mass; // f= m*a
                currentVel += acceleration*timeStep; // v = a*t -> sum of all directional (vector) velocities
        }
    }
    public void UpdatePosition(float timeStep)
    {
        Vector3 pos = currentVel*timeStep; // calculating position
        position += pos;
        //pastPos.Add((position - ((uniC.mainB != null)?uniC.mainB.position:Vector3.zero))/Universe.relativeDistance);
        //shortenTrail();
    }
    public void updateRelativePosition(CelestialBody mainB)
    {
        rb.position = (position - ((mainB != null)?mainB.position:Vector3.zero))/Universe.relativeDistance;
    }
    // void shortenTrail()
    // {
    //     updateTrail = false;
    //     int removeAm = pastPos.Count-Universe.trailLength;
    //     if(removeAm > 0)
    //         pastPos.RemoveRange(0,removeAm);
    //     updateTrail = true;
    // }
    void OnMouseDown()
    {
        for(int i = 0; i < uniC.bodies.Length; i ++)
        {
            uniC.bodies[i].gameObject.GetComponent<CelestialTrail>().trailInPLay.Clear();
        }
        uniC.mainB = this;
        // Camera.main.transform.position = this.transform.position + Vector3.forward*Camera.main.transform.position.z;
        // Camera.main.transform.SetParent(this.transform);
        
    }
}
