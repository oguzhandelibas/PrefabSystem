using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public enum CloneType { Cube, Sphere };
    public CloneType cloneType;
    [Range(1, 10)] public float spawnSpeed;

    private void Start()
    {
        InvokeRepeating("CreateObject", 0, 1 / spawnSpeed);
    }

    void CreateObject()
    {
        ProceduralCloner.Clone(this.transform.position, cloneType.ToString());
    }
}
