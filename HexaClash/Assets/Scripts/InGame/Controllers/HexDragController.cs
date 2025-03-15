using DG.Tweening;
using UnityEngine;

public class HexDragController : MonoBehaviour
{
    [SerializeField] private PlacementController hexPlacementController;
    [SerializeField] private HexSpawnController hexSpawnController;
    [SerializeField] private LayerMask transportableLayerMask, droppableLayerMask;
    
    private bool isDragging;
    private Vector3 offset;
    private Camera mainCamera;

    private HexGround targetHexGround;
    private HexStack carriedStack;
    private Vector3 carriedObjectStartPosition;
    
    private bool isOverTarget;
    
    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isDragging)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, transportableLayerMask))
            {
                if (hit.collider.gameObject.CompareTag("HexStack"))
                {
                    carriedStack = hexSpawnController.DraggedHexStack(hit.collider.gameObject);
                    if (carriedStack != null)
                    {
                        isDragging = true;
                        carriedObjectStartPosition = carriedStack.gameObject.transform.position;
                        offset = carriedStack.gameObject.transform.position - mainCamera.ScreenToWorldPoint(
                            new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
                    }
                    
                }
            }
        }
        if (isDragging && Input.GetMouseButton(0))
        {
            Vector3 newPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f)) + offset;
            carriedStack.gameObject.transform.position = new Vector3(newPosition.x, newPosition.y, carriedStack.gameObject.transform.position.z);
            
            CheckTargetUnderMouse();
        }
        if (isDragging && Input.GetMouseButtonUp(0))
        {
            if (isOverTarget && targetHexGround != null)
            {
                // if can drop, drop hex stack.
                targetHexGround.OnNewStackEnter(carriedStack);
                carriedStack.gameObject.transform.parent = targetHexGround.transform;
                carriedStack.gameObject.transform.DOMove(targetHexGround.HexStackPoint, 0.5f).SetEase(Ease.OutQuad);
            }
            else
            {
                // return the start point
                carriedStack.gameObject.transform.DOMove(carriedObjectStartPosition, 0.5f).SetEase(Ease.OutQuad);
            }

            isDragging = false;
            isOverTarget = false;
            targetHexGround = null; 
        }
    }
    
    private void CheckTargetUnderMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity ,droppableLayerMask))
        {
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject.CompareTag("HexGround"))  
            {
                if (!isOverTarget || targetHexGround.gameObject != hitObject)
                {
                    var newHex = hexPlacementController.HoveredHexGround(hitObject);
                    if (newHex != null)
                    {
                        isOverTarget = true;
                        targetHexGround = newHex;
                    }
                }
            }
            else
            {
                if (isOverTarget && targetHexGround != null)
                {
                    isOverTarget = false;
                    targetHexGround = null;
                }
            }
        }
        else
        {
            if (isOverTarget && targetHexGround != null)
            {
                targetHexGround.OnHoverExit();
                isOverTarget = false;
                targetHexGround = null;
            }
        }
    }
}
