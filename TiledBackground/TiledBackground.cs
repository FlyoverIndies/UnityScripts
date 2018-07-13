using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiledBackground : MonoBehaviour {

    public GameObject backgroundTexture;
    public int platformCount;

    [SerializeField]
    private float scrollSpeed = -1.0f;

    private float leftConstraint;
    private float rightConstraint;
    private float tileWidth;
    private float tileHeight;

    private void Awake()
    {
        leftConstraint = Camera.main.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, 0 - Camera.main.transform.position.z)).x;
        rightConstraint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0.0f, 0 - Camera.main.transform.position.z)).x;
        tileWidth = backgroundTexture.GetComponent<SpriteRenderer>().size.x;
        tileHeight = backgroundTexture.GetComponent<SpriteRenderer>().size.y;
    }

    // Use this for initialization
    void Start () {
        SpriteRenderer newRenderer = backgroundTexture.GetComponent<SpriteRenderer>();
        Camera gameCamera = FindObjectOfType<Camera>();

        float screenWidth = rightConstraint - leftConstraint;
        this.transform.position = new Vector2(leftConstraint, this.transform.position.y);

        int tileCount = (int) Mathf.Ceil((screenWidth / tileWidth)) + 2 * platformCount;
        for (int i = 0; i < tileCount; i++)
        {
            for (int j = 0; j < platformCount; j++) {
                var topPlatform = this.gameObject.transform.position + new Vector3((i * newRenderer.size.x), (float) -tileHeight * j, 0.0f);
                GameObject newTopBackground = Instantiate(backgroundTexture, topPlatform, Quaternion.identity);
                newTopBackground.transform.parent = this.gameObject.transform;
                newTopBackground.GetComponent<Rigidbody2D>().velocity = new Vector2(scrollSpeed, 0.0f);
            }
        }
	}

    public void updateScrollSpeed(float newScrollSpeed) {
        this.scrollSpeed = newScrollSpeed;

        for (int i = 0; i < transform.childCount; i++) {
            Transform currentPlatform = this.gameObject.transform.GetChild(i);
            currentPlatform.GetComponent<Rigidbody2D>().velocity = new Vector2(newScrollSpeed, 0.0f);
        }
    }

	// Update is called once per frame
	void Update () {
        float rightStartPosition = ((transform.childCount - platformCount) * tileWidth / 2) + leftConstraint;


        for (int i = 0; i < transform.childCount; i++) {
            int platformPosition = (i + (transform.childCount - platformCount)) % transform.childCount;

            Transform currentPlatform = this.gameObject.transform.GetChild(i);
            if (currentPlatform.position.x < leftConstraint - tileWidth) {
                float newXPosition = this.gameObject.transform.GetChild(platformPosition).transform.position.x + tileWidth;
                currentPlatform.position = new Vector3(newXPosition, currentPlatform.position.y, 0.0f);
            }
        }
	}
}
