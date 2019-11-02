using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCircle : MonoBehaviour {

    public ParticleSystem myparticleSystem;
    private ParticleSystem.Particle[] particleArray;
    private SingleParticle[] points;
    public Gradient grad;
    int count = 1000;
    public float size = 0.5f;
    public float minRadius = 3.0f;
    public float maxRadius = 6.0f;
    public bool rotate_way = false; // 决定圈扩大还是缩小
    private float rotate_speed = -1; // 颜色旋转速度（正负代表方向）
    public float speed = 0.5f; // 速度参数
    private float time = 0;

    private void Init() {
        int i;
        for (i = 0; i < count; i++) {
            float midRadius = (minRadius + maxRadius) / 2.0f;
            float minRate = Random.Range(1.0f, midRadius / minRadius);
            float maxRate = Random.Range(midRadius / maxRadius, 1.0f);
            float radius = Random.Range(minRadius * minRate, maxRadius * maxRate);
            float angle = Random.Range(0.0f, 360.0f);
            points[i] = new SingleParticle(angle, radius);
            points[i].CalPosition();
            particleArray[i].position = new Vector3(points[i].getX(), points[i].getY(), 0f);
        }

        myparticleSystem.SetParticles(particleArray, particleArray.Length);
    }
    // Use this for initialization
    void Start () {
        // myparticleSystem = this.GetComponent<ParticleSystem>();
        particleArray = new ParticleSystem.Particle[count];
        points = new SingleParticle[count];
        var m = myparticleSystem.main;
        m.startSpeed = 0;
        m.startSize = size;
        m.maxParticles = count;
        myparticleSystem.Emit(count);
        myparticleSystem.GetParticles(particleArray);

        Init();
    }

    // Update is called once per frame
    void Update () {
        time += Time.deltaTime;
        if (time < 10) {
            if (time < 5) {
                rotate_way = false;
                rotate_speed += 0.01f;
            }
            else {
                rotate_way = true;
                rotate_speed -= 0.01f;
            }
            
        } else {
            time = 0;
            rotate_speed = -1;
        }
        int i;
        int level = 10;
        for (i = 0; i < count; i++) {

            if (i % level < 3 || i % level > 6)
            {
                points[i].angle -= rotate_speed * (i % level + 1) * speed;
            } else {
                points[i].angle += rotate_speed * (i % level + 1) * speed;
            }

            points[i].angle = (points[i].angle + 360.0f) % 360.0f;
            
            if (i % level > 5) {
                float tmp = rotate_way? 1 : -1;
                    points[i].radius += tmp * 0.05f;
            }
            if (i % level <= 5) {
                float tmp = rotate_way? 1 : -1;
                points[i].radius += tmp * 0.052f;
            }
            points[i].CalPosition();

            float value = Time.realtimeSinceStartup % 1.0f;
            value -= rotate_speed * points [i].angle /360.0f;
            while (value > 1)
                value--;
            while (value < 0)
                value ++;
            particleArray[i].startColor = grad.Evaluate(value);
            particleArray[i].position = new Vector3(points[i].getX(), points[i].getY(), 0.0f);
        }
        myparticleSystem.SetParticles(particleArray, particleArray.Length);
    }
}

