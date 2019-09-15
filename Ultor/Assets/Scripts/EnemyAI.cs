using UnityEngine;
using Pathfinding;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (Seeker))]
public class EnemyAI : MonoBehaviour {

    public Transform target;
    public float updateRate = 2f;

    private Seeker seeker;
    private Rigidbody2D rb;

    public Path path;
    public float speed = 300f;
    public ForceMode2D forceMode;

    private bool pathIsEnded = false;
    private bool facingRight = true;

    private bool searchingForPlayer = false;

	private Animator m_Anim;
	

	private Vector3 dir;

    // The max point from AI to a waypoint for it to continue to the next waypoint
    public float nextWayPointDistance = 3f;

    // The waypoint we are currently moving towards to
    private int currentWaypoint = 0;

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
		m_Anim = GetComponent<Animator>();

		if (target == null)
        {
            if(!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }

            return;
        }

        StartCoroutine(UpdatePath());
    }

    IEnumerator SearchForPlayer()
    {
        GameObject sResult = GameObject.FindGameObjectWithTag("Player");
        if(sResult == null)
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine(SearchForPlayer());
        }
        else
        {
            target = sResult.transform;
            searchingForPlayer = false;
            StartCoroutine(UpdatePath());
            yield return false;
        }
    }

    IEnumerator UpdatePath()
    {
        if (target != null) {
            seeker.StartPath(transform.position, target.position, OnPathComplete);

            yield return new WaitForSeconds(1f/updateRate) ;
            StartCoroutine(UpdatePath());
        }
        else
        {
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }

            yield return false;
        }

        
    }

    public void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void FixedUpdate()
    {
        if(target == null)
        {
            return;
        }

        if(path == null)
        {
            return;
        }

        if(currentWaypoint >= path.vectorPath.Count)
        {
            if (pathIsEnded)
                return;

            pathIsEnded = true;
            return;
        }

        pathIsEnded = false;

        // Direction to the next waypoint
        dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;

        dir *= speed * Time.fixedDeltaTime/5;

        // Move the AI
        //if(Mathf.Abs(transform.position.x - target.position.x) < 20)
        rb.AddForce(dir, forceMode);

        Vector3 localScale = transform.localScale;
        if (rb.velocity.x > 0)
        {
            facingRight = true;
        }
        else
            facingRight = false;

        // check to see if scale x is right for the player
        // if not, multiple by -1 which is an easy way to flip a sprite
        if (((!facingRight) && (localScale.x < 0)) || ((facingRight) && (localScale.x > 0)))
        {
            localScale.x *= -1;
        }

        // update the scale
        transform.localScale = localScale;

        float dist = (Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]));

		m_Anim.SetFloat("vSpeed", rb.velocity.y);

		if (dist < nextWayPointDistance && dist < .5)
        {
            currentWaypoint++;
            return;
        }
    }
}
