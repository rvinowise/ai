using System;
using UnityEngine;


namespace rvinowise.unity.persistence {
public class SaraBehavior : MonoBehaviour {
    private Animator animator;
    private Rigidbody2D rigidbody2d;

    public float speed = 2.0f;
    public float destroyRadius = 1.0f;

    public GameObject treePrefab;
    public GameObject treeContainer;

    public GameObject wolfPrefab;
    public GameObject wolfContainer;

    private Vector2 moveVector = new Vector2(0, 0);
    private float moveThreshold = 0.3f;

    void Start() {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();

        Persistent dynamicObject = GetComponent<Persistent>();
        dynamicObject.prepareToSaveDelegates += PrepareToSaveObjectState;
        dynamicObject.loadObjectStateDelegates += LoadObjectState;
    }

    void Update() {
        // Check for mouse clicks to spawn trees and wolves
        CheckMouse();

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Sara cast")) {
            moveVector.Set(0, 0);
            return;
        }

        Vector2 move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (Input.GetKeyDown(KeyCode.Space)) {
            animator.SetTrigger("Casting");
            DoCast();
        }
        else if (move.magnitude >= moveThreshold) {
            animator.SetFloat("MoveX", moveVector.x);
            animator.SetFloat("MoveY", moveVector.y);
            animator.SetBool("Walking", true);
            moveVector = move;
        }
        else {
            animator.SetBool("Walking", false);
            moveVector.Set(0, 0);
        }

        if (Input.GetKeyDown(KeyCode.LeftBracket)) {
            Scene_saver.instance.save_scene();
        }
        if (Input.GetKeyDown(KeyCode.RightBracket)) {
            Scene_loader.instance.load_scene();
        }
    }

    private void CheckMouse() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(treePrefab, new Vector3(worldPosition.x, worldPosition.y, 0), treePrefab.transform.rotation,
                treeContainer.transform);
        }
        else if (Input.GetMouseButtonDown(1)) {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(wolfPrefab, new Vector3(worldPosition.x, worldPosition.y, 0), wolfPrefab.transform.rotation,
                wolfContainer.transform);
        }
    }

    private void FixedUpdate() {
        Vector2 position = rigidbody2d.position;
        position += speed * moveVector * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }

    private void DoCast() {
        // Destroy all objects with tag "Destructible" within the radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(rigidbody2d.position, destroyRadius);
        foreach (Collider2D collider in colliders) {
            if (collider.gameObject.CompareTag("Destructible")) {
                Destroy(collider.gameObject);
            }
        }
    }

    private void PrepareToSaveObjectState(Persistent_state persistent_state) {
        persistent_state.genericValues["SaraBehavior.treeContainer"] =
            treeContainer.GetComponent<Persistent>().persistentState.guid;
        persistent_state.genericValues["SaraBehavior.wolfContainer"] =
            wolfContainer.GetComponent<Persistent>().persistentState.guid;
    }

    private void LoadObjectState(Persistent_state persistent_state) {
        // Load Sara's position
        Vector2 position = Convertor.ConvertToVector2(persistent_state.position);
        transform.position = new Vector3(position.x, position.y, 0);
        // Load the reference to the containers to place new trees/wolves under
        treeContainer = Scene_loader.instance
            .FindDynamicObjectByGuid(Convert.ToString(persistent_state.genericValues["SaraBehavior.treeContainer"]))
            .gameObject;
        wolfContainer = Scene_loader.instance
            .FindDynamicObjectByGuid(Convert.ToString(persistent_state.genericValues["SaraBehavior.wolfContainer"]))
            .gameObject;
    }
}

}