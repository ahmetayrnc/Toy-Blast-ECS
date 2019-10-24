using System.Collections;
using AutoTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoalUi : MonoBehaviour
{
    public TextMeshProUGUI goalAmountText;
    public Image image;

    public ParticleHandler explodeParticle;

    private static readonly Vector3 ImageScaleState1 = new Vector3(.67f, .97f, 1) * 1.3f;
    private static readonly Vector3 ImageScaleState2 = new Vector3(0.97f, .88f, 1) * 1.3f;
    private static readonly Vector3 ImageScaleStateIdle = Vector3.one;

    private const float ImageScaleDuration = .08f; // image duration

    private int _remainingAmount;

    public void UpdateAmount(int amount)
    {
        if (_remainingAmount > amount)
        {
            GoalDecreased();
        }

        goalAmountText.text = amount.ToString();
        _remainingAmount = amount;
    }

    private void GoalDecreased()
    {
        StartCoroutine(ItemCollectedAnimation());
        explodeParticle.Play();
    }

    private IEnumerator ItemCollectedAnimation()
    {
        StartCoroutine(image.rectTransform.ScaleTo(ImageScaleState1, ImageScaleDuration));
        yield return new WaitForSeconds(ImageScaleDuration);
        StartCoroutine(image.rectTransform.ScaleTo(ImageScaleState2, ImageScaleDuration));
        yield return new WaitForSeconds(ImageScaleDuration);
        StartCoroutine(image.rectTransform.ScaleTo(ImageScaleStateIdle, ImageScaleDuration));
        yield return new WaitForSeconds(ImageScaleDuration);
    }

    public void UpdateGoal(Sprite goalSprite)
    {
        image.sprite = goalSprite;
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void DeActivate()
    {
        gameObject.SetActive(false);
    }

    public Vector2 GetPosition()
    {
        return image.rectTransform.position;
    }
}