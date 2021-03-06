using CliWrap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Andtech.DJ
{

	internal class AudioPlayerProcess
	{
		public string WorkingDirectory { get; set; } = Environment.CurrentDirectory;

		private readonly string command;

		public AudioPlayerProcess(string command)
		{
			this.command = command;
		}

		public void Play(AudioFile audioFile)
		{
			Utility.RunInDirectory(WorkingDirectory, Play);

			void Play()
			{
				var tokens = Utility.SplitCommand(command);
				var executable = tokens.First();

				var filePath = Path.GetRelativePath(Environment.CurrentDirectory, audioFile.Path);

				Environment.CurrentDirectory = WorkingDirectory;
				var arguments = new List<string>(tokens.Skip(1))
				{
					$"\"{filePath}\"",
				};

				Log.WriteLine($"{executable} {string.Join(" ", arguments)}", Verbosity.verbose);

				_ = Cli.Wrap(executable)
					.WithWorkingDirectory(WorkingDirectory)
					.WithArguments(arguments)
					.ExecuteAsync();
			}
		}
	}
}
