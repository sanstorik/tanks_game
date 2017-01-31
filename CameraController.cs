using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CameraController : MonoBehaviour {

    static private CameraController instance;

    [SerializeField]
    Transform upperLeftBorder;
    [SerializeField]
    Transform lowerRightBorder;
    public Transform spawnSpot;

    [SerializeField]
    GameObject additionalGround;
    [SerializeField]
    MeshRenderer kupaKeepText;

    [HideInInspector]
    public bool gameIsStarted = false;
    [HideInInspector]
    public Vector3 leftCorner;
    [HideInInspector]
    public Vector3 rightCorner;

    Camera mainCamera;
    Vector3 cameraStartingPosition;
    Vector3 moveToPosition;
    bool targetIsAquiered = false;
    Transform target;

    private void Awake()
    {
        instance = this;
        mainCamera = GetComponent<Camera>();
    }

    void Start () {
        cameraStartingPosition = mainCamera.transform.position;
        leftCorner = cameraStartingPosition;
        rightCorner = cameraStartingPosition;

        rightCorner.x += Camera.main.ScreenToWorldPoint(Vector3.right * Screen.width).x * 1.5f;

        kupaKeepText.sortingOrder = 4;
    }
	
	void FixedUpdate () {
        if (targetIsAquiered)
        {
            moveToPosition = new Vector3(
                Mathf.Clamp(target.position.x * Time.deltaTime * 6f, upperLeftBorder.position.x, lowerRightBorder.position.x),
                Mathf.Clamp(target.position.y * Time.deltaTime * 22f, lowerRightBorder.position.y, upperLeftBorder.position.y),
                -10);
            mainCamera.transform.position = moveToPosition;
        }
    }

    public void SetTrackTarget(Transform target)
    {
        if (gameIsStarted)
        {
            targetIsAquiered = true;
            instance.target = target;
        }
    }

    public void ResetTarget()
    {
        if (gameIsStarted)
        {
            targetIsAquiered = false;
            mainCamera.transform.DOMove(cameraStartingPosition, 1f);
        }
    }

    public void MoveSmoothToPosition(Vector3 position)
    {

        moveToPosition = new Vector3(
               Mathf.Clamp(position.x * Time.deltaTime * 5f, upperLeftBorder.position.x, lowerRightBorder.position.x),
               Mathf.Clamp(position.y * Time.deltaTime * 27f, lowerRightBorder.position.y, upperLeftBorder.position.y),
              -10);
        mainCamera.transform.DOMove(moveToPosition, 1f);
    }

    public void SetActiveAdditionalGround(bool active)
    {
        additionalGround.SetActive(active);
    }

    static public CameraController INSTANCE
    {
        get { return instance; }
    }

}
