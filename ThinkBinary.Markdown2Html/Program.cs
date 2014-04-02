using System;
using Mono.Options;

namespace ThinkBinary.Markdown2Html
{
	class Program
	{
		private static readonly ProgramSettings Settings = new ProgramSettings();

		static int Main(string[] args)
		{
			var options = ProgramSettings();

			try
			{
				options.Parse(args);
			}
			catch (OptionException oe)
			{
				Console.Write("md2html: ");
				Console.WriteLine(oe.Message);
				Console.WriteLine("Try `md2html --help' for more information.");
				return ReturnCodes.IncorrectSettings;
			}

			if (Settings.Help)
			{
				ShowHelp(options);
				return ReturnCodes.Normal;
			}

			if (MinimumSettingsSpecified())
			{
				try
				{
					var convert = new Convert(Settings);
					convert.Execute();
					return ReturnCodes.Normal;
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
					return ReturnCodes.ApplicationFailure;
				}
			}
			
			Console.WriteLine("Not all minimum settings were specified\n");
			ShowHelp(options);
			return ReturnCodes.IncorrectSettings;
		}

		private static bool MinimumSettingsSpecified()
		{
			return !string.IsNullOrWhiteSpace(Settings.InputFile)
				   && !string.IsNullOrWhiteSpace(Settings.OutputFile);
		}

		private static void ShowHelp(OptionSet p)
		{
			Console.WriteLine("Usage: md2html [OPTIONS]");
			Console.WriteLine("Render markdown document to HTML.");
			Console.WriteLine();
			Console.WriteLine("Options:");
			p.WriteOptionDescriptions(Console.Out);
		}

		private static OptionSet ProgramSettings()
		{
			return new OptionSet
				       {
					       {"h|?|help", "Display help information", h => Settings.Help = h != null},
					       {"t|title=", "Title of output HTML document", t => Settings.Title = t},
					       {"n|normalize", "Embed Normalize.css to output HTML document", n => Settings.AddNormalizeCss = n != null},
					       {"i|input=", "Input file name", i => Settings.InputFile = i},
					       {"o|output=", "Output file name", o => Settings.OutputFile = o},
						   {"css=", "Custom css file to embed", css => Settings.CustomCssFile = css}
				       };
		}
	}
}
