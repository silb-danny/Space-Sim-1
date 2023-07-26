using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class visaul2D : MonoBehaviour
{
    public Color planetC;
    SpriteRenderer spr;
    public Shader plSH;
    Material planetMat;
    void Awake()
    {
        planetC = Random.ColorHSV();
        planetMat = new Material(plSH);
        spr = this.GetComponent<SpriteRenderer>();
        planetMat.SetColor("_BaseC",planetC);
        spr.sharedMaterial = planetMat;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
