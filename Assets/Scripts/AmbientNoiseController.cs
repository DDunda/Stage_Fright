using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientNoiseController : MonoBehaviour
{
    public AudioClip[] ambientClips = new AudioClip[5];
    public AnimationCurve[] volumesOverTime = new AnimationCurve[5];
    private AudioSource[] ambientSources;
    private float currentAnxiety = 0;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource s;

        // assign each audio source an audio clip, and set default parameters
        ambientSources = new AudioSource[ambientClips.Length];
        for (var i = 0; i < ambientClips.Length; i++)
		{
            s = gameObject.AddComponent<AudioSource>();
            s.volume = volumesOverTime[i].Evaluate(0); // set to initial volume
            s.loop = true;
            s.clip = ambientClips[i];
            ambientSources[i] = s;
            ambientSources[i].Play();
		}
    }

    // Update is called once per frame
    void Update()
    {
        // only change the ambient noise when the anxiety level changes
        if (currentAnxiety != AnxietyController.anxietyLevel)
		{
            currentAnxiety = AnxietyController.anxietyLevel;
            UpdateSounds(AnxietyController.anxietyLevel);
        }
    }

    public void UpdateSounds(float value)
	{
        AudioSource s;
        AnimationCurve vot; // volume over time
        float time = value / 100; // since anxiety goes from 0 - 100, move it back to 0 - 1

        for (var i = 0; i < ambientClips.Length; i++)
		{
            s = ambientSources[i];
            vot = volumesOverTime[i];

            s.volume = vot.Evaluate(time);
        }
	}
}
