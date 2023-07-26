using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CelestialTrail : MonoBehaviour
{
    CelestialBody planet;
    LineRenderer trail;
    public TrailRenderer trailInPLay;
    public Material trailM;
    public bool showT = true;
    public Color trailC;
    public float relativeWidth;
    void Awake()
    {
        trailM = new Material(Shader.Find("Sprites/Default"));
        planet = this.GetComponent<CelestialBody>();
        trailC = this.gameObject.GetComponentInChildren<visaul2D>().planetC;  
        trail = this.gameObject.AddComponent<LineRenderer>();
        trailInPLay = this.gameObject.AddComponent<TrailRenderer>();
        trail.material = trailM;
        trail.numCornerVertices = 90;
        trail.numCapVertices = 90;
        trail.startColor = trailC;
        trail.endColor = trailC;    
        trailInPLay.material = trailM;
        trailInPLay.numCornerVertices = 90;
        trailInPLay.numCapVertices = 90;
        trailInPLay.startColor = trailC;
        trailInPLay.endColor = trailC;     
    }

    // Update is called once per frame
    void Update()
    {
        relativeWidth = Mathf.Abs((Camera.main.orthographicSize/Universe.relativeTrailSize))+1;
        trail.widthMultiplier = relativeWidth;
        if(!Universe.simStart)
        {
            trail.enabled = true;
            trailInPLay.enabled = false;
            trail.startWidth = Universe.ghosttrailWidthS*relativeWidth;
            trail.endWidth = Universe.ghosttrailWidthE*relativeWidth;
            // if(planet.updateTrail && showT)
            // {
            //     trail.positionCount = planet.pastPos.Count;
            //     trail.SetPositions(planet.pastPos.ToArray());
            // }
        }
        else
        {
            trailInPLay.enabled = true;
            trail.enabled = false;
            trailInPLay.startWidth = Universe.trailWidthS*relativeWidth;
            trailInPLay.endWidth = Universe.trailWidthE*relativeWidth;
            // if(showT)
            // {
            //     trail.positionCount = planet.future.futurePos.Count;
            //     trail.SetPositions(planet.future.futurePos.ToArray());
            // }
            // trail.enabled = showT;
        }
    }
}
