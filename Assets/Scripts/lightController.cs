using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class lightController : MonoBehaviour
{
    public float minIntensity = .75f;
    public float maxIntensity = 1.5f;
    public float lightChangeSpeed = .05f;
    
    public List<Light2D> lights;
    public bool lighting = false;


    // Start is called before the first frame update
    void Start()
    {
        lights = new List<Light2D>();
        lighting = false;
    }


    // Update is called once per frame
    void Update()
    {
        lightHandeller();
    }
    private void lightHandeller()
    {
        if (lightChangeSpeed == 0) return;
        if (!lighting) return;
        if (lights.Count == 0) return;

        if (lights[0].intensity > maxIntensity && lightChangeSpeed > 0)
            lightChangeSpeed *= -1;

        else if (lights[0].intensity < minIntensity && lightChangeSpeed < 0)
            lightChangeSpeed *= -1;

        for (int i = 0; i < lights.Count(); i++)
            lights[i].intensity += lightChangeSpeed * Time.deltaTime;
    }

    public void turnON()
    {
        
        if (lighting) return;

        lighting = true;
        Debug.Log("we hit it " + maxIntensity+" "+ lights.Count());
        for (int i = 0; i < lights.Count(); i++)
        {
            lights[i].intensity = maxIntensity;
        }
    }
    public void turnOof()
    {
        lighting = false;
        for (int i = 0; i < lights.Count(); i++)
        {
            lights[i].intensity = 0f;
        }
    }
    public void push(Light2D light)
    {
        lights.Add(light);
    }
}
