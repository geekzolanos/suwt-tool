using System;
using System.Windows;
using NAudio.Wave;

namespace suwt.Services
{
    class MicService
    {
        private const string LEVEL_MIC_UP_TAG = "LevelMicUp";
        private const string LEVEL_MIC_DOWN_TAG = "LevelMicDown";

        private static MicService _instance;

        private WaveIn _recorder;        
        private double _levelMicUp;
        private double _levelMicDown;

        public event EventHandler AmplitudeChanged;
        public event EventHandler MicUp;
        public event EventHandler MicDown;

        public static MicService GetInstance()
        {
            if (_instance == null) {
                _instance = new MicService();
            }

            return _instance;
        }

        private MicService()
        {
            _setPreferences();

            _recorder = new WaveIn();
            _recorder.DataAvailable += new EventHandler<WaveInEventArgs>(DataArrived);
        }

        private void _setPreferences()
        {
            _levelMicUp = Double.Parse(Application.Current.Resources[LEVEL_MIC_UP_TAG].ToString());
            _levelMicDown = Double.Parse(Application.Current.Resources[LEVEL_MIC_DOWN_TAG].ToString());
        }

        public void StartListener()
        {
            _recorder.StartRecording();
        }

        public void StopListener()
        {
            _recorder.StopRecording();
        }

        private void DataArrived(object sender, WaveInEventArgs e)
        {
            double sum = 0;
            double[] _waveLeft = new double[e.Buffer.Length / 4];

            // Split out channels from sample
            for (int i = 0; i < e.Buffer.Length; i += 4)
            {
                double sample = (double)BitConverter.ToInt16(e.Buffer, i) / 32768.0;
                sum += (sample * sample);
            }

            double rms = Math.Sqrt(sum / (_waveLeft.Length / 2));
            double res = 20 * Math.Log10(rms);

            AmplitudeChanged?.Invoke(null, new AmplitudeChangedEventArgs(res));
            InvokeEventsIfReq(res);
        }

        private void InvokeEventsIfReq(double val)
        {
            if(_levelMicDown >= _levelMicUp)
            {
                throw new Exception("LVLDWN_MORE_EQUAL_THAN_LVLUP");
            }

            if(_levelMicUp <= val)
            {
                MicUp?.Invoke(null, null);
            }
            else if(_levelMicDown >= val)
            {
                MicDown?.Invoke(null, null);
            }
        }
    }

    public class AmplitudeChangedEventArgs : EventArgs
    {
        public double Amplitude { get; private set; }

        public AmplitudeChangedEventArgs(double amplitude)
        {
            Amplitude = amplitude;
        }
    }
}
