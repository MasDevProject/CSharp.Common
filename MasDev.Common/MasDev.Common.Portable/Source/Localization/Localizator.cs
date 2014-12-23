using System;
using System.Collections.Generic;
using MasDev.Common.Utils;
using System.Linq;

namespace MasDev.Common.Localization
{
	public static class Localizator
	{
		static readonly Dictionary<Type, dynamic> _localizations = new Dictionary<Type, dynamic> ();

		public static TLocalizationInterface Get<TLocalizationInterface> (string locale) where TLocalizationInterface : ILocalization
		{
			var localizationInterface = typeof(TLocalizationInterface);
			if (!_localizations.ContainsKey (localizationInterface))
				throw new InvalidOperationException (localizationInterface + " localization interface is not subscribed");

			ICollection<LocalizationWrapper<TLocalizationInterface>> instances = _localizations [localizationInterface];
			if (CollectionUtils.IsNullOrEmpty (instances))
				throw new InvalidOperationException (localizationInterface + " localization interface is not subscribed");
				
			if (!StringUtils.ContainsSomethingReadable (locale))
				return instances.Single (i => i.IsDefault).Localization;
				
			var instance = instances.SingleOrDefault (wrapper => wrapper.Localization.Locale.Trim ().ToLowerInvariant () == locale);
			if (instance == null)
				instance = instances.FirstOrDefault (wrapper => AreSameLanguage (wrapper.Localization.Locale, locale));
			if (instance == null)
				instance = instances.Single (wrapper => wrapper.IsDefault);

			return instance.Localization;
		}


		public static void Subscribe<TLocalizationInterface, TLocalizationInstance> (bool isDefault)
			where TLocalizationInterface : ILocalization 
			where TLocalizationInstance : class, TLocalizationInterface, new()
		{
			var localizationInterface = typeof(TLocalizationInterface);
			ICollection<LocalizationWrapper<TLocalizationInterface>> collection = _localizations.ContainsKey (localizationInterface) ?
				_localizations [localizationInterface] :
				new HashSet<LocalizationWrapper<TLocalizationInterface>> ();

			if (isDefault && collection.Any (w => w.IsDefault))
				throw new ArgumentException (localizationInterface + " localization interface already contains a default localization instance");

			var localizationInstance = new TLocalizationInstance ();
			var locale = localizationInstance.Locale.Trim ().ToLowerInvariant ();

			if (collection.Any (w => w.Localization.Locale.Trim ().ToLowerInvariant () == locale))
				throw new ArgumentException (localizationInterface + " localization interface already contains a localization instance for locale \"" + locale + "\"");

			var wrapper = new LocalizationWrapper<TLocalizationInterface> (localizationInstance, isDefault);
			collection.Add (wrapper);

			if (collection.Count == 1)
				_localizations.Add (localizationInterface, collection);
		}


		private static bool AreSameLanguage (string locale1, string locale2)
		{
			locale1 = locale1.Trim ().ToLowerInvariant ();
			locale2 = locale2.Trim ().ToLowerInvariant ();
			var underscore1 = locale1.IndexOf ('_');
			var underscore2 = locale2.IndexOf ('_');
			if (underscore1 < 0 || underscore2 < 0)
				return false;

			var lang1 = locale1.Substring (0, underscore1);
			var lang2 = locale2.Substring (0, underscore2);

			return lang1 == lang2;
		}


	}

	internal class LocalizationWrapper<TLocalization> where TLocalization : ILocalization
	{
		public TLocalization Localization { get; private set; }

		public bool IsDefault { get; private set; }

		public LocalizationWrapper (TLocalization localization, bool isDefault)
		{
			Localization = localization;
			IsDefault = isDefault;
		}
	}
}

