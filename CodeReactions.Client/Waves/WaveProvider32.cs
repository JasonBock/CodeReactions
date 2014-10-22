using NAudio.Wave;

namespace CodeReactions.Client.Waves
{
	// Lifted from:
	// http://mark-dot-net.blogspot.com/2009/10/playback-of-sine-wave-in-naudio.html
	public abstract class WaveProvider32
		: IWaveProvider
	{
		private WaveFormat waveFormat;

		public WaveProvider32()
			: this(44100, 1) { }

		public WaveProvider32(int sampleRate, int channels)
		{
			this.SetWaveFormat(sampleRate, channels);
		}

		public void SetWaveFormat(int sampleRate, int channels)
		{
			this.waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channels);
		}

		public int Read(byte[] buffer, int offset, int count)
		{
			var waveBuffer = new WaveBuffer(buffer);
			int samplesRequired = count / 4;
			int samplesRead = this.Read(waveBuffer.FloatBuffer, offset / 4, samplesRequired);
			return samplesRead * 4;
		}

		public abstract int Read(float[] buffer, int offset, int sampleCount);

		public WaveFormat WaveFormat
		{
			get { return this.waveFormat; }
		}
	}
}
