using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float turnSpeed = 10f;
    private Transform m_Player;
    private Vector3 offset;
    private RaycastHit hit;
    private float distance;
    private Vector3[] currentPoints;

    void Awake()
    {
        m_Player = GameObject.FindWithTag("Player").transform;
        currentPoints = new Vector3[5];
    }

    void Start()
    {
        distance = Vector3.Distance(transform.position, m_Player.position);
        offset = m_Player.position - transform.position;
    }

    void LateUpdate()
    {
        Vector3 startPosition = m_Player.position - offset;
        Vector3 endPosition = m_Player.position + Vector3.up * distance;

        currentPoints[1] = Vector3.Slerp(startPosition, endPosition, 0.25f);
        currentPoints[2] = Vector3.Slerp(startPosition, endPosition, 0.5f);
        currentPoints[3] = Vector3.Slerp(startPosition, endPosition, 0.75f);
        currentPoints[0] = startPosition;
        currentPoints[4] = endPosition;

        Vector3 viewposition = currentPoints[0];

        for (int i = 0; i < currentPoints.Length; i++)
        {
            if (CheckView(currentPoints[i]))
            {
                viewposition = currentPoints[i];
                break;
            }
        }

        transform.position = Vector3.Lerp(transform.position, viewposition, Time.deltaTime * moveSpeed);
        SmoothRotate();
    }

    bool CheckView(Vector3 pos)
    {
        Vector3 dir = m_Player.position - pos;
        
        if (Physics.Raycast(pos, dir, out hit))
        {
            if (hit.collider.CompareTag("Player")) // Changed to CompareTag
            {
                return true;
            }
        }
        
        return false;
    }

    void SmoothRotate()
    {
        Vector3 m_Dir = m_Player.position - transform.position;
        Quaternion qua = Quaternion.LookRotation(m_Dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, qua, Time.deltaTime * turnSpeed);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, 0);
    }
}
