using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    public GameObject[] environments = new GameObject[2]; // the environments to modify

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // change the color of all objects in the environments that have the ColorFadeScript
    public void FadeEnvironment(GameObject obj, float value)
	{
        var script = obj.GetComponent<ColorFadeScript>();

        // update the object's color if possible
        if (script != null)
		{
            script.UpdateColor(value);
		}

        // call this function on any children of this object
        for (var i = 0; i < obj.transform.childCount; i++)
		{
            FadeEnvironment(obj.transform.GetChild(i).gameObject, value);
		}
	}

    // set up the variables for changing the environment
    public void UpdateEnvironment(float anxiety)
	{
        for (var i = 0; i < environments.Length; i++)
        {
            FadeEnvironment(environments[i], anxiety);
        }
    }
}
