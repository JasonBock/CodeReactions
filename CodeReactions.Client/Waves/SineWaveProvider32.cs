using System;

namespace CodeReactions.Client.Waves
{
	// Lifted from:
	// http://mark-dot-net.blogspot.com/2009/10/playback-of-sine-wave-in-naudio.html
	public sealed class SineWaveProvider32
		: WaveProvider32
	{
		int sample;

		public SineWaveProvider32()
		{
			this.Frequency = 1000;
			this.Amplitude = 0.25f;           
		}

		public override int Read(float[] buffer, int offset, int sampleCount)
		{
			var sampleRate = WaveFormat.SampleRate;

			for (var n = 0; n < sampleCount; n++)
			{
				buffer[n + offset] = (float)(this.Amplitude * Math.Sin((2 * Math.PI * sample * this.Frequency) / sampleRate));
				sample++;
				if (sample >= sampleRate) sample = 0;
			}
			return sampleCount;
		}

		public float Frequency { get; set; }
		public float Amplitude { get; set; }
	}
}
