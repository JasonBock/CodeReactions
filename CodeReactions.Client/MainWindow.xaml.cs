using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CodeReactions.Client.Extensions;
using CodeReactions.Client.Waves;
using CodeReactions.Messages;
using Microsoft.AspNet.SignalR.Client;
using NAudio.Wave;

namespace CodeReactions.Client
{
	public partial class MainWindow
		: Window
	{
		private static readonly char[] pianoKeys = new[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G' };
		private static readonly ReadOnlyDictionary<char, float> frequencies = new ReadOnlyDictionary<char, float>(
			new Dictionary<char, float>
			{
				{ 'A', 440f },
				{ 'B', 493.88f },
				{ 'C', 523.25f },
				{ 'D', 587.33f },
				{ 'E', 659.25f },
				{ 'F', 698.46f },
				{ 'G', 783.99f },
			});

		private HubConnection connection;
		private IHubProxy proxy;
		private IDisposable audioSubscription;
		private IDisposable autoSaveSubscription;
		private IDisposable resultsSubscription;
		private IDisposable onStartSubscription;
		private IDisposable onStopSubscription;

		public MainWindow()
			: base()
		{
			this.InitializeComponent();
			this.StartListening.IsEnabled = true;
			this.StopListening.IsEnabled = false;
			this.Results.BorderBrush = Brushes.Gray;

			this.connection = new HubConnection("http://localhost:22503");
			this.proxy = this.connection.CreateHubProxy("KeyloggerHub");

			this.onStartSubscription = Observable.FromEventPattern<RoutedEventArgs>(
				this.StartListening, nameof(Button.Click))
				.Subscribe(e => this.OnStartListeningClick(e.EventArgs));
			this.onStopSubscription = Observable.FromEventPattern<RoutedEventArgs>(
				this.StopListening, nameof(Button.Click))
				.Subscribe(e => this.OnStopListeningClick(e.EventArgs));
			this.autoSaveSubscription = Observable.FromEventPattern<TextChangedEventArgs>(
				this.Results, nameof(TextBox.TextChanged))
				.Throttle(TimeSpan.FromSeconds(2))
				.ObserveOn(SynchronizationContext.Current)
				.Subscribe(e => this.AutoSave());
		}

		private void AutoSave()
		{
			File.Delete("autosave.txt");
			File.WriteAllText("autosave.txt", this.Results.Text);
			this.Results.BorderBrush = Brushes.Gray;
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			this.audioSubscription.SafeDispose();
			this.resultsSubscription.SafeDispose();
			this.onStartSubscription.SafeDispose();
			this.onStartSubscription.SafeDispose();
			this.autoSaveSubscription.SafeDispose();
			base.OnClosing(e);
		}

		private async void OnStartListeningClick(RoutedEventArgs e)
		{
			this.StartListening.IsEnabled = false;
			this.StopListening.IsEnabled = true;

			this.resultsSubscription.SafeDispose();
			this.audioSubscription.SafeDispose();

			this.Results.Text = string.Empty;

			var observable = this.proxy.ObserveAs<KeyloggerMessage>("KeysPressed");

			this.resultsSubscription = observable
				.ObserveOn(SynchronizationContext.Current)
				.Subscribe(message =>
				{
					this.Results.Text += new string(message.Keys);
					this.Results.BorderBrush = Brushes.Red;
				});

			this.audioSubscription = observable
				.Select(message => message.Keys.In(pianoKeys).ToArray())
				.Where(foundKeys => foundKeys.Length > 0)
				.Subscribe(foundKeys => this.PlayKeys(foundKeys));

			await this.connection.Start();
		}

		private void OnStopListeningClick(RoutedEventArgs e)
		{
			this.resultsSubscription.SafeDispose();
			this.audioSubscription.SafeDispose();
			this.connection.Stop();

			this.StartListening.IsEnabled = true;
			this.StopListening.IsEnabled = false;
		}

		private void PlayKeys(char[] foundKeys)
		{
			var provider = new SineWaveProvider32();
			provider.SetWaveFormat(16000, 1);

			using (var waveOut = new WaveOut())
			{
				waveOut.Init(provider);

				foreach (var foundKey in foundKeys)
				{
					provider.Frequency = MainWindow.frequencies[foundKey];
					waveOut.Play();
					Thread.Sleep(250);
				}

				waveOut.Stop();
			}
		}
	}
}
