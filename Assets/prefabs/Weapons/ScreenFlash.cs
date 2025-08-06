using UnityEngine;
using UnityEngine.UI;

public class ScreenFlash : MonoBehaviour
{
    public Image flashImage;
    public float flashDuration = 0.3f;
    public Color flashColor = new Color(1, 0, 0, 0.5f);

    private Color transparent = new Color(1, 0, 0, 0f);
    private float t;
    private bool isFlashing;

    void Update()
    {
        if (isFlashing)
        {
            t += Time.deltaTime / flashDuration;
            flashImage.color = Color.Lerp(flashColor, transparent, t);

            if (t >= 1)
            {
                isFlashing = false;
                flashImage.color = transparent;
            }
        }
    }

    public void Flash()
    {
        t = 0f;
        isFlashing = true;
        flashImage.color = flashColor;
    }
}
