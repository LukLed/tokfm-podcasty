using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TokFM.Model
{
	public class Podcast : Notifier
	{
		private string _href;
		public string Href
		{
			get { return _href; }
			set { _href = value; RaisePropertyChanged("Href"); }
		}

		private string _title;
		public string Title
		{
			get { return _title; }
			set { _title = value; RaisePropertyChanged("Title"); }
		}

		private string _imageURL;
		private int _length;
		private TimeSpan _position;
		private bool _isBeingPlayed;

		public string ImageURL
		{
			get { return _imageURL; }
			set { _imageURL = value; RaisePropertyChanged("ImageURL"); }
		}

		public int Length
		{
			get { return _length; }
			set { _length = value; RaisePropertyChanged("Length"); }
		}

		public TimeSpan Position
		{
			get { return _position; }
			set { _position = value; RaisePropertyChanged("Position"); RaisePropertyChanged("PositionSeconds"); }
		}

		public int PositionSeconds
		{
			get { return Convert.ToInt32(Math.Round(_position.TotalSeconds)); }
			set
			{
				Position = new TimeSpan(0,0,0,value);
			}
		}

		public bool IsBeingPlayed
		{
			get { return _isBeingPlayed; }
			set { _isBeingPlayed = value; RaisePropertyChanged("IsBeingPlayed"); RaisePropertyChanged("IsNotBeingPlayed"); }
		}

		public bool IsNotBeingPlayed
		{
			get { return !IsBeingPlayed; }
		}
	}
}
