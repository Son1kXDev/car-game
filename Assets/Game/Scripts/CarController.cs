using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float speed = 10.0f; // �������� �������
    public float torque = 10.0f; // ������ �������, ���������� �� ��������� ������� ��� ��������
    public float gravity = 10.0f; // ���� ����������, ���������� �� �������� ������ ������� ���� �� ������

    public float suspensionDistance = 0.1f; // ����������, �� ������� �������� ��������� ��� �����������
    public float suspensionSpring = 50.0f; // ��������� �������, ���������� �� ������ ��������
    public float suspensionDamper = 5.0f; // ��������� ��������, ����������� �� ���������� ������� ��������
    public float power = 100.0f; // �������� ���������

    private Rigidbody2D rb; // ������ �� ��������� Rigidbody

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); // �������� ���� � �������������� ��� (A � D ��� ������� ����� � ������)
        float verticalInput = Input.GetAxis("Vertical"); // �������� ���� � ������������ ��� (W � S ��� ������� ����� � ����)

        // ��������� ��������� ��������
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, suspensionDistance);
        if (hit.collider != null)
        {
            float force = suspensionSpring * (suspensionDistance - hit.distance) - suspensionDamper * rb.velocity.y;
            rb.AddForceAtPosition(force * transform.up, hit.point);
        }

        // ����������� ���� ��������� � ������������ � ��������� ���������
        rb.AddForce(transform.right * speed * verticalInput * power);
        rb.AddTorque(torque * horizontalInput); // ��������� ������ �������, ����� ������� ��������������

        rb.AddForce(Vector2.down * gravity); // ��������� ���� ����������, ����� ������� ���������� ���� �� ������
    }
}