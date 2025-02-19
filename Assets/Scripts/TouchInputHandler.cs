using UnityEngine;

public class TouchInputHandler
{
    private TouchController controller;
    private Camera mainCamera;
    private GameObject selectedObject;
    private Vector3 lastPos;
    private Vector3 shapeScaleTemp;
    private Vector3 firstTouchPos;

    public TouchInputHandler(TouchController controller)
    {
        this.controller = controller;
        mainCamera = Camera.main;
    }

    public void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            firstTouchPos = ray.origin;
            RaycastHit2D hitShape = Physics2D.GetRayIntersection(ray, Mathf.Infinity, controller.targetLayer);
            RaycastHit2D hitCell = Physics2D.GetRayIntersection(ray, Mathf.Infinity, controller.cellLayer);

            if (hitShape.collider != null)
            {
                selectedObject = hitShape.collider.gameObject;
                shapeScaleTemp = selectedObject.transform.localScale;
                selectedObject.transform.localScale = TouchController.instance.gridSystem.cellSize.x * Vector3.one;
                selectedObject.GetComponent<Collider2D>().enabled = false;
                lastPos = selectedObject.transform.position;
            }
            if (hitCell.collider != null)
            {
                Cell targetCell = hitCell.collider.GetComponent<Cell>();
                SkillManager.instance.UseSkill(targetCell);
            }
            SkillManager.instance.DeselectSkill();
        }

        if (Input.GetMouseButton(0) && selectedObject != null)
        {
            Vector3 newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition) + new Vector3(lastPos.x- firstTouchPos.x, controller.offset.y,0)  ;
            newPosition.z = selectedObject.transform.position.z;
            selectedObject.transform.position = newPosition;
            CheckVirtual();
        }

        if (Input.GetMouseButtonUp(0) && selectedObject != null)
        {
            Vector3 rayOrigin = VirtualPlacement.instance.cellObject.transform.position+Vector3.back;
            Ray ray = new Ray(rayOrigin, Vector3.forward);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, controller.cellLayer);
            VirtualPlacement.instance.ReplaceVirtualCell();
            if (hit.collider != null)
            {
                Cell targetCell = hit.collider.GetComponent<Cell>();
                if (!controller.OnShapeDropped(selectedObject, targetCell))
                {
                    selectedObject.transform.position = lastPos;
                    selectedObject.transform.localScale = shapeScaleTemp;
                    selectedObject.GetComponent<Collider2D>().enabled = true;
                }
            }
            else
            {
                selectedObject.transform.position = lastPos;
                selectedObject.transform.localScale = shapeScaleTemp;
                selectedObject.GetComponent<Collider2D>().enabled = true;
            }

            selectedObject = null;
        }
    }
    void CheckVirtual()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        ray.origin += new Vector3(lastPos.x - firstTouchPos.x, controller.offset.y);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, controller.cellLayer);

        if (hit.collider == null)
        {
            Vector3 baseRayOrigin = ray.origin;
            Vector3[] directions = { Vector3.right, Vector3.left, Vector3.up, Vector3.down };
            float cellSize = TouchController.instance.gridSystem.cellSize.x;

            foreach (Vector3 direction in directions)
            {
                ray.origin = baseRayOrigin + (direction * cellSize);
                hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, controller.cellLayer);

                if (hit.collider != null)
                    break;
            }

            ray.origin = baseRayOrigin;
        }

        if (hit.collider != null)
        {
            Cell targetCell = hit.collider.GetComponent<Cell>();
            VirtualCell virtualCell = controller.CanShapeDrop(selectedObject, targetCell);
            if (virtualCell!=null)
            {
                VirtualPlacement.instance.PlaceVirtualCell(virtualCell);
                return;
            }
            
        }
        VirtualPlacement.instance.ReplaceVirtualCell();
    }
}
