using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public float _speed;
    public Text _countText;
    public Text _winText;

    private Rigidbody _rb;
    private int _count;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _count = 0;
        _winText.text = "";
        SetCountText();
    }
    // Use this about physics
    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        _rb.AddForce(movement * _speed);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick up"))
        {
            other.gameObject.SetActive(false);
            _count++;
            SetCountText();
        }
    }

    private void SetCountText() {
        _countText.text = "Count : " + _count.ToString();
        if(_count >= 12)
        {
            _winText.text = "You win !";
        }
    }
}
