﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbbyyLS.Globalization
{
	[TestFixture]
	public class LanguageTagTests
	{
		[TestCase("", null, null, null)]
		[TestCase("en", Language.EN, null, null)]
		[TestCase("en-Latn", Language.EN, null, null)]
		[TestCase("ru-Latn", Language.RU, Script.Latn, null)]
		[TestCase("afb", Language.AFB, null, null)]
		[TestCase("ar-afb", Language.AFB, null, null)]
		[TestCase("yue", Language.YUE, null, null)]
		[TestCase("zh-yue", Language.YUE, null, null)]
		[TestCase("ru-Latn", Language.RU, Script.Latn, null)]
		[TestCase("zh-Hans", Language.ZH, Script.Hans, null)]
		[TestCase("zh-TW", Language.ZH, null, Region.TW)]
		[TestCase("zh-Hans-TW", Language.ZH, Script.Hans, Region.TW)]
		public void SimpleParse(string text, Language? langEx, Script? scrEx, Region? regionEx)
		{
			var tag = LanguageTag.Parse(text);
			Assert.That(tag.Language, Is.EqualTo(langEx));
			Assert.That(tag.Script, Is.EqualTo(scrEx));
			Assert.That(tag.Region, Is.EqualTo(regionEx));
			Assert.That(tag.Variants, Is.Empty);
			Assert.That(tag.Extensions, Is.Empty);
			Assert.That(tag.PrivateUse, Is.Empty);
		}

		[TestCase("en", new Variant[]{})]
		[TestCase("en-scotland", new Variant[] { Variant.Scotland })]
		[TestCase("en-GB-scotland", new Variant[] { Variant.Scotland })]
		[TestCase("sl-rozaj", new Variant[] { Variant.Rozaj })]
		[TestCase("sl-rozaj-biske", new Variant[] { Variant.Rozaj, Variant.Biske })]
		[TestCase("sl-rozaj-biske-1994", new Variant[] { Variant.Rozaj, Variant.Biske, Variant.V1994 })]
		[TestCase("sl-rozaj-1994", new Variant[] { Variant.Rozaj, Variant.V1994 })]
		public void ParseWithVariants(string text, Variant[] variantsEx)
		{
			var tag = LanguageTag.Parse(text);
			Assert.That(tag.Variants, Is.EqualTo(new VariantCollection(variantsEx)));
		}

		[TestCase("xxx")]
		[TestCase("xxx-")]
		[TestCase("ar-")]
		[TestCase("ar-xxx")]
		[TestCase("ar-xxx-")]
		[TestCase("xx-xxx-")]
		[TestCase("afb-xxx")]
		[TestCase("ar-afb-")]
		[TestCase("ar-afb-xxx")]
		[TestCase("sl-rozaj-biske-1996")]
		[TestCase("sl-1994")]
		[ExpectedException(typeof(FormatException))]
		public void Parse_Fail(string text)
		{
			LanguageTag.Parse(text);
		}

		[TestCase("i-klingon", Language.TLH)]
		[TestCase("zh-min-nan", Language.NAN)]
		[TestCase("zh-xiang", Language.HSN)]
		[TestCase("art-lojban", Language.JBO)]
		public void ParseGrandfathered(string text, Language expected)
		{
			Assert.AreEqual(new LanguageTag(expected), new LanguageTag(text));
		}

		[TestCase("zh-CHS", "zh-Hans")]
		[TestCase("zh-CHT", "zh-Hant")]
		public void ParseAdditionalGrandfathered(string text, string expected)
		{
			Assert.AreEqual(new LanguageTag(expected), new LanguageTag(text));
		}

		[TestCase("en-GB-oed")]
		[TestCase("i-default")]
		[TestCase("cel-gaulish")]
		[TestCase("zh-min")]
		[ExpectedException(typeof(NotSupportedException))]
		public void NotSupportedGrandfathered(string grandfathered)
		{
			LanguageTag.Parse(grandfathered);
		}
	}
}