using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class OrbitsDisplayDebug : MonoBehaviour
{
    [Range(0,10000)]
    public int iter = 2000;
    
    [Range(0,100)]
    public float time = 1;
    [Range(1,1000)]
    public float width = 10;
    // public bool useThickLines;
    public UniverseController univC;
    float previosWidth;
    [HideInInspector]
    public bool changeRB = true;
    void Awake () {
        previosWidth = width;
        univC = this.gameObject.GetComponent<UniverseController>();
        if (Universe.simStart) {
            HideOrbits ();
        }
    }

    void Update () {
        Universe.relativeDistance = width;
        Universe.itterLength = iter;
        Time.timeScale = time;
        if (!Universe.simStart) {
            DrawOrbits (Universe.itterLength,univC.mainB != null,Universe.physicsTimeStep*width/10,univC.mainB);
        }
       
    }
    void changeDistance(float widthN)
    {
        changeRB = false;
        Debug.Log(" p");
        for (int i = 0; i < univC.bodies.Length; i++)
        {
            univC.bodies[i].position *= widthN/previosWidth;
        }
        previosWidth = widthN;
        changeRB = true;
    }
    void FixedUpdate()
    {
        if(width != previosWidth)
        {
            changeDistance(width);
        }
    }
    void DrawOrbits (int numSteps, bool relativeToBody, float timeStep, CelestialBody centralBody) {
        CelestialBody[] bodies = FindObjectsOfType<CelestialBody> ();
        var virtualBodies = new VirtualBody[bodies.Length];
        var drawPoints = new Vector3[bodies.Length][];
        int referenceFrameIndex = 0;
        Vector3 referenceBodyInitialPosition = Vector3.zero;

        // Initialize virtual bodies (don't want to move the actual bodies)
        for (int i = 0; i < virtualBodies.Length; i++) {
            virtualBodies[i] = new VirtualBody (bodies[i]);
            drawPoints[i] = new Vector3[numSteps];
            if (relativeToBody && bodies[i] == centralBody) {
                referenceFrameIndex = i;
                referenceBodyInitialPosition = virtualBodies[i].position;
            }
        }

        // Simulate
        for (int step = 0; step < numSteps; step++) {
            Vector3 referenceBodyPosition = (relativeToBody) ? referenceBodyInitialPosition : Vector3.zero;
            // Update velocities
            for (int i = 0; i < virtualBodies.Length; i++) {
                virtualBodies[i].velocity += CalculateAcceleration (i, virtualBodies) * timeStep;
            }
            // Update positions
            for (int i = 0; i < virtualBodies.Length; i++) {
                Vector3 newPos = virtualBodies[i].position + virtualBodies[i].velocity * timeStep;
                virtualBodies[i].position = newPos;
                if (relativeToBody) {
                    var referenceFrameOffset = referenceBodyPosition;// - referenceBodyInitialPosition;
                    newPos -= referenceFrameOffset;
                }
                if (relativeToBody && i == referenceFrameIndex) {
                    newPos = Vector3.zero;
                }
                
                drawPoints[i][step] = newPos/Universe.relativeDistance;
            }
        }

        // Draw paths
        for (int bodyIndex = 0; bodyIndex < virtualBodies.Length; bodyIndex++) {
            // if (useThickLines) {
            var lineRenderer = bodies[bodyIndex].gameObject.GetComponentInChildren<LineRenderer> ();
            lineRenderer.enabled = true;
            lineRenderer.positionCount = drawPoints[bodyIndex].Length;
            lineRenderer.SetPositions (drawPoints[bodyIndex]);
            // } else {
            //     for (int i = 0; i < drawPoints[bodyIndex].Length - 1; i++) {
            //         Debug.DrawLine (drawPoints[bodyIndex][i], drawPoints[bodyIndex][i + 1], pathColour);
            //     }

            //     // Hide renderer
            //     var lineRenderer = bodies[bodyIndex].gameObject.GetComponentInChildren<LineRenderer> ();
            //     if (lineRenderer) {
            //         lineRenderer.enabled = false;
            //     }
            // }

        }
    }

    Vector3 CalculateAcceleration (int i, VirtualBody[] virtualBodies) {
        Vector3 acceleration = Vector3.zero;
        for (int j = 0; j < virtualBodies.Length; j++) {
            if (i == j) {
                continue;
            }
            Vector3 difVector = (virtualBodies[j].position - virtualBodies[i].position); // the vector that is the difference between current positions
            float sqrDst = Mathf.Clamp(difVector.sqrMagnitude,25,2500); // distance squared between the two bodies
            Vector3 forceDir = difVector.normalized; // the direction of the new vector
            acceleration += forceDir * Universe.G * virtualBodies[j].mass / sqrDst;
        }
        return acceleration;
    }

    void HideOrbits () {
        CelestialBody[] bodies = FindObjectsOfType<CelestialBody> ();
        // Draw paths
        for (int bodyIndex = 0; bodyIndex < bodies.Length; bodyIndex++) {
            var lineRenderer = bodies[bodyIndex].gameObject.GetComponentInChildren<LineRenderer> ();
            lineRenderer.positionCount = 0;
        }
    }

    // void OnValidate () {
    //     if (usePhysicsTimeStep) {
    //         timeStep = Universe.physicsTimeStep;
    //     }
    // }

    class VirtualBody {
        public Vector3 position;
        public Vector3 velocity;
        public float mass;

        public VirtualBody (CelestialBody body) {
            position = body.position;
            velocity = body.initialVel;
            mass = body.mass;
        }
    }
}
