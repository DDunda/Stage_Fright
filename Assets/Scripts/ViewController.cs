using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ViewController : MonoBehaviour
{
    public Transform[] views = new Transform[2]; // the views to change between
    public int viewIndex = 0; // the current view

    // Start is called before the first frame update
    void Start()
    {
        var enabled = true;

        // loop through all objects, changing their visibility so that only the viewIndex is visible
        for (var i = 0; i < views.Length; i++)
		{
            enabled = false;
            if (i == viewIndex)
			{
                enabled = true;
			}

            ChangeVisibility(views[i], enabled);
		}
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
            var oldIndex = viewIndex;

            viewIndex -= 1;
            if (viewIndex < 0) { viewIndex += views.Length; }

            ChangeView(oldIndex, viewIndex);
		}
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            var oldIndex = viewIndex;

            viewIndex += 1;
            if (viewIndex > (views.Length - 1)) { viewIndex = 0; }

            ChangeView(oldIndex, viewIndex);
        }
    }

    // hide the current view, and show the new view
    private void ChangeView(int oldIndex, int newIndex)
	{
        ChangeVisibility(views[oldIndex], false);
        ChangeVisibility(views[newIndex], true);
    }

    // disable/enable sprite renderers for all children under an object
    private void ChangeVisibility(Transform obj, bool state)
	{
        // either hide or show the sprite, if this object has a sprite
        var sprite = obj.GetComponent<SpriteRenderer>();
        if (sprite != null)
		{
            sprite.enabled = state;
		}
        // do the same for any text
        var text = obj.GetComponent<TMP_Text>();
        if (text != null)
        {
            text.enabled = state;
        }

        for (var i = 0; i < obj.childCount; i++)
		{
            ChangeVisibility(obj.GetChild(i), state);
		}
	}
}
