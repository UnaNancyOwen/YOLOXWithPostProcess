using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Sentis;
using HoloLab.DNN.ObjectDetection;
using System.Text.RegularExpressions;
using System.IO;

namespace Sample
{
    public class ObjectDetection : MonoBehaviour
    {
        [SerializeField] private RawImage image = null;
        [SerializeField] private ModelAsset weights = null;
        [SerializeField] private TextAsset names = null;
        [SerializeField, Range(0.0f, 1.0f)] private float score_threshold = 0.6f;

        private WebCamera webcamera = null;
        private ObjectDetectionModel_YOLOXWithPostProcess model;
        private List<Color> colors;
        private List<string> labels;

        private void Start()
        {
            // Create Object Detection Model
            model = new ObjectDetectionModel_YOLOXWithPostProcess(weights);

            // Read Label List from Text Asset
            labels = new List<string>(Regex.Split(names.text, "\r\n|\r|\n"));

            // Create Colors for Visualize
            colors = Visualizer.GenerateRandomColors(labels.Count);

            // Find Web Camera Component
            webcamera = FindObjectOfType<WebCamera>();
        }

        private void Update()
        {
            // Get Texture from Web Camera
            var input_texture = webcamera.GetTexture();
            if (input_texture == null)
            {
                return;
            }

            // Detect Objects
            var objects = model.Detect(input_texture, score_threshold);

            // Draw Objects on Unity UI
            Visualizer.ClearBoundingBoxes(image);
            objects.ForEach(o => Visualizer.DrawBoudingBox(image, o.rect, colors[o.class_id]));
        }

        private void OnDestroy()
        {
            model?.Dispose();
            model = null;
        }
    }
}
