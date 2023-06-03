using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] public Material hitMaterial;
    [SerializeField] public Material selectedMaterial;
    [SerializeField] private GameObject playerCam;
    BartenderAgent agent;

    LineRenderer lineRenderer;
    private Transform cameraPos;

    public Material HitMaterial
    { get { return hitMaterial; } }
    public Material SelectedMaterial
    { get { return selectedMaterial; } }

    private void Awake()
    {
        lineRenderer = playerCam.GetComponent<LineRenderer>();
        lineRenderer.enabled = true;
        cameraPos = playerCam.transform;
        agent = GameObject.FindObjectOfType<BartenderAgent>();
    }
    private void Update()
    {
        Ray ray = new Ray(cameraPos.position, cameraPos.rotation * Vector3.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Transform selection = hit.transform;
            Selectable selectable = hit.collider.GetComponent<Selectable>();
            if (selectable)
            {
                if (!selectable.selected)
                {
                    selectable.selected = true;
                }

                MoveSelectedObject(selection);
            }
            lineRenderer.SetPosition(0, ray.origin);
            lineRenderer.SetPosition(1, hit.point);
        }
    }
        
    void MoveSelectedObject(Transform selection)
    {
        if (agent.mousePressed)
        {
            float distance = (selection.position - cameraPos.position).magnitude;
            Vector3 forward = Vector3.zero;

            selection.transform.position = cameraPos.position + cameraPos.rotation * Vector3.forward * distance;

            if (agent.back) forward = -cameraPos.forward;
            else forward = cameraPos.forward;
            selection.transform.position += forward * Time.deltaTime;
        }
    }
}
