using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Base
{
    [TestClass]
    public class Naming
    {
        [TestMethod]
        public void CamelCase()
        {
            Assert.IsNull(Energy.Base.Naming.CamelCase(null));
            Assert.AreEqual("", Energy.Base.Naming.CamelCase(new string[] { }));
            string[] words;
            string check;
            words = new string[] { "The", "quick", "brown", "fox", "jumps", "over", "the", "lazy", "dog", };
            check = Energy.Base.Naming.CamelCase(words);
            Assert.AreEqual("theQuickBrownFoxJumpsOverTheLazyDog", check);
            words = new string[] { "A", "short", "brimless", "felt", "hat", "barely", "blocks", "out", "the", "sound", "of", "a", "Celtic", "violin", };
            check = Energy.Base.Naming.CamelCase(words);
            Assert.AreEqual("aShortBrimlessFeltHatBarelyBlocksOutTheSoundOfACelticViolin", check);
            words = new string[] { "Zażółć", "gęślą", "jaźń", };
            check = Energy.Base.Naming.CamelCase(words);
            Assert.AreEqual("zażółćGęśląJaźń", check);

            Assert.IsTrue(Energy.Base.Naming.IsCamelCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsCobolCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsConstantCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsDashCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsHyphenCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsKebabCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsPascalCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsSnakeCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsTrainCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsUnderscoreCase(check));
        }

        [TestMethod]
        public void CobolCase()
        {
            Assert.IsNull(Energy.Base.Naming.CobolCase(null));
            Assert.AreEqual("", Energy.Base.Naming.CobolCase(new string[] { }));
            string[] words;
            string check;
            words = new string[] { "The", "quick", "brown", "fox", "jumps", "over", "the", "lazy", "dog", };
            check = Energy.Base.Naming.CobolCase(words);
            Assert.AreEqual("THE-QUICK-BROWN-FOX-JUMPS-OVER-THE-LAZY-DOG", check);
            words = new string[] { "A", "short", "brimless", "felt", "hat", "barely", "blocks", "out", "the", "sound", "of", "a", "Celtic", "violin", };
            check = Energy.Base.Naming.CobolCase(words);
            Assert.AreEqual("A-SHORT-BRIMLESS-FELT-HAT-BARELY-BLOCKS-OUT-THE-SOUND-OF-A-CELTIC-VIOLIN", check);
            words = new string[] { "Zażółć", "gęślą", "jaźń", };
            check = Energy.Base.Naming.CobolCase(words);
            Assert.AreEqual("ZAŻÓŁĆ-GĘŚLĄ-JAŹŃ", check);

            Assert.IsFalse(Energy.Base.Naming.IsCamelCase(check));
            Assert.IsTrue(Energy.Base.Naming.IsCobolCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsConstantCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsDashCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsHyphenCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsKebabCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsPascalCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsSnakeCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsTrainCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsUnderscoreCase(check));
        }

        [TestMethod]
        public void ConstantCase()
        {
            Assert.IsNull(Energy.Base.Naming.ConstantCase(null));
            Assert.AreEqual("", Energy.Base.Naming.ConstantCase(new string[] { }));
            string[] words;
            string check;
            words = new string[] { "The", "quick", "brown", "fox", "jumps", "over", "the", "lazy", "dog", };
            check = Energy.Base.Naming.ConstantCase(words);
            Assert.AreEqual("THE_QUICK_BROWN_FOX_JUMPS_OVER_THE_LAZY_DOG", check);
            words = new string[] { "A", "short", "brimless", "felt", "hat", "barely", "blocks", "out", "the", "sound", "of", "a", "Celtic", "violin", };
            check = Energy.Base.Naming.ConstantCase(words);
            Assert.AreEqual("A_SHORT_BRIMLESS_FELT_HAT_BARELY_BLOCKS_OUT_THE_SOUND_OF_A_CELTIC_VIOLIN", check);
            words = new string[] { "Zażółć", "gęślą", "jaźń", };
            check = Energy.Base.Naming.ConstantCase(words);
            Assert.AreEqual("ZAŻÓŁĆ_GĘŚLĄ_JAŹŃ", check);

            Assert.IsFalse(Energy.Base.Naming.IsCamelCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsCobolCase(check));
            Assert.IsTrue(Energy.Base.Naming.IsConstantCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsDashCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsHyphenCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsKebabCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsPascalCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsSnakeCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsTrainCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsUnderscoreCase(check));
        }

        [TestMethod]
        public void DashCase()
        {
            Assert.IsNull(Energy.Base.Naming.DashCase(null));
            Assert.AreEqual("", Energy.Base.Naming.DashCase(new string[] { }));
            string[] words;
            string check;
            words = new string[] { "The", "quick", "brown", "fox", "jumps", "over", "the", "lazy", "dog", };
            check = Energy.Base.Naming.DashCase(words);
            Assert.AreEqual("the-quick-brown-fox-jumps-over-the-lazy-dog", check);
            words = new string[] { "A", "short", "brimless", "felt", "hat", "barely", "blocks", "out", "the", "sound", "of", "a", "Celtic", "violin", };
            check = Energy.Base.Naming.DashCase(words);
            Assert.AreEqual("a-short-brimless-felt-hat-barely-blocks-out-the-sound-of-a-celtic-violin", check);
            words = new string[] { "Zażółć", "gęślą", "jaźń", };
            check = Energy.Base.Naming.DashCase(words);
            Assert.AreEqual("zażółć-gęślą-jaźń", check);

            Assert.IsFalse(Energy.Base.Naming.IsCamelCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsCobolCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsConstantCase(check));
            Assert.IsTrue(Energy.Base.Naming.IsDashCase(check));
            Assert.IsTrue(Energy.Base.Naming.IsHyphenCase(check));
            Assert.IsTrue(Energy.Base.Naming.IsKebabCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsPascalCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsSnakeCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsTrainCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsUnderscoreCase(check));
        }

        [TestMethod]
        public void HyphenCase()
        {
            Assert.IsNull(Energy.Base.Naming.HyphenCase(null));
            Assert.AreEqual("", Energy.Base.Naming.HyphenCase(new string[] { }));
            string[] words;
            string check;
            words = new string[] { "The", "quick", "brown", "fox", "jumps", "over", "the", "lazy", "dog", };
            check = Energy.Base.Naming.HyphenCase(words);
            Assert.AreEqual("the-quick-brown-fox-jumps-over-the-lazy-dog", check);
            words = new string[] { "A", "short", "brimless", "felt", "hat", "barely", "blocks", "out", "the", "sound", "of", "a", "Celtic", "violin", };
            check = Energy.Base.Naming.HyphenCase(words);
            Assert.AreEqual("a-short-brimless-felt-hat-barely-blocks-out-the-sound-of-a-celtic-violin", check);
            words = new string[] { "Zażółć", "gęślą", "jaźń", };
            check = Energy.Base.Naming.HyphenCase(words);
            Assert.AreEqual("zażółć-gęślą-jaźń", check);

            Assert.IsFalse(Energy.Base.Naming.IsCamelCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsCobolCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsConstantCase(check));
            Assert.IsTrue(Energy.Base.Naming.IsDashCase(check));
            Assert.IsTrue(Energy.Base.Naming.IsHyphenCase(check));
            Assert.IsTrue(Energy.Base.Naming.IsKebabCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsPascalCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsSnakeCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsTrainCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsUnderscoreCase(check));
        }

        [TestMethod]
        public void KebabCase()
        {
            Assert.IsNull(Energy.Base.Naming.KebabCase(null));
            Assert.AreEqual("", Energy.Base.Naming.KebabCase(new string[] { }));
            string[] words;
            string check;
            words = new string[] { "The", "quick", "brown", "fox", "jumps", "over", "the", "lazy", "dog", };
            check = Energy.Base.Naming.KebabCase(words);
            Assert.AreEqual("the-quick-brown-fox-jumps-over-the-lazy-dog", check);
            words = new string[] { "A", "short", "brimless", "felt", "hat", "barely", "blocks", "out", "the", "sound", "of", "a", "Celtic", "violin", };
            check = Energy.Base.Naming.KebabCase(words);
            Assert.AreEqual("a-short-brimless-felt-hat-barely-blocks-out-the-sound-of-a-celtic-violin", check);
            words = new string[] { "Zażółć", "gęślą", "jaźń", };
            check = Energy.Base.Naming.KebabCase(words);
            Assert.AreEqual("zażółć-gęślą-jaźń", check);

            Assert.IsFalse(Energy.Base.Naming.IsCamelCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsCobolCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsConstantCase(check));
            Assert.IsTrue(Energy.Base.Naming.IsDashCase(check));
            Assert.IsTrue(Energy.Base.Naming.IsHyphenCase(check));
            Assert.IsTrue(Energy.Base.Naming.IsKebabCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsPascalCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsSnakeCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsTrainCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsUnderscoreCase(check));
        }

        [TestMethod]
        public void PascalCase()
        {
            Assert.IsNull(Energy.Base.Naming.PascalCase(null));
            Assert.AreEqual("", Energy.Base.Naming.PascalCase(new string[] { }));
            string[] words;
            string check;
            words = new string[] { "The", "quick", "brown", "fox", "jumps", "over", "the", "lazy", "dog", };
            check = Energy.Base.Naming.PascalCase(words);
            Assert.AreEqual("TheQuickBrownFoxJumpsOverTheLazyDog", check);
            words = new string[] { "A", "short", "brimless", "felt", "hat", "barely", "blocks", "out", "the", "sound", "of", "a", "Celtic", "violin", };
            check = Energy.Base.Naming.PascalCase(words);
            Assert.AreEqual("AShortBrimlessFeltHatBarelyBlocksOutTheSoundOfACelticViolin", check);
            words = new string[] { "Zażółć", "gęślą", "jaźń", };
            check = Energy.Base.Naming.PascalCase(words);
            Assert.AreEqual("ZażółćGęśląJaźń", check);

            Assert.IsFalse(Energy.Base.Naming.IsCamelCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsCobolCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsConstantCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsDashCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsHyphenCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsKebabCase(check));
            Assert.IsTrue(Energy.Base.Naming.IsPascalCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsSnakeCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsTrainCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsUnderscoreCase(check));
        }

        [TestMethod]
        public void SnakeCase()
        {
            Assert.IsNull(Energy.Base.Naming.SnakeCase(null));
            Assert.AreEqual("", Energy.Base.Naming.SnakeCase(new string[] { }));
            string[] words;
            string check;
            words = new string[] { "The", "quick", "brown", "fox", "jumps", "over", "the", "lazy", "dog", };
            check = Energy.Base.Naming.SnakeCase(words);
            Assert.AreEqual("the_quick_brown_fox_jumps_over_the_lazy_dog", check);
            words = new string[] { "A", "short", "brimless", "felt", "hat", "barely", "blocks", "out", "the", "sound", "of", "a", "Celtic", "violin", };
            check = Energy.Base.Naming.SnakeCase(words);
            Assert.AreEqual("a_short_brimless_felt_hat_barely_blocks_out_the_sound_of_a_celtic_violin", check);
            words = new string[] { "Zażółć", "gęślą", "jaźń", };
            check = Energy.Base.Naming.SnakeCase(words);
            Assert.AreEqual("zażółć_gęślą_jaźń", check);

            Assert.IsFalse(Energy.Base.Naming.IsCamelCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsCobolCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsConstantCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsDashCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsHyphenCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsKebabCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsPascalCase(check));
            Assert.IsTrue(Energy.Base.Naming.IsSnakeCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsTrainCase(check));
            Assert.IsTrue(Energy.Base.Naming.IsUnderscoreCase(check));
        }

        [TestMethod]
        public void TrainCase()
        {
            Assert.IsNull(Energy.Base.Naming.TrainCase(null));
            Assert.AreEqual("", Energy.Base.Naming.TrainCase(new string[] { }));
            string[] words;
            string check;
            words = new string[] { "The", "quick", "brown", "fox", "jumps", "over", "the", "lazy", "dog", };
            check = Energy.Base.Naming.TrainCase(words);
            Assert.AreEqual("The-Quick-Brown-Fox-Jumps-Over-The-Lazy-Dog", check);
            words = new string[] { "A", "short", "brimless", "felt", "hat", "barely", "blocks", "out", "the", "sound", "of", "a", "Celtic", "violin", };
            check = Energy.Base.Naming.TrainCase(words);
            Assert.AreEqual("A-Short-Brimless-Felt-Hat-Barely-Blocks-Out-The-Sound-Of-A-Celtic-Violin", check);
            words = new string[] { "Zażółć", "gęślą", "jaźń", };
            check = Energy.Base.Naming.TrainCase(words);
            Assert.AreEqual("Zażółć-Gęślą-Jaźń", check);

            Assert.IsFalse(Energy.Base.Naming.IsCamelCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsCobolCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsConstantCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsDashCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsHyphenCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsKebabCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsPascalCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsSnakeCase(check));
            Assert.IsTrue(Energy.Base.Naming.IsTrainCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsUnderscoreCase(check));
        }

        [TestMethod]
        public void UnderscoreCase()
        {
            Assert.IsNull(Energy.Base.Naming.UnderscoreCase(null));
            Assert.AreEqual("", Energy.Base.Naming.UnderscoreCase(new string[] { }));
            string[] words;
            string check;
            words = new string[] { "The", "quick", "brown", "fox", "jumps", "over", "the", "lazy", "dog", };
            check = Energy.Base.Naming.UnderscoreCase(words);
            Assert.AreEqual("the_quick_brown_fox_jumps_over_the_lazy_dog", check);
            words = new string[] { "A", "short", "brimless", "felt", "hat", "barely", "blocks", "out", "the", "sound", "of", "a", "Celtic", "violin", };
            check = Energy.Base.Naming.UnderscoreCase(words);
            Assert.AreEqual("a_short_brimless_felt_hat_barely_blocks_out_the_sound_of_a_celtic_violin", check);
            words = new string[] { "Zażółć", "gęślą", "jaźń", };
            check = Energy.Base.Naming.UnderscoreCase(words);
            Assert.AreEqual("zażółć_gęślą_jaźń", check);

            Assert.IsFalse(Energy.Base.Naming.IsCamelCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsCobolCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsConstantCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsDashCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsHyphenCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsKebabCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsPascalCase(check));
            Assert.IsTrue(Energy.Base.Naming.IsSnakeCase(check));
            Assert.IsFalse(Energy.Base.Naming.IsTrainCase(check));
            Assert.IsTrue(Energy.Base.Naming.IsUnderscoreCase(check));
        }
    }
}