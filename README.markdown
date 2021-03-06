# MonoTouch.TidyManaged

This is a managed MonoTouch / iPhone iOS wrapper for the open source, cross-platform Tidy library, a HTML/XHTML/XML markup parser & cleaner originally created by Dave Raggett.

This wrapper is originally created by Mark Beaton. For more informations see Mark Beaton's original project [TidyManaged](https://github.com/markbeaton/TidyManaged).

For information about Tidy visit [Dave Raggett's original web page](http://www.w3.org/People/Raggett/tidy/) and the [SourceForge project](http://tidy.sourceforge.net/) that has taken over maintenance of the library.

## libtidy

This wrapper is written in MonoTouch/C#, and makes use of .NET platform invoke (p/invoke) functionality to interoperate with the Tidy library "libtidy.dylib" (written in portable ANSI C) which is part of the iOS operating system libraries and already available on any iPhone with iOS4 or above.

## Sample Usage

Here's a quick'n'dirty example using a simple console app.  
Note: always remember to .Dispose() of your Document instance (or wrap it in a "using" statement), so the interop layer can clean up any unmanaged resources (memory, file handles etc) when it's done cleaning.
    
		public static void Execute()
		{
			// vars for tidy results
			string tidyHtml;
			var errorStream = new MemoryStream();
			int warningCount = 0;
			int errorCount = 0;

			// ensure tidy document will be disposed after use 
			// (because tidy needs to be tidy up after use ;-)
			using (Document tidyDoc = Document.FromString("<hTml><title>test</tootle><body>asd</body>"))
			{
				// set some tidy options
				tidyDoc.Quiet = true;
				tidyDoc.OutputXhtml = true;
				tidyDoc.ShowWarnings = true;

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

results in:

    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
        "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <head>
    <meta name="generator" content=
    "HTML Tidy for Mac OS X (vers 31 October 2006 - Apple Inc. build 13), see www.w3.org" />
    <title>test</title>
    </head>
    <body>
    asd
    </body>
    </html>
