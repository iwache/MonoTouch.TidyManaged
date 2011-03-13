// More info see:
// http://github.com/******/MonoTouch.TidyManaged
// http://github.com/markbeaton/TidyManaged


using System;
using System.IO;
using TidyManaged;
namespace TidyManagedDemo
{
	public class TidyManagedTest
	{
		public TidyManagedTest ()
		{
		}

		public static void Execute()
		{
			// correct XHTML frame
			var template = @"
				<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN""
				    ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
				<html xmlns=""http://www.w3.org/1999/xhtml"">
				<head>
				<title></title>
				</head>
				<body>
				{0}
				</body>
				</html>";
			
			// wrong HTML formatted body content
			var html = "<p>This <em>is <strong>my</em> document</strong>.<p>Inner paragraph.</p>Next paragraph</p>";
			
			// mix them together
			html = String.Format(template, html);
			
			// vars for tidy results
			string tidyHtml;
			var errorStream = new MemoryStream();
			int warningCount = 0;
			int errorCount = 0;
			
			// ensure tidy document will be disposed after use 
			// (because tidy needs to be tidy up after use ;-)
			using (Document tidyDoc = Document.FromString(html))
			{
				// set some tidy options
				tidyDoc.Quiet = true;
				tidyDoc.OutputXhtml = true;
				tidyDoc.AddTidyMetaElement = false;
				tidyDoc.ShowWarnings = true;
				tidyDoc.UseGnuEmacsErrorFormat = true;
				
				// set error sink to stream
				tidyDoc.SetErrorSink(errorStream);
				
				// let tidy do it's work
				tidyDoc.CleanAndRepair();
				
				// check if any HTML errors exist
				if (tidyDoc.RunDiagnostics())
					tidyDoc.ForceOutput = true;
				
				// get any error and warning count
				warningCount = tidyDoc.WarningCount();
				errorCount = tidyDoc.ErrorCount();
				
				tidyHtml = tidyDoc.Save();
			}
			// Output any errors and warnings
			errorStream.Position = 0;
			StreamReader sr = new StreamReader(errorStream);
			Console.WriteLine ("Warnings: {0}", warningCount);
			Console.WriteLine ("Errors: {0}", errorCount);
			Console.WriteLine (sr.ReadToEnd());
			
			// output tidy html
			Console.WriteLine(tidyHtml);
		}
	}
}

