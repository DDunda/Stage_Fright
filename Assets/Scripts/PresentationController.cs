using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentationController : MonoBehaviour
{
    public Sprite[] slideImages = new Sprite[5]; // the list of slide sprites
    public Sprite defaultSlide; // the slide used when the presentation is over
    private SpriteRenderer slide; // this object's sprite renderer
    public float slideTimerLength = 25; // how many seconds each slide lasts for
    private float slideTimer;
    private int currentSlide = 0; // the index of the current slide

    // enum modes are
    // Read: the slide is up and visible for the player to read
    // Choose: the current slide is obscured, and the player must choose a cue card
    // Speak: the player is currently speaking their presentation, and must wait for the speech to end
    private enum PresentMode {Read, Choose, Speak};
    private PresentMode mode = PresentMode.Read;

    public float agitationRight = 0.8f; // the rate that agitation changes when a correct answer is chosen
    public float agitationWrong = 1.3f; // the rate that agitation changes when an incorrect answer is chosen
    public float agitationUndecided = 1.0f; // the rate that agitation changes while a cue card is being chosen

    private bool presenting = true; // whether the presentation is over

    // Start is called before the first frame update
    void Start()
    {
        slideTimer = slideTimerLength;
        slide = this.GetComponent<SpriteRenderer>();
        slide.sprite = slideImages[0]; // start with the first slide
    }

    private void NextSlide()
	{
        currentSlide++;
        if (currentSlide <= slideImages.Length)
		{
            slide.sprite = slideImages[currentSlide];
        }
        else
		{
            presenting = false;
            slide.sprite = defaultSlide;
		}
	}

    // Update is called once per frame
    void Update()
    {
        switch (mode)
        {
            case PresentMode.Read:
                slideTimer -= Time.deltaTime;
                if (slideTimer <= 0)
				{
                    NextSlide();
                    slideTimer = slideTimerLength;
				}
                break;

            default:
                Debug.LogWarning("The current presentation mode is invalid!");
                break;
        }
    }
}
