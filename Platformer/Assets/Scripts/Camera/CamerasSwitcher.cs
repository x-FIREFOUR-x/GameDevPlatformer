using UnityEngine;

namespace GameCamera
{
    public class CamerasSwitcher : MonoBehaviour
    {
        [SerializeField] private GameLevelInitializer _gameLevelInitializer;

        [SerializeField] private Cinemachine.CinemachineBrain _cameraBrain;
        [SerializeField] private Cinemachine.CinemachineVirtualCamera[] _cameras;

        private int _currentBlend = 0;
        private Cinemachine.CinemachineBlenderSettings.CustomBlend[] _blends;

        private float _time = 0;
        private float _timeChangeCameras;


        private void Start()
        {
            _blends = _cameraBrain.m_CustomBlends.m_CustomBlends;
            SwitchCameras(_blends[_currentBlend]);
        }

        private void Update()
        {
            _time += Time.deltaTime;
            if (_time >= _timeChangeCameras && _currentBlend < _blends.Length)
            {
                SwitchCameras(_blends[_currentBlend]);
            }

            if (_currentBlend == _blends.Length)
            {
                _gameLevelInitializer.StartLevel();
                this.enabled = false;
            }
        }

        private void SwitchCameras(Cinemachine.CinemachineBlenderSettings.CustomBlend blend)
        {
            Cinemachine.CinemachineVirtualCamera currentCamera = FindCamera(blend.m_From);
            currentCamera.Priority = 0;

            Cinemachine.CinemachineVirtualCamera nextCamera = FindCamera(blend.m_To);
            nextCamera.Priority = 1;

            _time = 0;
            _timeChangeCameras = blend.m_Blend.m_Time;
            _currentBlend++;
        }

        private Cinemachine.CinemachineVirtualCamera FindCamera(string nameCamera)
        {
            foreach (var camera in _cameras)
            {
                if (camera.Name == nameCamera)
                {
                    return camera;
                }
            }
            return null;
        }
    }

}
