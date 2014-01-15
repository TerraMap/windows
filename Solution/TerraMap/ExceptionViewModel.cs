using System;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Windows;

namespace TerraMap
{
	public class ExceptionViewModel : INotifyPropertyChanged
	{
		private string info;

		private Exception exception;
		private string message;
		private string heading;

		public static ExceptionViewModel Create(Exception exception, string heading = "Exception Encountered")
		{
			AggregateException aggregateException = exception as AggregateException;
			if (aggregateException != null)
			{
				exception = aggregateException.Flatten().InnerException;
			}

			return new ExceptionViewModel(exception, heading);
		}

		public ExceptionViewModel(Exception exception, string heading = "Exception Encountered")
		{
			info = Assembly.GetEntryAssembly().GetName().Version.ToString();
			this.heading = heading;

			this.exception = exception;
			if (exception != null)
				this.message = exception.Message;
		}

		public bool Failed
		{
			get { return true; }
		}

		public string Heading
		{
			get { return this.heading; }
			set
			{
				this.heading = value;
				this.RaisePropertyChanged("Heading");
			}
		}

		public string Message
		{
			get { return this.message; }
			set
			{
				this.message = value;
				this.RaisePropertyChanged("Message");
			}
		}

		public Exception Exception
		{
			get { return this.exception; }
			set
			{
				this.exception = value;
			}
		}

		public Visibility InfoVisibility
		{
			get
			{
				return string.IsNullOrEmpty(this.Info) ? Visibility.Collapsed : Visibility.Visible;
			}
		}

		public string Info
		{
			get { return info; }
			set
			{
				info = value;
				this.RaisePropertyChanged("Info");
				this.RaisePropertyChanged("InfoVisibility");
			}
		}

		private Visibility detailsVisibility = Visibility.Visible;

		public Visibility DetailsVisibility
		{
			get { return detailsVisibility; }
			set
			{
				detailsVisibility = value;
				RaisePropertyChanged("DetailsVisibility");
			}
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			if (exception != null)
			{
				sb.AppendLine(exception.ToString());

				if (!string.IsNullOrEmpty(info) || !string.IsNullOrEmpty(info))
					sb.AppendLine();
			}

			if (!string.IsNullOrEmpty(info))
				sb.AppendLine("Version: " + info);

			return sb.ToString();
		}

		protected virtual void RaisePropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}