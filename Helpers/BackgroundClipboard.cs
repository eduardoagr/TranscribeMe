using System.Threading;

namespace Helpers;
public static class BackgroundClipboard
{
    public static void SetText(string p_Text)
    {
        var STAThread = new Thread(
            delegate ()
            {
                // Use a fully qualified name for Clipboard otherwise it
                // will end up calling itself.
                System.Windows.Forms.Clipboard.SetText(p_Text);
            });
        STAThread.SetApartmentState(ApartmentState.STA);
        STAThread.Start();
        STAThread.Join();
    }
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