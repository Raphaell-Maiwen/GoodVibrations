using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingManager : MonoBehaviour
{
    public PostProcessLayer layer;
    public PostProcessVolume volume;
    private PostProcessProfile profile;

    // Start is called before the first frame update
    void Start()
    {
        profile = volume.profile;
    }

    // Update is called once per frame
    void Update()
    {
        //volume.weight -= Time.deltaTime * 0.1f;
        Bloom bloom;
        profile.TryGetSettings<Bloom>(out bloom);
        bloom.intensity.Override(10f);
    }
}