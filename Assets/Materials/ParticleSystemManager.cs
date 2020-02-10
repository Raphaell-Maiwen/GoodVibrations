using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemManager : MonoBehaviour
{
    private ParticleSystem ps;
    private float timer = 0;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (ps.isEmitting) {
                ps.Stop();
                ps.Clear();
            } 
            else ps.Play(); ;
        }

        var shape = ps.shape;
        shape.angle = 25 * Mathf.Sin(timer) * 10;

        //var main = ps.main;
    }
}
