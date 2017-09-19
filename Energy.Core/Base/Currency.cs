using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Energy.Base
{
/// <summary>
/// Currency information
/// </summary>
    public class Currency
    {
        #region Class

        public string Code { get; private set; }

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

#endregion

        #region Dictionary

        public class Dictionary : System.Collections.Generic.Dictionary<string, Currency>
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

            public readonly Currency AED = new Currency("AED", "UAE Dirham", 784, 2);
            public readonly Currency AFN = new Currency("AFN", "Afghani", 971, 2, "؋");
            public readonly Currency ALL = new Currency("ALL", "Lek", 008, 2, "Lek");
            public readonly Currency AMD = new Currency("AMD", "Armenian Dram", 051, 2);
            public readonly Currency ANG = new Currency("ANG", "Netherlands Antillean Guilder", 532, 2);
            public readonly Currency AOA = new Currency("AOA", "Kwanza", 973, 2);
            public readonly Currency ARS = new Currency("ARS", "Argentine Peso", 032, 2);
            public readonly Currency AUD = new Currency("AUD", "Australian Dollar", 036, 2, "$");
            public readonly Currency AWG = new Currency("AWG", "Aruban Florin", 533, 2, "ƒ");
            public readonly Currency AZN = new Currency("AZN", "Azerbaijanian Manat", 944, 2, "ман");
            public readonly Currency BAM = new Currency("BAM", "Convertible Mark", 977, 2);
            public readonly Currency BBD = new Currency("BBD", "Barbados Dollar", 052, 2, "$");
            public readonly Currency BDT = new Currency("BDT", "Taka", 050, 2);
            public readonly Currency BGN = new Currency("BGN", "Bulgarian Lev", 975, 2);
            public readonly Currency BHD = new Currency("BHD", "Bahraini Dinar", 048, 3);
            public readonly Currency BIF = new Currency("BIF", "Burundi Franc", 108, 0);
            public readonly Currency BMD = new Currency("BMD", "Bermudian Dollar", 060, 2, "$");
            public readonly Currency BND = new Currency("BND", "Brunei Dollar", 096, 2, "$");
            public readonly Currency BOB = new Currency("BOB", "Boliviano", 068, 2, "$b");
            public readonly Currency BOV = new Currency("BOV", "Mvdol", 984, 2);
            public readonly Currency BRL = new Currency("BRL", "Brazilian Real", 986, 2);
            public readonly Currency BSD = new Currency("BSD", "Bahamian Dollar", 044, 2, "$");
            public readonly Currency BTN = new Currency("BTN", "Ngultrum", 064, 2);
            public readonly Currency BWP = new Currency("BWP", "Pula", 072, 2);
            public readonly Currency BYR = new Currency("BYR", "Belarusian Ruble", 974, 0);
            public readonly Currency BZD = new Currency("BZD", "Belize Dollar", 084, 2, "$");
            public readonly Currency CAD = new Currency("CAD", "Canadian Dollar", 124, 2, "$");
            public readonly Currency CDF = new Currency("CDF", "Congolese Franc", 976, 2);
            public readonly Currency CHE = new Currency("CHE", "WIR Euro", 947, 2);
            public readonly Currency CHF = new Currency("CHF", "Swiss Franc", 756, 2);
            public readonly Currency CHW = new Currency("CHW", "WIR Franc", 948, 2);
            public readonly Currency CLF = new Currency("CLF", "Unidad de Fomento", 990, 4);
            public readonly Currency CLP = new Currency("CLP", "Chilean Peso", 152, 0);
            public readonly Currency CNY = new Currency("CNY", "Yuan Renminbi", 156, 2);
            public readonly Currency COP = new Currency("COP", "Colombian Peso", 170, 2);
            public readonly Currency COU = new Currency("COU", "Unidad de Valor Real", 970, 2);
            public readonly Currency CRC = new Currency("CRC", "Costa Rican Colon", 188, 2);
            public readonly Currency CUC = new Currency("CUC", "Peso Convertible", 931, 2);
            public readonly Currency CUP = new Currency("CUP", "Cuban Peso", 192, 2, "₱");
            public readonly Currency CVE = new Currency("CVE", "Cabo Verde Escudo", 132, 2);
            public readonly Currency CZK = new Currency("CZK", "Czech Koruna", 203, 2, "Kč");
            public readonly Currency DJF = new Currency("DJF", "Djibouti Franc", 262, 0);
            public readonly Currency DKK = new Currency("DKK", "Danish Krone", 208, 2, "kr");
            public readonly Currency DOP = new Currency("DOP", "Dominican Peso", 214, 2);
            public readonly Currency DZD = new Currency("DZD", "Algerian Dinar", 012, 2);
            public readonly Currency EGP = new Currency("EGP", "Egyptian Pound", 818, 2, "£");
            public readonly Currency ERN = new Currency("ERN", "Nakfa", 232, 2);
            public readonly Currency ETB = new Currency("ETB", "Ethiopian Birr", 230, 2);
            public readonly Currency EUR = new Currency("EUR", "Euro", 978, 2, "€");
            public readonly Currency FJD = new Currency("FJD", "Fiji Dollar", 242, 2);
            public readonly Currency FKP = new Currency("FKP", "Falkland Islands Pound", 238, 2, "£");
            public readonly Currency GBP = new Currency("GBP", "Pound Sterling", 826, 2, "£");
            public readonly Currency GEL = new Currency("GEL", "Lari", 981, 2);
            public readonly Currency GHS = new Currency("GHS", "Ghana Cedi", 936, 2, "¢");
            public readonly Currency GIP = new Currency("GIP", "Gibraltar Pound", 292, 2, "£");
            public readonly Currency GMD = new Currency("GMD", "Dalasi", 270, 2);
            public readonly Currency GNF = new Currency("GNF", "Guinea Franc", 324, 0);
            public readonly Currency GTQ = new Currency("GTQ", "Quetzal", 320, 2, "Q");
            public readonly Currency GYD = new Currency("GYD", "Guyana Dollar", 328, 2);
            public readonly Currency HKD = new Currency("HKD", "Hong Kong Dollar", 344, 2, "$");
            public readonly Currency HNL = new Currency("HNL", "Lempira", 340, 2);
            public readonly Currency HRK = new Currency("HRK", "Kuna", 191, 2);
            public readonly Currency HTG = new Currency("HTG", "Gourde", 332, 2);
            public readonly Currency HUF = new Currency("HUF", "Forint", 348, 2);
            public readonly Currency IDR = new Currency("IDR", "Rupiah", 360, 2);
            public readonly Currency ILS = new Currency("ILS", "New Israeli Sheqel", 376, 2, "₪");
            public readonly Currency INR = new Currency("INR", "Indian Rupee", 356, 2);
            public readonly Currency IQD = new Currency("IQD", "Iraqi Dinar", 368, 3);
            public readonly Currency IRR = new Currency("IRR", "Iranian Rial", 364, 2, "﷼");
            public readonly Currency ISK = new Currency("ISK", "Iceland Krona", 352, 0, "kr");
            public readonly Currency JMD = new Currency("JMD", "Jamaican Dollar", 388, 2);
            public readonly Currency JOD = new Currency("JOD", "Jordanian Dinar", 400, 3);
            public readonly Currency JPY = new Currency("JPY", "Yen", 392, 0, "¥");
            public readonly Currency KES = new Currency("KES", "Kenyan Shilling", 404, 2);
            public readonly Currency KGS = new Currency("KGS", "Som", 417, 2);
            public readonly Currency KHR = new Currency("KHR", "Riel", 116, 2);
            public readonly Currency KMF = new Currency("KMF", "Comoro Franc", 174, 0);
            public readonly Currency KPW = new Currency("KPW", "North Korean Won", 408, 2, "₩");
            public readonly Currency KRW = new Currency("KRW", "Won", 410, 0, "₩");
            public readonly Currency KWD = new Currency("KWD", "Kuwaiti Dinar", 414, 3);
            public readonly Currency KYD = new Currency("KYD", "Cayman Islands Dollar", 136, 2);
            public readonly Currency KZT = new Currency("KZT", "Tenge", 398, 2, "лв");
            public readonly Currency LAK = new Currency("LAK", "Kip", 418, 2, "₭");
            public readonly Currency LBP = new Currency("LBP", "Lebanese Pound", 422, 2);
            public readonly Currency LKR = new Currency("LKR", "Sri Lanka Rupee", 144, 2);
            public readonly Currency LRD = new Currency("LRD", "Liberian Dollar", 430, 2);
            public readonly Currency LSL = new Currency("LSL", "Loti", 426, 2);
            public readonly Currency LYD = new Currency("LYD", "Libyan Dinar", 434, 3);
            public readonly Currency MAD = new Currency("MAD", "Moroccan Dirham", 504, 2);
            public readonly Currency MDL = new Currency("MDL", "Moldovan Leu", 498, 2);
            public readonly Currency MGA = new Currency("MGA", "Malagasy Ariary", 969, 2);
            public readonly Currency MKD = new Currency("MKD", "Denar", 807, 2);
            public readonly Currency MMK = new Currency("MMK", "Kyat", 104, 2);
            public readonly Currency MNT = new Currency("MNT", "Tugrik", 496, 2);
            public readonly Currency MOP = new Currency("MOP", "Pataca", 446, 2);
            public readonly Currency MRO = new Currency("MRO", "Ouguiya", 478, 2);
            public readonly Currency MUR = new Currency("MUR", "Mauritius Rupee", 480, 2);
            public readonly Currency MVR = new Currency("MVR", "Rufiyaa", 462, 2);
            public readonly Currency MWK = new Currency("MWK", "Malawi Kwacha", 454, 2);
            public readonly Currency MXN = new Currency("MXN", "Mexican Peso", 484, 2);
            public readonly Currency MXV = new Currency("MXV", "Mexican Unidad de Inversion (UDI)", 979, 2);
            public readonly Currency MYR = new Currency("MYR", "Malaysian Ringgit", 458, 2);
            public readonly Currency MZN = new Currency("MZN", "Mozambique Metical", 943, 2);
            public readonly Currency NAD = new Currency("NAD", "Namibia Dollar", 516, 2);
            public readonly Currency NGN = new Currency("NGN", "Naira", 566, 2);
            public readonly Currency NIO = new Currency("NIO", "Cordoba Oro", 558, 2);
            public readonly Currency NOK = new Currency("NOK", "Norwegian Krone", 578, 2);
            public readonly Currency NPR = new Currency("NPR", "Nepalese Rupee", 524, 2);
            public readonly Currency NZD = new Currency("NZD", "New Zealand Dollar", 554, 2);
            public readonly Currency OMR = new Currency("OMR", "Rial Omani", 512, 3);
            public readonly Currency PAB = new Currency("PAB", "Balboa", 590, 2);
            public readonly Currency PEN = new Currency("PEN", "Sol", 604, 2);
            public readonly Currency PGK = new Currency("PGK", "Kina", 598, 2);
            public readonly Currency PHP = new Currency("PHP", "Philippine Peso", 608, 2, "₱");
            public readonly Currency PKR = new Currency("PKR", "Pakistan Rupee", 586, 2);
            public readonly Currency PLN = new Currency("PLN", "Zloty", 985, 2, "zł");
            public readonly Currency PYG = new Currency("PYG", "Guarani", 600, 0);
            public readonly Currency QAR = new Currency("QAR", "Qatari Rial", 634, 2);
            public readonly Currency RON = new Currency("RON", "Romanian Leu", 946, 2);
            public readonly Currency RSD = new Currency("RSD", "Serbian Dinar", 941, 2);
            public readonly Currency RUB = new Currency("RUB", "Russian Ruble", 643, 2);
            public readonly Currency RWF = new Currency("RWF", "Rwanda Franc", 646, 0);
            public readonly Currency SAR = new Currency("SAR", "Saudi Riyal", 682, 2);
            public readonly Currency SBD = new Currency("SBD", "Solomon Islands Dollar", 090, 2);
            public readonly Currency SCR = new Currency("SCR", "Seychelles Rupee", 690, 2);
            public readonly Currency SDG = new Currency("SDG", "Sudanese Pound", 938, 2);
            public readonly Currency SEK = new Currency("SEK", "Swedish Krona", 752, 2);
            public readonly Currency SGD = new Currency("SGD", "Singapore Dollar", 702, 2);
            public readonly Currency SHP = new Currency("SHP", "Saint Helena Pound", 654, 2);
            public readonly Currency SLL = new Currency("SLL", "Leone", 694, 2);
            public readonly Currency SOS = new Currency("SOS", "Somali Shilling", 706, 2);
            public readonly Currency SRD = new Currency("SRD", "Surinam Dollar", 968, 2);
            public readonly Currency SSP = new Currency("SSP", "South Sudanese Pound", 728, 2);
            public readonly Currency STD = new Currency("STD", "Dobra", 678, 2);
            public readonly Currency SVC = new Currency("SVC", "El Salvador Colon", 222, 2);
            public readonly Currency SYP = new Currency("SYP", "Syrian Pound", 760, 2);
            public readonly Currency SZL = new Currency("SZL", "Lilangeni", 748, 2);
            public readonly Currency THB = new Currency("THB", "Baht", 764, 2);
            public readonly Currency TJS = new Currency("TJS", "Somoni", 972, 2);
            public readonly Currency TMT = new Currency("TMT", "Turkmenistan New Manat", 934, 2);
            public readonly Currency TND = new Currency("TND", "Tunisian Dinar", 788, 3);
            public readonly Currency TOP = new Currency("TOP", "Pa’anga", 776, 2);
            public readonly Currency TRY = new Currency("TRY", "Turkish Lira", 949, 2);
            public readonly Currency TTD = new Currency("TTD", "Trinidad and Tobago Dollar", 780, 2, "TT$");
            public readonly Currency TWD = new Currency("TWD", "New Taiwan Dollar", 901, 2, "NT$");
            public readonly Currency TZS = new Currency("TZS", "Tanzanian Shilling", 834, 2);
            public readonly Currency UAH = new Currency("UAH", "Hryvnia", 980, 2, "₴");
            public readonly Currency UGX = new Currency("UGX", "Uganda Shilling", 800, 0);
            public readonly Currency USD = new Currency("USD", "US Dollar", 840, 2);
            public readonly Currency USN = new Currency("USN", "US Dollar (Next day)", 997, 2);
            public readonly Currency UYI = new Currency("UYI", "Uruguay Peso en Unidades Indexadas (URUIURUI)", 940, 0);
            public readonly Currency UYU = new Currency("UYU", "Peso Uruguayo", 858, 2);
            public readonly Currency UZS = new Currency("UZS", "Uzbekistan Sum", 860, 2);
            public readonly Currency VEF = new Currency("VEF", "Bolívar", 937, 2);
            public readonly Currency VND = new Currency("VND", "Dong", 704, 0);
            public readonly Currency VUV = new Currency("VUV", "Vatu", 548, 0);
            public readonly Currency WST = new Currency("WST", "Tala", 882, 2);
            public readonly Currency XAF = new Currency("XAF", "CFA Franc BEAC", 950, 0);
            public readonly Currency XAG = new Currency("XAG", "Silver", 961, 0);
            public readonly Currency XAU = new Currency("XAU", "Gold", 959, 0);
            public readonly Currency XBA = new Currency("XBA", "Bond Markets Unit European Composite Unit (EURCO)", 955, 0);
            public readonly Currency XBB = new Currency("XBB", "Bond Markets Unit European Monetary Unit (E.M.U.-6)", 956, 0);
            public readonly Currency XBC = new Currency("XBC", "Bond Markets Unit European Unit of Account 9 (E.U.A.-9)", 957, 0);
            public readonly Currency XBD = new Currency("XBD", "Bond Markets Unit European Unit of Account 17 (E.U.A.-17)", 958, 0);
            public readonly Currency XCD = new Currency("XCD", "East Caribbean Dollar", 951, 2);
            public readonly Currency XDR = new Currency("XDR", "SDR (Special Drawing Right)", 960, 0);
            public readonly Currency XOF = new Currency("XOF", "CFA Franc BCEAO", 952, 0);
            public readonly Currency XPD = new Currency("XPD", "Palladium", 964, 0);
            public readonly Currency XPF = new Currency("XPF", "CFP Franc", 953, 0);
            public readonly Currency XPT = new Currency("XPT", "Platinum", 962, 0);
            public readonly Currency XSU = new Currency("XSU", "Sucre", 994, 0);
            public readonly Currency XTS = new Currency("XTS", "Codes specifically reserved for testing purposes", 963, 0);
            public readonly Currency XUA = new Currency("XUA", "ADB Unit of Account", 965, 0);
            public readonly Currency XXX = new Currency("XXX", "The codes assigned for transactions where no readonly Currency is involved", 999, 0);
            public readonly Currency YER = new Currency("YER", "Yemeni Rial", 886, 2);
            public readonly Currency ZAR = new Currency("ZAR", "Rand", 710, 2);
            public readonly Currency ZMW = new Currency("ZMW", "Zambian Kwacha", 967, 2);
            public readonly Currency ZWL = new Currency("ZWL", "Zimbabwe Dollar", 932, 2, "$");

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
