using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Energy.Base
{
    /// <summary>
    /// Currency
    /// </summary>
    public class Monetary
    {
        #region Currency

        public class Currency
        {
            /// <summary>
            /// Currency code.
            /// </summary>
            public string Code { get; private set; }

            /// <summary>
            /// Currency name.
            /// </summary>
            public string Name { get; private set; }

            public string Symbol { get; private set; }

            [DefaultValue(0)]
            public int Numeric { get; private set; }

            [DefaultValue(0)]
            public int Minor { get; private set; }

            #region Constructor

            public Currency(string code, string name, int numeric, int minor, string symbol)
            {
                Code = code;
                Name = name;
                Numeric = numeric;
                Minor = minor;
                Symbol = symbol;
            }

            public Currency(string code, string name, int numeric, int minor)
                : this(code, name, numeric, minor, "")
            {
            }

            #endregion

            public override bool Equals(object obj)
            {
                Currency left = obj as Currency;
                if (left == null)
                {
                    return false;
                }
                return Code.Equals(left.Code);
            }

            public override int GetHashCode()
            {
                return Code.GetHashCode();
            }

            public override string ToString()
            {
                return Code;
            }

            public static bool operator ==(Currency c1, Currency c2)
            {
                if (ReferenceEquals(c1, c2))
                {
                    return true;
                }

                if ((object)c1 == null || (object)c2 == null)
                {
                    return false;
                }

                return c1.Code == c2.Code;
            }

            public static bool operator !=(Currency c1, Currency c2)
            {
                return !(c1 == c2);
            }
        }

        #endregion

        #region Symbol

        /// <summary>
        /// Class which represents currency symbol information.
        /// Related to ISO 4217.
        /// </summary>
        public class Symbol
        {
            /// <summary>
            /// Currency symbol unicode character.
            /// </summary>
            public char Character;

            /// <summary>
            /// Currency symbol code.
            /// Related to ISO 4217.
            /// </summary>
            public string Code;

            /// <summary>
            /// Currency symbol english name.
            /// </summary>
            public string English;

            /// <summary>
            /// Currency symbol local name.
            /// </summary>
            public string Local;

            #region Constructor

            public Symbol() { }

            public Symbol(char character)
            {
                this.Character = character;
            }

            public Symbol(char character, string code, string english)
            {
                this.Character = character;
                this.Code = code;
                this.English = english;
            }

            public Symbol(char character, string code, string english, string local)
            {
                this.Character = character;
                this.Code = code;
                this.English = english;
                this.Local = local;
            }

            #endregion

            #region Array

            /// <summary>
            /// Array of currency symbols.
            /// </summary>
            public class Array: Energy.Base.Collection.Array<Energy.Base.Monetary.Symbol>
            {
                public Energy.Base.Monetary.Symbol Add(char character)
                {
                    Energy.Base.Monetary.Symbol item;
                    item = new Energy.Base.Monetary.Symbol(character);
                    this.Add(item);
                    return item;
                }

                public Energy.Base.Monetary.Symbol Add(char character, string code, string english)
                {
                    Energy.Base.Monetary.Symbol item;
                    item = new Energy.Base.Monetary.Symbol(character, code, english);
                    this.Add(item);
                    return item;
                }

                public Energy.Base.Monetary.Symbol Add(char character, string code, string english, string local)
                {
                    Energy.Base.Monetary.Symbol item;
                    item = new Energy.Base.Monetary.Symbol(character, code, english, local);
                    this.Add(item);
                    return item;
                }
            }

            #endregion
        }

        public static Energy.Base.Monetary.Symbol.Array CreateDefaultCurrencySymbolArray()
        {
            Energy.Base.Monetary.Symbol.Array array = new Energy.Base.Monetary.Symbol.Array();
            array.Add('$', null, "dollar");
            array.Add('€', "EUR", "euro");

            array.Add('¢', null, "cent");
            array.Add('￠', null, "cent");
            array.Add('＄', null, "dollar");

            array.Add('₵', "GHS", "cedi");
            array.Add('₡', null, "colón");

            array.Add('؋', "AFN", "afghani");
            array.Add('₮', "MNT", "tugrik", "tögrög");

            array.Add('₳', "ARA", "austral");

            // TODO

            return array;
        }

        #endregion

        #region Dictionary

        public class Dictionary : System.Collections.Generic.Dictionary<string, Base.Monetary>
        {

        }

        #endregion

        #region Manager

        #region Class

        /// <summary>
        /// Currency manager
        /// </summary>
        public partial class Manager
        {
            #region ISO 4217

            //public static Currency AED => new Currency("AED", "UAE Dirham", 784, 2);
            //public static Currency AFN => new Currency("AFN", "Afghani", 971, 2, "؋");
            //public static Currency ALL => new Currency("ALL", "Lek", 008, 2, "Lek");
            //public static Currency AMD => new Currency("AMD", "Armenian Dram", 051, 2);
            //public static Currency ANG => new Currency("ANG", "Netherlands Antillean Guilder", 532, 2);
            //public static Currency AOA => new Currency("AOA", "Kwanza", 973, 2);
            //public static Currency ARS => new Currency("ARS", "Argentine Peso", 032, 2);
            //public static Currency AUD => new Currency("AUD", "Australian Dollar", 036, 2, "$");
            //public static Currency AWG => new Currency("AWG", "Aruban Florin", 533, 2, "ƒ");
            //public static Currency AZN => new Currency("AZN", "Azerbaijanian Manat", 944, 2, "ман");
            //public static Currency BAM => new Currency("BAM", "Convertible Mark", 977, 2);
            //public static Currency BBD => new Currency("BBD", "Barbados Dollar", 052, 2, "$");
            //public static Currency BDT => new Currency("BDT", "Taka", 050, 2);
            //public static Currency BGN => new Currency("BGN", "Bulgarian Lev", 975, 2);
            //public static Currency BHD => new Currency("BHD", "Bahraini Dinar", 048, 3);
            //public static Currency BIF => new Currency("BIF", "Burundi Franc", 108, 0);
            //public static Currency BMD => new Currency("BMD", "Bermudian Dollar", 060, 2, "$");
            //public static Currency BND => new Currency("BND", "Brunei Dollar", 096, 2, "$");
            //public static Currency BOB => new Currency("BOB", "Boliviano", 068, 2, "$b");
            //public static Currency BOV => new Currency("BOV", "Mvdol", 984, 2);
            //public static Currency BRL => new Currency("BRL", "Brazilian Real", 986, 2);
            //public static Currency BSD => new Currency("BSD", "Bahamian Dollar", 044, 2, "$");
            //public static Currency BTN => new Currency("BTN", "Ngultrum", 064, 2);
            //public static Currency BWP => new Currency("BWP", "Pula", 072, 2);
            //public static Currency BYR => new Currency("BYR", "Belarusian Ruble", 974, 0);
            //public static Currency BZD => new Currency("BZD", "Belize Dollar", 084, 2, "$");
            //public static Currency CAD => new Currency("CAD", "Canadian Dollar", 124, 2, "$");
            //public static Currency CDF => new Currency("CDF", "Congolese Franc", 976, 2);
            //public static Currency CHE => new Currency("CHE", "WIR Euro", 947, 2);
            //public static Currency CHF => new Currency("CHF", "Swiss Franc", 756, 2);
            //public static Currency CHW => new Currency("CHW", "WIR Franc", 948, 2);
            //public static Currency CLF => new Currency("CLF", "Unidad de Fomento", 990, 4);
            //public static Currency CLP => new Currency("CLP", "Chilean Peso", 152, 0);
            //public static Currency CNY => new Currency("CNY", "Yuan Renminbi", 156, 2);
            //public static Currency COP => new Currency("COP", "Colombian Peso", 170, 2);
            //public static Currency COU => new Currency("COU", "Unidad de Valor Real", 970, 2);
            //public static Currency CRC => new Currency("CRC", "Costa Rican Colon", 188, 2);
            //public static Currency CUC => new Currency("CUC", "Peso Convertible", 931, 2);
            //public static Currency CUP => new Currency("CUP", "Cuban Peso", 192, 2, "₱");
            //public static Currency CVE => new Currency("CVE", "Cabo Verde Escudo", 132, 2);
            //public static Currency CZK => new Currency("CZK", "Czech Koruna", 203, 2, "Kč");
            //public static Currency DJF => new Currency("DJF", "Djibouti Franc", 262, 0);
            //public static Currency DKK => new Currency("DKK", "Danish Krone", 208, 2, "kr");
            //public static Currency DOP => new Currency("DOP", "Dominican Peso", 214, 2);
            //public static Currency DZD => new Currency("DZD", "Algerian Dinar", 012, 2);
            //public static Currency EGP => new Currency("EGP", "Egyptian Pound", 818, 2, "£");
            //public static Currency ERN => new Currency("ERN", "Nakfa", 232, 2);
            //public static Currency ETB => new Currency("ETB", "Ethiopian Birr", 230, 2);
            //public static Currency EUR => new Currency("EUR", "Euro", 978, 2, "€");
            //public static Currency FJD => new Currency("FJD", "Fiji Dollar", 242, 2);
            //public static Currency FKP => new Currency("FKP", "Falkland Islands Pound", 238, 2, "£");
            //public static Currency GBP => new Currency("GBP", "Pound Sterling", 826, 2, "£");
            //public static Currency GEL => new Currency("GEL", "Lari", 981, 2);
            //public static Currency GHS => new Currency("GHS", "Ghana Cedi", 936, 2, "¢");
            //public static Currency GIP => new Currency("GIP", "Gibraltar Pound", 292, 2, "£");
            //public static Currency GMD => new Currency("GMD", "Dalasi", 270, 2);
            //public static Currency GNF => new Currency("GNF", "Guinea Franc", 324, 0);
            //public static Currency GTQ => new Currency("GTQ", "Quetzal", 320, 2, "Q");
            //public static Currency GYD => new Currency("GYD", "Guyana Dollar", 328, 2);
            //public static Currency HKD => new Currency("HKD", "Hong Kong Dollar", 344, 2, "$");
            //public static Currency HNL => new Currency("HNL", "Lempira", 340, 2);
            //public static Currency HRK => new Currency("HRK", "Kuna", 191, 2);
            //public static Currency HTG => new Currency("HTG", "Gourde", 332, 2);
            //public static Currency HUF => new Currency("HUF", "Forint", 348, 2);
            //public static Currency IDR => new Currency("IDR", "Rupiah", 360, 2);
            //public static Currency ILS => new Currency("ILS", "New Israeli Sheqel", 376, 2, "₪");
            //public static Currency INR => new Currency("INR", "Indian Rupee", 356, 2);
            //public static Currency IQD => new Currency("IQD", "Iraqi Dinar", 368, 3);
            //public static Currency IRR => new Currency("IRR", "Iranian Rial", 364, 2, "﷼");
            //public static Currency ISK => new Currency("ISK", "Iceland Krona", 352, 0, "kr");
            //public static Currency JMD => new Currency("JMD", "Jamaican Dollar", 388, 2);
            //public static Currency JOD => new Currency("JOD", "Jordanian Dinar", 400, 3);
            //public static Currency JPY => new Currency("JPY", "Yen", 392, 0, "¥");
            //public static Currency KES => new Currency("KES", "Kenyan Shilling", 404, 2);
            //public static Currency KGS => new Currency("KGS", "Som", 417, 2);
            //public static Currency KHR => new Currency("KHR", "Riel", 116, 2);
            //public static Currency KMF => new Currency("KMF", "Comoro Franc", 174, 0);
            //public static Currency KPW => new Currency("KPW", "North Korean Won", 408, 2, "₩");
            //public static Currency KRW => new Currency("KRW", "Won", 410, 0, "₩");
            //public static Currency KWD => new Currency("KWD", "Kuwaiti Dinar", 414, 3);
            //public static Currency KYD => new Currency("KYD", "Cayman Islands Dollar", 136, 2);
            //public static Currency KZT => new Currency("KZT", "Tenge", 398, 2, "лв");
            //public static Currency LAK => new Currency("LAK", "Kip", 418, 2, "₭");
            //public static Currency LBP => new Currency("LBP", "Lebanese Pound", 422, 2);
            //public static Currency LKR => new Currency("LKR", "Sri Lanka Rupee", 144, 2);
            //public static Currency LRD => new Currency("LRD", "Liberian Dollar", 430, 2);
            //public static Currency LSL => new Currency("LSL", "Loti", 426, 2);
            //public static Currency LYD => new Currency("LYD", "Libyan Dinar", 434, 3);
            //public static Currency MAD => new Currency("MAD", "Moroccan Dirham", 504, 2);
            //public static Currency MDL => new Currency("MDL", "Moldovan Leu", 498, 2);
            //public static Currency MGA => new Currency("MGA", "Malagasy Ariary", 969, 2);
            //public static Currency MKD => new Currency("MKD", "Denar", 807, 2);
            //public static Currency MMK => new Currency("MMK", "Kyat", 104, 2);
            //public static Currency MNT => new Currency("MNT", "Tugrik", 496, 2);
            //public static Currency MOP => new Currency("MOP", "Pataca", 446, 2);
            //public static Currency MRO => new Currency("MRO", "Ouguiya", 478, 2);
            //public static Currency MUR => new Currency("MUR", "Mauritius Rupee", 480, 2);
            //public static Currency MVR => new Currency("MVR", "Rufiyaa", 462, 2);
            //public static Currency MWK => new Currency("MWK", "Malawi Kwacha", 454, 2);
            //public static Currency MXN => new Currency("MXN", "Mexican Peso", 484, 2);
            //public static Currency MXV => new Currency("MXV", "Mexican Unidad de Inversion (UDI)", 979, 2);
            //public static Currency MYR => new Currency("MYR", "Malaysian Ringgit", 458, 2);
            //public static Currency MZN => new Currency("MZN", "Mozambique Metical", 943, 2);
            //public static Currency NAD => new Currency("NAD", "Namibia Dollar", 516, 2);
            //public static Currency NGN => new Currency("NGN", "Naira", 566, 2);
            //public static Currency NIO => new Currency("NIO", "Cordoba Oro", 558, 2);
            //public static Currency NOK => new Currency("NOK", "Norwegian Krone", 578, 2);
            //public static Currency NPR => new Currency("NPR", "Nepalese Rupee", 524, 2);
            //public static Currency NZD => new Currency("NZD", "New Zealand Dollar", 554, 2);
            //public static Currency OMR => new Currency("OMR", "Rial Omani", 512, 3);
            //public static Currency PAB => new Currency("PAB", "Balboa", 590, 2);
            //public static Currency PEN => new Currency("PEN", "Sol", 604, 2);
            //public static Currency PGK => new Currency("PGK", "Kina", 598, 2);
            //public static Currency PHP => new Currency("PHP", "Philippine Peso", 608, 2, "₱");
            //public static Currency PKR => new Currency("PKR", "Pakistan Rupee", 586, 2);
            //public static Currency PLN => new Currency("PLN", "Zloty", 985, 2, "zł");
            //public static Currency PYG => new Currency("PYG", "Guarani", 600, 0);
            //public static Currency QAR => new Currency("QAR", "Qatari Rial", 634, 2);
            //public static Currency RON => new Currency("RON", "Romanian Leu", 946, 2);
            //public static Currency RSD => new Currency("RSD", "Serbian Dinar", 941, 2);
            //public static Currency RUB => new Currency("RUB", "Russian Ruble", 643, 2);
            //public static Currency RWF => new Currency("RWF", "Rwanda Franc", 646, 0);
            //public static Currency SAR => new Currency("SAR", "Saudi Riyal", 682, 2);
            //public static Currency SBD => new Currency("SBD", "Solomon Islands Dollar", 090, 2);
            //public static Currency SCR => new Currency("SCR", "Seychelles Rupee", 690, 2);
            //public static Currency SDG => new Currency("SDG", "Sudanese Pound", 938, 2);
            //public static Currency SEK => new Currency("SEK", "Swedish Krona", 752, 2);
            //public static Currency SGD => new Currency("SGD", "Singapore Dollar", 702, 2);
            //public static Currency SHP => new Currency("SHP", "Saint Helena Pound", 654, 2);
            //public static Currency SLL => new Currency("SLL", "Leone", 694, 2);
            //public static Currency SOS => new Currency("SOS", "Somali Shilling", 706, 2);
            //public static Currency SRD => new Currency("SRD", "Surinam Dollar", 968, 2);
            //public static Currency SSP => new Currency("SSP", "South Sudanese Pound", 728, 2);
            //public static Currency STD => new Currency("STD", "Dobra", 678, 2);
            //public static Currency SVC => new Currency("SVC", "El Salvador Colon", 222, 2);
            //public static Currency SYP => new Currency("SYP", "Syrian Pound", 760, 2);
            //public static Currency SZL => new Currency("SZL", "Lilangeni", 748, 2);
            //public static Currency THB => new Currency("THB", "Baht", 764, 2);
            //public static Currency TJS => new Currency("TJS", "Somoni", 972, 2);
            //public static Currency TMT => new Currency("TMT", "Turkmenistan New Manat", 934, 2);
            //public static Currency TND => new Currency("TND", "Tunisian Dinar", 788, 3);
            //public static Currency TOP => new Currency("TOP", "Pa’anga", 776, 2);
            //public static Currency TRY => new Currency("TRY", "Turkish Lira", 949, 2);
            //public static Currency TTD => new Currency("TTD", "Trinidad and Tobago Dollar", 780, 2, "TT$");
            //public static Currency TWD => new Currency("TWD", "New Taiwan Dollar", 901, 2, "NT$");
            //public static Currency TZS => new Currency("TZS", "Tanzanian Shilling", 834, 2);
            //public static Currency UAH => new Currency("UAH", "Hryvnia", 980, 2, "₴");
            //public static Currency UGX => new Currency("UGX", "Uganda Shilling", 800, 0);
            //public static Currency USD => new Currency("USD", "US Dollar", 840, 2);
            //public static Currency USN => new Currency("USN", "US Dollar (Next day)", 997, 2);
            //public static Currency UYI => new Currency("UYI", "Uruguay Peso en Unidades Indexadas (URUIURUI)", 940, 0);
            //public static Currency UYU => new Currency("UYU", "Peso Uruguayo", 858, 2);
            //public static Currency UZS => new Currency("UZS", "Uzbekistan Sum", 860, 2);
            //public static Currency VEF => new Currency("VEF", "Bolívar", 937, 2);
            //public static Currency VND => new Currency("VND", "Dong", 704, 0);
            //public static Currency VUV => new Currency("VUV", "Vatu", 548, 0);
            //public static Currency WST => new Currency("WST", "Tala", 882, 2);
            //public static Currency XAF => new Currency("XAF", "CFA Franc BEAC", 950, 0);
            //public static Currency XAG => new Currency("XAG", "Silver", 961, 0);
            //public static Currency XAU => new Currency("XAU", "Gold", 959, 0);
            //public static Currency XBA => new Currency("XBA", "Bond Markets Unit European Composite Unit (EURCO)", 955, 0);
            //public static Currency XBB => new Currency("XBB", "Bond Markets Unit European Monetary Unit (E.M.U.-6)", 956, 0);
            //public static Currency XBC => new Currency("XBC", "Bond Markets Unit European Unit of Account 9 (E.U.A.-9)", 957, 0);
            //public static Currency XBD => new Currency("XBD", "Bond Markets Unit European Unit of Account 17 (E.U.A.-17)", 958, 0);
            //public static Currency XCD => new Currency("XCD", "East Caribbean Dollar", 951, 2);
            //public static Currency XDR => new Currency("XDR", "SDR (Special Drawing Right)", 960, 0);
            //public static Currency XOF => new Currency("XOF", "CFA Franc BCEAO", 952, 0);
            //public static Currency XPD => new Currency("XPD", "Palladium", 964, 0);
            //public static Currency XPF => new Currency("XPF", "CFP Franc", 953, 0);
            //public static Currency XPT => new Currency("XPT", "Platinum", 962, 0);
            //public static Currency XSU => new Currency("XSU", "Sucre", 994, 0);
            //public static Currency XTS => new Currency("XTS", "Codes specifically reserved for testing purposes", 963, 0);
            //public static Currency XUA => new Currency("XUA", "ADB Unit of Account", 965, 0);
            //public static Currency XXX => new Currency("XXX", "The codes assigned for transactions where no currency is involved", 999, 0);
            //public static Currency YER => new Currency("YER", "Yemeni Rial", 886, 2);
            //public static Currency ZAR => new Currency("ZAR", "Rand", 710, 2);
            //public static Currency ZMW => new Currency("ZMW", "Zambian Kwacha", 967, 2);
            //public static Currency ZWL => new Currency("ZWL", "Zimbabwe Dollar", 932, 2, "$");

            public readonly Energy.Base.Monetary.Currency AED = new Energy.Base.Monetary.Currency("AED", "UAE Dirham", 784, 2);
            public readonly Energy.Base.Monetary.Currency AFN = new Energy.Base.Monetary.Currency("AFN", "Afghani", 971, 2, "؋");
            public readonly Energy.Base.Monetary.Currency ALL = new Energy.Base.Monetary.Currency("ALL", "Lek", 008, 2, "Lek");
            public readonly Energy.Base.Monetary.Currency AMD = new Energy.Base.Monetary.Currency("AMD", "Armenian Dram", 051, 2);
            public readonly Energy.Base.Monetary.Currency ANG = new Energy.Base.Monetary.Currency("ANG", "Netherlands Antillean Guilder", 532, 2);
            public readonly Energy.Base.Monetary.Currency AOA = new Energy.Base.Monetary.Currency("AOA", "Kwanza", 973, 2);
            public readonly Energy.Base.Monetary.Currency ARS = new Energy.Base.Monetary.Currency("ARS", "Argentine Peso", 032, 2);
            public readonly Energy.Base.Monetary.Currency AUD = new Energy.Base.Monetary.Currency("AUD", "Australian Dollar", 036, 2, "$");
            public readonly Energy.Base.Monetary.Currency AWG = new Energy.Base.Monetary.Currency("AWG", "Aruban Florin", 533, 2, "ƒ");
            public readonly Energy.Base.Monetary.Currency AZN = new Energy.Base.Monetary.Currency("AZN", "Azerbaijanian Manat", 944, 2, "ман");
            public readonly Energy.Base.Monetary.Currency BAM = new Energy.Base.Monetary.Currency("BAM", "Convertible Mark", 977, 2);
            public readonly Energy.Base.Monetary.Currency BBD = new Energy.Base.Monetary.Currency("BBD", "Barbados Dollar", 052, 2, "$");
            public readonly Energy.Base.Monetary.Currency BDT = new Energy.Base.Monetary.Currency("BDT", "Taka", 050, 2);
            public readonly Energy.Base.Monetary.Currency BGN = new Energy.Base.Monetary.Currency("BGN", "Bulgarian Lev", 975, 2);
            public readonly Energy.Base.Monetary.Currency BHD = new Energy.Base.Monetary.Currency("BHD", "Bahraini Dinar", 048, 3);
            public readonly Energy.Base.Monetary.Currency BIF = new Energy.Base.Monetary.Currency("BIF", "Burundi Franc", 108, 0);
            public readonly Energy.Base.Monetary.Currency BMD = new Energy.Base.Monetary.Currency("BMD", "Bermudian Dollar", 060, 2, "$");
            public readonly Energy.Base.Monetary.Currency BND = new Energy.Base.Monetary.Currency("BND", "Brunei Dollar", 096, 2, "$");
            public readonly Energy.Base.Monetary.Currency BOB = new Energy.Base.Monetary.Currency("BOB", "Boliviano", 068, 2, "$b");
            public readonly Energy.Base.Monetary.Currency BOV = new Energy.Base.Monetary.Currency("BOV", "Mvdol", 984, 2);
            public readonly Energy.Base.Monetary.Currency BRL = new Energy.Base.Monetary.Currency("BRL", "Brazilian Real", 986, 2);
            public readonly Energy.Base.Monetary.Currency BSD = new Energy.Base.Monetary.Currency("BSD", "Bahamian Dollar", 044, 2, "$");
            public readonly Energy.Base.Monetary.Currency BTN = new Energy.Base.Monetary.Currency("BTN", "Ngultrum", 064, 2);
            public readonly Energy.Base.Monetary.Currency BWP = new Energy.Base.Monetary.Currency("BWP", "Pula", 072, 2);
            public readonly Energy.Base.Monetary.Currency BYR = new Energy.Base.Monetary.Currency("BYR", "Belarusian Ruble", 974, 0);
            public readonly Energy.Base.Monetary.Currency BZD = new Energy.Base.Monetary.Currency("BZD", "Belize Dollar", 084, 2, "$");
            public readonly Energy.Base.Monetary.Currency CAD = new Energy.Base.Monetary.Currency("CAD", "Canadian Dollar", 124, 2, "$");
            public readonly Energy.Base.Monetary.Currency CDF = new Energy.Base.Monetary.Currency("CDF", "Congolese Franc", 976, 2);
            public readonly Energy.Base.Monetary.Currency CHE = new Energy.Base.Monetary.Currency("CHE", "WIR Euro", 947, 2);
            public readonly Energy.Base.Monetary.Currency CHF = new Energy.Base.Monetary.Currency("CHF", "Swiss Franc", 756, 2);
            public readonly Energy.Base.Monetary.Currency CHW = new Energy.Base.Monetary.Currency("CHW", "WIR Franc", 948, 2);
            public readonly Energy.Base.Monetary.Currency CLF = new Energy.Base.Monetary.Currency("CLF", "Unidad de Fomento", 990, 4);
            public readonly Energy.Base.Monetary.Currency CLP = new Energy.Base.Monetary.Currency("CLP", "Chilean Peso", 152, 0);
            public readonly Energy.Base.Monetary.Currency CNY = new Energy.Base.Monetary.Currency("CNY", "Yuan Renminbi", 156, 2);
            public readonly Energy.Base.Monetary.Currency COP = new Energy.Base.Monetary.Currency("COP", "Colombian Peso", 170, 2);
            public readonly Energy.Base.Monetary.Currency COU = new Energy.Base.Monetary.Currency("COU", "Unidad de Valor Real", 970, 2);
            public readonly Energy.Base.Monetary.Currency CRC = new Energy.Base.Monetary.Currency("CRC", "Costa Rican Colon", 188, 2);
            public readonly Energy.Base.Monetary.Currency CUC = new Energy.Base.Monetary.Currency("CUC", "Peso Convertible", 931, 2);
            public readonly Energy.Base.Monetary.Currency CUP = new Energy.Base.Monetary.Currency("CUP", "Cuban Peso", 192, 2, "₱");
            public readonly Energy.Base.Monetary.Currency CVE = new Energy.Base.Monetary.Currency("CVE", "Cabo Verde Escudo", 132, 2);
            public readonly Energy.Base.Monetary.Currency CZK = new Energy.Base.Monetary.Currency("CZK", "Czech Koruna", 203, 2, "Kč");
            public readonly Energy.Base.Monetary.Currency DJF = new Energy.Base.Monetary.Currency("DJF", "Djibouti Franc", 262, 0);
            public readonly Energy.Base.Monetary.Currency DKK = new Energy.Base.Monetary.Currency("DKK", "Danish Krone", 208, 2, "kr");
            public readonly Energy.Base.Monetary.Currency DOP = new Energy.Base.Monetary.Currency("DOP", "Dominican Peso", 214, 2);
            public readonly Energy.Base.Monetary.Currency DZD = new Energy.Base.Monetary.Currency("DZD", "Algerian Dinar", 012, 2);
            public readonly Energy.Base.Monetary.Currency EGP = new Energy.Base.Monetary.Currency("EGP", "Egyptian Pound", 818, 2, "£");
            public readonly Energy.Base.Monetary.Currency ERN = new Energy.Base.Monetary.Currency("ERN", "Nakfa", 232, 2);
            public readonly Energy.Base.Monetary.Currency ETB = new Energy.Base.Monetary.Currency("ETB", "Ethiopian Birr", 230, 2);
            public readonly Energy.Base.Monetary.Currency EUR = new Energy.Base.Monetary.Currency("EUR", "Euro", 978, 2, "€");
            public readonly Energy.Base.Monetary.Currency FJD = new Energy.Base.Monetary.Currency("FJD", "Fiji Dollar", 242, 2);
            public readonly Energy.Base.Monetary.Currency FKP = new Energy.Base.Monetary.Currency("FKP", "Falkland Islands Pound", 238, 2, "£");
            public readonly Energy.Base.Monetary.Currency GBP = new Energy.Base.Monetary.Currency("GBP", "Pound Sterling", 826, 2, "£");
            public readonly Energy.Base.Monetary.Currency GEL = new Energy.Base.Monetary.Currency("GEL", "Lari", 981, 2);
            public readonly Energy.Base.Monetary.Currency GHS = new Energy.Base.Monetary.Currency("GHS", "Ghana Cedi", 936, 2, "¢");
            public readonly Energy.Base.Monetary.Currency GIP = new Energy.Base.Monetary.Currency("GIP", "Gibraltar Pound", 292, 2, "£");
            public readonly Energy.Base.Monetary.Currency GMD = new Energy.Base.Monetary.Currency("GMD", "Dalasi", 270, 2);
            public readonly Energy.Base.Monetary.Currency GNF = new Energy.Base.Monetary.Currency("GNF", "Guinea Franc", 324, 0);
            public readonly Energy.Base.Monetary.Currency GTQ = new Energy.Base.Monetary.Currency("GTQ", "Quetzal", 320, 2, "Q");
            public readonly Energy.Base.Monetary.Currency GYD = new Energy.Base.Monetary.Currency("GYD", "Guyana Dollar", 328, 2);
            public readonly Energy.Base.Monetary.Currency HKD = new Energy.Base.Monetary.Currency("HKD", "Hong Kong Dollar", 344, 2, "$");
            public readonly Energy.Base.Monetary.Currency HNL = new Energy.Base.Monetary.Currency("HNL", "Lempira", 340, 2);
            public readonly Energy.Base.Monetary.Currency HRK = new Energy.Base.Monetary.Currency("HRK", "Kuna", 191, 2);
            public readonly Energy.Base.Monetary.Currency HTG = new Energy.Base.Monetary.Currency("HTG", "Gourde", 332, 2);
            public readonly Energy.Base.Monetary.Currency HUF = new Energy.Base.Monetary.Currency("HUF", "Forint", 348, 2);
            public readonly Energy.Base.Monetary.Currency IDR = new Energy.Base.Monetary.Currency("IDR", "Rupiah", 360, 2);
            public readonly Energy.Base.Monetary.Currency ILS = new Energy.Base.Monetary.Currency("ILS", "New Israeli Sheqel", 376, 2, "₪");
            public readonly Energy.Base.Monetary.Currency INR = new Energy.Base.Monetary.Currency("INR", "Indian Rupee", 356, 2);
            public readonly Energy.Base.Monetary.Currency IQD = new Energy.Base.Monetary.Currency("IQD", "Iraqi Dinar", 368, 3);
            public readonly Energy.Base.Monetary.Currency IRR = new Energy.Base.Monetary.Currency("IRR", "Iranian Rial", 364, 2, "﷼");
            public readonly Energy.Base.Monetary.Currency ISK = new Energy.Base.Monetary.Currency("ISK", "Iceland Krona", 352, 0, "kr");
            public readonly Energy.Base.Monetary.Currency JMD = new Energy.Base.Monetary.Currency("JMD", "Jamaican Dollar", 388, 2);
            public readonly Energy.Base.Monetary.Currency JOD = new Energy.Base.Monetary.Currency("JOD", "Jordanian Dinar", 400, 3);
            public readonly Energy.Base.Monetary.Currency JPY = new Energy.Base.Monetary.Currency("JPY", "Yen", 392, 0, "¥");
            public readonly Energy.Base.Monetary.Currency KES = new Energy.Base.Monetary.Currency("KES", "Kenyan Shilling", 404, 2);
            public readonly Energy.Base.Monetary.Currency KGS = new Energy.Base.Monetary.Currency("KGS", "Som", 417, 2);
            public readonly Energy.Base.Monetary.Currency KHR = new Energy.Base.Monetary.Currency("KHR", "Riel", 116, 2);
            public readonly Energy.Base.Monetary.Currency KMF = new Energy.Base.Monetary.Currency("KMF", "Comoro Franc", 174, 0);
            public readonly Energy.Base.Monetary.Currency KPW = new Energy.Base.Monetary.Currency("KPW", "North Korean Won", 408, 2, "₩");
            public readonly Energy.Base.Monetary.Currency KRW = new Energy.Base.Monetary.Currency("KRW", "Won", 410, 0, "₩");
            public readonly Energy.Base.Monetary.Currency KWD = new Energy.Base.Monetary.Currency("KWD", "Kuwaiti Dinar", 414, 3);
            public readonly Energy.Base.Monetary.Currency KYD = new Energy.Base.Monetary.Currency("KYD", "Cayman Islands Dollar", 136, 2);
            public readonly Energy.Base.Monetary.Currency KZT = new Energy.Base.Monetary.Currency("KZT", "Tenge", 398, 2, "лв");
            public readonly Energy.Base.Monetary.Currency LAK = new Energy.Base.Monetary.Currency("LAK", "Kip", 418, 2, "₭");
            public readonly Energy.Base.Monetary.Currency LBP = new Energy.Base.Monetary.Currency("LBP", "Lebanese Pound", 422, 2);
            public readonly Energy.Base.Monetary.Currency LKR = new Energy.Base.Monetary.Currency("LKR", "Sri Lanka Rupee", 144, 2);
            public readonly Energy.Base.Monetary.Currency LRD = new Energy.Base.Monetary.Currency("LRD", "Liberian Dollar", 430, 2);
            public readonly Energy.Base.Monetary.Currency LSL = new Energy.Base.Monetary.Currency("LSL", "Loti", 426, 2);
            public readonly Energy.Base.Monetary.Currency LYD = new Energy.Base.Monetary.Currency("LYD", "Libyan Dinar", 434, 3);
            public readonly Energy.Base.Monetary.Currency MAD = new Energy.Base.Monetary.Currency("MAD", "Moroccan Dirham", 504, 2);
            public readonly Energy.Base.Monetary.Currency MDL = new Energy.Base.Monetary.Currency("MDL", "Moldovan Leu", 498, 2);
            public readonly Energy.Base.Monetary.Currency MGA = new Energy.Base.Monetary.Currency("MGA", "Malagasy Ariary", 969, 2);
            public readonly Energy.Base.Monetary.Currency MKD = new Energy.Base.Monetary.Currency("MKD", "Denar", 807, 2);
            public readonly Energy.Base.Monetary.Currency MMK = new Energy.Base.Monetary.Currency("MMK", "Kyat", 104, 2);
            public readonly Energy.Base.Monetary.Currency MNT = new Energy.Base.Monetary.Currency("MNT", "Tugrik", 496, 2);
            public readonly Energy.Base.Monetary.Currency MOP = new Energy.Base.Monetary.Currency("MOP", "Pataca", 446, 2);
            public readonly Energy.Base.Monetary.Currency MRO = new Energy.Base.Monetary.Currency("MRO", "Ouguiya", 478, 2);
            public readonly Energy.Base.Monetary.Currency MUR = new Energy.Base.Monetary.Currency("MUR", "Mauritius Rupee", 480, 2);
            public readonly Energy.Base.Monetary.Currency MVR = new Energy.Base.Monetary.Currency("MVR", "Rufiyaa", 462, 2);
            public readonly Energy.Base.Monetary.Currency MWK = new Energy.Base.Monetary.Currency("MWK", "Malawi Kwacha", 454, 2);
            public readonly Energy.Base.Monetary.Currency MXN = new Energy.Base.Monetary.Currency("MXN", "Mexican Peso", 484, 2);
            public readonly Energy.Base.Monetary.Currency MXV = new Energy.Base.Monetary.Currency("MXV", "Mexican Unidad de Inversion (UDI)", 979, 2);
            public readonly Energy.Base.Monetary.Currency MYR = new Energy.Base.Monetary.Currency("MYR", "Malaysian Ringgit", 458, 2);
            public readonly Energy.Base.Monetary.Currency MZN = new Energy.Base.Monetary.Currency("MZN", "Mozambique Metical", 943, 2);
            public readonly Energy.Base.Monetary.Currency NAD = new Energy.Base.Monetary.Currency("NAD", "Namibia Dollar", 516, 2);
            public readonly Energy.Base.Monetary.Currency NGN = new Energy.Base.Monetary.Currency("NGN", "Naira", 566, 2);
            public readonly Energy.Base.Monetary.Currency NIO = new Energy.Base.Monetary.Currency("NIO", "Cordoba Oro", 558, 2);
            public readonly Energy.Base.Monetary.Currency NOK = new Energy.Base.Monetary.Currency("NOK", "Norwegian Krone", 578, 2);
            public readonly Energy.Base.Monetary.Currency NPR = new Energy.Base.Monetary.Currency("NPR", "Nepalese Rupee", 524, 2);
            public readonly Energy.Base.Monetary.Currency NZD = new Energy.Base.Monetary.Currency("NZD", "New Zealand Dollar", 554, 2);
            public readonly Energy.Base.Monetary.Currency OMR = new Energy.Base.Monetary.Currency("OMR", "Rial Omani", 512, 3);
            public readonly Energy.Base.Monetary.Currency PAB = new Energy.Base.Monetary.Currency("PAB", "Balboa", 590, 2);
            public readonly Energy.Base.Monetary.Currency PEN = new Energy.Base.Monetary.Currency("PEN", "Sol", 604, 2);
            public readonly Energy.Base.Monetary.Currency PGK = new Energy.Base.Monetary.Currency("PGK", "Kina", 598, 2);
            public readonly Energy.Base.Monetary.Currency PHP = new Energy.Base.Monetary.Currency("PHP", "Philippine Peso", 608, 2, "₱");
            public readonly Energy.Base.Monetary.Currency PKR = new Energy.Base.Monetary.Currency("PKR", "Pakistan Rupee", 586, 2);
            public readonly Energy.Base.Monetary.Currency PLN = new Energy.Base.Monetary.Currency("PLN", "Zloty", 985, 2, "zł");
            public readonly Energy.Base.Monetary.Currency PYG = new Energy.Base.Monetary.Currency("PYG", "Guarani", 600, 0);
            public readonly Energy.Base.Monetary.Currency QAR = new Energy.Base.Monetary.Currency("QAR", "Qatari Rial", 634, 2);
            public readonly Energy.Base.Monetary.Currency RON = new Energy.Base.Monetary.Currency("RON", "Romanian Leu", 946, 2);
            public readonly Energy.Base.Monetary.Currency RSD = new Energy.Base.Monetary.Currency("RSD", "Serbian Dinar", 941, 2);
            public readonly Energy.Base.Monetary.Currency RUB = new Energy.Base.Monetary.Currency("RUB", "Russian Ruble", 643, 2);
            public readonly Energy.Base.Monetary.Currency RWF = new Energy.Base.Monetary.Currency("RWF", "Rwanda Franc", 646, 0);
            public readonly Energy.Base.Monetary.Currency SAR = new Energy.Base.Monetary.Currency("SAR", "Saudi Riyal", 682, 2);
            public readonly Energy.Base.Monetary.Currency SBD = new Energy.Base.Monetary.Currency("SBD", "Solomon Islands Dollar", 090, 2);
            public readonly Energy.Base.Monetary.Currency SCR = new Energy.Base.Monetary.Currency("SCR", "Seychelles Rupee", 690, 2);
            public readonly Energy.Base.Monetary.Currency SDG = new Energy.Base.Monetary.Currency("SDG", "Sudanese Pound", 938, 2);
            public readonly Energy.Base.Monetary.Currency SEK = new Energy.Base.Monetary.Currency("SEK", "Swedish Krona", 752, 2);
            public readonly Energy.Base.Monetary.Currency SGD = new Energy.Base.Monetary.Currency("SGD", "Singapore Dollar", 702, 2);
            public readonly Energy.Base.Monetary.Currency SHP = new Energy.Base.Monetary.Currency("SHP", "Saint Helena Pound", 654, 2);
            public readonly Energy.Base.Monetary.Currency SLL = new Energy.Base.Monetary.Currency("SLL", "Leone", 694, 2);
            public readonly Energy.Base.Monetary.Currency SOS = new Energy.Base.Monetary.Currency("SOS", "Somali Shilling", 706, 2);
            public readonly Energy.Base.Monetary.Currency SRD = new Energy.Base.Monetary.Currency("SRD", "Surinam Dollar", 968, 2);
            public readonly Energy.Base.Monetary.Currency SSP = new Energy.Base.Monetary.Currency("SSP", "South Sudanese Pound", 728, 2);
            public readonly Energy.Base.Monetary.Currency STD = new Energy.Base.Monetary.Currency("STD", "Dobra", 678, 2);
            public readonly Energy.Base.Monetary.Currency SVC = new Energy.Base.Monetary.Currency("SVC", "El Salvador Colon", 222, 2);
            public readonly Energy.Base.Monetary.Currency SYP = new Energy.Base.Monetary.Currency("SYP", "Syrian Pound", 760, 2);
            public readonly Energy.Base.Monetary.Currency SZL = new Energy.Base.Monetary.Currency("SZL", "Lilangeni", 748, 2);
            public readonly Energy.Base.Monetary.Currency THB = new Energy.Base.Monetary.Currency("THB", "Baht", 764, 2);
            public readonly Energy.Base.Monetary.Currency TJS = new Energy.Base.Monetary.Currency("TJS", "Somoni", 972, 2);
            public readonly Energy.Base.Monetary.Currency TMT = new Energy.Base.Monetary.Currency("TMT", "Turkmenistan New Manat", 934, 2);
            public readonly Energy.Base.Monetary.Currency TND = new Energy.Base.Monetary.Currency("TND", "Tunisian Dinar", 788, 3);
            public readonly Energy.Base.Monetary.Currency TOP = new Energy.Base.Monetary.Currency("TOP", "Pa’anga", 776, 2);
            public readonly Energy.Base.Monetary.Currency TRY = new Energy.Base.Monetary.Currency("TRY", "Turkish Lira", 949, 2);
            public readonly Energy.Base.Monetary.Currency TTD = new Energy.Base.Monetary.Currency("TTD", "Trinidad and Tobago Dollar", 780, 2, "TT$");
            public readonly Energy.Base.Monetary.Currency TWD = new Energy.Base.Monetary.Currency("TWD", "New Taiwan Dollar", 901, 2, "NT$");
            public readonly Energy.Base.Monetary.Currency TZS = new Energy.Base.Monetary.Currency("TZS", "Tanzanian Shilling", 834, 2);
            public readonly Energy.Base.Monetary.Currency UAH = new Energy.Base.Monetary.Currency("UAH", "Hryvnia", 980, 2, "₴");
            public readonly Energy.Base.Monetary.Currency UGX = new Energy.Base.Monetary.Currency("UGX", "Uganda Shilling", 800, 0);
            public readonly Energy.Base.Monetary.Currency USD = new Energy.Base.Monetary.Currency("USD", "US Dollar", 840, 2);
            public readonly Energy.Base.Monetary.Currency USN = new Energy.Base.Monetary.Currency("USN", "US Dollar (Next day)", 997, 2);
            public readonly Energy.Base.Monetary.Currency UYI = new Energy.Base.Monetary.Currency("UYI", "Uruguay Peso en Unidades Indexadas (URUIURUI)", 940, 0);
            public readonly Energy.Base.Monetary.Currency UYU = new Energy.Base.Monetary.Currency("UYU", "Peso Uruguayo", 858, 2);
            public readonly Energy.Base.Monetary.Currency UZS = new Energy.Base.Monetary.Currency("UZS", "Uzbekistan Sum", 860, 2);
            public readonly Energy.Base.Monetary.Currency VEF = new Energy.Base.Monetary.Currency("VEF", "Bolívar", 937, 2);
            public readonly Energy.Base.Monetary.Currency VND = new Energy.Base.Monetary.Currency("VND", "Dong", 704, 0);
            public readonly Energy.Base.Monetary.Currency VUV = new Energy.Base.Monetary.Currency("VUV", "Vatu", 548, 0);
            public readonly Energy.Base.Monetary.Currency WST = new Energy.Base.Monetary.Currency("WST", "Tala", 882, 2);
            public readonly Energy.Base.Monetary.Currency XAF = new Energy.Base.Monetary.Currency("XAF", "CFA Franc BEAC", 950, 0);
            public readonly Energy.Base.Monetary.Currency XAG = new Energy.Base.Monetary.Currency("XAG", "Silver", 961, 0);
            public readonly Energy.Base.Monetary.Currency XAU = new Energy.Base.Monetary.Currency("XAU", "Gold", 959, 0);
            public readonly Energy.Base.Monetary.Currency XBA = new Energy.Base.Monetary.Currency("XBA", "Bond Markets Unit European Composite Unit (EURCO)", 955, 0);
            public readonly Energy.Base.Monetary.Currency XBB = new Energy.Base.Monetary.Currency("XBB", "Bond Markets Unit European Monetary Unit (E.M.U.-6)", 956, 0);
            public readonly Energy.Base.Monetary.Currency XBC = new Energy.Base.Monetary.Currency("XBC", "Bond Markets Unit European Unit of Account 9 (E.U.A.-9)", 957, 0);
            public readonly Energy.Base.Monetary.Currency XBD = new Energy.Base.Monetary.Currency("XBD", "Bond Markets Unit European Unit of Account 17 (E.U.A.-17)", 958, 0);
            public readonly Energy.Base.Monetary.Currency XCD = new Energy.Base.Monetary.Currency("XCD", "East Caribbean Dollar", 951, 2);
            public readonly Energy.Base.Monetary.Currency XDR = new Energy.Base.Monetary.Currency("XDR", "SDR (Special Drawing Right)", 960, 0);
            public readonly Energy.Base.Monetary.Currency XOF = new Energy.Base.Monetary.Currency("XOF", "CFA Franc BCEAO", 952, 0);
            public readonly Energy.Base.Monetary.Currency XPD = new Energy.Base.Monetary.Currency("XPD", "Palladium", 964, 0);
            public readonly Energy.Base.Monetary.Currency XPF = new Energy.Base.Monetary.Currency("XPF", "CFP Franc", 953, 0);
            public readonly Energy.Base.Monetary.Currency XPT = new Energy.Base.Monetary.Currency("XPT", "Platinum", 962, 0);
            public readonly Energy.Base.Monetary.Currency XSU = new Energy.Base.Monetary.Currency("XSU", "Sucre", 994, 0);
            public readonly Energy.Base.Monetary.Currency XTS = new Energy.Base.Monetary.Currency("XTS", "Codes specifically reserved for testing purposes", 963, 0);
            public readonly Energy.Base.Monetary.Currency XUA = new Energy.Base.Monetary.Currency("XUA", "ADB Unit of Account", 965, 0);
            public readonly Energy.Base.Monetary.Currency XXX = new Energy.Base.Monetary.Currency("XXX", "The codes assigned for transactions where no readonly Currency is involved", 999, 0);
            public readonly Energy.Base.Monetary.Currency YER = new Energy.Base.Monetary.Currency("YER", "Yemeni Rial", 886, 2);
            public readonly Energy.Base.Monetary.Currency ZAR = new Energy.Base.Monetary.Currency("ZAR", "Rand", 710, 2);
            public readonly Energy.Base.Monetary.Currency ZMW = new Energy.Base.Monetary.Currency("ZMW", "Zambian Kwacha", 967, 2);
            public readonly Energy.Base.Monetary.Currency ZWL = new Energy.Base.Monetary.Currency("ZWL", "Zimbabwe Dollar", 932, 2, "$");

            #endregion
        }

        #endregion

        #region Singleton

        /// <summary>
        /// Singleton
        /// </summary>
        public partial class Manager
        {
            private static Manager _Default;

            private static readonly object _DefaultLock = new object();

            public static Manager Default
            {
                get
                {
                    if (_Default == null)
                    {
                        lock (_DefaultLock)
                        {
                            if (_Default == null)
                            {
                                _Default = new Manager();
                            }
                        }
                    }
                    return _Default;
                }
            }
        }

        #endregion

        #endregion
    }
}
