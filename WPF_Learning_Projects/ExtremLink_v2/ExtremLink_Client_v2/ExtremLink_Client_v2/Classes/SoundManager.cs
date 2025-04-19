using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NAudio.Wave;
using System.IO;
using OpenCvSharp.Internal.Vectors;

namespace ExtremLink_Client_v2.Classes
{

    // An enum which contains the name of the sounds
    public enum EPlaylist
    {
        Entrance,
        Background,
        Controlling,
        ButtonClick,
        InsturctionsReaderAI
    }

    public class SoundManager
    {

        /*
        A class which manage the sounds of the program.
        */

        // Attributes:

        // A WaveOutEvent which handle the playlist with low latency. 
        private WaveOutEvent outputDevice;
        
        // An AudioFileReader which reads the sounds files.
        private AudioFileReader audioFile;


        // Singelton Behavior
        private static SoundManager soundManagerInstance;
        public static SoundManager SoundManagerInstance
        {
            get
            {
                if (soundManagerInstance == null)
                {
                    soundManagerInstance = new SoundManager();
                }
                return soundManagerInstance;
            }
        }

        // Constructor:
        private SoundManager()
        {
            
        }


        private string GetSongPath(EPlaylist song)
        {
            // A function which converts the sound Enum name to it's actual file path.
            string projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
            return song switch
            {
                EPlaylist.Entrance => Path.Combine(projectRoot, "Sounds", "EntranceMusic.mp3"),
                EPlaylist.Background => Path.Combine(projectRoot, "Sounds", "BackgroundMusic.mp3"),
                EPlaylist.Controlling => Path.Combine(projectRoot, "Sounds", "ControlMusic.mp3"),
                EPlaylist.ButtonClick => Path.Combine(projectRoot, "Sounds", "ButtonClick.mp3"),
                EPlaylist.InsturctionsReaderAI => Path.Combine(projectRoot, "Sounds", "InstrucationsReader.mp3")
            };
        }
        public void PlaySound(EPlaylist sound)
        {
            // A function which plays a specific sound.
            string currentSoundFilePath = GetSongPath(sound);
            try
            {
                // Stop current playback if something is already playing
                outputDevice?.Stop();
                outputDevice?.Dispose();
                audioFile?.Dispose();

                // Set up new audio
                audioFile = new AudioFileReader(currentSoundFilePath);
                outputDevice = new WaveOutEvent();
                outputDevice.Init(audioFile);
                outputDevice.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
