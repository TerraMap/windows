using System.IO;

namespace TerraMap
{
	public class WorldFileViewModel : ViewModelBase
	{
		private FileInfo fileInfo;

		public FileInfo FileInfo
		{
			get { return fileInfo; }
			set
			{
				fileInfo = value;
				RaisePropertyChanged();
			}
		}

		private string name;

		public string Name
		{
			get { return name; }
			set
			{
				name = value;
				RaisePropertyChanged();
			}
		}
	}
}
