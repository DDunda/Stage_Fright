using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController : MonoBehaviour
{
	public Collider2D collider;
	public LayerMask visionLayer;
	public LayerMask eyeContactLayer;
	private SpriteRenderer sprite;

	[Header("Discomfort")]
	[Range(0f, 1f)]
	public float discomfort = 0f; // Rises when you look at somebody (0-1)
	public float passiveDiscomfortRate = -0.2f;
	public float backDiscomfortRate = -0.4f; // Rate when you have your back to the audience
	public float staringDiscomfortRate = 0.1f; // How fast staring increases discomfort

	public float discomfortAgitationThreshold = 0.7f; // The amount of discomfort needed to start getting agitated

	[Header("Agitation")]
	[Range(0f,1f)]
	public float agitation = 0f; // How agitated this person is (0-1)
	public float passiveAgitationRate = 0.01f;
	public float ignoreAgitationRate = 0.02f;
	public float staringAgitationRate = -0.02f; // How fast staring decreases agitation if not uncomfortable
	public float discomfortAgitationRate = 0.03f; // How fast discomfort increases agitation

	public float agitationAnxietyThreshold = 0.7f; // The amount of agitation needed to start adding anxiety

	public float agitationAnxietyRate = 0.01f; // How fast agitation increases anxiety

	private void Start()
	{
		sprite = transform.GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		if (sprite.enabled)
		{
			if (collider.IsTouchingLayers(eyeContactLayer))
			{
				if (discomfort < discomfortAgitationThreshold)
				{
					agitation += staringAgitationRate * Time.deltaTime;
				}
				discomfort += staringDiscomfortRate * Time.deltaTime;
			}
			else
			{
				discomfort += passiveDiscomfortRate * Time.deltaTime;
			}
		} else
		{
			discomfort += backDiscomfortRate * Time.deltaTime;
		}

		if (discomfort >= discomfortAgitationThreshold)
		{
			agitation += discomfortAgitationRate * Time.deltaTime;
		}
		else if (sprite.enabled)
		{
			agitation += passiveAgitationRate * Time.deltaTime;
		}
		else
		{
			agitation += ignoreAgitationRate * Time.deltaTime;
		}

		discomfort = Mathf.Clamp01(discomfort);
		agitation = Mathf.Clamp01(agitation);

		if (agitation >= agitationAnxietyThreshold)
		{
			AnxietyController.ChangeAnxiety01(agitationAnxietyRate * Time.deltaTime); 
		}
	}
}
