using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.game_object
{
    public class MusicObject
    {
        private string _currentSong = "";
        private AudioStreamPlayer _streamPlayer;
        private Dictionary<string, float> _lastPlayedPosition;
        private string _overworldTheme;
        private bool _isMusicCustom = false;
        public bool IsMusicCustom { get => _isMusicCustom; set => _isMusicCustom = value; }

        /// <summary>
        /// A key to the overworld theme song.
        /// </summary>
        public string OverworldThemeCustom
        {
            get => _overworldTheme;
            set => _overworldTheme = value;
        }

        
        /// <summary>
        /// Returns a track path to the overworld theme.
        /// </summary>
        public string OverworldTheme
        {
            get => MusicAssets.GetOverworldTrackNormal();
        }

        public MusicObject()
        {
            if (string.IsNullOrEmpty(_overworldTheme))
                _overworldTheme = MusicAssets.GetOverworldTracks(10, false)[0];

            _streamPlayer = new AudioStreamPlayer();
            _lastPlayedPosition = new Dictionary<string, float>();
        }

        /// <summary>
        /// Change the current stream player with a new one.
        /// This will stop the current music playing so we can host new music from
        /// the new Stream Player. This will usually be needed when scene hopping.
        /// </summary>
        /// <param name="streamPlayer"></param>
        public void SetStreamPlayer(AudioStreamPlayer streamPlayer)
        {
            // stop the ongoing stream player if it's playing
            try
            {
                if (_streamPlayer.Playing)
                    SavePlaybackPosition();
            }
            catch (System.ObjectDisposedException) 
            { 
                // This bug is possible when loading a new game after quitting from inside the menu.
                // This catch will stop that from happening.
            }
            finally
            {
                _streamPlayer = streamPlayer;
            }
        }

        public void ResetCurrentSong()
        {
            _currentSong = "";
        }

        public void ResetAllTracksAfterBoss()
        {
            _lastPlayedPosition.Clear();
        }

        public void PlayMusic(string music, float position = 0)
        {
            if (music != _currentSong)
            {
                if (_streamPlayer.Playing)
                    SavePlaybackPosition();

                _currentSong = music;
                _streamPlayer.Stream = ResourceLoader.Load<AudioStream>(music);

                _streamPlayer.Play(position);
            }
        }

        private void SavePlaybackPosition()
        {
            float playbackPosition = _streamPlayer.GetPlaybackPosition();
            _streamPlayer.Stop();

            // save our playback position to resume later if need be
            if (_lastPlayedPosition.ContainsKey(_currentSong))
                _lastPlayedPosition[_currentSong] = playbackPosition;
            else
                _lastPlayedPosition.Add(_currentSong, playbackPosition);

        }
    }
}
