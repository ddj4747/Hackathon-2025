using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class HealthBarManager : MonoBehaviour
{
    public int SceneIndex = 0;
    [Header("Health Stripes")]
    [SerializeField] private GameObject[] healthStripes; // Array of health stripes (child objects)

    public Material damageMaterial;
    private Material defaultMaterial;

    private int health = 8;
    public float damageMaterialTime;

    private bool cooldown = false;

    private void Start()
    {
        defaultMaterial = PlayerMovement.Instance.GetComponent<SpriteRenderer>().material;
    }


    private IEnumerator TakeDamage()
    {
        List<Material> l = new List<Material>();
        l.Add(damageMaterial);
        PlayerMovement.Instance.GetComponent<SpriteRenderer>().SetMaterials(l);
        cooldown = true;

        yield return new WaitForSeconds(damageMaterialTime);

        cooldown = false;
        l.Clear();
        l.Add(defaultMaterial);
        PlayerMovement.Instance.GetComponent<SpriteRenderer>().SetMaterials(l);
    }


    public void UpdateHealthBar(float currentHealth)
    {
        if (cooldown) return;
        health--;
        healthStripes[health].SetActive(false);
        StartCoroutine(TakeDamage());

        if (health == 0)
        {
            SceneManager.LoadScene(2, LoadSceneMode.Single);
        }
    }
}
