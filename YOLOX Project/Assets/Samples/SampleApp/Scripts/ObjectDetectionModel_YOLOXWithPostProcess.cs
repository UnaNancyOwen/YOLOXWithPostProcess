using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Unity.Sentis;
using HoloLab.DNN.Base;

namespace HoloLab.DNN.ObjectDetection
{
    /// <summary>
    /// object detection model class for yolox with post process
    /// </summary>
    public class ObjectDetectionModel_YOLOXWithPostProcess : BaseModel, IDisposable
    {
        private int input_width = 0;
        private int input_height = 0;

        /// <summary>
        /// create object detection model for yolox with post process from onnx file
        /// </summary>
        /// <param name="file_path">model file path</param>
        /// <param name="backend_type">backend type for inference engine</param>
        public ObjectDetectionModel_YOLOXWithPostProcess(string file_path, BackendType backend_type = BackendType.GPUCompute)
            : base(file_path, backend_type)
        {
            Initialize();
        }

        /// <summary>
        /// create object detection model for yolox with post process from model asset
        /// </summary>
        /// <param name="model_asset">model asset</param>
        /// <param name="backend_type">backend type for inference engine</param>
        public ObjectDetectionModel_YOLOXWithPostProcess(ModelAsset model_asset, BackendType backend_type = BackendType.GPUCompute)
            : base(model_asset, backend_type)
        {
            Initialize();
        }

        /// <summary>
        /// dispose object detection model
        /// </summary>
        public new void Dispose()
        {
            base.Dispose();
        }

        /// <summary>
        /// detect objects
        /// </summary>
        /// <param name="image">input image</param>
        /// <param name="score_threshold">confidence score threshold</param>
        /// <returns>detected object list</returns>
        public List<HoloLab.DNN.ObjectDetection.Object> Detect(Texture2D image, float score_threshold = 0.8f)
        {
            var output_tensors = Predict(image);

            var output_name = runtime_model.outputs[0].name;
            var output_tensor = output_tensors[output_name] as TensorFloat;

            output_tensor.CompleteOperationsAndDownload();
            var output_span = output_tensor.ToReadOnlySpan();

            var objects = new List<HoloLab.DNN.ObjectDetection.Object>();
            for (var i = 0; i < output_tensor.shape[0]; i++)
            {
                // class_id, score, x_min, y_min, x_max, y_max
                var span = output_span.Slice(i * output_tensor.shape[1], output_tensor.shape[1]);

                var class_id = (int)span[1];

                var score = span[2];
                if (score < score_threshold)
                {
                    continue;
                }

                var x_min = (int)(Math.Max(0, span[3]) * image.width / input_width);
                var y_min = (int)(Math.Max(0, span[4]) * image.height / input_height);
                var x_max = (int)(Math.Min(span[5], input_width) * image.width / input_width);
                var y_max = (int)(Math.Min(span[6], input_height) * image.height / input_height);
                var rect = new Rect(x_min, y_min, x_max - x_min, y_max - y_min);

                objects.Add(new Object(rect, class_id, score));
            }

            output_tensors.AllDispose();

            return objects;
        }

        private void Initialize()
        {
            SetInputMax(255.0f);

            var input_shape = GetInputShapes().First().Value;
            input_width = input_shape[3];
            input_height = input_shape[2];
        }
    }
}
