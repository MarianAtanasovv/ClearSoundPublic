using ClearSoundCompany.Data;
using ClearSoundCompany.Data.Models.Products;
using ClearSoundCompany.Data.Models.Rental;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using ClearSoundCompany.Data.Models.About;

namespace ClearSoundCompany.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase
            (this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();

            var data = scopedServices.ServiceProvider.GetService<ClearSoundDbContext>();

            data.Database.Migrate();

            SeedCategories(data);
            SeedCountries(data);
            SeedAdministrator(scopedServices.ServiceProvider);
            SeedAbout(data);
            return app;
        }

        private static void SeedAbout(ClearSoundDbContext data)
        {
            if (data.About.Any()) return;

            const string about =
                "Clear Sound was found in 2005 to respond to all modern requirements of quality sound and design. The company is developing rapidly and more and more professionals and customers from the country and abroad are interested in its products. In the beginning of 2008 LA-LCS was invented- a unique line array with liquid cooling system. \n"
                +
                "Clear Sound' speakers are furnished with Sound Definition - company's new brand loudspeakers accepted with great interest because of their characteristics and reasonable price! \n"
                +
                "Clear Sound uses high quality materials, modern development tools, precise measurement systems and follows strict quality control that guarantees optimal results satisfying every customer. \n"
                +
                "Clear Sound's acoustic systems are suitable for stadiums, public houses and coffee-houses. They are also the perfect solution for home cinemas, hotels, restaurants and churches.\n";

            const string image = "/about.jpg";
            data.About.AddRange(
                new About
                    {Description = about, Image = image});

            data.SaveChanges();
        }

        private static void SeedCountries(ClearSoundDbContext data)
        {
            if (data.Countries.Any()) return;

            data.Countries.AddRange(new Country {Name = "Afghanistan", FlagImage = "AF"},
                new Country {Name = "Aland Islands", FlagImage = "AX"},
                new Country {Name = "Albania", FlagImage = "AL"}, new Country {Name = "Algeria", FlagImage = "DZ"},
                new Country {Name = "American Samoa", FlagImage = "AS"},
                new Country {Name = "Andorra", FlagImage = "AD"}, new Country {Name = "Angola", FlagImage = "AO"},
                new Country {Name = "Anguilla", FlagImage = "AI"}, new Country {Name = "Antarctica", FlagImage = "AQ"},
                new Country {Name = "Antigua and Barbuda", FlagImage = "AG"},
                new Country {Name = "Argentina", FlagImage = "AR"}, new Country {Name = "Armenia", FlagImage = "AM"},
                new Country {Name = "Aruba", FlagImage = "AW"}, new Country {Name = "Australia", FlagImage = "AU"},
                new Country {Name = "Austria", FlagImage = "AT"}, new Country {Name = "Azerbaijan", FlagImage = "AZ"},
                new Country {Name = "Bahamas", FlagImage = "BS"}, new Country {Name = "Bahrain", FlagImage = "BH"},
                new Country {Name = "Bangladesh", FlagImage = "BD"}, new Country {Name = "Barbados", FlagImage = "BB"},
                new Country {Name = "Belarus", FlagImage = "BY"}, new Country {Name = "Belgium", FlagImage = "BE"},
                new Country {Name = "Belize", FlagImage = "BZ"}, new Country {Name = "Benin", FlagImage = "BJ"},
                new Country {Name = "Bermuda", FlagImage = "BM"}, new Country {Name = "Bhutan", FlagImage = "BT"},
                new Country {Name = "Bolivia", FlagImage = "BO"}, new Country {Name = "Bonaire", FlagImage = "BQ"},
                new Country {Name = "Bosnia and Herzegovina", FlagImage = "BA"},
                new Country {Name = "Botswana", FlagImage = "BW"},
                new Country {Name = "Bouvet Island", FlagImage = "BV"}, new Country {Name = "Brazil", FlagImage = "BR"},
                new Country {Name = "British Indian Ocean Territory", FlagImage = "IO"},
                new Country {Name = "Brunei Darussalam", FlagImage = "BN"},
                new Country {Name = "Bulgaria", FlagImage = "BG"},
                new Country {Name = "Burkina Faso", FlagImage = "BF"}, new Country {Name = "Burundi", FlagImage = "BI"},
                new Country {Name = "Cambodia", FlagImage = "KH"}, new Country {Name = "Cameroon", FlagImage = "CM"},
                new Country {Name = "Canada", FlagImage = "CA"}, new Country {Name = "Cape Verde", FlagImage = "CV"},
                new Country {Name = "Cayman Islands", FlagImage = "KY"},
                new Country {Name = "Central African Republic", FlagImage = "CF"},
                new Country {Name = "Chad", FlagImage = "TD"}, new Country {Name = "Chile", FlagImage = "CL"},
                new Country {Name = "China", FlagImage = "CN"},
                new Country {Name = "Christmas Island", FlagImage = "CX"},
                new Country {Name = "Cocos (Keeling) Islands", FlagImage = "CC"},
                new Country {Name = "Colombia", FlagImage = "CO"}, new Country {Name = "Comoros", FlagImage = "KM"},
                new Country {Name = "Congo", FlagImage = "CG"}, new Country {Name = "Congo", FlagImage = "CD"},
                new Country {Name = "Cook Islands", FlagImage = "CK"},
                new Country {Name = "Costa Rica", FlagImage = "CR"}, new Country {Name = "Croatia", FlagImage = "HR"},
                new Country {Name = "Cuba", FlagImage = "CU"}, new Country {Name = "Curacao", FlagImage = "CW"},
                new Country {Name = "Cyprus", FlagImage = "CY"},
                new Country {Name = "Czech Republic", FlagImage = "CZ"},
                new Country {Name = "Denmark", FlagImage = "DK"}, new Country {Name = "Djibouti", FlagImage = "DJ"},
                new Country {Name = "Dominica", FlagImage = "DM"},
                new Country {Name = "Dominican Republic", FlagImage = "DO"},
                new Country {Name = "Ecuador", FlagImage = "EC"}, new Country {Name = "Egypt", FlagImage = "EG"},
                new Country {Name = "El Salvador", FlagImage = "SV"},
                new Country {Name = "Equatorial Guinea", FlagImage = "GQ"},
                new Country {Name = "Eritrea", FlagImage = "ER"}, new Country {Name = "Estonia", FlagImage = "EE"},
                new Country {Name = "Ethiopia", FlagImage = "ET"},
                new Country {Name = "Falkland Islands (Malvinas)", FlagImage = "FK"},
                new Country {Name = "Faroe Islands", FlagImage = "FO"}, new Country {Name = "Fiji", FlagImage = "FJ"},
                new Country {Name = "Finland", FlagImage = "FI"}, new Country {Name = "France", FlagImage = "FR"},
                new Country {Name = "French Guiana", FlagImage = "GF"},
                new Country {Name = "French Polynesia", FlagImage = "PF"},
                new Country {Name = "French Southern Territories", FlagImage = "TF"},
                new Country {Name = "Gabon", FlagImage = "GA"}, new Country {Name = "Gambia", FlagImage = "GM"},
                new Country {Name = "Georgia", FlagImage = "GE"}, new Country {Name = "Germany", FlagImage = "DE"},
                new Country {Name = "Ghana", FlagImage = "GH"}, new Country {Name = "Gibraltar", FlagImage = "GI"},
                new Country {Name = "Greece", FlagImage = "GR"}, new Country {Name = "Greenland", FlagImage = "GL"},
                new Country {Name = "Grenada", FlagImage = "GD"}, new Country {Name = "Guadeloupe", FlagImage = "GP"},
                new Country {Name = "Guam", FlagImage = "GU"}, new Country {Name = "Guatemala", FlagImage = "GT"},
                new Country {Name = "Guernsey", FlagImage = "GG"}, new Country {Name = "Guinea", FlagImage = "GN"},
                new Country {Name = "Guinea-Bissau", FlagImage = "GW"}, new Country {Name = "Guyana", FlagImage = "GY"},
                new Country {Name = "Haiti", FlagImage = "HT"},
                new Country {Name = "Heard Island and McDonald Islands", FlagImage = "HM"},
                new Country {Name = "Holy See (Vatican City State)", FlagImage = "VA"},
                new Country {Name = "Honduras", FlagImage = "HN"}, new Country {Name = "Hong Kong", FlagImage = "HK"},
                new Country {Name = "Hungary", FlagImage = "HU"}, new Country {Name = "Iceland", FlagImage = "IS"},
                new Country {Name = "India", FlagImage = "IN"}, new Country {Name = "Indonesia", FlagImage = "ID"},
                new Country {Name = "Iran", FlagImage = "IR"}, new Country {Name = "Iraq", FlagImage = "IQ"},
                new Country {Name = "Ireland", FlagImage = "IE"}, new Country {Name = "Isle of Man", FlagImage = "IM"},
                new Country {Name = "Israel", FlagImage = "IL"}, new Country {Name = "Italy", FlagImage = "IT"},
                new Country {Name = "Ivory Coast", FlagImage = "CI"}, new Country {Name = "Jamaica", FlagImage = "JM"},
                new Country {Name = "Japan", FlagImage = "JP"}, new Country {Name = "Jersey", FlagImage = "JE"},
                new Country {Name = "Jordan", FlagImage = "JO"}, new Country {Name = "Kazakhstan", FlagImage = "KZ"},
                new Country {Name = "Kenya", FlagImage = "KE"}, new Country {Name = "Kiribati", FlagImage = "KI"},
                new Country {Name = "North Korea", FlagImage = "KP"},
                new Country {Name = "South Korea", FlagImage = "KR"}, new Country {Name = "Kuwait", FlagImage = "KW"},
                new Country {Name = "Kyrgyzstan", FlagImage = "KG"},
                new Country {Name = "Lao People's Democratic Republic", FlagImage = "LA"},
                new Country {Name = "Latvia", FlagImage = "LV"}, new Country {Name = "Lebanon", FlagImage = "LB"},
                new Country {Name = "Lesotho", FlagImage = "LS"}, new Country {Name = "Liberia", FlagImage = "LR"},
                new Country {Name = "Libya", FlagImage = "LY"}, new Country {Name = "Liechtenstein", FlagImage = "LI"},
                new Country {Name = "Lithuania", FlagImage = "LT"}, new Country {Name = "Luxembourg", FlagImage = "LU"},
                new Country {Name = "Macao", FlagImage = "MO"}, new Country {Name = "Macedonia", FlagImage = "MK"},
                new Country {Name = "Madagascar", FlagImage = "MG"}, new Country {Name = "Malawi", FlagImage = "MW"},
                new Country {Name = "Malaysia", FlagImage = "MY"}, new Country {Name = "Maldives", FlagImage = "MV"},
                new Country {Name = "Mali", FlagImage = "ML"}, new Country {Name = "Malta", FlagImage = "MT"},
                new Country {Name = "Marshall Islands", FlagImage = "MH"},
                new Country {Name = "Martinique", FlagImage = "MQ"},
                new Country {Name = "Mauritania", FlagImage = "MR"}, new Country {Name = "Mauritius", FlagImage = "MU"},
                new Country {Name = "Mayotte", FlagImage = "YT"}, new Country {Name = "Mexico", FlagImage = "MX"},
                new Country {Name = "Micronesia", FlagImage = "FM"}, new Country {Name = "Moldova", FlagImage = "MD"},
                new Country {Name = "Monaco", FlagImage = "MC"}, new Country {Name = "Mongolia", FlagImage = "MN"},
                new Country {Name = "Montenegro", FlagImage = "ME"},
                new Country {Name = "Montserrat", FlagImage = "MS"}, new Country {Name = "Morocco", FlagImage = "MA"},
                new Country {Name = "Mozambique", FlagImage = "MZ"}, new Country {Name = "Myanmar", FlagImage = "MM"},
                new Country {Name = "Namibia", FlagImage = "NA"}, new Country {Name = "Nauru", FlagImage = "NR"},
                new Country {Name = "Nepal", FlagImage = "NP"}, new Country {Name = "Netherlands", FlagImage = "NL"},
                new Country {Name = "New Caledonia", FlagImage = "NC"},
                new Country {Name = "New Zealand", FlagImage = "NZ"},
                new Country {Name = "Nicaragua", FlagImage = "NI"}, new Country {Name = "Niger", FlagImage = "NE"},
                new Country {Name = "Nigeria", FlagImage = "NG"}, new Country {Name = "Niue", FlagImage = "NU"},
                new Country {Name = "Norfolk Island", FlagImage = "NF"},
                new Country {Name = "Northern Mariana Islands", FlagImage = "MP"},
                new Country {Name = "Norway", FlagImage = "NO"}, new Country {Name = "Oman", FlagImage = "OM"},
                new Country {Name = "Pakistan", FlagImage = "PK"}, new Country {Name = "Palau", FlagImage = "PW"},
                new Country {Name = "Palestine", FlagImage = "PS"}, new Country {Name = "Panama", FlagImage = "PA"},
                new Country {Name = "Papua New Guinea", FlagImage = "PG"},
                new Country {Name = "Paraguay", FlagImage = "PY"}, new Country {Name = "Peru", FlagImage = "PE"},
                new Country {Name = "Philippines", FlagImage = "PH"}, new Country {Name = "Pitcairn", FlagImage = "PN"},
                new Country {Name = "Poland", FlagImage = "PL"}, new Country {Name = "Portugal", FlagImage = "PT"},
                new Country {Name = "Puerto Rico", FlagImage = "PR"}, new Country {Name = "Qatar", FlagImage = "QA"},
                new Country {Name = "Reunion", FlagImage = "RE"}, new Country {Name = "Romania", FlagImage = "RO"},
                new Country {Name = "Russian Federation", FlagImage = "RU"},
                new Country {Name = "Rwanda", FlagImage = "RW"},
                new Country {Name = "Saint Barthеlemy", FlagImage = "BL"},
                new Country {Name = "Saint Helena", FlagImage = "SH"},
                new Country {Name = "Saint Kitts and Nevis", FlagImage = "KN"},
                new Country {Name = "Saint Lucia", FlagImage = "LC"},
                new Country {Name = "Saint Martin (French part)", FlagImage = "MF"},
                new Country {Name = "Saint Pierre and Miquelon", FlagImage = "PM"},
                new Country {Name = "Saint Vincent and the Grenadines", FlagImage = "VC"},
                new Country {Name = "Samoa", FlagImage = "WS"}, new Country {Name = "San Marino", FlagImage = "SM"},
                new Country {Name = "Sao Tome and Principe", FlagImage = "ST"},
                new Country {Name = "Saudi Arabia", FlagImage = "SA"}, new Country {Name = "Senegal", FlagImage = "SN"},
                new Country {Name = "Serbia", FlagImage = "RS"}, new Country {Name = "Seychelles", FlagImage = "SC"},
                new Country {Name = "Sierra Leone", FlagImage = "SL"},
                new Country {Name = "Singapore", FlagImage = "SG"},
                new Country {Name = "Sint Maarten (Dutch part)", FlagImage = "SX"},
                new Country {Name = "Slovakia", FlagImage = "SK"}, new Country {Name = "Slovenia", FlagImage = "SI"},
                new Country {Name = "Solomon Islands", FlagImage = "SB"},
                new Country {Name = "Somalia", FlagImage = "SO"}, new Country {Name = "South Africa", FlagImage = "ZA"},
                new Country {Name = "South Georgia and the South Sandwich Islands", FlagImage = "GS"},
                new Country {Name = "South Sudan", FlagImage = "SS"}, new Country {Name = "Spain", FlagImage = "ES"},
                new Country {Name = "Sri Lanka", FlagImage = "LK"}, new Country {Name = "Sudan", FlagImage = "SD"},
                new Country {Name = "Suriname", FlagImage = "SR"},
                new Country {Name = "Svalbard and Jan Mayen", FlagImage = "SJ"},
                new Country {Name = "Swaziland", FlagImage = "SZ"}, new Country {Name = "Sweden", FlagImage = "SE"},
                new Country {Name = "Switzerland", FlagImage = "CH"},
                new Country {Name = "Syrian Arab Republic", FlagImage = "SY"},
                new Country {Name = "Taiwan", FlagImage = "TW"}, new Country {Name = "Tajikistan", FlagImage = "TJ"},
                new Country {Name = "Tanzania", FlagImage = "TZ"}, new Country {Name = "Thailand", FlagImage = "TH"},
                new Country {Name = "Timor-Leste", FlagImage = "TL"}, new Country {Name = "Togo", FlagImage = "TG"},
                new Country {Name = "Tokelau", FlagImage = "TK"}, new Country {Name = "Tonga", FlagImage = "TO"},
                new Country {Name = "Trinidad and Tobago", FlagImage = "TT"},
                new Country {Name = "Tunisia", FlagImage = "TN"}, new Country {Name = "Turkey", FlagImage = "TR"},
                new Country {Name = "Turkmenistan", FlagImage = "TM"},
                new Country {Name = "Turks and Caicos Islands", FlagImage = "TC"},
                new Country {Name = "Tuvalu", FlagImage = "TV"}, new Country {Name = "Uganda", FlagImage = "UG"},
                new Country {Name = "Ukraine", FlagImage = "UA"},
                new Country {Name = "United Arab Emirates", FlagImage = "AE"},
                new Country {Name = "United Kingdom", FlagImage = "GB"},
                new Country {Name = "United States", FlagImage = "US"},
                new Country {Name = "United States Minor Outlying Islands", FlagImage = "UM"},
                new Country {Name = "Uruguay", FlagImage = "UY"}, new Country {Name = "Uzbekistan", FlagImage = "UZ"},
                new Country {Name = "Vanuatu", FlagImage = "VU"}, new Country {Name = "Venezuela", FlagImage = "VE"},
                new Country {Name = "Viet Nam", FlagImage = "VN"},
                new Country {Name = "Virgin Islands British", FlagImage = "VG"},
                new Country {Name = "Virgin Islands US", FlagImage = "VI"},
                new Country {Name = "Wallis and Futuna", FlagImage = "WF"},
                new Country {Name = "Western Sahara", FlagImage = "EH"}, new Country {Name = "Yemen", FlagImage = "YE"},
                new Country {Name = "Zambia", FlagImage = "ZM"}, new Country {Name = "Zimbabwe", FlagImage = "ZW"});

            data.SaveChanges();
        }

        private static void SeedCategories(ClearSoundDbContext data)
        {
            if (data.Categories.Any()) return;

            data.Categories.AddRange(
                new Category {Name = "Coaxial & Triaxial", IsAddable = true, MinimumQuantity = 2},
                new Category {Name = "Club Fixed Series", IsAddable = true, MinimumQuantity = 2},
                new Category {Name = "Monitor Series", IsAddable = true, MinimumQuantity = 2},
                new Category {Name = "Point Source Series", IsAddable = true, MinimumQuantity = 2},
                new Category {Name = "Line Array Series", IsAddable = true, MinimumQuantity = 4},
                new Category {Name = "Touring Series", IsAddable = true, MinimumQuantity = 2},
                new Category {Name = "Loudspeakers", IsAddable = false, MinimumQuantity = 2},
                new Category {Name = "Subwoofer Series", IsAddable = true, MinimumQuantity = 2},
                new Category {Name = "High-End Solutions", IsAddable = true, MinimumQuantity = 2},
                new Category {Name = "Speech Speakers", IsAddable = true, MinimumQuantity = 2});

            data.SaveChanges();
        }

        private static void SeedAdministrator(IServiceProvider service)
        {
            var userManager = service.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();

            Task
                .Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync("Administrator")) return;

                    var role = new IdentityRole {Name = "Administrator"};
                    await roleManager.CreateAsync(role);

                    const string adminUsername = "";
                    const string adminEmail = "";
                    const string adminPassword = "";

                    var user = new IdentityUser
                    {
                        UserName = adminUsername,
                        Email = adminEmail,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(user, adminPassword);
                    await userManager.AddToRoleAsync(user, role.Name);
                })
                .GetAwaiter()
                .GetResult();
        }
    }
}