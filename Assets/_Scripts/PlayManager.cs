using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayManager : MonoBehaviour
{
    [Header("Elementos")]
    public SnakeHead _snakeHead;
    public Transform comida;
    public GameObject poder;

    [Header("Configuracion")]
    public Vector2 limitesComidaSpaw = new Vector2(4, 4);

    [Header("UI")]
    public Text txt_puntaje;
    public int puntoComida = 8;

    private int puntuacion = 0000;

    [Header("Multiplos de PuntoComida")]
    public Text txt_tiempoPoder;
    public int puntosMultiplicacionPoder = 5;
    public int cadaCuantoPoder = 6;
    public int aumentoEsperaPoder = 1;

    private int esperaPoder = 0;
    public int tiempoPoder = 40;
    private int tiempoPoderRestante = 0;

    public Image img_poder;

    private bool hayPoder = false;

    public Text txt_comboPoder;
    private int comboPoder = 0;

    private Vector2 posValida = new Vector2(0, 0);
    private List<int> posLibreX = new List<int>();
    private List<int> posLibreY = new List<int>();
    private List<Vector2> ColasPos = new List<Vector2>();
    private System.Random rand;
    private Vector2 valores = new Vector2(0, 0);
    private float limX = 0;
    private float limY = 0;
    private float valX = 0;
    private float valY = 0;
    private int index1 = 0;
    private float _index1 = 0;
    private int index2 = 0;
    private float _index2 = 0;

    private const string stringNull = "";
    private const string formato4ceros = "0000";
    private const string formato2ceros = "00";
    private const string _x = "x";

    private void Awake()
    {
        poder.SetActive(false);
        esperaPoder = puntoComida * cadaCuantoPoder;
        tiempoPoderRestante = tiempoPoder;
        txt_puntaje.text = puntuacion.ToString(formato4ceros);
        txt_tiempoPoder.text = stringNull;
        txt_comboPoder.text = stringNull;
        img_poder.enabled = false;
        rand = new System.Random();
    }

    private void OnEnable()
    {
        _snakeHead.OnComida += TocoComida;
        _snakeHead.OnMovimiento += MovimientoSnake;
    }

    private void Start()
    {
        MoverComida();
    }

    public void MovimientoSnake()
    {
        if (hayPoder)
        {
            tiempoPoderRestante -= 1;

            if (tiempoPoderRestante < 0)
            {
                comboPoder = 0;
                txt_comboPoder.text = stringNull;
                ApagarPoder();
            }
            else
            {
                txt_tiempoPoder.text = tiempoPoderRestante.ToString(formato2ceros);
            }
        }
    }

    public void ApagarPoder()
    {
        img_poder.enabled = false;
        txt_tiempoPoder.text = stringNull;
        poder.SetActive(false);
        tiempoPoderRestante = tiempoPoder;
        hayPoder = false;
        esperaPoder += (puntoComida * cadaCuantoPoder) + (puntoComida * comboPoder);
    }

    public void TocoComida(int tipo)
    {
        //Comprobar que hay suficientes espacios libres para mover
        switch (tipo)
        {
            case 1:
                ActualizarPuntuacion();
                MoverComida();
                break;

            case 2:
                MoverPoder();
                break;

            default:
                break;
        }
    }

    public void ActualizarPuntuacion()
    {
        puntuacion += puntoComida;

        if (puntuacion == esperaPoder)
        {
            img_poder.enabled = true;
            poder.SetActive(true);
            poder.transform.localPosition = DamePosicionLibre();
            hayPoder = true;
        }

        txt_puntaje.text = puntuacion.ToString(formato4ceros);
    }

    public void MoverPoder()
    {
        comboPoder += 1;
        txt_comboPoder.text = _x + comboPoder.ToString();
        puntuacion += (puntoComida * puntosMultiplicacionPoder);
        txt_puntaje.text = puntuacion.ToString(formato4ceros);
        ApagarPoder();
        poder.SetActive(false);
    }

    public void MoverComida()
    {
        comida.localPosition = DamePosicionLibre();
    }

    public Vector2 DamePosicionLibre()
    {
        posLibreX.Clear();
        posLibreY.Clear();
        ColasPos.Clear();

        for (int i = 0; i < _snakeHead.Colas.Count; i++)
        {
            valores = new Vector2(Convert.ToSingle(Math.Round(_snakeHead.Colas[i].localPosition.x, 1)), Convert.ToSingle(Math.Round(_snakeHead.Colas[i].localPosition.y, 1)));
            ColasPos.Add(valores);
        }

        limX = limitesComidaSpaw.x * 10;
        for (int i = (int)-limX; i <= (int)limX; i++)
        {
            posLibreX.Add(i);
            for (int p = 0; p < ColasPos.Count; p++)
            {
                valX = ColasPos[p].x * 10;
                if (i == valX)
                {
                    posLibreX.Remove(i);
                }
            }
        }
        limY = limitesComidaSpaw.y * 10;
        for (int i = (int)-limY; i <= (int)limY; i++)
        {
            posLibreY.Add(i);
            for (int p = 0; p < ColasPos.Count; p++)
            {
                valY = ColasPos[p].y * 10;
                if (i == valY)
                {
                    posLibreY.Remove(i);
                }
            }
        }

        index1 = rand.Next(posLibreX.Count);
        _index1 = (float)posLibreX[index1] / 10;
        index2 = rand.Next(posLibreY.Count);
        _index2 = (float)posLibreY[index2] / 10;

        posValida = new Vector2(_index1, _index2);

        return posValida;
    }

    private void OnDisable()
    {
        _snakeHead.OnComida -= TocoComida;
        _snakeHead.OnMovimiento -= MovimientoSnake;
    }
}