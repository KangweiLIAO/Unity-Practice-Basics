using UnityEngine;

public class Fractal : MonoBehaviour {
    [SerializeField, Range(1, 8)]
    int depth = 5;


    // Start is called before the first frame update
    void Start() {
        if (depth <= 1) { // Base case
            return;
        }
        Fractal childUp = CreateChild(Vector3.up, Quaternion.identity);
        Fractal childDown = CreateChild(Vector3.down, Quaternion.Euler(0, 0, -90));
        Fractal childRight = CreateChild(Vector3.right, Quaternion.identity);
        Fractal childLeft = CreateChild(Vector3.left, Quaternion.Euler(0, 0, -90));
        Fractal childFwd = CreateChild(Vector3.forward, Quaternion.identity);
        Fractal childBack = CreateChild(Vector3.back, Quaternion.Euler(0, 0, -90));

        childUp.transform.SetParent(transform, false); // false = do not modify the parent's transform
        childDown.transform.SetParent(transform, false);
        childRight.transform.SetParent(transform, false);
        childLeft.transform.SetParent(transform, false);
        childFwd.transform.SetParent(transform, false);
        childBack.transform.SetParent(transform, false);
    }

    // Update is called once per frame
    void Update() {

    }

    Fractal CreateChild(Vector3 direction, Quaternion rotation) {
        Fractal child = GameObject.Instantiate(this); // recursion called
        child.gameObject.name = "Fractal-" + (depth - 1); // give it a good name
        child.transform.localPosition = 0.75f * direction; // next.transform.localPosition == parent.transform.localPosition
        child.transform.localRotation = rotation;
        child.transform.localScale = 0.5f * Vector3.one;
        child.depth--;
        return child;
    }
}
