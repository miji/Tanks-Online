using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDFPS : MonoBehaviour
{
    public float updateInterval = 0.5F;

    private float accum = 0; // FPS accumulated over the interval
    private int frames = 0; // Frames drawn over the interval
    private float timeleft; // Left time for current interval

    public Vector2 benchmarkPeriod = new Vector2(2.0f, 12.0f);
    private float accumBench = 0;
    private int framesBench = 0;

    private Text textComp;

    private void Start()
    {

        textComp = GetComponent<Text>();
        if (!textComp)
        {
            Debug.Log("UtilityFramesPerSecond needs a GUIText component!");
            enabled = false;
            return;
        }
        timeleft = updateInterval;
    }

    private void Update()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        // Interval ended - update GUI text and start new interval
        if (timeleft <= 0.0)
        {
            // display two fractional digits (f2 format)
            float fps = accum / frames;
            string format = System.String.Format("{0:F2} FPS", fps);
            textComp.text = format;

            if (fps < 30)
                textComp.material.color = Color.yellow;
            else
                if (fps < 10)
                    textComp.material.color = Color.red;
                else
                    textComp.material.color = Color.green;
            //	DebugConsole.Log(format,level);
            timeleft = updateInterval;
            accum = 0.0F;
            frames = 0;

            textComp.text += " Benchmark: " + accumBench / framesBench;
        }

        if (Time.realtimeSinceStartup >= benchmarkPeriod.x && Time.realtimeSinceStartup <= benchmarkPeriod.y)
        {
            accumBench += Time.timeScale / Time.deltaTime;
            ++framesBench;
        }
    }
}