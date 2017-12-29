﻿using System;
using System.Collections;
using UnityEngine;

public class NightMode : MonoBehaviour
{
    bool m_nightMode = false;
    DateTime m_dateAndTime;
    SpriteRenderer m_spriteRenderer;

    [SerializeField] int m_hour , m_minute;
    [SerializeField] Sprite m_nightSprite;

    void Reset()
    {
        m_hour = 18;
        m_minute = 00;
    }

    void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        m_dateAndTime = DateTime.Now;

        if(m_dateAndTime.Hour >= m_hour && m_dateAndTime.Minute >= m_minute && !m_nightMode)
        {
            m_spriteRenderer.sprite = m_nightSprite;
            m_nightMode = true;   
        }
    }
}
