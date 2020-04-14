using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Camera camera;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private CinemachineVirtualCamera virtualVatsCamera;
    [SerializeField]
    private CinemachineVirtualCamera virtualMainCamera;

    private HightlightLimb hightlightLimb;

    private bool vatsIsActive;
    private Ray cameraRay;

    // Update is called once per frame
    void Update()
    {
        if (!vatsIsActive)
        {
            ClickOnEnemy();
            LookAtMovement();
        }
        else
        {
            LimbDetection();
            ResetCamera();
        }
    }

    private void LimbDetection()
    {
        RaycastHit hit;
        cameraRay = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(cameraRay, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.transform.tag == "Enemy")
            {
                string limbName = hit.transform.gameObject.GetComponent<Collider>().name;
                hightlightLimb.LimbSelection(limbName);
            }
        }
    }

    private void ResetCamera()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(vatsIsActive)
            {
                virtualVatsCamera.gameObject.SetActive(false);
                virtualMainCamera.gameObject.SetActive(true);
                vatsIsActive = false;
                hightlightLimb.LimbSelection("None");
            }
        }
    }

    private void ClickOnEnemy()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            cameraRay = camera.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(cameraRay, out hit, Mathf.Infinity))
            {
                if(hit.transform.tag == "Enemy")
                {
                    hightlightLimb = hit.transform.gameObject.GetComponent<HightlightLimb>();
                    vatsZoom(hit.transform);
                }
            }
        }
    }

    private void vatsZoom(Transform enemiesTransform)
    {
        vatsIsActive = true;
        virtualMainCamera.gameObject.SetActive(false);
        virtualVatsCamera.gameObject.SetActive(true);

        virtualVatsCamera.transform.DOLocalRotate(new Vector3(enemiesTransform.position.x, enemiesTransform.position.y, 0), 1f);
        virtualVatsCamera.m_Follow = enemiesTransform;
    }

    private void LookAtMovement()
    {
        cameraRay = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(cameraRay, out hit, Mathf.Infinity))
        {
            Vector3 playerToMouse = hit.point - transform.position;
            playerToMouse.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            transform.rotation = newRotation;
        }
    }
}
