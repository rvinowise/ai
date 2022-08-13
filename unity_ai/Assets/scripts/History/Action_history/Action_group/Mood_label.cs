using TMPro;
using UnityEngine;

public class Mood_label : MonoBehaviour
{
    public TextMeshPro text_label;
    private SpriteRenderer sprite_renderer;

    void Awake() {
        sprite_renderer = GetComponent<SpriteRenderer>();
    }
    public void set_mood(float in_mood) {
        text_label.SetText(in_mood.ToString());
        float red=1;
        float blue=1;
        if (in_mood < 0) {
            blue = 1-Mathf.Abs(in_mood)/5f;
        } else {
            red = 1-Mathf.Abs(in_mood)/5f;
        }
        sprite_renderer.color = new Color(
            red,
            blue,
            1f
        );
    }
}
