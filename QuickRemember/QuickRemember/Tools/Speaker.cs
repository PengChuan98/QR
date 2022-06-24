using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace QuickRemember.Tools
{
    public static class Speaker
    {
        static SpeechSynthesizer speaker = new SpeechSynthesizer();
        static Prompt? prompt = null;

        static MediaPlayer player = new MediaPlayer();

        static string path = Path.GetTempPath() + "temp_voice.mp3";

        /// <summary>
        /// 调用Synthesis合成语音
        /// </summary>
        /// <param name="text"></param>
        public static void ReadStringAsync(string text)
        {
            if (prompt != null) speaker.SpeakAsyncCancel(prompt);
            prompt = speaker.SpeakAsync(text);
        }

        /// <summary>
        /// 对实际音频播放
        /// </summary>
        /// <param name="voice"></param>
        public static void ReadBytesAsync(byte[] voice)
        {

            player.Close();

            SafeWriteAllBytes(path, voice);
            player.Open(new Uri(path, UriKind.Relative));
            player.Play();
        }

        static System.IO.Stream? stream = null;
        static System.IO.BinaryWriter? writer = null;

        private static void SafeWriteAllBytes(string path, byte[] voice)
        {
            if (stream != null || writer != null)
            {
                stream?.Close();
                writer?.Close();
            }

            stream = new FileStream(path, FileMode.Create);
            writer = new BinaryWriter(stream);

            writer.Write(voice);
        }
    }
}
