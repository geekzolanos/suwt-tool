using System;
using System.Timers;
using System.Windows;
using AudioSwitcher.AudioApi.CoreAudio;

namespace suwt.Services
{
    class VolumeService
    {
        private const string VOL_TIMER_INTERVAL = "IntervalTimerVol";
        private const string LEVEL_VOL_UP_TAG = "LevelVolUp";
        private const string LEVEL_VOL_DOWN_TAG = "LevelVolDown";
        private const string LEVEL_VOL_STEP_TAG = "LevelVolStep";

        private static VolumeService _instance;

        private Double _levelVolUp;
        private Double _levelVolDown;
        private Double _levelVolStep;
        private Double _timerInterval;

        private CoreAudioDevice _defaultDevice;
        private bool _volumeLock;        
        private Timer _timerVolUp;
        private Timer _timerVolDown;

        public static VolumeService GetInstance()
        {
            if (_instance == null)
            {
                _instance = new VolumeService();
            }

            return _instance;
        }

        private VolumeService()
        {
            _setPreferences();

            _defaultDevice = new CoreAudioController().DefaultPlaybackDevice;

            _timerVolUp = new Timer(_timerInterval);
            _timerVolDown = new Timer(_timerInterval);
            _timerVolUp.Elapsed += _timerVolUp_Elapsed;
            _timerVolDown.Elapsed += _timerVolDown_Elapsed;
        }

        private void _setPreferences()
        {
            _levelVolUp = Double.Parse(Application.Current.Resources[LEVEL_VOL_UP_TAG].ToString());
            _levelVolDown = Double.Parse(Application.Current.Resources[LEVEL_VOL_DOWN_TAG].ToString());
            _levelVolStep = Double.Parse(Application.Current.Resources[LEVEL_VOL_STEP_TAG].ToString());
            _timerInterval = Double.Parse(Application.Current.Resources[VOL_TIMER_INTERVAL].ToString());
        }

        public void FadeVolumeUp()
        {
            if (!_volumeLock)
            {
                SetVolumeLock(true);
                _timerVolUp.Start();
            }
        }

        public void FadeVolumeDown()
        {
            if(!_volumeLock)
            {
                SetVolumeLock(true);
                _timerVolDown.Start();
            }
        }    
        
        private void SetVolumeLock(bool val)
        {
            _volumeLock = val;
        }

        private void _timerVolUp_Elapsed(object sender, ElapsedEventArgs e)
        {
            double newVal = _defaultDevice.Volume + _levelVolStep;
            if (newVal > _levelVolUp)
            {
                SetVolumeLock(false);
                _timerVolUp.Stop();
            }
            else
            {
                _defaultDevice.Volume++;
            }            
        }

        private void _timerVolDown_Elapsed(object sender, ElapsedEventArgs e)
        {
            double newVal = _defaultDevice.Volume - _levelVolStep;
            if (newVal < _levelVolDown)
            {
                SetVolumeLock(false);
                _timerVolDown.Stop();
            }
            else
            {
                _defaultDevice.Volume--;
            }
        }
    }
}
