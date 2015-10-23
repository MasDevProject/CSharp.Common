using System.Collections.Generic;
using Android.Speech.Tts;
using Android.App;
using MasDev.Common.Utils;
using Android.OS;

namespace GreatQuotes
{
	public class TextToSpeechService : Java.Lang.Object, ITextToSpeechService, TextToSpeech.IOnInitListener
	{
		TextToSpeech speech;
		string lastText;
		Bundle _bundle;

		public TextToSpeechService ()
		{
			_bundle = new Bundle ();
		}

		public void Speak(string text)
		{
			if (speech == null) {
				lastText = text;
				speech = new TextToSpeech(Application.Context, this);
			}
			else {
				speech.Speak(text, QueueMode.Flush, _bundle, null);
			}
		}

		public void OnInit(OperationResult status)
		{
			if (status == OperationResult.Success) {
				speech.Speak(lastText, QueueMode.Flush, _bundle, null);
				lastText = null;
			}
		}
	}
}

