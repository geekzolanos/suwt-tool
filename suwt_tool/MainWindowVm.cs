/* 
 * Copyright © 2017 Geekzolanos
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 */

using System;
using System.ComponentModel;
using suwt.Services;

namespace suwt
{
    class MainWindowVm : INotifyPropertyChanged
    {
        private MicService _micService;
        private VolumeService _volService;

        public MainWindowVm()
        {
            _micService = MicService.GetInstance();
            _micService.AmplitudeChanged += OnAmplitudeChanged;
            _micService.MicUp += OnMicUp;
            _micService.MicDown += OnMicDown;

            _volService = VolumeService.GetInstance();
        }

        public void StartService()
        {            
            _volService.FadeVolumeUp();
            _micService.StartListener();
            Status = true;
        }

        public void StopService()
        {
            _volService.FadeVolumeDown();
            _micService.StopListener();
            Status = false;
        }

        private void OnAmplitudeChanged(object sender, EventArgs e)
        {
            Amplitude = ((AmplitudeChangedEventArgs)e).Amplitude;
        }

        private void OnMicUp(object sender, EventArgs e)
        {
            _volService.FadeVolumeDown();
            VolStatus = "Bajo";
        }

        private void OnMicDown(object sender, EventArgs e)
        {
            _volService.FadeVolumeUp();
            VolStatus = "Alto";
        }

        private bool _status;
        public bool Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }

        private double _amplitude;
        public double Amplitude
        {
            get
            {
                return _amplitude;
            }
            set
            {
                _amplitude = value;
                OnPropertyChanged("Amplitude");
            }
        }

        private string _volstatus;
        public string VolStatus
        {
            get
            {
                return _volstatus;
            }
            set
            {
                _volstatus = value;
                OnPropertyChanged("VolStatus");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
