using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class SpriteFader : MonoBehaviour
{
    [Serializable]
    public class SpriteStage
    {
        public SpriteRenderer spriteRenderer;
        public float targetAnxiety01;
        public Color tint = Color.white;
    }

    [SerializeField] private SpriteStage[] sprites;

    // Start is called before the first frame update
    void Start()
    {
        sprites = (from sprite in sprites
                   orderby sprite.targetAnxiety01 ascending
                            select sprite).ToArray();
        SetState();
    }

    // Update is called once per frame
    void Update()
    {
        SetState();
    }

    protected void SetState()
    {
        if (sprites.Length == 0) return;

        if (sprites.Length == 1)
        {
            sprites[0].spriteRenderer.color = sprites[0].tint;
            return;
        }

        float t = AnxietyController.anxietyLevel01;

        foreach (var sprite in sprites)
        {
            sprite.spriteRenderer.color = Color.clear;
        }

        if (t <= sprites.First().targetAnxiety01)
        {
            sprites.First().spriteRenderer.color = sprites.First().tint;
            return;
        }

        if (t >= sprites.Last().targetAnxiety01)
        {
            sprites.Last().spriteRenderer.color = sprites.Last().tint;
            return;
        }

        SpriteStage lsprite = sprites[0];
        float t1 = lsprite.targetAnxiety01;

        for (int i = 1; i < sprites.Length; i++)
        {
            SpriteStage sprite = sprites[i];
            float t2 = sprite.targetAnxiety01;

            if (t >= t1 && t <= t2)
            {
                t = Mathf.InverseLerp(t1, t2, t);

                if (lsprite.spriteRenderer == sprite.spriteRenderer)
                {
                    Color c = Color.Lerp(lsprite.tint, sprite.tint, t);
                    sprite.spriteRenderer.color = c;
                }
                else
                {
                    Color c = lsprite.tint;
                    c.a *= (1 - t);
                    lsprite.spriteRenderer.color = c;

                    c = sprite.tint;
                    c.a *= t;
                    sprite.spriteRenderer.color = c;
                }

                return;
            }

            lsprite = sprite;
            t1 = t2;
        }
    }
}
