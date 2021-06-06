using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    
    public float speed = 9f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;
    public float Egilmehizi = 5f;
    public float Yuruyushizi = 9f;
    public int KutuSayısı;

    public GameObject PlayerBody;
    public GameObject Cube;
    public GameObject PlayerCam;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public Text KutusayisiMetni;
    public Text AnahtarAlındımıMetni;

    Vector3 velocity;
    bool isGrounded;
    bool isEgildimi;
    bool didGetAKey;

    private void Start()
    {
        KutuSayısı = 0;
        didGetAKey = false;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

       float x = Input.GetAxis("Horizontal");
       float z = Input.GetAxis("Vertical");
       
        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftShift)) 
        {
            speed = Egilmehizi;
            PlayerBody.transform.Rotate(45f, 0f, 0f);
        }
        
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = Yuruyushizi;
            PlayerBody.transform.Rotate(-45f, 0, 0);
        }
        
        RaycastHit Kapi;
        if (Input.GetKeyDown(KeyCode.F) && Physics.Raycast(gameObject.transform.position, PlayerCam.transform.forward, out Kapi, 5f) && Kapi.transform.tag == "Kapi" && didGetAKey == true)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 3);
            Cursor.lockState = CursorLockMode.None;
            didGetAKey = false;
        }

        RaycastHit Kutu;
        if (Input.GetKeyDown(KeyCode.F) && Physics.Raycast(gameObject.transform.position, PlayerCam.transform.forward, out Kutu, 5f) && Kutu.transform.tag == "Kutu")
        {
            Destroy(Kutu.collider.gameObject);
            KutuSayısı = KutuSayısı + 1;
        }

        RaycastHit Key;
        if (Input.GetKeyDown(KeyCode.F) && Physics.Raycast(gameObject.transform.position, PlayerCam.transform.forward, out Key, 5f) && Key.transform.tag == "Key")
        {
            Destroy(Key.collider.gameObject);
            didGetAKey = true;
        }

        if (Input.GetKeyDown(KeyCode.E) && KutuSayısı > 0)
        {
            Instantiate(Cube, transform.position + transform.forward * 3, Quaternion.identity);
            KutuSayısı = KutuSayısı - 1;
        }

        KutusayisiMetni.text = "Kutu Sayısı: " + KutuSayısı.ToString();
        AnahtarAlındımıMetni.text = "Anahtar: " + didGetAKey.ToString();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Debug.DrawRay(gameObject.transform.position, transform.forward);
    }
}
