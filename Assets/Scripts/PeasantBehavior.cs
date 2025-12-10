using UnityEngine;
using System.Collections;

public class PeasantBehavior : MonoBehaviour
{
    [Header("Movement")]
    public Transform runTarget;
    public Transform walkTarget;
    public float runSpeed = 3f;
    public float walkSpeed = 1.5f;
    public float turnDuration = 0.6f;

    [Header("Animation Names")]
    public string animRun = "Run";
    public string animWalk = "Walk";
    public string animTalk = "Talk";
    public string animTurn = "Turn";
    public Animator animator;

    private void Awake()
    {
        StartCoroutine(PeasantRoutine());
    }

    private IEnumerator PeasantRoutine()
    {
        // 1. Run to the first point
        animator.Play(animRun);
        yield return MoveTo(runTarget.position, runSpeed);

        // 2. Talk
        animator.Play(animTalk);
        yield return new WaitForSeconds(2f);

        // 3. Turn animation
        animator.Play(animTurn);
        yield return new WaitForSeconds(turnDuration);

        // 4. Walk to second point
        animator.Play(animWalk);
        yield return MoveTo(walkTarget.position, walkSpeed);

        // 5. Delete Peasant
        Destroy(gameObject, 1f);
    }

    // Simple movement helper
    private IEnumerator MoveTo(Vector3 targetPos, float speed)
    {
        while (Vector3.Distance(transform.position, targetPos) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                speed * Time.deltaTime
            );
            yield return null;
        }
    }
}
