﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using TokFM.Model;

namespace TokFM.ViewModel
{
	public class MainViewModel : ViewModelBase
	{
		public RelayCommand<Podcast> PlayPodcastCommand { get; private set; }
		public RelayCommand<Podcast> StopPodcastCommand { get; private set; }
		public RelayCommand<string> LoadPodcastsCommand { get; private set; }

		private ObservableCollection<Podcast> _podcasts;
		private Podcast _currentPodcast;
		private readonly DispatcherTimer _dispatcherTimer;
		private bool _changingPosition;
		private string _activePage;

		public ObservableCollection<Podcast> Podcasts
		{
			get { return _podcasts; }
			set { _podcasts = value; RaisePropertyChanged("Podcasts"); }
		}

		public MediaPlayer MediaPlayer { get; set; }
		public string ActivePage
		{
			get { return _activePage; }
			set { _activePage = value; RaisePropertyChanged("ActivePage"); }
		}

		/// <summary>
		/// Initializes a new instance of the MainViewModel class.
		/// </summary>
		public MainViewModel()
		{
			_dispatcherTimer = new DispatcherTimer();
			_dispatcherTimer.Tick += DispatcherTimerOnTick;
			_dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
			_dispatcherTimer.Start();

			MediaPlayer = new MediaPlayer();
			MediaPlayer.MediaOpened += MediaPlayerOnMediaOpened;
		
			PlayPodcastCommand = new RelayCommand<Podcast>(PlayPodcast);
			LoadPodcastsCommand = new RelayCommand<string>(LoadPodcasts);
			StopPodcastCommand = new RelayCommand<Podcast>(StopPodcast);
		}

		private void StopPodcast(Podcast obj)
		{
			MediaPlayer.Pause();
			_currentPodcast.IsBeingPlayed = false;
		}

		private void DispatcherTimerOnTick(object sender, EventArgs eventArgs)
		{
			_changingPosition = true;
			try
			{
				if (_currentPodcast != null)
				{
					_currentPodcast.Position = MediaPlayer.Position;
				}
			}
			finally
			{
				_changingPosition = false;
			}
		}

		private void MediaPlayerOnMediaOpened(object sender, EventArgs eventArgs)
		{
			_currentPodcast.Length = Convert.ToInt32(Math.Round(MediaPlayer.NaturalDuration.TimeSpan.TotalSeconds));
		}

		private void PlayPodcast(Podcast podcast)
		{
			if (_currentPodcast == podcast)
			{
				MediaPlayer.Play();
				_currentPodcast.IsBeingPlayed = true;
			}
			else
			{
				MediaPlayer.Stop();
				_currentPodcast = podcast;
				foreach (var pod in Podcasts)
				{
					pod.IsBeingPlayed = false;
				}
				_currentPodcast.IsBeingPlayed = true;
				_currentPodcast.PropertyChanged += CurrentPodcastOnPropertyChanged;
				MediaPlayer.Open(new Uri(podcast.Href));
				MediaPlayer.Play();
			}
		}

		private void LoadPodcasts(string page)
		{
			ActivePage = page;
			if (Podcasts != null)
				Podcasts.Clear();
			System.Threading.ThreadPool.QueueUserWorkItem(CallBack, null);
		}

		private void CallBack(object state)
		{
			var service = new Business.PodcastService();
			Podcasts = new ObservableCollection<Podcast>(service.GetLatestPodcasts(Convert.ToInt32(ActivePage)));
		}

		private void CurrentPodcastOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			if (!_changingPosition)
			{
				if (propertyChangedEventArgs.PropertyName == "Position")
				{
					MediaPlayer.Position = _currentPodcast.Position;
				}
			}
		}
	}
}