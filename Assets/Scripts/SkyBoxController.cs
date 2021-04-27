using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxController : MonoBehaviour
{
    // Start is called before the first frame update
    public Material[] skyboxes;
    int idx = 0;
    DateTime limit;

    void Start()
    {
        limit = DateTime.Now.AddSeconds(18);
        RenderSettings.skybox = skyboxes[idx];
    }

    // Update is called once per frame
    void Update()
    {
        while (DateTime.Now > limit)
        {
            int prev = idx;
            idx++;
            idx = idx % skyboxes.Length;
            limit = DateTime.Now.AddSeconds(18);
            RenderSettings.skybox = skyboxes[idx];
        }
    }
}
