using System;
using UnityEngine;

namespace rvinowise.unity.persistence {

public class WolfBehavior : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rigidbody2d;
    private LineRenderer lineRenderer;

    public GameObject favoriteTree;

    public float speed = 1.5f;
    public float timeToWalkTotal = 3.0f;
    public float timeToWalkRemaining = 0;
    public bool walkingLeft = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.05f;

        // Choose a favorite tree randomly
        TreeBehavior[] treeBehaviors = FindObjectsOfType<TreeBehavior>();
        if (treeBehaviors.Length > 0)
        {
            favoriteTree = treeBehaviors[UnityEngine.Random.Range(0, treeBehaviors.Length)].gameObject;
        }

        Persistent dynamicObject = GetComponent<Persistent>();
        dynamicObject.prepare_to_saving += prepare_to_saving;
        dynamicObject.load_persistent_state += LoadObjectState;
    }

    void Update()
    {
        timeToWalkRemaining -= Time.deltaTime;
        if (timeToWalkRemaining <= 0)
        {
            timeToWalkRemaining = timeToWalkTotal;
            walkingLeft = !walkingLeft;
        }
        animator.SetFloat("MoveX", walkingLeft ? -1 : 1);
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position += speed * (walkingLeft ? Vector2.left : Vector2.right) * Time.deltaTime;
        rigidbody2d.MovePosition(position);

        if (favoriteTree == null)
        {
            favoriteTree = null;
            lineRenderer.positionCount = 0;
        } else
        {
            Vector3[] positions = { transform.position, favoriteTree.transform.position };
            lineRenderer.positionCount = 2;
            lineRenderer.SetPositions(positions);
        }
    }

    private void prepare_to_saving(Persistent_state persistent_state)
    {
        persistent_state.genericValues["WolfBehavior.speed"] = speed;
        persistent_state.genericValues["WolfBehavior.timeToWalkRemaining"] = timeToWalkRemaining;
        persistent_state.genericValues["WolfBehavior.walkingLeft"] = walkingLeft;
        if (favoriteTree != null)
        {
            persistent_state.genericValues["WolfBehavior.favoriteTree"]
            = favoriteTree.GetComponent<Persistent>().persistent_state.guid;
        } else if (persistent_state.genericValues.ContainsKey("WolfBehavior.favoriteTree"))
        {
            persistent_state.genericValues.Remove("WolfBehavior.favoriteTree");
        }
    }

    private void LoadObjectState(Persistent_state persistent_state)
    {
        speed = Convert.ToSingle(persistent_state.genericValues["WolfBehavior.speed"]);
        timeToWalkRemaining = Convert.ToSingle(persistent_state.genericValues["WolfBehavior.timeToWalkRemaining"]);
        walkingLeft = Convert.ToBoolean(persistent_state.genericValues["WolfBehavior.walkingLeft"]);
        if (persistent_state.genericValues.ContainsKey("WolfBehavior.favoriteTree"))
        {
            favoriteTree = Scene_loader.instance.FindDynamicObjectByGuid(Convert.ToString(persistent_state.genericValues["WolfBehavior.favoriteTree"])).gameObject;
        }
    }
}

}