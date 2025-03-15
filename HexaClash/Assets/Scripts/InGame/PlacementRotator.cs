using DG.Tweening;
using UnityEngine;

public class PlacementRotator : MonoBehaviour
{
    [SerializeField] private Transform rotateObject;
    [SerializeField] private LayerMask layerMask;
    
    private Vector2 lastTouchPosition;
    private Camera cam;
    public float rotationSpeed = 200f;
    private float targetAngle;
    private float initialAngle;
    private bool isDragging;
    

    private void Awake()
    {
        cam = Camera.main;
        initialAngle = rotateObject.eulerAngles.z;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = touch.position;
            rotateObject.DOKill();
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = cam.ScreenPointToRay(touchPosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity,layerMask))
                {
                    if (hit.collider.gameObject == gameObject)
                    {
                        isDragging = true;
                        lastTouchPosition = touchPosition;
                    }
                }
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                Vector2 currentTouchPosition = touchPosition;
                float deltaX = currentTouchPosition.x - lastTouchPosition.x;

                float rotationAmount = deltaX * rotationSpeed * Time.deltaTime;
                rotateObject.Rotate(0, 0, rotationAmount, Space.Self);

                lastTouchPosition = currentTouchPosition;
            }
            else if (touch.phase == TouchPhase.Ended && isDragging)
            {
                isDragging = false;
                SnapToNearest120();
            }
        }
    }
    private void SnapToNearest120()
    {
        float currentAngle = rotateObject.eulerAngles.z; 
        float normalizedAngle = currentAngle - initialAngle;
        float snappedAngle = Mathf.Round(normalizedAngle / 60f) * 60f;
        targetAngle = initialAngle + snappedAngle;

        targetAngle = targetAngle % 360f;
        if (targetAngle < 0) targetAngle += 360f;
        
        rotateObject.DORotate(new Vector3(0, 0, targetAngle), 0.5f).SetEase(Ease.OutQuad);
    }
}