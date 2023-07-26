using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[ExecuteInEditMode]
public class Universe
{
    [Range(1, 10000)]
    public static int trailLength = 2000;
    public static int itterLength = 2000;
    public static float trailWidthS = 0.3f;
    public static float trailWidthE = 0.2f;
    public static float ghosttrailWidthS = 0.2f;
    public static float ghosttrailWidthE = 0.2f;
    public static float G = 1f;
    public static float physicsTimeStep = Time.fixedDeltaTime;
    public static bool simStart = false;
    public static Shader planet2dshader = Shader.Find("ShaderGraphs/planet");
    public static Shader planet3dshader = Shader.Find("ShaderGraphs/PLANET3D");
    public static Shader atmoshpere = Shader.Find("ShaderGraphs/atmoshpere");
    public static float radSizeD = 10; // the size of the radius
    public static float relativeDistance = 10; // dividing by this - > affect of the distance
    public static float relativeTrailSize = 30;
}
