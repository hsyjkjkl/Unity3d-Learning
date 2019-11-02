using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SingleParticle {
    public float angle;
    public float radius;

    private float x = 0.0f;
    private float y = 0.0f;


    public void CalPosition() {
        float temp = angle / 180.0f * Mathf.PI;
        y = radius * Mathf.Sin(temp);
        x = radius * Mathf.Cos(temp);
    }
    public SingleParticle(float angle, float radius) {
        this.angle = angle;
        this.radius = radius;
    }

    public float getX() {
        return x;
    }

    public float getY() {
        return y;
    }
}