using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Sample
{
    public class WebCamera : MonoBehaviour
    {
        [SerializeField] private RawImage image = null;
        private WebCamTexture webcam_texture = null;
        private Texture2D texture = null;
        private RenderTexture render_texture = null;

        private IEnumerator Start()
        {
            yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
            if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
            {
                Debug.LogError("error : not authorized access to camera devices.");
                yield break;
            }

            webcam_texture = new WebCamTexture();
            image.texture = webcam_texture;
            webcam_texture.Play();
        }

        private void Update()
        {
#if UNITY_IOS
            image.uvRect = new Rect(0f, 1f, 1f, -1f);
#endif
        }

        public Texture2D GetTexture()
        {
            if (webcam_texture == null || !webcam_texture.isPlaying || webcam_texture.width < 100)
            {
                return null;
            }

            if (texture == null)
            {
                texture = new Texture2D(webcam_texture.width, webcam_texture.height, TextureFormat.RGBA32, false);
            }
            if (render_texture == null)
            {
                render_texture = RenderTexture.GetTemporary(webcam_texture.width, webcam_texture.height, 0, RenderTextureFormat.ARGB32);
            }

            var previous_texture = RenderTexture.active;
            RenderTexture.active = render_texture;

#if !UNITY_IOS
            Graphics.Blit(webcam_texture, render_texture);
#else
            Graphics.Blit(webcam_texture, render_texture, new Vector2(1, -1), new Vector2(0, 1));
#endif

            texture.ReadPixels(new Rect(0, 0, render_texture.width, render_texture.height), 0, 0);
            texture.Apply();

            RenderTexture.active = previous_texture;

            return texture;
        }

        private void OnDestroy()
        {
            if (webcam_texture != null && webcam_texture.isPlaying)
            {
                webcam_texture.Stop();
                webcam_texture = null;
            }

            if (texture != null)
            {
                Destroy(texture);
                texture = null;
            }

            if (render_texture != null)
            {
                RenderTexture.ReleaseTemporary(render_texture);
                render_texture = null;
            }
        }
    }
}
