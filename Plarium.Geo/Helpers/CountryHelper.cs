namespace Plarium.Geo.Helpers
{
    using System;

    public class CountryHelper
    {
        private static CountryHelper _default;

        private static readonly string[] _countryCode = {
    "--","AP","EU","AD","AE","AF","AG","AI","AL","AM","CW",
	"AO","AQ","AR","AS","AT","AU","AW","AZ","BA","BB",
	"BD","BE","BF","BG","BH","BI","BJ","BM","BN","BO",
	"BR","BS","BT","BV","BW","BY","BZ","CA","CC","CD",
	"CF","CG","CH","CI","CK","CL","CM","CN","CO","CR",
	"CU","CV","CX","CY","CZ","DE","DJ","DK","DM","DO",
	"DZ","EC","EE","EG","EH","ER","ES","ET","FI","FJ",
	"FK","FM","FO","FR","SX","GA","GB","GD","GE","GF",
	"GH","GI","GL","GM","GN","GP","GQ","GR","GS","GT",
	"GU","GW","GY","HK","HM","HN","HR","HT","HU","ID",
	"IE","IL","IN","IO","IQ","IR","IS","IT","JM","JO",
	"JP","KE","KG","KH","KI","KM","KN","KP","KR","KW",
	"KY","KZ","LA","LB","LC","LI","LK","LR","LS","LT",
	"LU","LV","LY","MA","MC","MD","MG","MH","MK","ML",
	"MM","MN","MO","MP","MQ","MR","MS","MT","MU","MV",
	"MW","MX","MY","MZ","NA","NC","NE","NF","NG","NI",
	"NL","NO","NP","NR","NU","NZ","OM","PA","PE","PF",
	"PG","PH","PK","PL","PM","PN","PR","PS","PT","PW",
	"PY","QA","RE","RO","RU","RW","SA","SB","SC","SD",
	"SE","SG","SH","SI","SJ","SK","SL","SM","SN","SO",
	"SR","ST","SV","SY","SZ","TC","TD","TF","TG","TH",
	"TJ","TK","TM","TN","TO","TL","TR","TT","TV","TW",
	"TZ","UA","UG","UM","US","UY","UZ","VA","VC","VE",
	"VG","VI","VN","VU","WF","WS","YE","YT","RS","ZA",
	"ZM","ME","ZW","A1","A2","O1","AX","GG","IM","JE",
    "BL","MF", "BQ", "SS"
	};

        private static readonly int[] _countryDialCode =
        {
        0, 1, 388, 376, 971, 93, 1268, 1264,
        355, 374, 599, 244, 672, 54, 1684, 43,
        61, 297, 994, 387, 1246, 880, 32, 226,
        359, 973, 257, 229, 1441, 673, 591, 55,
        1242, 975, 47, 267, 375, 501, 1, 61,
        243, 236, 242, 41, 225, 682, 56, 237, 86,
        57, 506, 53, 238, 61, 357, 420, 49, 253,
        45, 1767, 1809, 213, 593, 372, 20, 212,
        291, 34, 251, 358, 679, 500, 691, 298,
        33, 1721, 241, 44, 1473, 995, 594, 233,
        350, 299, 220, 224, 590, 240, 30, 500,
        502, 1671, 245, 592, 852, 672, 504, 385,
        509, 36, 62, 353, 972, 91, 246, 964, 98,
        354, 39, 1876, 962, 81, 254, 996, 855,
        686, 269, 1869, 850, 82, 965, 1345, 7,
        856, 961, 1758, 423, 94, 231, 266, 370,
        352, 371, 218, 212, 377, 373, 261, 692,
        389, 223, 95, 976, 853, 1670, 596, 222,
        1664, 356, 230, 960, 265, 52, 60, 258,
        264, 687, 227, 672, 234, 505, 31, 47,
        977, 674, 683, 64, 968, 507, 51, 689,
        675, 63, 92, 48, 508, 870, 1, 970, 351,
        680, 595, 974, 262, 40, 7, 250, 966, 677,
        248, 249, 46, 65, 290, 386, 47, 421, 232,
        378, 221, 252, 597, 239, 503, 963, 268,
        1649, 235, 262, 228, 66, 992, 690, 993,
        216, 676, 670, 90, 1868, 688, 886, 255,
        380, 256, 1, 1, 598, 998, 379, 1784, 58,
        1284, 1340, 84, 678, 681, 685, 967, 262,
        381, 27, 260, 382, 263, 1, 1, 1, 358,
        44, 44, 44, 590, 590, 599, 211
        };

        private static readonly string[] _countryName = {
    "N/A","Asia/Pacific Region","Europe","Andorra","United Arab Emirates","Afghanistan","Antigua and Barbuda","Anguilla","Albania","Armenia","Curacao",
	"Angola","Antarctica","Argentina","American Samoa","Austria","Australia","Aruba","Azerbaijan","Bosnia and Herzegovina","Barbados",
	"Bangladesh","Belgium","Burkina Faso","Bulgaria","Bahrain","Burundi","Benin","Bermuda","Brunei Darussalam","Bolivia",
	"Brazil","Bahamas","Bhutan","Bouvet Island","Botswana","Belarus","Belize","Canada","Cocos (Keeling) Islands","Congo, The Democratic Republic of the",
	"Central African Republic","Congo","Switzerland","Cote D'Ivoire","Cook Islands","Chile","Cameroon","China","Colombia","Costa Rica",
	"Cuba","Cape Verde","Christmas Island","Cyprus","Czech Republic","Germany","Djibouti","Denmark","Dominica","Dominican Republic",
	"Algeria","Ecuador","Estonia","Egypt","Western Sahara","Eritrea","Spain","Ethiopia","Finland","Fiji",
	"Falkland Islands (Malvinas)","Micronesia, Federated States of","Faroe Islands","France","Sint Maarten (Dutch part)","Gabon","United Kingdom","Grenada","Georgia","French Guiana",
	"Ghana","Gibraltar","Greenland","Gambia","Guinea","Guadeloupe","Equatorial Guinea","Greece","South Georgia and the South Sandwich Islands","Guatemala",
	"Guam","Guinea-Bissau","Guyana","Hong Kong","Heard Island and McDonald Islands","Honduras","Croatia","Haiti","Hungary","Indonesia",
	"Ireland","Israel","India","British Indian Ocean Territory","Iraq","Iran, Islamic Republic of","Iceland","Italy","Jamaica","Jordan",
	"Japan","Kenya","Kyrgyzstan","Cambodia","Kiribati","Comoros","Saint Kitts and Nevis","Korea, Democratic People's Republic of","Korea, Republic of","Kuwait",
	"Cayman Islands","Kazakhstan","Lao People's Democratic Republic","Lebanon","Saint Lucia","Liechtenstein","Sri Lanka","Liberia","Lesotho","Lithuania",
	"Luxembourg","Latvia","Libya","Morocco","Monaco","Moldova, Republic of","Madagascar","Marshall Islands","Macedonia","Mali",
	"Myanmar","Mongolia","Macau","Northern Mariana Islands","Martinique","Mauritania","Montserrat","Malta","Mauritius","Maldives",
	"Malawi","Mexico","Malaysia","Mozambique","Namibia","New Caledonia","Niger","Norfolk Island","Nigeria","Nicaragua",
	"Netherlands","Norway","Nepal","Nauru","Niue","New Zealand","Oman","Panama","Peru","French Polynesia",
	"Papua New Guinea","Philippines","Pakistan","Poland","Saint Pierre and Miquelon","Pitcairn Islands","Puerto Rico","Palestinian Territory","Portugal","Palau",
	"Paraguay","Qatar","Reunion","Romania","Russian Federation","Rwanda","Saudi Arabia","Solomon Islands","Seychelles","Sudan",
	"Sweden","Singapore","Saint Helena","Slovenia","Svalbard and Jan Mayen","Slovakia","Sierra Leone","San Marino","Senegal","Somalia","Suriname",
	"Sao Tome and Principe","El Salvador","Syrian Arab Republic","Swaziland","Turks and Caicos Islands","Chad","French Southern Territories","Togo","Thailand",
	"Tajikistan","Tokelau","Turkmenistan","Tunisia","Tonga","Timor-Leste","Turkey","Trinidad and Tobago","Tuvalu","Taiwan",
	"Tanzania, United Republic of","Ukraine","Uganda","United States Minor Outlying Islands","United States","Uruguay","Uzbekistan","Holy See (Vatican City State)","Saint Vincent and the Grenadines","Venezuela",
	"Virgin Islands, British","Virgin Islands, U.S.","Vietnam","Vanuatu","Wallis and Futuna","Samoa","Yemen","Mayotte","Serbia","South Africa",
	"Zambia","Montenegro","Zimbabwe","Anonymous Proxy","Satellite Provider","Other","Aland Islands","Guernsey","Isle of Man","Jersey",
    "Saint Barthelemy","Saint Martin", "Bonaire, Saint Eustatius and Saba", "South Sudan"};

        private CountryHelper()
        {

        }

        public static CountryHelper Default
        {
            get
            {
                return _default ?? (_default = new CountryHelper());
            }
        }

        public string GetCountryName(string code)
        {
            var index = Array.IndexOf(_countryCode, code);
            if (index < 0)
            {
                index = 0;
            }
            return _countryName[index];
        }

        public byte CountryToByte(string code)
        {
            var index = Array.IndexOf(_countryCode, code);
            return Convert.ToByte(index < 0 ? 0 : index);
        }

        public string GetCountryName(byte code)
        {
            var index = Convert.ToInt32(code);
            return _countryName[index];
        }

        public string GetCountryCode(byte code)
        {
            var index = Convert.ToInt32(code);
            return _countryCode[index];
        }

        public string GetDialCode(string code)
        {
            var index = Array.IndexOf(_countryCode, code);
            return _countryDialCode[index].ToString();
        }
    }
}
