using System.Threading;

namespace Helpers;
public static class BackgroundClipboard
{
    /// <summary>
    /// This is a clipboard, that sets the text in a background thread
    /// </summary>
    /// <param name="c_text">Text to be set to the clipboard</param>
    public static void SetText(string c_text)
    {
        var STAThread = new Thread(
            delegate ()
            {
                // Use a fully qualified name for Clipboard otherwise it
                // will end up calling itself.
                System.Windows.Forms.Clipboard.SetText(c_text);
            });
        STAThread.SetApartmentState(ApartmentState.STA);
        STAThread.Start();
        STAThread.Join();
    }

    /// <summary>
    /// Method for returning the text 
    /// </summary>
    /// <returns></returns>
    public static string GetText()
    {
        var ReturnValue = string.Empty;
        var STAThread = new Thread(
            delegate ()
            {
                // Use a fully qualified name for Clipboard otherwise it
                // will end up calling itself.
                ReturnValue = System.Windows.Forms.Clipboard.GetText();
            });
        STAThread.SetApartmentState(ApartmentState.STA);
        STAThread.Start();
        STAThread.Join();

        return ReturnValue;
    }
}