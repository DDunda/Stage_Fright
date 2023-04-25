using UnityEngine;

public abstract class AnxietySource : MonoBehaviour
{
    [SerializeField] private float _anxietyLevel = 0f;
    public float anxietyLevel
    {
        get => _anxietyLevel;
        protected set => ChangeAnxiety(value - _anxietyLevel);
    }

    protected virtual void Start()
    {
        if (_anxietyLevel != 0f)
        {
            AnxietyController.ChangeAnxiety(_anxietyLevel);
        }
    }

    protected virtual bool ChangeAnxiety(float difference)
    {
        if (difference == 0) return false;

        AnxietyController.ChangeAnxiety(difference);
        _anxietyLevel += difference;
        return true;
    }
}