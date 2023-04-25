using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    public GameObject[] environments = new GameObject[2]; // the environments to modify
    private float testValue = 0; // FOR TESTING PURPOSES: the fade value for all environment objects

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // FOR TESTING PURPOSES: press up/down to change the fade of the environment
        if (Input.GetKeyDown(KeyCode.UpArrow))
		{
            testValue += 2;
            Debug.Log(string.Format("Fade value is now {0}", testValue));
            
		}
        if (Input.GetKeyDown(KeyCode.DownArrow))
		{
            testValue -= 2;
            Debug.Log(string.Format("Fade value is now {0}", testValue));
        }
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
    // TODO: this needs to be changed so that the anxiety value is passed as an argument to replace testValue
    public void UpdateEnvironment()
	{
        for (var i = 0; i < environments.Length; i++)
        {
            FadeEnvironment(environments[i], testValue);
        }
    }
}
