using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.screens.settings_screen
{
    public class SettingsObject
    {
        public long WindowModeIndex { get; set; }
        public bool FullScreenEnabled { get; set; }
        public long WindowSizeIndex { get; set; }
        public double Volume { get; set; }

        public SettingsObject()
        {
            WindowModeIndex = 0;
            WindowSizeIndex = 0;
            Volume = 0;
            FullScreenEnabled = true;
        }

        public void SetSettings()
        {
            SetWindowMode(WindowModeIndex);
            AudioServer.SetBusVolumeDb(0, (float)Volume);
        }

        public void SetWindowMode(long index)
        {
            WindowModeIndex = index;
            if (index == 0)
            {
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);

                FullScreenEnabled = true;
            }
            else
            {
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
                ProcessWindowSelected(WindowSizeIndex);
                FullScreenEnabled = false;
            }
        }

        public void SetWindowSize(long index)
        {
            WindowSizeIndex = index;
            ProcessWindowSelected(index);
        }

        public void SetVolume(double volume)
        {
            Volume = volume / 5;
            AudioServer.SetBusVolumeDb(0, (float)Volume);
        }

        private void ProcessWindowSelected(long index)
        {
            if (index == 0)
                DisplayServer.WindowSetSize(new Vector2I(1920, 1080));
            else if (index == 1)
                DisplayServer.WindowSetSize(new Vector2I(1600, 900));
            else if (index == 2)
                DisplayServer.WindowSetSize(new Vector2I(1280, 720));
            else
                DisplayServer.WindowSetSize(new Vector2I(640, 360));
        }
    }
}
