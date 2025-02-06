using UnityEngine;

public class TouchInputHandler
{
    private TouchController controller;
    private Camera mainCamera;
    private GameObject selectedObject;
    private Vector3 lastPos;

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
            RaycastHit2D hitShape = Physics2D.GetRayIntersection(ray, Mathf.Infinity, controller.targetLayer);
            RaycastHit2D hitCell = Physics2D.GetRayIntersection(ray, Mathf.Infinity, controller.cellLayer);

            if (hitShape.collider != null)
            {
                selectedObject = hitShape.collider.gameObject;
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
            Vector3 newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition) + controller.offset;
            newPosition.z = selectedObject.transform.position.z;
            selectedObject.transform.position = newPosition;
            CheckVirtual();
        }

        if (Input.GetMouseButtonUp(0) && selectedObject != null)
        {
            VirtualPlacement.instance.ReplaceVirtualCell();
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            ray.origin += controller.offset;
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, controller.cellLayer);

            if (hit.collider != null)
            {
                Cell targetCell = hit.collider.GetComponent<Cell>();
                if (!controller.OnShapeDropped(selectedObject, targetCell))
                {
                    selectedObject.transform.position = lastPos;
                    selectedObject.GetComponent<Collider2D>().enabled = true;
                }
            }
            else
            {
                selectedObject.transform.position = lastPos;
                selectedObject.GetComponent<Collider2D>().enabled = true;
            }

            selectedObject = null;
        }
    }
    void CheckVirtual()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        ray.origin += controller.offset;
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, controller.cellLayer);

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
