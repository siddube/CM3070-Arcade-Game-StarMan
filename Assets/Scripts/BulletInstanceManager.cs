/* ------------------------------------------------------------------------------
<BulletManager Class>
  This script handles
  1> Playing bullet sound fx
  2> Moving bullets
  3> Destory bullets if out of screen bounds
<BulletManager Class>
--------------------------------------------------------------------------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletInstanceManager : MonoBehaviour
{
    // Update is called once per frame
    private float m_speed = 10f;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    private void Start()
    {
        audioSource.Play();
    }
    private void Update()
    {
        MoveBullets();
        DestroyOnOutOfScreenBounds();
    }

    private void MoveBullets()
    {
        this.gameObject.transform.Translate(Vector3.up * m_speed * Time.deltaTime);
    }

    private void DestroyOnOutOfScreenBounds()
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(this.gameObject.transform.position);
        if (viewportPos.x < -0.1f ||
        viewportPos.x > 1.1f ||
        viewportPos.y < -0.1f ||
        viewportPos.y > 1.1f)
        {
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
    }
}
