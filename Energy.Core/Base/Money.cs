using System;
using System.ComponentModel;

namespace Energy.Base
{
    /// <summary>
    /// Money
    /// </summary>
    public class Money
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
            public class Array : Energy.Base.Collection.Array<Energy.Base.Money.Symbol>
            {
                public Energy.Base.Money.Symbol Add(char character)
                {
                    Energy.Base.Money.Symbol item;
                    item = new Energy.Base.Money.Symbol(character);
                    this.Add(item);
                    return item;
                }

                public Energy.Base.Money.Symbol Add(char character, string code, string english)
                {
                    Energy.Base.Money.Symbol item;
                    item = new Energy.Base.Money.Symbol(character, code, english);
                    this.Add(item);
                    return item;
                }

                public Energy.Base.Money.Symbol Add(char character, string code, string english, string local)
                {
                    Energy.Base.Money.Symbol item;
                    item = new Energy.Base.Money.Symbol(character, code, english, local);
                    this.Add(item);
                    return item;
                }
            }

            #endregion
        }

        public static Energy.Base.Money.Symbol.Array CreateDefaultCurrencySymbolArray()
        {
            Energy.Base.Money.Symbol.Array array = new Energy.Base.Money.Symbol.Array();
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

        public class Dictionary : System.Collections.Generic.Dictionary<string, Energy.Base.Money>
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

            public readonly Energy.Base.Money.Currency AED = new Energy.Base.Money.Currency("AED", "UAE Dirham", 784, 2);
            public readonly Energy.Base.Money.Currency AFN = new Energy.Base.Money.Currency("AFN", "Afghani", 971, 2, "؋");
            public readonly Energy.Base.Money.Currency ALL = new Energy.Base.Money.Currency("ALL", "Lek", 008, 2, "Lek");
            public readonly Energy.Base.Money.Currency AMD = new Energy.Base.Money.Currency("AMD", "Armenian Dram", 051, 2);
            public readonly Energy.Base.Money.Currency ANG = new Energy.Base.Money.Currency("ANG", "Netherlands Antillean Guilder", 532, 2);
            public readonly Energy.Base.Money.Currency AOA = new Energy.Base.Money.Currency("AOA", "Kwanza", 973, 2);
            public readonly Energy.Base.Money.Currency ARS = new Energy.Base.Money.Currency("ARS", "Argentine Peso", 032, 2);
            public readonly Energy.Base.Money.Currency AUD = new Energy.Base.Money.Currency("AUD", "Australian Dollar", 036, 2, "$");
            public readonly Energy.Base.Money.Currency AWG = new Energy.Base.Money.Currency("AWG", "Aruban Florin", 533, 2, "ƒ");
            public readonly Energy.Base.Money.Currency AZN = new Energy.Base.Money.Currency("AZN", "Azerbaijanian Manat", 944, 2, "ман");
            public readonly Energy.Base.Money.Currency BAM = new Energy.Base.Money.Currency("BAM", "Convertible Mark", 977, 2);
            public readonly Energy.Base.Money.Currency BBD = new Energy.Base.Money.Currency("BBD", "Barbados Dollar", 052, 2, "$");
            public readonly Energy.Base.Money.Currency BDT = new Energy.Base.Money.Currency("BDT", "Taka", 050, 2);
            public readonly Energy.Base.Money.Currency BGN = new Energy.Base.Money.Currency("BGN", "Bulgarian Lev", 975, 2);
            public readonly Energy.Base.Money.Currency BHD = new Energy.Base.Money.Currency("BHD", "Bahraini Dinar", 048, 3);
            public readonly Energy.Base.Money.Currency BIF = new Energy.Base.Money.Currency("BIF", "Burundi Franc", 108, 0);
            public readonly Energy.Base.Money.Currency BMD = new Energy.Base.Money.Currency("BMD", "Bermudian Dollar", 060, 2, "$");
            public readonly Energy.Base.Money.Currency BND = new Energy.Base.Money.Currency("BND", "Brunei Dollar", 096, 2, "$");
            public readonly Energy.Base.Money.Currency BOB = new Energy.Base.Money.Currency("BOB", "Boliviano", 068, 2, "$b");
            public readonly Energy.Base.Money.Currency BOV = new Energy.Base.Money.Currency("BOV", "Mvdol", 984, 2);
            public readonly Energy.Base.Money.Currency BRL = new Energy.Base.Money.Currency("BRL", "Brazilian Real", 986, 2);
            public readonly Energy.Base.Money.Currency BSD = new Energy.Base.Money.Currency("BSD", "Bahamian Dollar", 044, 2, "$");
            public readonly Energy.Base.Money.Currency BTN = new Energy.Base.Money.Currency("BTN", "Ngultrum", 064, 2);
            public readonly Energy.Base.Money.Currency BWP = new Energy.Base.Money.Currency("BWP", "Pula", 072, 2);
            public readonly Energy.Base.Money.Currency BYR = new Energy.Base.Money.Currency("BYR", "Belarusian Ruble", 974, 0);
            public readonly Energy.Base.Money.Currency BZD = new Energy.Base.Money.Currency("BZD", "Belize Dollar", 084, 2, "$");
            public readonly Energy.Base.Money.Currency CAD = new Energy.Base.Money.Currency("CAD", "Canadian Dollar", 124, 2, "$");
            public readonly Energy.Base.Money.Currency CDF = new Energy.Base.Money.Currency("CDF", "Congolese Franc", 976, 2);
            public readonly Energy.Base.Money.Currency CHE = new Energy.Base.Money.Currency("CHE", "WIR Euro", 947, 2);
            public readonly Energy.Base.Money.Currency CHF = new Energy.Base.Money.Currency("CHF", "Swiss Franc", 756, 2);
            public readonly Energy.Base.Money.Currency CHW = new Energy.Base.Money.Currency("CHW", "WIR Franc", 948, 2);
            public readonly Energy.Base.Money.Currency CLF = new Energy.Base.Money.Currency("CLF", "Unidad de Fomento", 990, 4);
            public readonly Energy.Base.Money.Currency CLP = new Energy.Base.Money.Currency("CLP", "Chilean Peso", 152, 0);
            public readonly Energy.Base.Money.Currency CNY = new Energy.Base.Money.Currency("CNY", "Yuan Renminbi", 156, 2);
            public readonly Energy.Base.Money.Currency COP = new Energy.Base.Money.Currency("COP", "Colombian Peso", 170, 2);
            public readonly Energy.Base.Money.Currency COU = new Energy.Base.Money.Currency("COU", "Unidad de Valor Real", 970, 2);
            public readonly Energy.Base.Money.Currency CRC = new Energy.Base.Money.Currency("CRC", "Costa Rican Colon", 188, 2);
            public readonly Energy.Base.Money.Currency CUC = new Energy.Base.Money.Currency("CUC", "Peso Convertible", 931, 2);
            public readonly Energy.Base.Money.Currency CUP = new Energy.Base.Money.Currency("CUP", "Cuban Peso", 192, 2, "₱");
            public readonly Energy.Base.Money.Currency CVE = new Energy.Base.Money.Currency("CVE", "Cabo Verde Escudo", 132, 2);
            public readonly Energy.Base.Money.Currency CZK = new Energy.Base.Money.Currency("CZK", "Czech Koruna", 203, 2, "Kč");
            public readonly Energy.Base.Money.Currency DJF = new Energy.Base.Money.Currency("DJF", "Djibouti Franc", 262, 0);
            public readonly Energy.Base.Money.Currency DKK = new Energy.Base.Money.Currency("DKK", "Danish Krone", 208, 2, "kr");
            public readonly Energy.Base.Money.Currency DOP = new Energy.Base.Money.Currency("DOP", "Dominican Peso", 214, 2);
            public readonly Energy.Base.Money.Currency DZD = new Energy.Base.Money.Currency("DZD", "Algerian Dinar", 012, 2);
            public readonly Energy.Base.Money.Currency EGP = new Energy.Base.Money.Currency("EGP", "Egyptian Pound", 818, 2, "£");
            public readonly Energy.Base.Money.Currency ERN = new Energy.Base.Money.Currency("ERN", "Nakfa", 232, 2);
            public readonly Energy.Base.Money.Currency ETB = new Energy.Base.Money.Currency("ETB", "Ethiopian Birr", 230, 2);
            public readonly Energy.Base.Money.Currency EUR = new Energy.Base.Money.Currency("EUR", "Euro", 978, 2, "€");
            public readonly Energy.Base.Money.Currency FJD = new Energy.Base.Money.Currency("FJD", "Fiji Dollar", 242, 2);
            public readonly Energy.Base.Money.Currency FKP = new Energy.Base.Money.Currency("FKP", "Falkland Islands Pound", 238, 2, "£");
            public readonly Energy.Base.Money.Currency GBP = new Energy.Base.Money.Currency("GBP", "Pound Sterling", 826, 2, "£");
            public readonly Energy.Base.Money.Currency GEL = new Energy.Base.Money.Currency("GEL", "Lari", 981, 2);
            public readonly Energy.Base.Money.Currency GHS = new Energy.Base.Money.Currency("GHS", "Ghana Cedi", 936, 2, "¢");
            public readonly Energy.Base.Money.Currency GIP = new Energy.Base.Money.Currency("GIP", "Gibraltar Pound", 292, 2, "£");
            public readonly Energy.Base.Money.Currency GMD = new Energy.Base.Money.Currency("GMD", "Dalasi", 270, 2);
            public readonly Energy.Base.Money.Currency GNF = new Energy.Base.Money.Currency("GNF", "Guinea Franc", 324, 0);
            public readonly Energy.Base.Money.Currency GTQ = new Energy.Base.Money.Currency("GTQ", "Quetzal", 320, 2, "Q");
            public readonly Energy.Base.Money.Currency GYD = new Energy.Base.Money.Currency("GYD", "Guyana Dollar", 328, 2);
            public readonly Energy.Base.Money.Currency HKD = new Energy.Base.Money.Currency("HKD", "Hong Kong Dollar", 344, 2, "$");
            public readonly Energy.Base.Money.Currency HNL = new Energy.Base.Money.Currency("HNL", "Lempira", 340, 2);
            public readonly Energy.Base.Money.Currency HRK = new Energy.Base.Money.Currency("HRK", "Kuna", 191, 2);
            public readonly Energy.Base.Money.Currency HTG = new Energy.Base.Money.Currency("HTG", "Gourde", 332, 2);
            public readonly Energy.Base.Money.Currency HUF = new Energy.Base.Money.Currency("HUF", "Forint", 348, 2);
            public readonly Energy.Base.Money.Currency IDR = new Energy.Base.Money.Currency("IDR", "Rupiah", 360, 2);
            public readonly Energy.Base.Money.Currency ILS = new Energy.Base.Money.Currency("ILS", "New Israeli Sheqel", 376, 2, "₪");
            public readonly Energy.Base.Money.Currency INR = new Energy.Base.Money.Currency("INR", "Indian Rupee", 356, 2);
            public readonly Energy.Base.Money.Currency IQD = new Energy.Base.Money.Currency("IQD", "Iraqi Dinar", 368, 3);
            public readonly Energy.Base.Money.Currency IRR = new Energy.Base.Money.Currency("IRR", "Iranian Rial", 364, 2, "﷼");
            public readonly Energy.Base.Money.Currency ISK = new Energy.Base.Money.Currency("ISK", "Iceland Krona", 352, 0, "kr");
            public readonly Energy.Base.Money.Currency JMD = new Energy.Base.Money.Currency("JMD", "Jamaican Dollar", 388, 2);
            public readonly Energy.Base.Money.Currency JOD = new Energy.Base.Money.Currency("JOD", "Jordanian Dinar", 400, 3);
            public readonly Energy.Base.Money.Currency JPY = new Energy.Base.Money.Currency("JPY", "Yen", 392, 0, "¥");
            public readonly Energy.Base.Money.Currency KES = new Energy.Base.Money.Currency("KES", "Kenyan Shilling", 404, 2);
            public readonly Energy.Base.Money.Currency KGS = new Energy.Base.Money.Currency("KGS", "Som", 417, 2);
            public readonly Energy.Base.Money.Currency KHR = new Energy.Base.Money.Currency("KHR", "Riel", 116, 2);
            public readonly Energy.Base.Money.Currency KMF = new Energy.Base.Money.Currency("KMF", "Comoro Franc", 174, 0);
            public readonly Energy.Base.Money.Currency KPW = new Energy.Base.Money.Currency("KPW", "North Korean Won", 408, 2, "₩");
            public readonly Energy.Base.Money.Currency KRW = new Energy.Base.Money.Currency("KRW", "Won", 410, 0, "₩");
            public readonly Energy.Base.Money.Currency KWD = new Energy.Base.Money.Currency("KWD", "Kuwaiti Dinar", 414, 3);
            public readonly Energy.Base.Money.Currency KYD = new Energy.Base.Money.Currency("KYD", "Cayman Islands Dollar", 136, 2);
            public readonly Energy.Base.Money.Currency KZT = new Energy.Base.Money.Currency("KZT", "Tenge", 398, 2, "лв");
            public readonly Energy.Base.Money.Currency LAK = new Energy.Base.Money.Currency("LAK", "Kip", 418, 2, "₭");
            public readonly Energy.Base.Money.Currency LBP = new Energy.Base.Money.Currency("LBP", "Lebanese Pound", 422, 2);
            public readonly Energy.Base.Money.Currency LKR = new Energy.Base.Money.Currency("LKR", "Sri Lanka Rupee", 144, 2);
            public readonly Energy.Base.Money.Currency LRD = new Energy.Base.Money.Currency("LRD", "Liberian Dollar", 430, 2);
            public readonly Energy.Base.Money.Currency LSL = new Energy.Base.Money.Currency("LSL", "Loti", 426, 2);
            public readonly Energy.Base.Money.Currency LYD = new Energy.Base.Money.Currency("LYD", "Libyan Dinar", 434, 3);
            public readonly Energy.Base.Money.Currency MAD = new Energy.Base.Money.Currency("MAD", "Moroccan Dirham", 504, 2);
            public readonly Energy.Base.Money.Currency MDL = new Energy.Base.Money.Currency("MDL", "Moldovan Leu", 498, 2);
            public readonly Energy.Base.Money.Currency MGA = new Energy.Base.Money.Currency("MGA", "Malagasy Ariary", 969, 2);
            public readonly Energy.Base.Money.Currency MKD = new Energy.Base.Money.Currency("MKD", "Denar", 807, 2);
            public readonly Energy.Base.Money.Currency MMK = new Energy.Base.Money.Currency("MMK", "Kyat", 104, 2);
            public readonly Energy.Base.Money.Currency MNT = new Energy.Base.Money.Currency("MNT", "Tugrik", 496, 2);
            public readonly Energy.Base.Money.Currency MOP = new Energy.Base.Money.Currency("MOP", "Pataca", 446, 2);
            public readonly Energy.Base.Money.Currency MRO = new Energy.Base.Money.Currency("MRO", "Ouguiya", 478, 2);
            public readonly Energy.Base.Money.Currency MUR = new Energy.Base.Money.Currency("MUR", "Mauritius Rupee", 480, 2);
            public readonly Energy.Base.Money.Currency MVR = new Energy.Base.Money.Currency("MVR", "Rufiyaa", 462, 2);
            public readonly Energy.Base.Money.Currency MWK = new Energy.Base.Money.Currency("MWK", "Malawi Kwacha", 454, 2);
            public readonly Energy.Base.Money.Currency MXN = new Energy.Base.Money.Currency("MXN", "Mexican Peso", 484, 2);
            public readonly Energy.Base.Money.Currency MXV = new Energy.Base.Money.Currency("MXV", "Mexican Unidad de Inversion (UDI)", 979, 2);
            public readonly Energy.Base.Money.Currency MYR = new Energy.Base.Money.Currency("MYR", "Malaysian Ringgit", 458, 2);
            public readonly Energy.Base.Money.Currency MZN = new Energy.Base.Money.Currency("MZN", "Mozambique Metical", 943, 2);
            public readonly Energy.Base.Money.Currency NAD = new Energy.Base.Money.Currency("NAD", "Namibia Dollar", 516, 2);
            public readonly Energy.Base.Money.Currency NGN = new Energy.Base.Money.Currency("NGN", "Naira", 566, 2);
            public readonly Energy.Base.Money.Currency NIO = new Energy.Base.Money.Currency("NIO", "Cordoba Oro", 558, 2);
            public readonly Energy.Base.Money.Currency NOK = new Energy.Base.Money.Currency("NOK", "Norwegian Krone", 578, 2);
            public readonly Energy.Base.Money.Currency NPR = new Energy.Base.Money.Currency("NPR", "Nepalese Rupee", 524, 2);
            public readonly Energy.Base.Money.Currency NZD = new Energy.Base.Money.Currency("NZD", "New Zealand Dollar", 554, 2);
            public readonly Energy.Base.Money.Currency OMR = new Energy.Base.Money.Currency("OMR", "Rial Omani", 512, 3);
            public readonly Energy.Base.Money.Currency PAB = new Energy.Base.Money.Currency("PAB", "Balboa", 590, 2);
            public readonly Energy.Base.Money.Currency PEN = new Energy.Base.Money.Currency("PEN", "Sol", 604, 2);
            public readonly Energy.Base.Money.Currency PGK = new Energy.Base.Money.Currency("PGK", "Kina", 598, 2);
            public readonly Energy.Base.Money.Currency PHP = new Energy.Base.Money.Currency("PHP", "Philippine Peso", 608, 2, "₱");
            public readonly Energy.Base.Money.Currency PKR = new Energy.Base.Money.Currency("PKR", "Pakistan Rupee", 586, 2);
            public readonly Energy.Base.Money.Currency PLN = new Energy.Base.Money.Currency("PLN", "Zloty", 985, 2, "zł");
            public readonly Energy.Base.Money.Currency PYG = new Energy.Base.Money.Currency("PYG", "Guarani", 600, 0);
            public readonly Energy.Base.Money.Currency QAR = new Energy.Base.Money.Currency("QAR", "Qatari Rial", 634, 2);
            public readonly Energy.Base.Money.Currency RON = new Energy.Base.Money.Currency("RON", "Romanian Leu", 946, 2);
            public readonly Energy.Base.Money.Currency RSD = new Energy.Base.Money.Currency("RSD", "Serbian Dinar", 941, 2);
            public readonly Energy.Base.Money.Currency RUB = new Energy.Base.Money.Currency("RUB", "Russian Ruble", 643, 2);
            public readonly Energy.Base.Money.Currency RWF = new Energy.Base.Money.Currency("RWF", "Rwanda Franc", 646, 0);
            public readonly Energy.Base.Money.Currency SAR = new Energy.Base.Money.Currency("SAR", "Saudi Riyal", 682, 2);
            public readonly Energy.Base.Money.Currency SBD = new Energy.Base.Money.Currency("SBD", "Solomon Islands Dollar", 090, 2);
            public readonly Energy.Base.Money.Currency SCR = new Energy.Base.Money.Currency("SCR", "Seychelles Rupee", 690, 2);
            public readonly Energy.Base.Money.Currency SDG = new Energy.Base.Money.Currency("SDG", "Sudanese Pound", 938, 2);
            public readonly Energy.Base.Money.Currency SEK = new Energy.Base.Money.Currency("SEK", "Swedish Krona", 752, 2);
            public readonly Energy.Base.Money.Currency SGD = new Energy.Base.Money.Currency("SGD", "Singapore Dollar", 702, 2);
            public readonly Energy.Base.Money.Currency SHP = new Energy.Base.Money.Currency("SHP", "Saint Helena Pound", 654, 2);
            public readonly Energy.Base.Money.Currency SLL = new Energy.Base.Money.Currency("SLL", "Leone", 694, 2);
            public readonly Energy.Base.Money.Currency SOS = new Energy.Base.Money.Currency("SOS", "Somali Shilling", 706, 2);
            public readonly Energy.Base.Money.Currency SRD = new Energy.Base.Money.Currency("SRD", "Surinam Dollar", 968, 2);
            public readonly Energy.Base.Money.Currency SSP = new Energy.Base.Money.Currency("SSP", "South Sudanese Pound", 728, 2);
            public readonly Energy.Base.Money.Currency STD = new Energy.Base.Money.Currency("STD", "Dobra", 678, 2);
            public readonly Energy.Base.Money.Currency SVC = new Energy.Base.Money.Currency("SVC", "El Salvador Colon", 222, 2);
            public readonly Energy.Base.Money.Currency SYP = new Energy.Base.Money.Currency("SYP", "Syrian Pound", 760, 2);
            public readonly Energy.Base.Money.Currency SZL = new Energy.Base.Money.Currency("SZL", "Lilangeni", 748, 2);
            public readonly Energy.Base.Money.Currency THB = new Energy.Base.Money.Currency("THB", "Baht", 764, 2);
            public readonly Energy.Base.Money.Currency TJS = new Energy.Base.Money.Currency("TJS", "Somoni", 972, 2);
            public readonly Energy.Base.Money.Currency TMT = new Energy.Base.Money.Currency("TMT", "Turkmenistan New Manat", 934, 2);
            public readonly Energy.Base.Money.Currency TND = new Energy.Base.Money.Currency("TND", "Tunisian Dinar", 788, 3);
            public readonly Energy.Base.Money.Currency TOP = new Energy.Base.Money.Currency("TOP", "Pa’anga", 776, 2);
            public readonly Energy.Base.Money.Currency TRY = new Energy.Base.Money.Currency("TRY", "Turkish Lira", 949, 2);
            public readonly Energy.Base.Money.Currency TTD = new Energy.Base.Money.Currency("TTD", "Trinidad and Tobago Dollar", 780, 2, "TT$");
            public readonly Energy.Base.Money.Currency TWD = new Energy.Base.Money.Currency("TWD", "New Taiwan Dollar", 901, 2, "NT$");
            public readonly Energy.Base.Money.Currency TZS = new Energy.Base.Money.Currency("TZS", "Tanzanian Shilling", 834, 2);
            public readonly Energy.Base.Money.Currency UAH = new Energy.Base.Money.Currency("UAH", "Hryvnia", 980, 2, "₴");
            public readonly Energy.Base.Money.Currency UGX = new Energy.Base.Money.Currency("UGX", "Uganda Shilling", 800, 0);
            public readonly Energy.Base.Money.Currency USD = new Energy.Base.Money.Currency("USD", "US Dollar", 840, 2);
            public readonly Energy.Base.Money.Currency USN = new Energy.Base.Money.Currency("USN", "US Dollar (Next day)", 997, 2);
            public readonly Energy.Base.Money.Currency UYI = new Energy.Base.Money.Currency("UYI", "Uruguay Peso en Unidades Indexadas (URUIURUI)", 940, 0);
            public readonly Energy.Base.Money.Currency UYU = new Energy.Base.Money.Currency("UYU", "Peso Uruguayo", 858, 2);
            public readonly Energy.Base.Money.Currency UZS = new Energy.Base.Money.Currency("UZS", "Uzbekistan Sum", 860, 2);
            public readonly Energy.Base.Money.Currency VEF = new Energy.Base.Money.Currency("VEF", "Bolívar", 937, 2);
            public readonly Energy.Base.Money.Currency VND = new Energy.Base.Money.Currency("VND", "Dong", 704, 0);
            public readonly Energy.Base.Money.Currency VUV = new Energy.Base.Money.Currency("VUV", "Vatu", 548, 0);
            public readonly Energy.Base.Money.Currency WST = new Energy.Base.Money.Currency("WST", "Tala", 882, 2);
            public readonly Energy.Base.Money.Currency XAF = new Energy.Base.Money.Currency("XAF", "CFA Franc BEAC", 950, 0);
            public readonly Energy.Base.Money.Currency XAG = new Energy.Base.Money.Currency("XAG", "Silver", 961, 0);
            public readonly Energy.Base.Money.Currency XAU = new Energy.Base.Money.Currency("XAU", "Gold", 959, 0);
            public readonly Energy.Base.Money.Currency XBA = new Energy.Base.Money.Currency("XBA", "Bond Markets Unit European Composite Unit (EURCO)", 955, 0);
            public readonly Energy.Base.Money.Currency XBB = new Energy.Base.Money.Currency("XBB", "Bond Markets Unit European Monetary Unit (E.M.U.-6)", 956, 0);
            public readonly Energy.Base.Money.Currency XBC = new Energy.Base.Money.Currency("XBC", "Bond Markets Unit European Unit of Account 9 (E.U.A.-9)", 957, 0);
            public readonly Energy.Base.Money.Currency XBD = new Energy.Base.Money.Currency("XBD", "Bond Markets Unit European Unit of Account 17 (E.U.A.-17)", 958, 0);
            public readonly Energy.Base.Money.Currency XCD = new Energy.Base.Money.Currency("XCD", "East Caribbean Dollar", 951, 2);
            public readonly Energy.Base.Money.Currency XDR = new Energy.Base.Money.Currency("XDR", "SDR (Special Drawing Right)", 960, 0);
            public readonly Energy.Base.Money.Currency XOF = new Energy.Base.Money.Currency("XOF", "CFA Franc BCEAO", 952, 0);
            public readonly Energy.Base.Money.Currency XPD = new Energy.Base.Money.Currency("XPD", "Palladium", 964, 0);
            public readonly Energy.Base.Money.Currency XPF = new Energy.Base.Money.Currency("XPF", "CFP Franc", 953, 0);
            public readonly Energy.Base.Money.Currency XPT = new Energy.Base.Money.Currency("XPT", "Platinum", 962, 0);
            public readonly Energy.Base.Money.Currency XSU = new Energy.Base.Money.Currency("XSU", "Sucre", 994, 0);
            public readonly Energy.Base.Money.Currency XTS = new Energy.Base.Money.Currency("XTS", "Codes specifically reserved for testing purposes", 963, 0);
            public readonly Energy.Base.Money.Currency XUA = new Energy.Base.Money.Currency("XUA", "ADB Unit of Account", 965, 0);
            public readonly Energy.Base.Money.Currency XXX = new Energy.Base.Money.Currency("XXX", "The codes assigned for transactions where no readonly Currency is involved", 999, 0);
            public readonly Energy.Base.Money.Currency YER = new Energy.Base.Money.Currency("YER", "Yemeni Rial", 886, 2);
            public readonly Energy.Base.Money.Currency ZAR = new Energy.Base.Money.Currency("ZAR", "Rand", 710, 2);
            public readonly Energy.Base.Money.Currency ZMW = new Energy.Base.Money.Currency("ZMW", "Zambian Kwacha", 967, 2);
            public readonly Energy.Base.Money.Currency ZWL = new Energy.Base.Money.Currency("ZWL", "Zimbabwe Dollar", 932, 2, "$");

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

        #region Value

        /// <summary>
        /// Money value
        /// </summary>
        public struct Value
        {
            /// <summary>
            /// Amount
            /// </summary>
            public decimal Amount { get; private set; }

            /// <summary>
            /// Currency
            /// </summary>
            public Currency Currency { get; private set; }

            public Value(decimal amount, Currency currency)
                : this()
            {
                if (currency == null)
                {
                    //throw new ArgumentNullException(nameof(currency));
                    throw new ArgumentNullException();
                }

                Amount = amount;
                Currency = currency;
            }

            public Value(double amount, Currency currency)
                : this((decimal)amount, currency)
            { }

            public Value Add(Value arg)
            {
                EnsureSameCurrency(this, arg);
                return new Value(Amount + arg.Amount, Currency);
            }

            public static Value operator +(Value m1, Value m2)
            {
                return m1.Add(m2);
            }

            public Value Subtract(Value arg)
            {
                EnsureSameCurrency(this, arg);
                return new Value(Amount - arg.Amount, Currency);
            }

            public static Value operator -(Value m1, Value m2)
            {
                return m1.Subtract(m2);
            }

            private void EnsureSameCurrency(Value arg1, Value arg2)
            {
                if (arg1.Currency != arg2.Currency)
                {
                    throw new InvalidOperationException("Can't add amounts with different currencies.");
                }
            }

            public override string ToString()
            {
                return String.Concat(Amount, " ", Currency);
            }

            //public static implicit operator Money(string text)
            //{
            //    return new Money();
            //}
        }
        #endregion
    }
}
