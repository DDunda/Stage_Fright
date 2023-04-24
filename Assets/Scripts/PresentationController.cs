using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    public float xOffset = 5.0f;
    public GameObject cueCardParent;
    private Transform card;
    private List<Transform> cueCards = new List<Transform>();
    public List<List<string>> cardTitles = new List<List<string>>();
    public List<List<string>> cardSpeeches = new List<List<string>>();

    // enum modes are
    // Read: the slide is up and visible for the player to read
    // Choose: the current slide is obscured, and the player must choose a cue card
    // Speak: the player is currently speaking their presentation, and must wait for the speech to end
    public enum PresentMode {Read, Choose, Speak};
    private PresentMode mode = PresentMode.Read;

    // speech variables
    public GameObject speechBox;
    public float speechTickLength = 0.1f; // how many seconds it takes for a new character of the speech to appear
    public float speechTick; 
    public float speechEndTimerLength = 3.0f; // how many seconds the speech stays up when it's completed
    public float speechEndTimer; 
    private string selectedSpeech;
    private int currentChar = 1;
    private bool correctSpeech = true; // whether the player chose the correct speech

    public float agitationRight = 0.8f; // the rate that agitation changes when a correct answer is chosen
    public float agitationWrong = 1.3f; // the rate that agitation changes when an incorrect answer is chosen
    public float agitationUndecided = 1.02f; // the rate that agitation changes while a cue card is being chosen

    private bool presenting = true; // whether the presentation is over

    // property: other scripts can read the current mode; can only change it thru ChangeMode()
    public PresentMode Mode
	{
        get { return mode; }
	}

    // property: other scripts can read the current agitation multiplier
    public float AgitationLevel
	{
        get
		{
            if (mode == PresentMode.Choose)
			{
                return agitationUndecided;
			}
            else if (mode == PresentMode.Speak)
			{
                // based on if this is the right answer or not, return a rate of change
                if (correctSpeech) { return agitationRight; }
                else { return agitationWrong; }
			}
            else
			{
                return 1.0f; // if reading the current slide, just use the normal rate of change
			}
		}
	}

    // Start is called before the first frame update
    void Start()
    {
        // define slide reference
        slide = this.GetComponent<SpriteRenderer>();
        // define initial variables for PresentMode.Read
        slideTimer = slideTimerLength;
        slide.sprite = slideImages[0]; // start with the first slide
        ChangeCardState(false); // hide cue cards
        speechBox.SetActive(false); // hide speech box

        // adding each cue card to a list
        for (var i = 0; i < 3; i++)
		{
            var c = cueCardParent.transform.GetChild(i);
            cueCards.Add(c);
            c.GetComponent<CardController>().index = i; // for determining what speech to play
        }

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
            case PresentMode.Speak:
                if (currentChar < selectedSpeech.Length) // if giving speech
				{
                    speechTick -= Time.deltaTime;
                    while (speechTick <= 0) // add characters to the speech if the timer is exhausted
                    {
                        currentChar++;
                        UpdateSpeechText(currentChar);
                        speechTick += speechTickLength; // add instead of set, for more accurate timing
                    }
                }
                else // if at end of speech
				{
                    speechEndTimer -= Time.deltaTime;
                    if (speechEndTimer <= 0) // once enough time has passed, show the next slide
                    {
                        NextSlide();
                    }
                }
                break;
        }
    }

    private void NextSlide()
    {
        currentSlide++;
        if (currentSlide < slideImages.Length)
        {
            slide.sprite = slideImages[currentSlide];
            ChangeMode(PresentMode.Read);
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
        cueCardParent.SetActive(state);
    }

    // change the current mode, performing all necessary functions to do so
    public void ChangeMode(PresentMode newMode)
    {
        mode = newMode;

        switch (newMode)
        {
            case PresentMode.Read:
                ChangeCardState(false); // hide cue cards
                speechBox.SetActive(false); // hide speech box
                slideTimer = slideTimerLength; // reset timer
                break;
            case PresentMode.Choose:
                ChangeCardState(true); // show cue cards (very important that this happens first!)
                InitialiseCards(); // change the text on the cue cards based on the current slide
                slide.sprite = defaultSlide; // black out the current slide
                break;
            case PresentMode.Speak:
                ChangeCardState(false); // hide cue cards
                speechBox.SetActive(true); // show speech box
                currentChar = 1; // reset to start of string
                speechEndTimer = speechEndTimerLength;
                UpdateSpeechText(currentChar); // show the first character
                break;
        }
    }

    // show a certain amount of the speech in the speech box, as determined by the argument
    private void UpdateSpeechText(int l = 0)
	{
        if (l == 0) // show the entire string
        {
            l = selectedSpeech.Length;
		}

        speechBox.GetComponent<TextMeshPro>().text = selectedSpeech.Substring(0, l);
	}

    private void InitialiseCards()
    {
        // randomise the positions of the cards
        List<int> cardOrder = RandomIntegerList();
        for (int i = 0; i < 3; i++)
		{
            card = cueCards[cardOrder[i]];
            card.position = new Vector3((xOffset * (i - 1)), card.position.y, card.position.z);
		}
        
        // set the text of each card
        for (int a = 0; a < 3; a++)
		{
            Transform TMPObject = cueCards[a].GetChild(0);
            TMPObject.GetComponent<TMP_Text>().text = cardTitles[currentSlide][a];
		}
	}

    // for the sake of this code, generates a 3 entry list, with 0, 1 and 2 in a random order
    // this isn't a great way to shuffle, but fuck it, it'll work!!
    private List<int> RandomIntegerList()
	{
        int r;
        var i = 3;
        List<int> numbers = new List<int> { 0, 1, 2 };
        List<int> intOrder = new List<int>();
        while (i > 0)
        {
            r = Random.Range(0, 3); // 0, 1 or 2
            if (numbers.Contains(r)) // if the random number hasn't been used yet, add it
            {
                intOrder.Add(r);
                numbers.Remove(r);
                i--;
            }
        }
        Debug.Log(string.Format("Card order is {0}, {1}, {2}", intOrder[0], intOrder[1], intOrder[2]));

        return intOrder;
    }

    public void CardSelected(GameObject card)
	{
        var controller = card.GetComponent<CardController>();
        if (controller.index == 0) // the correct answer was chosen
		{
            correctSpeech = true;
		}
        else // the incorrect answer was chosen
		{
            correctSpeech = false;
		}

        // update the speech, and begin to speak
        selectedSpeech = cardSpeeches[currentSlide][controller.index];
        ChangeMode(PresentMode.Speak);
    }
}
