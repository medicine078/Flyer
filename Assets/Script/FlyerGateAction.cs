using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerGateAction : MonoBehaviour
{
    Renderer[] myRenderer;
    public Color PassColor = new Color(1.0f, 0.0f, 0.5f);

    // Start is called before the first frame update
    void Start()
    {
        myRenderer = GetComponentsInChildren<Renderer>();

        foreach(Renderer Stored in myRenderer)
        {
            Stored.material.color = PassColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
