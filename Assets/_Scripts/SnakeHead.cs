using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnakeHead : MonoBehaviour
{
    [SerializeField] private float frameRate = 0.2f;
    [SerializeField] private float step = 0.16f;

    [Space]
    [SerializeField] private Transform cabeza_root;
    [SerializeField] private Transform colas_root;
    [Space]
    [SerializeField] private GameObject prefabCola;

    public enum Direccion
    {
        arriba,
        abajo,
        izquierda,
        derecha
    }
    private Direccion _dir;
    private Direccion _tempDir;

    public List<Transform> Colas = new List<Transform>();

    public delegate void Comida(int tipo);
    public event Comida OnComida;
    public delegate void Movimiento();
    public event Movimiento OnMovimiento;

    private Vector2 nextPos = Vector2.zero;
    private Vector2 lastPos = Vector2.zero;
    private Vector2 temporalPos = Vector2.zero;

    private readonly Vector2 posZero = Vector2.zero;
    private readonly Vector2 posUp = Vector2.up;
    private readonly Vector2 posdown = Vector2.down;
    private readonly Vector2 posleft = Vector2.left;
    private readonly Vector2 posright = Vector2.right;
    private readonly quaternion quaternionIdentity = quaternion.identity;

    private const string Limite1 = "Limite (1)";
    private const string Limite2 = "Limite (2)";
    private const string Limite3 = "Limite (3)";
    private const string Limite4 = "Limite (4)";
    private const string _Mover = "Mover";
    private const string _Bloque = "Bloque";
    private const string _Comida = "Comida";
    private const string _Poder = "Poder";
    private const string _Serpiente = "Serpiente";
    private const string _Limite = "Limite";

    private void Start()
    {
        InvokeRepeating(_Mover, frameRate, frameRate);
    }

    public void Mover()
    {
        lastPos = cabeza_root.localPosition;
        nextPos = posZero;

        if (_dir == Direccion.abajo && _tempDir == Direccion.arriba)
        {
            _dir = Direccion.arriba;
        }
        else if (_dir == Direccion.arriba && _tempDir == Direccion.abajo)
        {
            _dir = Direccion.abajo;
        }
        else if (_dir == Direccion.izquierda && _tempDir == Direccion.derecha)
        {
            _dir = Direccion.derecha;
        }
        else if (_dir == Direccion.derecha && _tempDir == Direccion.izquierda)
        {
            _dir = Direccion.izquierda;
        }

        switch (_dir)
        {
            case Direccion.arriba:
                nextPos = posUp;
                break;
            case Direccion.abajo:
                nextPos = posdown;
                break;
            case Direccion.izquierda:
                nextPos = posleft;
                break;
            case Direccion.derecha:
                nextPos = posright;
                break;
            default:
                break;
        }
        _tempDir = _dir;

        nextPos *= step;
        cabeza_root.localPosition = new Vector2(cabeza_root.localPosition.x + nextPos.x, cabeza_root.localPosition.y + nextPos.y);

        MoverCola();
        OnMovimiento();
    }

    public void MoverCola()
    {
        for (int i = 0; i < Colas.Count; i++)
        {
            temporalPos = Colas[i].localPosition;
            Colas[i].localPosition = lastPos;
            lastPos = temporalPos;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _dir = Direccion.arriba;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _dir = Direccion.abajo;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _dir = Direccion.izquierda;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _dir = Direccion.derecha;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(_Bloque))
        {
            CancelInvoke(_Mover);
            Debug.Log("Perdio");
        }
        else if (collision.CompareTag(_Limite))
        {
            switch (collision.gameObject.name)
            {
                case Limite1:
                    cabeza_root.localPosition = new Vector3(cabeza_root.localPosition.x, -cabeza_root.localPosition.y + step);
                    break;

                case Limite2:
                    cabeza_root.localPosition = new Vector3(cabeza_root.localPosition.x, -cabeza_root.localPosition.y - step);
                    break;

                case Limite3:
                    cabeza_root.localPosition = new Vector3(-cabeza_root.localPosition.x - step, cabeza_root.localPosition.y);
                    break;

                case Limite4:
                    cabeza_root.localPosition = new Vector3(-cabeza_root.localPosition.x + step, cabeza_root.localPosition.y);
                    break;

                default:
                    break;
            }
            //Debug.Log("Teletransportar");
        }
        else if (collision.CompareTag(_Comida))
        {
            //cambiar destroy por desactivar y pool
            //cambiar instanciate por activar y pool
            OnComida(1);
            //Debug.Log("Alargar");
            Colas.Add(Instantiate(prefabCola, Colas[^1].position, quaternionIdentity, colas_root).transform);
        }
        else if (collision.CompareTag(_Serpiente))
        {
            CancelInvoke(_Mover);
            Debug.Log("Auto Perdio");
            SceneManager.LoadScene(0);
        }
        else if (collision.CompareTag(_Poder))
        {
            OnComida(2);
            //Debug.Log("Poder");
        }
    }
}