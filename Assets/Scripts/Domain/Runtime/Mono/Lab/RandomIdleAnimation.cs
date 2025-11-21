using UnityEngine;
using System.Collections;

public class RandomIdleWithBaseState : MonoBehaviour
{
    [Header("Animator Settings")]
    [SerializeField] private Animator animator;

    [Header("Animation Names")]
    [SerializeField] private string[] animationNames = { "Idle1", "Idle2", "Idle3", "Idle4", "Idle5", "Idle6" };

    [Header("Timing Settings")]
    [SerializeField] private float minInterval = 3f;
    [SerializeField] private float maxInterval = 8f;

    [Header("Base State")]
    [SerializeField] private string baseIdleState = "BaseIdle";

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        StartCoroutine(RandomIdleRoutine());
    }

    IEnumerator RandomIdleRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));

            string randomAnimation = animationNames[Random.Range(0, animationNames.Length)];
            animator.Play(randomAnimation);

            yield return new WaitForSeconds(GetAnimationLength(randomAnimation));

            if (!string.IsNullOrEmpty(baseIdleState))
            {
                animator.Play(baseIdleState);
            }
        }
    }

    private float GetAnimationLength(string animationName)
    {
        if (animator.runtimeAnimatorController == null) return 1f;

        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == animationName)
                return clip.length;
        }
        return 1f;
    }
}