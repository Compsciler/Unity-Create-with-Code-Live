using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private GameObject focalPoint;
    public float speed;
    private float forwardInput;
    private bool hasPowerUp = false;
    public float powerUpStrength = 8f;
    public float powerUpTime = 15f;  // Changed from 7f

    public static bool isKeyboardControl = false;

    public GameObject powerUpIndicator;
    public Vector3 powerUpIndicatorOffset;

    public GameObject projectilePrefab;
    public float explosionForce;
    public float explosionRadius;
    public float upwardsModifier = 3f;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        if (isKeyboardControl)
        {
            forwardInput = Input.GetAxis("Vertical");
            playerRb.AddForce(focalPoint.transform.forward * forwardInput * speed);
        }
        else
        {
            forwardInput = Input.GetAxis("VerticalMouse");
            playerRb.AddForce(focalPoint.transform.forward * forwardInput * speed);
        }
        if (hasPowerUp && Input.GetKeyDown(KeyCode.Alpha1))
        {
            Instantiate(projectilePrefab, transform.position, focalPoint.transform.rotation);
        }
        if (hasPowerUp && Input.GetKeyDown(KeyCode.Alpha2))
        {
            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (hit.gameObject.CompareTag("Enemy"))
                {
                    rb.AddExplosionForce(explosionForce, explosionPos, explosionRadius, upwardsModifier);
                }
            }
            /*
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                Vector3 enemyDirection = enemy.transform.position - transform.position;
                enemy.GetComponent<Rigidbody>().AddForce(1);
            }
            */
        }
        powerUpIndicator.transform.position = transform.position - powerUpIndicatorOffset;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))  // TODO: Enemies can collect power-ups too
        {
            hasPowerUp = true;
            Destroy(other.gameObject);
            StartCoroutine("PowerUpCountdownRoutine");
            powerUpIndicator.SetActive(true);
        }
    }
    
    IEnumerator PowerUpCountdownRoutine()
    {
        yield return new WaitForSeconds(powerUpTime);
        hasPowerUp = false;
        powerUpIndicator.SetActive(false);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerUp)
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;
            enemyRb.AddForce(awayFromPlayer * powerUpStrength, ForceMode.Impulse);
        }
    }
}
