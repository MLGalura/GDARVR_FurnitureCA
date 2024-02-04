using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class AnchorPlacer : MonoBehaviour
{
    [SerializeField] private GameObject m_Chair;
    [SerializeField] private GameObject m_Light;
    [SerializeField] private GameObject m_Sofa;

    [SerializeField] private GameObject m_PrefabToMove;
    [SerializeField] private GameObject m_PrefabToAnchor;

    private ARAnchorManager m_AnchorManager;
    private ARPlaneManager m_PlaneManager;
    private ARRaycastManager m_RaycastManager;

    private List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();

    // Start is called before the first frame update
    void Start()
    {
        this.m_AnchorManager = this.GetComponent<ARAnchorManager>();
        this.m_PlaneManager = this.GetComponent<ARPlaneManager>();
        this.m_RaycastManager = this.GetComponent<ARRaycastManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (this.m_PrefabToMove)
        {
            Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);

            Ray m_Ray = Camera.main.ScreenPointToRay(screenCenter);

            if (this.m_RaycastManager.Raycast(m_Ray, this.m_Hits, TrackableType.PlaneWithinPolygon))
            {
                foreach (ARRaycastHit hit in this.m_Hits)
                {
                    if (hit.trackable is ARPlane plane) 
                    {
                        this.m_PrefabToMove.transform.position = hit.pose.position;
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(0) && !this.m_PrefabToMove)
        {
            Ray m_Ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (this.m_RaycastManager.Raycast(m_Ray, this.m_Hits, TrackableType.PlaneWithinPolygon))
            {
                foreach (ARRaycastHit hit in this.m_Hits)
                {
                    if (hit.trackable is ARPlane plane)
                    {
                        GameObject m_HitObject = null;
                        RaycastHit m_Hit;

                        if (Physics.Raycast(m_Ray, out m_Hit, Mathf.Infinity))
                            m_HitObject = m_Hit.collider.gameObject;

                        if (m_HitObject.CompareTag("SpawnedPrefab"))
                            this.m_PrefabToMove = m_HitObject;
                        //Destroy(m_HitObject.transform.parent.gameObject);

                        else if (plane.alignment == PlaneAlignment.HorizontalUp)
                            this.AnchorObject(hit.pose.position);
                    }
                }
            }
        }

        else if (Input.GetMouseButtonDown(0))
            if (this.m_PrefabToMove)
                this.m_PrefabToMove = null;
    }
    public void AnchorObject(Vector3 worldPos)
    {
        GameObject m_NewAnchor = new GameObject("NewAnchor");
        m_NewAnchor.transform.parent = null;
        m_NewAnchor.transform.position = worldPos;
        m_NewAnchor.AddComponent<ARAnchor>();

        GameObject obj = Instantiate(this.m_PrefabToAnchor, m_NewAnchor.transform);
        obj.transform.localPosition = Vector3.zero;
    }

    public GameObject PrefabToSpawn { get { return this.m_PrefabToAnchor; } set { this.m_PrefabToAnchor = value; } }
    public GameObject Chair { get { return this.m_Chair; }}
    public GameObject Light { get { return this.m_Light; }}
    public GameObject Sofa { get { return this.m_Sofa; }}
}
