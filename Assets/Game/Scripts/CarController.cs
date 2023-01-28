using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float speed = 10.0f; // скорость машинки
    public float torque = 10.0f; // момент инерции, отвечающий за поведение машинки при повороте
    public float gravity = 10.0f; // сила гравитации, отвечающая за скорость спуска машинки вниз по склону

    public float suspensionDistance = 0.1f; // расстояние, на которое подвеска смещается при приземлении
    public float suspensionSpring = 50.0f; // жесткость пружины, отвечающей за отскок подвески
    public float suspensionDamper = 5.0f; // жесткость демпфера, отвечающего за торможение отскока подвески
    public float power = 100.0f; // мощность двигателя

    private Rigidbody2D rb; // ссылка на компонент Rigidbody

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); // получаем ввод с горизонтальной оси (A и D или стрелки влево и вправо)
        float verticalInput = Input.GetAxis("Vertical"); // получаем ввод с вертикальной оси (W и S или стрелки вверх и вниз)

        // обновляем параметры подвески
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, suspensionDistance);
        if (hit.collider != null)
        {
            float force = suspensionSpring * (suspensionDistance - hit.distance) - suspensionDamper * rb.velocity.y;
            rb.AddForceAtPosition(force * transform.up, hit.point);
        }

        // увеличиваем силу двигателя в соответствии с введенной скоростью
        rb.AddForce(transform.right * speed * verticalInput * power);
        rb.AddTorque(torque * horizontalInput); // добавляем момент инерции, чтобы машинка поворачивалась

        rb.AddForce(Vector2.down * gravity); // добавляем силу гравитации, чтобы машинка спускалась вниз по склону
    }
}