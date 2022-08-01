using System.Collections.Generic;
using UnityEngine;

public class ParallaxScrolling : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float smoothing = 10f;
    [Space]
    [SerializeField] private List<ParallaxLayer> parallaxLayers;

    private Vector3 previousCameraPosition;

    private void Start() => previousCameraPosition = mainCamera.transform.position;

    private void Update()
    {
        foreach (var layer in parallaxLayers)
        {
            Vector3 parallax = (previousCameraPosition - mainCamera.transform.position) * (layer.paralaxOffset / smoothing);

            var positionX = layer.background.position.x + parallax.x;
            var positionY = layer.background.position.y;
            var positionZ = layer.background.position.z;
            layer.background.position = new Vector3(positionX, positionY, positionZ);
        }

        previousCameraPosition = mainCamera.transform.position;
    }

    [System.Serializable]
    public struct ParallaxLayer
    {
        public string layoutName;
        public Transform background;
        public float paralaxOffset;
    }
}