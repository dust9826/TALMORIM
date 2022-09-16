using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class CameraObstacle : MonoBehaviour
{
    [SerializeField] LayerMask _layer;
    [SerializeField] Material _material;
    private CapsuleCollider _bounder;
    private List<GameObject> _listPrevObstacleObject = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        _bounder = GetComponent<CapsuleCollider>();
    }
    // Update is called once per frame
    void Update()
    {
        //
        Vector3 pointCenter = transform.TransformPoint(_bounder.center);
        Vector3 pointUp = transform.TransformPoint(_bounder.center) + new Vector3(0, _bounder.height / 2.0f, 0) * 1;
        Vector3 pointDown = transform.TransformPoint(_bounder.center) - new Vector3(0, _bounder.height / 2.0f, 0) * 1;

        List<Ray> listRay = new List<Ray>();
        Vector3 targetPosition = Camera.main.transform.position;    // camera world position

        listRay.Add(new Ray(pointCenter, targetPosition - pointCenter));
        listRay.Add(new Ray(pointUp, targetPosition - pointUp));
        listRay.Add(new Ray(pointDown, targetPosition - pointDown));

        List<RaycastHit[]> listHitInfo = new List<RaycastHit[]>();

        foreach (Ray ray in listRay)
        {
            RaycastHit[] hitInfo = Physics.RaycastAll(ray, 1000.0f, _layer);
            listHitInfo.Add(hitInfo);

            Debug.DrawRay(ray.origin, ray.direction * 500, Color.red);
        }

        List<GameObject> listNewObstacleObject = new List<GameObject>();
        //
        foreach (RaycastHit[] listHits in listHitInfo)
        {
            RaycastHit[] listHit = listHits;

            foreach (RaycastHit hitInfo in listHit)
            {
                if (gameObject.name == hitInfo.collider.name)
                {
                    continue;
                }
                if (hitInfo.collider.name.Contains("Bip"))
                {
                    continue;
                }

                listNewObstacleObject.Add(hitInfo.transform.gameObject);
            }
        }

        foreach (GameObject obstacleObject in listNewObstacleObject)
        {
            string nameShader = "Shader Graphs/Transper";
            MeshRenderer renderer = obstacleObject.GetComponent<MeshRenderer>();
            renderer.material.shader = Shader.Find(nameShader);
        }

        foreach (GameObject obstacleObject in _listPrevObstacleObject)
        {
            if (!listNewObstacleObject.Find(delegate (GameObject inObject) { return (inObject.name == obstacleObject.name); }))
            {
                string nameShader = "HDRP/Lit";
                MeshRenderer renderer = obstacleObject.GetComponent<MeshRenderer>();
                renderer.material = _material;
            }
        }

        _listPrevObstacleObject = listNewObstacleObject;
    }

    private bool FindColliderByName(RaycastHit[] inListRayCastInfo, string inName)
    {
        foreach (RaycastHit hitInfo in inListRayCastInfo)
        {

            if (hitInfo.collider.name == inName)
            { return true; }
        }

        return false;
    }

}
