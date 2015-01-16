
using System.Html;
using jQueryApi;
namespace MasDev.Saltarelle
{
    class Program
    {
        static void Main()
        {
            jQuery.OnDocumentReady(() =>

                jQuery.Select("#main h2").Click(async (el, evt) =>
                {
                    await jQuery.Select("#main p").FadeOutTask();
                    await jQuery.Select("#main p").FadeInTask();
                    Window.Alert("Done");
                }
            ));
        }
    }
}
