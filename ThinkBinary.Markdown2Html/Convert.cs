using System.IO;
using System.Reflection;
using MarkdownSharp;
using RazorEngine;

namespace ThinkBinary.Markdown2Html
{
	internal class Convert
	{
		private const string TemplateFile = "ThinkBinary.Markdown2Html.Content.template.html";
		private const string CssFile = "ThinkBinary.Markdown2Html.Content.normalize.css";
		private readonly Assembly _assembly = Assembly.GetExecutingAssembly();
		private readonly ConversionSettings _settings;
		private readonly Markdown _markdown;

		internal Convert(ConversionSettings settings)
		{
			_settings = settings;
			_markdown = new Markdown();
		}

		internal void Execute()
		{
			var markdownText = File.ReadAllText(_settings.InputFile);
			var htmlText = _markdown.Transform(markdownText);
			var customCss = @".container { margin: 0 auto; min-width: 500px;max-width: 950px; }";
			if (_settings.CustomCssFile != null)
			{
				customCss = File.ReadAllText(_settings.CustomCssFile);
			}

			// ReSharper disable AssignNullToNotNullAttribute
			var template = new StreamReader( _assembly.GetManifestResourceStream(TemplateFile)).ReadToEnd();
			var normalizeCss = new StreamReader(_assembly.GetManifestResourceStream(CssFile)).ReadToEnd();
			// ReSharper restore AssignNullToNotNullAttribute
			var renderedHtml = Razor.Parse(template, new
				                                         {
					                                         Title = _settings.Title ?? "",
					                                         Text = htmlText,
					                                         Css = _settings.AddNormalizeCss
						                                               ? normalizeCss + customCss
						                                               : customCss,
				                                         });
			// save to file
			File.WriteAllText(_settings.OutputFile, renderedHtml);
		}
	}
}
