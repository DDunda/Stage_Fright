using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientNoiseController : MonoBehaviour
{
    public AudioClip[] ambientClips = new AudioClip[5];
    public AnimationCurve[] volumesOverTime = new AnimationCurve[5];
    private AudioSource[] ambientSources;
    private float testValue = 0; // the value used instead of anxiety FOR NOW

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
        // FOR TESTING PURPOSES: press up/down to change the fade of the environment
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            testValue += 2;
            UpdateSounds(testValue);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            testValue -= 2;
            UpdateSounds(testValue);
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
