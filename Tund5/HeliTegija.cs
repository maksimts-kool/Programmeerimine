using System;
using NAudio.Wave;

namespace Tund5
{
    public class LoopStream : WaveStream
    {
        private readonly WaveStream sourceStream;
        public LoopStream(WaveStream source) { sourceStream = source; }
        public override WaveFormat WaveFormat => sourceStream.WaveFormat;
        public override long Length => long.MaxValue;
        public override long Position
        {
            get => sourceStream.Position;
            set => sourceStream.Position = value;
        }
        public override int Read(byte[] buffer, int offset, int count)
        {
            int read = sourceStream.Read(buffer, offset, count);
            if (read == 0)
            {
                sourceStream.Position = 0;
                read = sourceStream.Read(buffer, offset, count);
            }
            return read;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing) sourceStream.Dispose();
            base.Dispose(disposing);
        }
    }

    class HeliTegija : IDisposable
    {
        private IWavePlayer bgPlayer;
        private LoopStream bgLoop;
        public HeliTegija()
        {
            try
            {
                var bgReader = new AudioFileReader("../../../sounds/bg.wav");
                bgLoop = new LoopStream(bgReader);
                bgPlayer = new WaveOutEvent();
                bgPlayer.Init(bgLoop);
            }
            catch (Exception)
            {
            }
        }

        public void PlayBackground()
        {
            try { bgPlayer?.Play(); }
            catch { }
        }

        public void StopBackground()
        {
            try { bgPlayer?.Stop(); }
            catch { }
        }

        public void PlayEat()
        {
            try
            {
                var reader = new AudioFileReader("../../../sounds/eat.wav");
                var player = new WaveOutEvent();
                player.Init(reader);
                player.PlaybackStopped += (s, e) =>
                {
                    player.Dispose();
                    reader.Dispose();
                };
                player.Play();
            }
            catch { }
        }

        public void PlayGameOver()
        {
            try
            {
                var reader = new AudioFileReader("../../../sounds/gameover.wav");
                var player = new WaveOutEvent();
                player.Init(reader);
                player.PlaybackStopped += (s, e) =>
                {
                    player.Dispose();
                    reader.Dispose();
                };
                player.Play();
            }
            catch { }
        }

        public void Dispose()
        {
            bgPlayer?.Stop();
            bgPlayer?.Dispose();
            bgLoop?.Dispose();
        }
    }
}