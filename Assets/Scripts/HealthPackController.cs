using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPackController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       transform.Rotate(0, 0, -0.5f);
    }

    void OnTriggerEnter2D(Collider2D other) {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            if (controller.health < controller.maxHealth) {
                controller.ChangeHealth(1);
                Destroy(gameObject);
            }
        }
    }
}
