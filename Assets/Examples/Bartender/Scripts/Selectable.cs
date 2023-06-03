using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    private Renderer renderer;
    private Material initialMaterial;
    private Material hitMaterial;
    private Material selectedMaterial;
    public bool selected = false;

    public Material Material
    {
        get { return renderer.material; }
        set { renderer.material = value;}
    }

    public bool Selected
    {
        get { return selected; }
        set { selected = value; }
    }

    public void Awake()
    {
        renderer = GetComponent<Renderer>();
        initialMaterial = renderer.material;
        SelectionManager selectionManager = GameObject.FindObjectOfType<SelectionManager>();
        hitMaterial = selectionManager.HitMaterial;
        selectedMaterial = selectionManager.SelectedMaterial;
    }

    private void Update()
    {
        if (selected)
        {
            renderer.material = hitMaterial;
            if (Input.GetMouseButton(0))
            {
                renderer.material = selectedMaterial;
            }
        }
        if (!selected)
        {
            renderer.material = initialMaterial;
        }
        selected = false;
    }
}
