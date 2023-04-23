using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentationController : MonoBehaviour
{
    // slide variables
    public Sprite[] slideImages = new Sprite[5]; // the list of slide sprites
    public Sprite defaultSlide; // the slide used when the presentation is over
    private SpriteRenderer slide; // this object's sprite renderer
    public float slideTimerLength = 25; // how many seconds each slide lasts for
    private float slideTimer;
    private int currentSlide = 0; // the index of the current slide

    // cue card variables
    public GameObject cueCards;
    public List<List<string>> cardTitles = new List<List<string>>();
    public List<List<string>> cardSpeeches = new List<List<string>>();

    // enum modes are
    // Read: the slide is up and visible for the player to read
    // Choose: the current slide is obscured, and the player must choose a cue card
    // Speak: the player is currently speaking their presentation, and must wait for the speech to end
    public enum PresentMode {Read, Choose, Speak};
    private PresentMode mode = PresentMode.Read;

    public float agitationRight = 0.8f; // the rate that agitation changes when a correct answer is chosen
    public float agitationWrong = 1.3f; // the rate that agitation changes when an incorrect answer is chosen
    public float agitationUndecided = 1.02f; // the rate that agitation changes while a cue card is being chosen

    private bool presenting = true; // whether the presentation is over

    // property: other scripts can read the current mode; can only change it thru ChangeMode()
    public PresentMode Mode
	{
        get { return mode; }
	}

    // Start is called before the first frame update
    void Start()
    {
        slideTimer = slideTimerLength;
        slide = this.GetComponent<SpriteRenderer>();
        slide.sprite = slideImages[0]; // start with the first slide
        ChangeCardState(false); // hide cue cards

        // adding titles and speeches for each slide
        cardTitles.Add(new List<string> { "based", "cringe", "L" });
        cardTitles.Add(new List<string> { "2 + 2 = 4", "2 + 2 = 5", "2 + 2 = 3" });
        cardTitles.Add(new List<string> { "third example", "second example", "fifth example" });

        cardSpeeches.Add(new List<string> 
        { "This is the correct answer: good job, you did it! That is so cool of you.", 
           "Bad news: you really goofed this one up REAL bad. That's a dang old shame.", 
           "Short wrong answer." });
        cardSpeeches.Add(new List<string>
        { "This is the correct answer: good job, you did it! That is so cool of you.",
          "Bad news: you really goofed this one up REAL bad. That's a dang old shame.",
          "Short wrong answer." });
        cardSpeeches.Add(new List<string>
        { "This is the correct answer: good job, you did it! That is so cool of you.",
          "Bad news: you really goofed this one up REAL bad. That's a dang old shame.",
          "Short wrong answer." });
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
                    ChangeMode(PresentMode.Choose);
				}
                break;
        }
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

    // enable/disable all cue cards
    private void ChangeCardState(bool state)
    {
        cueCards.SetActive(state);
    }

    // change the current mode, performing all necessary functions to do so
    public void ChangeMode(PresentMode newMode)
    {
        mode = newMode;

        switch (newMode)
        {
            case PresentMode.Read:
                ChangeCardState(false); // hide cue cards
                NextSlide();
                slideTimer = slideTimerLength; // reset timer
                break;
            case PresentMode.Choose:
                SetCardText(); // change the text on the cue cards based on the current slide
                ChangeCardState(true); // show cue cards
                slide.sprite = defaultSlide; // black out the current slide
                break;
            case PresentMode.Speak:
                ChangeCardState(false); // hide cue cards
                break;
        }
    }

    private void SetCardText()
	{

	}

    public void CardSelected(GameObject card)
	{
        Debug.Log("This button works!");
	}
}
