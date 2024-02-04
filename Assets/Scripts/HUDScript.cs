using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.ARFoundation;

public class HUDScript : MonoBehaviour
{
    [SerializeField] private ARPlaneManager m_Manager;
    [SerializeField] private AnchorPlacer m_Placer;

    private VisualElement root;
    private Toggle m_Toggle;

    private Button m_Chair;
    private Button m_Light;
    private Button m_Sofa;

    // Start is called before the first frame update
    private void OnEnable()
    {
        this.root = this.GetComponent<UIDocument>().rootVisualElement;
        this.m_Toggle = this.root.Q<Toggle>("Toggle");

        this.m_Chair = this.root.Q<Button>("Chair");
        this.m_Light = this.root.Q<Button>("Light");
        this.m_Sofa = this.root.Q<Button>("Sofa");

        this.m_Chair.clicked += () => { this.m_Placer.PrefabToSpawn = this.m_Placer.Chair; this.AlterButtonColor(1); };
        this.m_Light.clicked += () => { this.m_Placer.PrefabToSpawn = this.m_Placer.Light; this.AlterButtonColor(2); };
        this.m_Sofa.clicked += () => { this.m_Placer.PrefabToSpawn = this.m_Placer.Sofa; this.AlterButtonColor(3); };
    }

    // Update is called once per frame
    void Update()
    {
        this.m_Manager.SetTrackablesActive(this.m_Toggle.value);
    }

    private void AlterButtonColor(int index)
    {
        switch (index)
        {
            case 1:
                this.m_Chair.style.backgroundColor = Color.green;
                this.m_Light.style.backgroundColor = Color.gray;
                this.m_Sofa.style.backgroundColor = Color.gray;
                break;

            case 2:
                this.m_Chair.style.backgroundColor = Color.gray;
                this.m_Light.style.backgroundColor = Color.green;
                this.m_Sofa.style.backgroundColor = Color.gray;
                break;

            case 3:
                this.m_Chair.style.backgroundColor = Color.gray;
                this.m_Light.style.backgroundColor = Color.gray;
                this.m_Sofa.style.backgroundColor = Color.green;
                break;

            default:
                break;
        }
    }
}
