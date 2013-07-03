namespace ThinkBinary.Markdown2Html
{
	internal class ConversionSettings
	{
		public string InputFile { get; set; }
		public string OutputFile { get; set; }
		public string Title { get; set; }
		public bool AddNormalizeCss { get; set; }
		public string CustomCssFile { get; set; }
	}
}