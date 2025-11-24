using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FlipFlopHandleWalk : MonoBehaviour
{
    public Image targetImage;
    public Sprite spriteA;
    public Sprite spriteB;
    public float interval = 0.5f;

    private bool usingA = true;

    void Start()
    {
        if (targetImage == null || spriteA == null || spriteB == null)
        {
            return;
        }

        StartCoroutine(FlipFlopRoutine());
    }

    IEnumerator FlipFlopRoutine()
    {
        while (true)
        {
            targetImage.sprite = usingA ? spriteA : spriteB;
            usingA = !usingA;
            yield return new WaitForSeconds(interval);
        }
    }
}
