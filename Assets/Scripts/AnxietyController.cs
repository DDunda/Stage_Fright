using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class AnxietyController : MonoBehaviour
{
	// Singleton pattern
	public static AnxietyController Instance { get; private set; }

	[SerializeField]
	private float _anxietyLevel = 0f;
	[SerializeField]
	private float _minAnxiety = 0f;
	[SerializeField]
	private float _maxAnxiety = 100f;

	public static float anxietyLevel
	{
		get => Instance._anxietyLevel;
		private set => Instance._anxietyLevel = value;
	}

	// Transforms anxietyLevel to a 0 - 1 range where 0 represents minAnixety and 1 represents maxAnxiety
	public static float anxietyLevel01
	{
		get => Mathf.InverseLerp(minAnxiety, maxAnxiety, anxietyLevel);
		private set => anxietyLevel = Mathf.Lerp(minAnxiety, maxAnxiety, value);
	}

	public static float minAnxiety { get => Instance._minAnxiety; }
	public static float maxAnxiety { get => Instance._maxAnxiety; }

	public static void ChangeAnxiety(float difference)
	{
		anxietyLevel = Mathf.Clamp(anxietyLevel + difference, minAnxiety, maxAnxiety);
	}

	public static void ChangeAnxiety01(float difference)
	{
		anxietyLevel01 = Mathf.Clamp01(anxietyLevel01 + difference);
	}

	private void Awake()
	{
		if(Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(this);
		}
	}

	private void OnDestroy()
	{
		if(Instance == this)
		{
			Instance = null;
		}
	}
}
