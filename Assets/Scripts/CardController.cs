using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public PresentationController presentation;
	public int index = 0;

	private void OnMouseOver()
	{
		if (Input.GetMouseButtonDown(0)) // if left click on card
		{
			// only run the function if the card is visible
			if (this.GetComponent<SpriteRenderer>().enabled) 
			{
				presentation.CardSelected(this.gameObject); // pass this card as an argument
			}
		}
	}
}
